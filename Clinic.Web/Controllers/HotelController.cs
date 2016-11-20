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
    public class HotelController : Controller
    {
        //
        // GET: /Advise/
        
        public ActionResult Index(int? id)
        {
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Hotel : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];
            
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());
    
            int totalCount = 0;
            pagination.ActionUrl= "hotel/index";
            pagination.Size = 20;
            pagination.CurrentPageIndex = page;
            var myhotels = HotelServices.GetHotels(categoryCode, order, page, pagination.Size, out totalCount);
            pagination.TotalCount = totalCount;
            pagination.Order = order;
            pagination.CategoryCode = categoryCode;
            pagination.CategoryName = categoryName;
            ViewBag.myhotels = myhotels;
            ViewBag.mine = false;
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
            var hotel = HotelServices.GetHotel(id);
            hotel.ViewCount++;
            EnterRepository.GetRepositoryEnter().HotelRepository.EditEntity(hotel, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().HotelRepository.LoadEntities(m => m.ModifyTime > hotel.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().HotelRepository.LoadEntities(m => m.ModifyTime < hotel.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            return View(hotel);
        }
        
    }
}
