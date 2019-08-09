using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NexChip.SignMessage.API.AuthHelper;
using Swashbuckle.AspNetCore.Swagger;

namespace NexChip.SignMessage.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string key)
            {
                return key.ToLower();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(option =>
                {
                    //配置大小写问题，默认是首字母小写
                    option.SerializerSettings.ContractResolver = new LowercaseContractResolver();
                    //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            services.PostConfigure<MvcJsonOptions>(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region Token认证
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("System", policy => policy.RequireClaim("SystemType").Build());
                options.AddPolicy("Client", policy => policy.RequireClaim("ClientType").Build());
                options.AddPolicy("Admin", policy => policy.RequireClaim("AdminType").Build());
            });

            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.1.0",
                    Title = "NexChip.SignMessage WebAPI",
                    Description = "签核箱消息平台API接口",
                    TermsOfService = "None"
                    //,Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "", Email = "", Url = "" }
                });

                //添加读取注释服务
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var basePath = HostingEnvironment.ContentRootPath;
                var xmlPath = Path.Combine(basePath, "apidoc/APIHelper.xml");
                c.IncludeXmlComments(xmlPath, true);//添加控制器层注释（true表示显示控制器注释）

                //var entityXmlPath = Path.Combine(basePath, "apidoc/EntitiesHelper.xml");
                //c.IncludeXmlComments(entityXmlPath);

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Token
            app.UseMiddleware<TokenAuth>();
            #endregion


            app.UseMvc();


            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion
        }
    }
}
