﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace NexChip.SignMessage.Web.Controllers
{
    public class MessageController : Controller
    {
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