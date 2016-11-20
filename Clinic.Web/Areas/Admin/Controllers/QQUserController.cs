using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class QQUserController : Controller
    {
        //
        // GET: /Admin/OAuthUser/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryData()
        {
            try
            {
                int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
                int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
                string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
                string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
                string Name = Request.Form["Name"] != null ? Request.Form["Name"] : "";
                string Tel = Request.Form["Tel"] != null ? Request.Form["Tel"] : "";
                if (page < 1) return Content("");
                
                int totalCount = 0;

                var result = EnterRepository.GetRepositoryEnter().QQUserRepository.LoadPageList(Name,Tel,(page - 1) * size, size, out totalCount).ToList();
              
                return Json(new
                {
                    total = totalCount,
                    rows = result
                }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }
        
       
    }
}
