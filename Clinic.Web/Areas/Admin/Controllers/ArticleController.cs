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
    public class ArticleController : Controller
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
            string keywords = Request.Form["keywords"] != "" ? Request.Form["keywords"] : "";
            if (page < 1) return Content("");
            string categoryCode = string.Empty;
            if (categoryID != 0)
                categoryCode = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.ID == categoryID).FirstOrDefault().Code;
            int totalCount = 0;
            var result = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadPageList(title, 0, categoryCode,keywords, (page - 1) * size, size, out totalCount).ToList();

            JsonHelper json = new JsonHelper();
            string strJson = string.Empty;
            foreach (Article item in result)
            {
                json.AddItem("ID", item.ID.ToString());
                json.AddItem("Title", item.Title.Replace("\"", "“"));
                json.AddItem("CategoryName", item.CategoryName);
                json.AddItem("Keywords", item.Keywords);
                json.AddItem("CreateTime", item.CreateTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("CreateName", item.CreateName);
                json.AddItem("ModifyTime", item.ModifyTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("ModifyName", item.ModifyName);
                json.AddItem("ImagePath", item.ImagePath);
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
            var result = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form)
        {
            string writeMsg = string.Empty;
            Article item = new Article();
            item.ID = int.Parse(Request.Form["ID"]);
            item.Title = Request.Form["Title"];
            item.Content = form["Content"];
            item.Keywords = form["Keywords"];
            item.CategoryID = int.Parse(Request.Form["CategoryID"]);
            item.ModifyID = WebCookieHelper.GetEmployeeId();
            item.ModifyTime = DateTime.Now;
            item.ImagePath = HtmlContentHelper.GetFirstImgUrl(item.Content);
            item.Summary = HtmlContentHelper.GetSummary(item.Content);
            EnterRepository.GetRepositoryEnter().ArticleRepository.Get(m => m.ID == item.ID);
            bool result = EnterRepository.GetRepositoryEnter().ArticleRepository.EditEntity(item, new string[] { "Title", "CategoryID", "Content", "ModifyID", "ModifyTime", "ImagePath", "Summary", "Keywords" });
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
            Article item = new Article();
            item.Title = Request.Form["Title"];
            item.Keywords = form["Keywords"];
            item.CategoryID = int.Parse(Request.Form["CategoryID"]);
            item.Content = form["Content"];
            item.CreateID = WebCookieHelper.GetEmployeeId();
            item.CreateTime = DateTime.Now;
            item.ModifyID = WebCookieHelper.GetEmployeeId();
            item.ModifyTime = DateTime.Now;
            item.ImagePath = HtmlContentHelper.GetFirstImgUrl(item.Content);
            item.Summary = HtmlContentHelper.GetSummary(item.Content);
            EnterRepository.GetRepositoryEnter().ArticleRepository.AddEntity(item);
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
                        EnterRepository.GetRepositoryEnter().ArticleRepository.DeleteEntity(new Article() { ID = id });
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
        public ActionResult Stick()
        {
            string writeMsg = "置顶失败！";
            try
            {
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        var article = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                        article.ModifyTime = DateTime.Now;
                        EnterRepository.GetRepositoryEnter().ArticleRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().ArticleRepository.EditEntity(article, new string[] { "ModifyTime" });
                    }
                    //short shRet = SystemContext.Instance.EmployeeService.Delete(selectID);
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        writeMsg = string.Format("置顶成功");
                    }
                    else
                    {
                        writeMsg = "置顶失败！";
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
