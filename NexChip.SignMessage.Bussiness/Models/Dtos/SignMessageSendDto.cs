using Newtonsoft.Json;
using SqlSugar;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    /// <summary>
    /// 签核箱 消息接口实体
    /// </summary>
    public class SignMessageSendDto
    {
        /// <summary>
        /// 接口表主键
        /// </summary>
        public string interfaceOID { get; set; }

        /// <summary>
        /// 发送来源平台名称
        /// <example>某个名称</example>
        /// </summary>
        public string appname { get; set; }



        /// <summary>
        /// 
        /// <example></example>
        /// </summary>
        public int handleresult { get; set; }



        /// <summary>
        /// 
        /// <example></example>
        /// </summary>
        public string handleerrormsg { get; set; }

        /// <summary>
        /// 发送时间
        /// <example>2019-08-31 19:00:00</example>
        /// </summary>
        //[JsonConverter(typeof(CustomDateConverter))]        
        public DateTime? sendtime { get; set; }

        /// <summary>
        /// 签核箱消息内容主体
        /// </summary>
        public SignMessageSendBodyDto msgbody { get; set; }
    }

}
