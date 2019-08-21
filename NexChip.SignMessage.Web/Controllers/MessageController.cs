using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Bussiness.Models.Dtos;

namespace NexChip.SignMessage.Web.Controllers
{
    public class MessageController : Controller
    {
        private SignMessgeBiz biz = new SignMessgeBiz();

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult Index2()
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