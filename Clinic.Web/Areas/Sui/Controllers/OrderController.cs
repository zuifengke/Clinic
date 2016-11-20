﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.sui.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /sui/Order/

        public ActionResult Index()
        {
            if (!WebCookieHelper.UserCheckLogin())
                return Redirect("/weixin/account/");
            return View();
        }
        

    }
}
