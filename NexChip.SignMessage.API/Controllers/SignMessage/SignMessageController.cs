using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.API.Controllers.SignMessage
{
    /// <summary>
    /// 签核箱 消息接口
    [Produces("application/json")]
    [Route("api/[controller]")]
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
        public JsonResult PostNewSignMsg([FromBody]SignMessageModel value)
        {
            //return Json(bll.PostNewSignMsg(value));

            return Json(tempBiz.MockMessageBox());
        }

        /// <summary>
        /// 更新签核箱消息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSignMsg")]
        public JsonResult PostUpdateMsg([FromBody]SignMessageModel value)
        {
            //return Json(bll.PostUpdateMsg(value));
            return null;
        }
    }
}
