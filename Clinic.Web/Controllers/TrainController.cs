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
    public class TrainController : Controller
    {
        //
        // GET: /Advise/
        
        public ActionResult Index(int? id)
        {
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Train : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];
            
            string categoryName = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == categoryCode).FirstOrDefault().Name;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());
    
            int totalCount = 0;
            pagination.ActionUrl= "train/index";

            pagination.CurrentPageIndex = page;
            var mytrains = TrainServices.GetTrains(categoryCode, order, page, pagination.Size, out totalCount);
            pagination.TotalCount = totalCount;
            pagination.Order = order;
            pagination.CategoryCode = categoryCode;
            pagination.CategoryName = categoryName;
            ViewBag.mytrains = mytrains;
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
            var train = TrainServices.GetTrain(id);
            train.ViewCount++;
            EnterRepository.GetRepositoryEnter().TrainRepository.EditEntity(train, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().TrainRepository.LoadEntities(m => m.ModifyTime > train.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().TrainRepository.LoadEntities(m => m.ModifyTime < train.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            return View(train);
        }
        
    }
}
