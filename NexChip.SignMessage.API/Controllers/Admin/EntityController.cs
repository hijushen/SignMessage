using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexChip.SignMessage.API.Controllers.Admin
{
    [Route("api/Admin/[controller]")]
    public class EntityController: Controller
    {
        private EntityBiz bll = new EntityBiz();

        private IHostingEnvironment _hostingEnvironment;
        public EntityController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateEntity")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JsonResult CreateEntity(string entityName = null)
        {
            if (entityName == null)
                return Json("参数为空");
            return Json(bll.CreateEntity(entityName, _hostingEnvironment.ContentRootPath));
        }
    }
}
