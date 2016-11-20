using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Controllers
{
    public class AdviceController : Controller
    {
        //
        // GET: /Advise/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveAdvice(FormCollection form)
        {
            Models.Advice advice = new Models.Advice();
            advice.Contact = form["Contact"];
            advice.Name = form["Name"];
            advice.Content = form["Content"].Replace("\r\n", "");
            advice.CreateTime = DateTime.Now;
            if (string.IsNullOrEmpty(advice.Content))
            {
                ViewBag.Message = "建议或留言内容不能为空";
            }
            else
            {
                EnterRepository.GetRepositoryEnter().AdviceRepository.AddEntity(advice);
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    ViewBag.Message = "感谢您的建议或留言";
                    ViewBag.Url = "/";
                }
            }
            return View();
        }
    }
}
