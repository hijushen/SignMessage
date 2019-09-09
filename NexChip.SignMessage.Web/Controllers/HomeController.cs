using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Web.Models;

namespace NexChip.SignMessage.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var s = Request.QueryString.Value;
            return RedirectToRoute(new {controller="Message",action="Index2"+s});
            //return View();
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
    }
}
