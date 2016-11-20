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
    public class BookController : Controller
    {
        //
        // GET: /Advise/
        
        /// <summary>
        /// 考试咨讯
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Index(int? id)
        {
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
                ? SystemConst.CategoryCode.Book : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());
            int totalCount = 0;
            pagination.CurrentPageIndex = page;
            var books = BookServices.GetBooks(categoryCode,order, page, pagination.Size, out totalCount);
            pagination.TotalCount = totalCount;
            ViewBag.Order = order;
            ViewBag.CategoryCode = categoryCode;
            ViewBag.CategoryName = categoryName;
            ViewBag.Books = books;
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
            var book = BookServices.GetBook(id);
            book.ViewCount++;
            EnterRepository.GetRepositoryEnter().BookRepository.EditEntity(book, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().BookRepository.LoadEntities(m => m.ModifyTime > book.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().BookRepository.LoadEntities(m => m.ModifyTime < book.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            return View(book);
        }
        public ActionResult SideBox()
        {
            return PartialView("SideBox");
            //return PartialView("ViewUC", model); 
        }
    }
}
