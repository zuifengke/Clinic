using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Filters;
using Windy.WebMVC.Web2.ViewsModels;
namespace Windy.WebMVC.Web2.Controllers
{
    [MyException]
    public class ArticleController : Controller
    {
        //
        // GET: /Advise/

        /// <summary>
        /// 考试咨讯
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult News(int? id)
        {
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
                ? SystemConst.CategoryCode.News : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());
            int size = 30;
            int totalCount = 0;
            pagination.Size = size;
            pagination.CurrentPageIndex = page;
            var articles = ArticleServices.GetArticles(categoryCode, order, page, size, out totalCount);
            pagination.TotalCount = totalCount;
            ViewBag.Order = order;
            ViewBag.CategoryCode = categoryCode;
            ViewBag.CategoryName = categoryName;
            ViewBag.Articles = articles;
            ViewBag.Pagination = pagination;
            return View();
        }
        /// <summary>
        /// 浏览文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            Models.Article article = new Models.Article();
            article = ArticleServices.GetArticle(id);
            article.ViewCount++;
            EnterRepository.GetRepositoryEnter().ArticleRepository.EditEntity(article, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ModifyTime > article.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ModifyTime < article.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            return View(article);
        }
    }
}
