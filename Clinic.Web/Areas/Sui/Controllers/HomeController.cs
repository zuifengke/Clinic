using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.sui.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /sui/Home/

        public ActionResult Index()
        {
            int rowCount;
            var articles = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadPageList(null,0,string.Empty,string.Empty, 1, 10, out rowCount).ToList();
            ViewBag.Articles = articles;
            return View();
        }
        [HttpPost]
        public ActionResult LoadArticle(FormCollection form)
        {
            int lastIndex = int.Parse(form["lastindex"]);
            string categoryCode = form["categorycode"];
            int rowCount = 0;
            var articles = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadPageList(null, 0,categoryCode, string.Empty, lastIndex +1, 10, out rowCount).ToList();
            return Json(new
            {
                state = "success",
                result = articles
            });
        }
        [HttpPost]
        public ActionResult GetArticle(Models.Article article)
        {

            var result = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ID == article.ID).FirstOrDefault();
            return Json(new
            {
                state = "success",
                result = result
            });
        }
    }
}
