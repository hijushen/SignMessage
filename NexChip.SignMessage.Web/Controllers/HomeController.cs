using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Utils;
using NexChip.SignMessage.Web.Models;

namespace NexChip.SignMessage.Web.Controllers
{

    public class HomeController : Controller
    {
        private SignMessageBoxBiz boxBiz = new SignMessageBoxBiz();

        public IActionResult Index(string logonid, string id, string SHAEncry)
        {
            var queryString = Request.QueryString.Value;
            

            var partShowStr = Utils.SettingConfig.SignMessageBoxPartShow.Trim();
            var oldSignMessageUrl = Utils.SettingConfig.BrogSignMessageUrl;

            //return RedirectToAction("ValiFailError", new {ErrorMsg = "测试错误" });
            //Utils.LoginUserHelper.ValiResult valiLogonUser = LoginUserHelper.valiUser(logonid, SHAEncry);
            //if (!valiLogonUser.Success)
            //{
            //    return RedirectToAction("ValiFailError" , new {ErrorMsg = valiLogonUser.Msg });
            //}

            var email = LoginUserHelper.GetLoginUserName(logonid, User.Identity.Name);
            var emplyee = boxBiz.getUserInfo(email);
            if(emplyee == null)
            {
                return RedirectToAction("ValiFailError", "Home", new { ErrorMsg = "根据Email查询用户失败" });
            }

            if(checkContain(partShowStr, emplyee.dept_id))
            {
                var routeDictionary = new RouteValueDictionary { { "action", "Index2" }, { "controller", "Message" },
                    {"logonid",logonid },{ "id",id},{"SHAEncry",SHAEncry } };
                return RedirectToRoute(routeDictionary);
                    //new { controller = "Message", action = "Index2" , new string[]{ loginid, id, SHAEncry } });
            }

            return Redirect(oldSignMessageUrl + queryString);

            //return View();
        }


        /// <summary>
        /// 是否配置了特定部门代码，没配置返回true
        /// </summary>
        /// <param name="partShowStr"></param>
        /// <param name="depid"></param>
        /// <returns></returns>
        private bool checkContain(string partShowStr, string depid)
        {
            if (string.IsNullOrWhiteSpace(depid))
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(partShowStr)) //未配置返回首页
            {
                return true;
            }


            bool res = false;
            string[] lstPartShow = partShowStr.Split(",");
            foreach (var item in lstPartShow)
            {
                if (depid.Contains(item))
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ValiFailError(string ErrorMsg)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier ,ErrorMsg=ErrorMsg});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult MessageSignEmpty()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
