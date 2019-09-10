using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.Web.Controllers
{
    /// <summary>
    /// 2019-08-29
    /// </summary>
    public class MessageSignController : Controller
    {
        public SignMessageBoxBiz boxBiz = new SignMessageBoxBiz();

        // GET: /<controller>/
        /// </summary>
        /// <param name="logonid">sanzhang</param>
        /// <param name="id">E01000</param>
        /// <param name="SHAEncry"></param>
        /// <returns></returns>
        public IActionResult Index(string logonid, string eid, string SHAEncry)
        {

            ViewBag.logonid = logonid;
            ViewBag.id = eid;
            ViewBag.SHAEncry = SHAEncry;


            ViewBag.WebAPIBaseUrl = Utils.SettingConfig.PostUrl;
            ViewBag.WebAPICallAuth = Utils.SettingConfig.ApiTokenString;
            ViewBag.Title = Utils.SettingConfig.NoBroGTitle;
            ViewBag.AllString = Utils.SettingConfig.AllString;

            ViewBag.formTypes = boxBiz.GetDistinctFormNames();

            return View();
        }

        [HttpPost]
        public JsonResult DataTableList(DataTablesRequsetDto reqP)
        {
            var userName = LoginUserHelper.GetLoginUserName(reqP.logonid, User.Identity.Name);
            var res = boxBiz.ListForDataTables(reqP, userName);
            return Json(res);
        }

        public JsonResult GetS(string OID)
        {
            return Json(boxBiz.Get(OID));
        }


        /// <summary>
        /// 判断是否需要刷新
        /// </summary>
        /// <param name="reqP"></param>
        /// <returns></returns>
        public IActionResult GetUnReadCount(DataTablesRequsetDto reqP)
        {
            var userName = LoginUserHelper.GetLoginUserName(reqP.logonid, User.Identity.Name);
            var res =  boxBiz.GetUnReadCount(reqP,userName);

            return Json(res);
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

        public JsonResult UpdateRead(string OID)
        {
            return Json(boxBiz.UpdateRead(OID));
        }

        public JsonResult testSend(string OID,int type = 1)
        {
            return Json(boxBiz.testSend(OID, type));
        }
    }
}
