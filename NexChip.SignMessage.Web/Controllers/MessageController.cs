using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Utils;

namespace NexChip.SignMessage.Web.Controllers
{
    public class MessageController : Controller
    {
        private SignMessgeBiz biz = new SignMessgeBiz();
        private SignMessageBoxBiz boxBiz = new SignMessageBoxBiz();

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Index1()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logonid">sanzhang</param>
        /// <param name="id">E01000</param>
        /// <param name="SHAEncry"></param>
        /// <returns></returns>
        public IActionResult Index2(string logonid,string id, string SHAEncry)
        {
            ViewBag.logonid = logonid;
            ViewBag.id = id;
            ViewBag.SHAEncry = SHAEncry;

            
            ViewBag.NoBroGTitle = Utils.SettingConfig.NoBroGTitle;

            //Email 
            var email = LoginUserHelper.GetLoginUserName(logonid, User.Identity.Name);
            var emplyee = boxBiz.getUserInfo(email);
            if (emplyee == null)
            {
                return RedirectToAction("ValiFailError", "Home", new { ErrorMsg = "根据Email查询用户失败" });
            }

            ViewBag.UserName = emplyee.cname;

            ViewBag.UnReadRefreshSeconds = Utils.SettingConfig.UnReadRefreshSeconds;
            ViewBag.BrogSignMessageUrl = Utils.SettingConfig.BrogSignMessageUrl;
            //return View("ValiFailError");
            return View();
        }


        public IActionResult SystemHelper()
        {
            return View();
        }

        public IActionResult MessageSignView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MessageList([FromHeader]SignMessageBoxDto msg)
        {
            return Json(biz.MessageList(msg.offset, msg.limit));
        }


        [HttpGet]
        public IActionResult GetGridJSON(
            int page = 1,
            int rows = 10,
            string sort = "CustomerID",
            string order = "asc")
        {
            return Json(null);
            //JObject jo = new JObject();
            //jo.Add("total", service.TotalCount());
            //jo.Add("rows", service.GetJsonForGrid(page, rows, sort, order));

            //return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


    }
}