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
    public class ProductController : Controller
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
                ? SystemConst.CategoryCode.Advert : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];
            string keyword = Request.QueryString["keyword"];
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());
            int totalCount = 0;
            pagination.CurrentPageIndex = page;
            pagination.Size = 80;
            DateTime startTime = DateTime.Now;
            var products = ProductServices.GetProducts(keyword,categoryCode, order, page, pagination.Size, out totalCount);
            pagination.TotalCount = totalCount;
            string time = (DateTime.Now - startTime).TotalSeconds.ToString();
            ViewBag.Order = order;
            ViewBag.time = time;
            ViewBag.CategoryCode = categoryCode;
            ViewBag.CategoryName = categoryName;
            ViewBag.Products = products;
            ViewBag.Pagination = pagination;
            ViewBag.keyword = keyword;
            return View();
        }
        /// <summary>
        /// 浏览文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var product = ProductServices.GetProduct(id);
            product.ViewCount++;
            EnterRepository.GetRepositoryEnter().ProductRepository.EditEntity(product, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().ProductRepository.LoadEntities(m => m.ModifyTime > product.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().ProductRepository.LoadEntities(m => m.ModifyTime < product.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            return View(product);
        }
        public ActionResult SideBox()
        {
            return PartialView("SideBox");
            //return PartialView("ViewUC", model); 
        }
    }
}
