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
    public class ToutiaoController : Controller
    {
        //
        // GET: /Advise/

        public ActionResult Write(int? id)
        {
            int memberID = WebCookieHelper.GetUserId(0);
            if (memberID == 0)
                return Redirect("/member/login?returnurl=" + Request.Url);

            var item = new Models.Toutiao();
            if (id != null)
                item = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            else
                item.IsPublic = 1;
            var categorylist = EnterRepository.GetRepositoryEnter().CategoryRepository.GetCategorys("toutiao").ToList();
            ViewBag.categorylist = categorylist;
            return View(item);
        }
        public ActionResult Index(int? id)
        {
            string mine = string.IsNullOrEmpty(Request.QueryString["mine"])
               ? "false" : Request.QueryString["mine"];

            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Toutiao : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];

            string keyword = Request.QueryString["keyword"];

            int memberID = 0;
            if (mine == "true")
            {
                memberID = WebCookieHelper.GetUserId(0);
                if (memberID == 0)
                    return Redirect("/member/login?returnurl=" + Request.Url);
            }
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());

            int totalCount = 0;
            pagination.Size = 30;
            pagination.ActionUrl = "toutiao/index";

            pagination.CurrentPageIndex = page;
            DateTime startTime = DateTime.Now;
            var mytoutiaos = ToutiaoServices.GetToutiaos(keyword, memberID, categoryCode, order, page, pagination.Size, out totalCount);
            string time = (DateTime.Now - startTime).TotalSeconds.ToString();
            pagination.TotalCount = totalCount;
            pagination.Order = order;
            pagination.CategoryCode = categoryCode;
            pagination.CategoryName = categoryName;
            pagination.Keyword = keyword;
            ViewBag.mine = mine;
            ViewBag.time = time;
            ViewBag.keyword = keyword;
            ViewBag.mytoutiaos = mytoutiaos;
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
            var item = ToutiaoServices.GetToutiao(id);
            Random ran = new Random();
            int RandKey = ran.Next(1, 500);
            if (item == null)
            {
                item = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.Zhuanzai == 1).OrderBy(m => m.CreateTime).Skip(RandKey).FirstOrDefault();
            }
            item.ViewCount++;
            EnterRepository.GetRepositoryEnter().ToutiaoRepository.EditEntity(item, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ModifyTime > item.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ModifyTime < item.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            var others = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities().OrderByDescending(m => m.ModifyTime).Skip(RandKey).Take(8).ToList();
            ViewBag.others = others;
            return View(item);
        }

        /// <summary>
        /// 浏览相关文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewRelation()
        {
            string url = Request.QueryString["url"];
            if (!string.IsNullOrEmpty(url))
            {
                SpiderServices.SpiderRelationPageToutiao(url);
            }
            return RedirectToAction("index");
        }
        
    }
}
