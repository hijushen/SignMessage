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

        public class AccessFormDto
        {
            public string strLogonId { get; set; }
            public string strEncrypt { get; set; }
        }


        private string handleOACheckMsg(string code)
        {

            //var retSHAObj = {
            //    '1': '',
            //    '0': '资料验证错误',
            //    '-1':'网页网址已经过时',
            //    '-2': '无法取得登入帐号',
            //    '-3': '身分验证错误',
            //    '-4': '身分验证错误(Hash)',
            //    '-5': '对不起！您不是合法管理人员，请由晶合Portal切换身分开启签核箱，谢谢',
            //    '-6': '对不起！您使用的浏览器为 IE edge 或 Chrome，目前不支持此浏览器，谢谢'
            //};


            switch(code)
            {
                case "0":
                    return "资料验证错误";
                case "-1":
                    return "网页网址已经过时";
                case "-2":
                    return "无法取得登入帐号";
                case "-3":
                    return "身分验证错误";
                case "-4":
                    return "身分验证错误(Hash)";
                case "-5":
                    return "对不起！您不是合法管理人员，请由晶合Portal切换身分开启签核箱，谢谢";
                default:
                    return "资料验证失败";
            }
        }


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
            
            //权限验证
            if(SettingConfig.NewOACheck == "1")
            {
                if (!string.IsNullOrWhiteSpace(logonid))
                {
                    var postPath = "/Home/PostAccessEform";
                    var postData = new AccessFormDto { strLogonId = logonid, strEncrypt = SHAEncry };
                    var res = NewOARestSharpHttp.PostJson(postPath, postData);
                    if(res != "1")
                    {
                        //string returnContent = string.Format(@"<h3 class='error-title'>表单权限验证失败</h3><h4 class='error-issue'>错误原因:{0}</h4><h4 class='error-tips'>请刷新页面重试!</h4>",handleOACheckMsg(res));
                        return RedirectToAction("ValiFailError","Home", new { ErrorMsg = handleOACheckMsg(res) });
                        //return Content(returnContent);
                    }
                }
            }


            ViewBag.WebAPIBaseUrl = Utils.SettingConfig.PostUrl;
            ViewBag.WebAPICallAuth = Utils.SettingConfig.ApiTokenString;
            ViewBag.Title = Utils.SettingConfig.NoBroGTitle;
            ViewBag.AllString = Utils.SettingConfig.AllString;
            ViewBag.UnReadRefreshSeconds = Utils.SettingConfig.UnReadRefreshSeconds;


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
