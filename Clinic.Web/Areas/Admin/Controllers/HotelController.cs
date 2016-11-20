using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;
using Windy.WebMVC.Web2.Filters;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    [MyException]
    public class HotelController : Controller
    {
        //
        // GET: /Admin/News/

        public ActionResult Index()
        {
            return View();
        }
        
        #region 查询数据

        /// <summary>
        /// 查询数据
        /// </summary>
        public ActionResult QueryData()
        {
            int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
            int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
            string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
            string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
            string title = Request.Form["Title"] != "" ? Request.Form["Title"] : "";
            int categoryID = Request.Form["CategoryID"] != "" ? int.Parse(Request.Form["CategoryID"]) : 0;
            if (page < 1) return Content("");
            string categoryCode = string.Empty;
            if (categoryID != 0)
                categoryCode = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.ID == categoryID).FirstOrDefault().Code;
            int totalCount = 0;
            var result = EnterRepository.GetRepositoryEnter().HotelRepository.LoadPageList(title, 0, categoryCode, (page - 1) * size, size, out totalCount).ToList();

            JsonHelper json = new JsonHelper();
            string strJson = string.Empty;
            foreach (Hotel item in result)
            {
                json.AddItem("ID", item.ID.ToString());
                json.AddItem("Title", item.Title);
                json.AddItem("CategoryName", item.CategoryName);
                json.AddItem("Keywords", item.Keywords);
                json.AddItem("ArticleID", item.ArticleID.ToString());
                json.AddItem("CreateTime", item.CreateTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("CreateName", item.CreateName);
                json.AddItem("ModifyTime", item.ModifyTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("ModifyName", item.ModifyName);
                json.AddItem("ImagePath", item.ImagePath);
                json.AddItem("ArticleID", item.ArticleID.ToString());
                json.ItemOk();
            }
            json.totlalCount = totalCount;
            if (json.totlalCount > 0)
            {
                strJson = json.ToEasyuiGridJsonString();
            }
            else
            {
                strJson = @"[]";
            }
            return Content(strJson);
        }
        #endregion

        [HttpPost]
        public ActionResult GetDetail()
        {
            int id = Request.Form["id"] != "" ? int.Parse(Request.Form["id"]) : 0;
            var result = EnterRepository.GetRepositoryEnter().HotelRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form)
        {
            string writeMsg = string.Empty;
            Hotel item = new Hotel();
            item.ID = int.Parse(Request.Form["ID"]);
            item.Title = Request.Form["Title"];
            item.Keywords = Request.Form["Keywords"];
            item.Content = form["Content"];
            item.CategoryID = int.Parse(Request.Form["CategoryID"]);
            item.ModifyID = WebCookieHelper.GetEmployeeId();
            item.ModifyTime = DateTime.Now;
            item.ImagePath = HtmlContentHelper.GetFirstImgUrl(item.Content);
            item.Summary = HtmlContentHelper.GetSummary(item.Content);
            
            EnterRepository.GetRepositoryEnter().HotelRepository.Get(m => m.ID == item.ID);
            bool result = EnterRepository.GetRepositoryEnter().HotelRepository.EditEntity(item, new string[] { "Title", "Keywords", "CategoryID", "Content", "ModifyID", "ModifyTime", "ImagePath", "Summary"});
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                writeMsg = "修改成功!";
            }
            else
            {
                writeMsg = "修改失败!";
            }
            return Content(writeMsg);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection form)
        {
            string writeMsg = string.Empty;
            Hotel item = new Hotel();
            item.Title = Request.Form["Title"];
            item.Keywords = Request.Form["Keywords"];
            item.CategoryID = int.Parse(Request.Form["CategoryID"]);
            item.Content = form["Content"];
            item.CreateID = WebCookieHelper.GetEmployeeId();
            item.CreateTime = DateTime.Now;
            item.ModifyID = WebCookieHelper.GetEmployeeId();
            item.ModifyTime = DateTime.Now;
            item.ImagePath = HtmlContentHelper.GetFirstImgUrl(item.Content);
            item.Summary = HtmlContentHelper.GetSummary(item.Content);
           
            EnterRepository.GetRepositoryEnter().HotelRepository.AddEntity(item);
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                writeMsg = "{msg:\"保存成功!\",id:" + item.ID.ToString() + "}";
            }
            else
            {
                writeMsg = "{msg:\"保存失败!\",id:0}";
            }
            return Content(writeMsg);
        }

        [HttpPost]
        public ActionResult ArticleBack()
        {
            string writeMsg = "同步成功！";
            try
            {
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        var train = EnterRepository.GetRepositoryEnter().HotelRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                        if (train.ArticleID == 0)
                        {
                            var article = new Article();
                            article.CategoryID = train.CategoryID;
                            article.Content = train.Content;
                            article.Title = train.Title;
                            article.ViewCount = train.ViewCount;
                            article.Summary = train.Summary;
                            article.ModifyTime = train.ModifyTime;
                            article.ModifyID = train.ModifyID;
                            article.Keywords = train.Keywords;
                            article.ImagePath = train.ImagePath;
                            article.CreateID = train.CreateID;
                            article.CreateTime = train.CreateTime;
                            EnterRepository.GetRepositoryEnter().ArticleRepository.AddEntity(article);
                            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                            {
                                writeMsg = string.Format("同步失败");
                                break;
                            }
                            train.ArticleID = article.ID;
                            EnterRepository.GetRepositoryEnter().HotelRepository.Get(m => m.ID == id);
                            EnterRepository.GetRepositoryEnter().HotelRepository.EditEntity(train, new string[] {"ArticleID" });
                            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                            {
                                writeMsg = string.Format("同步失败");
                                break;
                            }
                        }
                        else
                        {
                            var article = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ID == train.ArticleID).FirstOrDefault();
                            if (article != null)
                            {
                                article.CategoryID = train.CategoryID;
                                article.Content = train.Content;
                                article.Title = train.Title;
                                article.ViewCount = train.ViewCount;
                                article.Summary = train.Summary;
                                article.Keywords = train.Keywords;
                                article.ModifyTime = train.ModifyTime;
                                article.ModifyID = train.ModifyID;
                                article.ImagePath = train.ImagePath;
                                article.CreateID = train.CreateID;
                                article.CreateTime = train.CreateTime;
                                EnterRepository.GetRepositoryEnter().ArticleRepository.Get(m => m.ID == article.ID);
                                EnterRepository.GetRepositoryEnter().ArticleRepository.EditEntity(article, new string[] {
                                    "CategoryID"
                                    ,"Content"
                                    ,"Title"
                                    ,"ViewCount"
                                    ,"Summary"
                                    ,"Keywords"
                                    ,"ModifyTime"
                                    ,"ModifyID"
                                    ,"CreateID"
                                    ,"CreateTime"
                                    ,"ImagePath" });
                                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                                {
                                    writeMsg = string.Format("同步失败");
                                    break;
                                }
                            }
                            else
                            {
                                article = new Article();
                                article.CategoryID = train.CategoryID;
                                article.Content = train.Content;
                                article.Title = train.Title;
                                article.ViewCount = train.ViewCount;
                                article.Summary = train.Summary;
                                article.Keywords = train.Keywords;
                                article.ModifyTime = train.ModifyTime;
                                article.ModifyID = train.ModifyID;
                                article.ImagePath = train.ImagePath;
                                article.CreateID = train.CreateID;
                                article.CreateTime = train.CreateTime;
                                EnterRepository.GetRepositoryEnter().ArticleRepository.AddEntity(article);
                                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                                {
                                    writeMsg = string.Format("同步失败");
                                    break;
                                }
                                train.ArticleID = article.ID;
                                EnterRepository.GetRepositoryEnter().HotelRepository.Get(m => m.ID == id);
                                EnterRepository.GetRepositoryEnter().HotelRepository.EditEntity(train, new string[] { "ArticleID" });
                                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                                {
                                    writeMsg = string.Format("同步失败");
                                    break;
                                }
                            }
                        }
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                writeMsg = "同步失败";
                return Content(writeMsg);
            }
        }

        public ActionResult Delete()
        {
            string writeMsg = "删除失败！";
            try
            {
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().HotelRepository.DeleteEntity(new Hotel() { ID = id });
                    }
                    //short shRet = SystemContext.Instance.EmployeeService.Delete(selectID);
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        writeMsg = string.Format("删除成功");
                    }
                    else
                    {
                        writeMsg = "删除失败！";
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                return Content(writeMsg);
            }
        }
    }
}
