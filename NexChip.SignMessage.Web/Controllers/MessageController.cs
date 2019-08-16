using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NexChip.SignMessage.Bussiness;

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


        public IActionResult MessageList(int page=1,int rows=10,string sort="",string order="asc")
        {
            return Json(biz.MessageList(page,rows,sort,order));
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