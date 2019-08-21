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
    /// <summary>
    /// 平台接口程式注册界面.
    /// </summary>
    public class InterfaceRegController : Controller
    {
        public SignMessageRoleBiz roleBiz = new SignMessageRoleBiz();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reg()
        {
            return View();
        }

        public IActionResult Reg2()
        {
            return View();
        }

        public IActionResult Register(SignMessageRoleDto dto)
        {
            return Json(roleBiz.register(dto,User));
        }

        public JsonResult List([FromHeader]SignMessageRoleDto reqP)
        {
            return Json(roleBiz.List(reqP));
        }

        public JsonResult GetS(string OID)
        {
            return Json(roleBiz.GetS(OID));
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditSave([FromForm]SignMessageRoleDto dto)
        {
            return Json(roleBiz.EditSave(dto));
        }
    }


}
