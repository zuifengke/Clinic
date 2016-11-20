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
    public class ToutiaoController : Controller
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
            var result = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadPageList(title, 0, categoryCode, (page - 1) * size, size, out totalCount).ToList();

            JsonHelper json = new JsonHelper();
            string strJson = string.Empty;
            foreach (Toutiao item in result)
            {
                json.AddItem("ID", item.ID.ToString());
                json.AddItem("Title", item.Title.Replace("\"", "“").Replace("\u001d", "").Replace("\b", ""));
                json.AddItem("CategoryName", item.CategoryName);
                json.AddItem("Keywords", item.Keywords.Replace("\t", "").Replace("\n", "").Replace("\\",""));
                json.AddItem("ArticleID", item.ArticleID.ToString());
                json.AddItem("CreateTime", item.CreateTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("MemberName", item.MemberName);
                json.AddItem("ModifyTime", item.ModifyTime.ToString("yyyy-MM-dd HH:mm"));
                json.AddItem("ImagePath", item.ImagePath);
                json.AddItem("ReprintUrl", item.ReprintUrl);
                json.AddItem("ArticleID", item.ArticleID.ToString());
                json.AddItem("ViewCount", item.ViewCount.ToString());
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
            var result = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            return Json(result);
        }
        [HttpPost]
        public ActionResult ArticleBack()
        {
            int employeeID = WebCookieHelper.GetEmployeeId();
            string writeMsg = "同步成功！";
            try
            {
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                string CategoryID = Request.Form["CategoryID"] != "" ? Request.Form["CategoryID"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        var result = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                        if (result.ArticleID == 0)
                        {
                            var article = new Article();
                            if (string.IsNullOrEmpty(CategoryID))
                                article.CategoryID = result.CategoryID;
                            else
                                article.CategoryID = int.Parse(CategoryID);
                            article.Content = result.Content;
                            article.Title = result.Title;
                            article.ViewCount = result.ViewCount;
                            article.Summary = result.Summary;
                            article.ModifyTime = result.ModifyTime;
                            article.ModifyID = employeeID;
                            article.Keywords = result.Keywords;
                            article.ImagePath = result.ImagePath;
                            article.CreateID = employeeID;
                            article.CreateTime = result.CreateTime;
                            EnterRepository.GetRepositoryEnter().ArticleRepository.AddEntity(article);
                            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                            {
                                writeMsg = string.Format("同步失败");
                                break;
                            }
                            result.ArticleID = article.ID;
                            EnterRepository.GetRepositoryEnter().ToutiaoRepository.Get(m => m.ID == id);
                            EnterRepository.GetRepositoryEnter().ToutiaoRepository.EditEntity(result, new string[] { "ArticleID" });
                            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                            {
                                writeMsg = string.Format("同步失败");
                                break;
                            }
                        }
                        else
                        {
                            var article = EnterRepository.GetRepositoryEnter().ArticleRepository.LoadEntities(m => m.ID == result.ArticleID).FirstOrDefault();
                            if (article != null)
                            {
                                if (!string.IsNullOrEmpty(CategoryID))
                                    article.CategoryID = int.Parse(CategoryID);
                                article.Content = result.Content;
                                article.Title = result.Title;
                                article.ViewCount = result.ViewCount;
                                article.Summary = result.Summary;
                                article.Keywords = result.Keywords;
                                article.ModifyTime = result.ModifyTime;
                                article.ModifyID = employeeID;
                                article.ImagePath = result.ImagePath;
                                article.CreateID = employeeID;
                                article.CreateTime = result.CreateTime;
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
                                if (string.IsNullOrEmpty(CategoryID))
                                    article.CategoryID = result.CategoryID;
                                else
                                    article.CategoryID = int.Parse(CategoryID);
                                article.Content = result.Content;
                                article.Title = result.Title;
                                article.ViewCount = result.ViewCount;
                                article.Summary = result.Summary;
                                article.Keywords = result.Keywords;
                                article.ModifyTime = result.ModifyTime;
                                article.ModifyID = employeeID;
                                article.ImagePath = result.ImagePath;
                                article.CreateID = employeeID;
                                article.CreateTime = result.CreateTime;
                                EnterRepository.GetRepositoryEnter().ArticleRepository.AddEntity(article);
                                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                                {
                                    writeMsg = string.Format("同步失败");
                                    break;
                                }
                                result.ArticleID = article.ID;
                                EnterRepository.GetRepositoryEnter().ToutiaoRepository.Get(m => m.ID == id);
                                EnterRepository.GetRepositoryEnter().ToutiaoRepository.EditEntity(result, new string[] { "ArticleID" });
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
                        EnterRepository.GetRepositoryEnter().ToutiaoRepository.DeleteEntity(new Toutiao() { ID = id });
                    }
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
        [HttpPost]
        public ActionResult SpiderRelation()
        {
            string writeMsg = "抓取成功！";
            try
            {
                string strurls = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (strurls != string.Empty)
                {
                    string[] urls = strurls.Split(',');
                    foreach (var item in urls)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        bool result =  SpiderServices.SpiderRelationPageToutiao(item);
                        if (!result)
                        {
                            writeMsg = "抓取失败";
                            break;
                        }
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                writeMsg = "抓取失败";
                return Content(writeMsg);
            }
        }
    }
}
