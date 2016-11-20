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
    public class BlogController : Controller
    {
        //
        // GET: /Advise/

        public ActionResult Write(int? id)
        {
            int memberID = WebCookieHelper.GetUserId(0);
            if (memberID == 0)
                return Redirect("/member/login?returnurl=" + Request.Url);

            var blog = new Models.Blog();
            if (id != null)
                blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            else
                blog.IsPublic = 1;
            var categorylist = EnterRepository.GetRepositoryEnter().CategoryRepository.GetCategorys("blog").ToList();
            ViewBag.categorylist = categorylist;
            return View(blog);
        }
        public ActionResult Index(int? id)
        {
            string mine = string.IsNullOrEmpty(Request.QueryString["mine"])
               ? "false" : Request.QueryString["mine"];

            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Blog : Request.QueryString["CategoryCode"];
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
            pagination.ActionUrl = "blog/index";

            pagination.CurrentPageIndex = page;
            DateTime startTime = DateTime.Now;
            var myblogs = BlogServices.GetBlogs(keyword, memberID, categoryCode, order, page, pagination.Size, out totalCount);
            string time = (DateTime.Now - startTime).TotalSeconds.ToString();
            pagination.TotalCount = totalCount;
            pagination.Order = order;
            pagination.CategoryCode = categoryCode;
            pagination.CategoryName = categoryName;
            pagination.Keyword = keyword;
            ViewBag.mine = mine;
            ViewBag.time = time;
            ViewBag.keyword = keyword;
            ViewBag.myblogs = myblogs;
            ViewBag.Pagination = pagination;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Write(FormCollection form)
        {
            var blog = new Models.Blog();
            blog.ID = int.Parse(form["ID"]);
            blog.CategoryID = int.Parse(form["CategoryID"]);
            blog.Content = form["Content"];
            blog.IsPublic = int.Parse(form["IsPublic"]);
            blog.Keywords = form["Keywords"];
            blog.ReprintUrl = form["ReprintUrl"];
            blog.Title = form["Title"];

            blog.Zhuanzai = int.Parse(form["Zhuanzai"]);

            blog.MemberID = WebCookieHelper.GetUserId(0);
            blog.Summary = HtmlContentHelper.GetSummary(blog.Content);
            blog.ImagePath = HtmlContentHelper.GetFirstImgUrl(blog.Content);
            //创建会员信息
            if (blog.ID == 0)
            {
                blog.CreateTime = DateTime.Now;
                blog.ModifyTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().BlogRepository.AddEntity(blog);
            }
            else
            {
                blog.ModifyTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().BlogRepository.Get(m => m.ID == blog.ID);
                EnterRepository.GetRepositoryEnter().BlogRepository.EditEntity(blog
                    , new string[] { "Content", "IsPublic", "ReprintUrl", "Keywords"
                    , "ModifyTime", "Summary", "ImagePath", "CategoryID","Title","Zhuanzai" });

            }
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
            {
                return Content("error");
            }
            return Content(blog.ID.ToString());
        }
        /// <summary>
        /// 浏览文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var blog = BlogServices.GetBlog(id);
            Random ran = new Random();
            int RandKey = ran.Next(1, 500);
            if (blog == null)
            {
                blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.Zhuanzai == 1).OrderBy(m => m.CreateTime).Skip(RandKey).FirstOrDefault();
            }
            blog.ViewCount++;
            EnterRepository.GetRepositoryEnter().BlogRepository.EditEntity(blog, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ModifyTime > blog.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ModifyTime < blog.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            var others = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities().OrderByDescending(m => m.ModifyTime).Skip(RandKey).Take(8).ToList();
            ViewBag.others = others;
            return View(blog);
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
                SpiderServices.SpiderRelationPage(url);
            }
            return RedirectToAction("index");
        }

        public ActionResult Delete(int id)
        {
            string mine = string.IsNullOrEmpty(Request.QueryString["mine"])
               ? "false" : Request.QueryString["mine"];
            string pageindex = string.IsNullOrEmpty(Request.QueryString["pageindex"])
               ? "1" : Request.QueryString["pageindex"];
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Blog : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];

            var blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            EnterRepository.GetRepositoryEnter().BlogRepository.DeleteEntity(blog);
            int result = EnterRepository.GetRepositoryEnter().SaveChange();
            return Redirect("/blog/index/" + pageindex + "?mine=" + mine + "&categorycode=" + categoryCode + "&order=" + order);
        }
    }
}
