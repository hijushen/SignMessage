using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.API.Controllers.SignMessage
{
    /// <summary>
    /// 签核箱 消息接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[EnableCors("AllowAnyOrigin")]
    public class SignMessageController : Controller
    {
        private SignMessgeBiz bll = new SignMessgeBiz();
        private TempBussiness tempBiz = new TempBussiness();

        /// <summary>
        /// 新增签核箱消息
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Route("NewSignMsg")]
        [Authorize(Policy = "Client")]
        public JsonResult PostNewSignMsg([FromBody]SignMessageSendDto value)
        {
            var appOID = User.Identity.Name;
            return Json(bll.PostNewSignMsg(value, appOID));

            //return Json(tempBiz.MockMessageBox());
        }

        /// <summary>
        /// 更新签核箱消息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSignMsg")]
        [Authorize(Policy="Client")]      
        public JsonResult PostUpdateSignMsg([FromBody]SignMessageSendDto value)
        {

            LogHelper.Debug(Request.ToString());
            var appOID = User.Identity.Name;

            return Json(bll.PostUpdateSignMsg(value, appOID));
            ///return null;
        }
    }
}
