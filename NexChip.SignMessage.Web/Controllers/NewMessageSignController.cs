using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Bussiness.Models.Dtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.Web.Controllers
{
    public class NewMessageSignController : Controller
    {
        public SignMessageBoxBiz boxBiz = new SignMessageBoxBiz();

        ///GET: /<controller>/
        public IActionResult Index()
        {
            return null;
            return View();
        }

        [HttpPost]
        public JsonResult DataTableList(DataTablesRequsetDto reqP)
        {
            var userName = User.Identity.Name;
            var res = boxBiz.ListForDataTables(reqP, userName);
            return Json(res);
        }

        public JsonResult GetS(string OID)
        {
            return Json(boxBiz.Get(OID));
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name = "dto" ></ param >
        /// < returns ></ returns >
        [HttpPost]
        public JsonResult EditSave([FromForm]SignMessageRoleDto dto)
        {
            return Json(boxBiz.EditSave(dto));
        }


        public JsonResult Delete(string[] OIDs)
        {
            return Json(boxBiz.Delete(OIDs));
        }

        public JsonResult testSend(string OID, int type)
        {
            if(type == 1)
            {
                return Json(boxBiz.testSendUpdate(OID));
            } 
            else
            {
                return Json(boxBiz.testSendUpdate(OID));
            }
        }
    }
}
