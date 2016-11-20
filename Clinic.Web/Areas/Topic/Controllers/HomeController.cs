using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Filters;

namespace Windy.WebMVC.Web2.Areas.Topic.Controllers
{
    [MyException]
    public class HomeController : Controller
    {
        //
        // GET: /Topic/Default/
        [MyOutputCache]
        public ActionResult Index()
        {
            var keywords = Request.QueryString["keywords"];
            ViewBag.Title = keywords;
            return View();
        }

    }
}
