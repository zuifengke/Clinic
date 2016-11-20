using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TopSDK()
        {
            Helpers.TopSDKHelper.GetTbkItemGetResponse("女装");
            return View();
        }
        public ActionResult UpdateProduct()
        {
            ProductServices.ImportByTopApi();
            return Redirect("/Product/Index");
        }
    }
}
