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
        public SignMessageBoxBiz boxBiz;

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
               
        [HttpPost]
        public JsonResult DataTableList(DataTablesRequsetDto reqP)
        {
            var res = boxBiz.ListForDataTables(reqP);
            return Json(res);
        }

        public JsonResult GetS(string OID)
        {
            return Json(boxBiz.Get(OID));
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditSave([FromForm]SignMessageRoleDto dto)
        {
            return Json(boxBiz.EditSave(dto));
        }


        public JsonResult Delete(string[] OIDs)
        {
            return Json(boxBiz.Delete(OIDs));
        }
    }
}
