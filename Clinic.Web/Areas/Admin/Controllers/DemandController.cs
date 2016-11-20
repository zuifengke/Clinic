using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Models;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class DemandController : Controller
    {
        //
        // GET: /Admin/Demand/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DemandEdit()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in SystemContext.ProductName.GetProductNames())
	        {
                items.Add(new SelectListItem() { Text = item, Value = item, Selected = false });
	        }
            ViewData["Product"] = items;
            ViewData["Creater"] = items;
            ViewData["Owner"] = items;
            return View();
        }
        public ActionResult DemandEdit(Demand demand)
        {
            string product= demand.Product;
            return View();
        }
        public ActionResult DelData()
        {
            try
            {
                string writeMsg = "删除失败！";

                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().DemandRepository.DeleteEntity(new Demand() { ID = id });
                    }
                    //short shRet = SystemContext.Instance.DemandService.Delete(selectID);

                    if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
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
                
                throw;
            }
        }
        #region 查询数据

        /// <summary>
        /// 查询数据
        /// </summary>
        public ActionResult QueryData()
        {
            try
            {
                int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
                int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
                string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
                string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
                if (page < 1) return Content("");

                //List<Demand> lstDemand = new List<Demand>();
                //short shRet = SystemContext.Instance.DemandService.GetDemandPageList(size, page, "", ref lstDemand);
                var query = EnterRepository.GetRepositoryEnter().DemandRepository.LoadEntities().OrderByDescending(m => m.SubmitTime);

                int totalCount = 0;
                totalCount = query.Count();
                var lstDemand = EnterRepository.GetRepositoryEnter().DemandRepository.LoadPageEnties(query, (page - 1) * size, size).ToList();

                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach (Demand item in lstDemand)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("Product", item.Product);
                    json.AddItem("Version", item.Version);
                    //json.AddItem("NewsContent", item.NewsContent);
                    json.AddItem("Creater", item.Creater.ToString());
                    json.AddItem("SubmitTime", item.SubmitTime.ToString());
                    json.AddItem("Owener", item.Owener);
                    json.AddItem("SoluteTime", item.SoluteTime.ToString());
                    json.AddItem("State", item.State);
                    json.AddItem("Title", item.Title);
                    json.AddItem("Expense", item.Expense);
                    //json.AddItem("Solution", item.Solution);
                    json.ItemOk();
                }
                //shRet = SystemContext.Instance.DemandService.GetDemandTotalCount("", ref totalCount);
                json.totlalCount = totalCount;
                if (json.totlalCount > 0)
                {
                    strJson = json.ToEasyuiGridJsonString();
                }
                else
                {
                    strJson = @"[]";
                }
                // json = "{ \"rows\":[ { \"ID\":\"48\",\"NewsTitle\":\"mr\",\"NewsContent\":\"mrsoft\",\"CreateTime\":\"2013-12-23\",\"CreateUser\":\"ceshi\",\"ModifyTime\":\"2013-12-23\",\"ModifyUser\":\"ceshi\"} ],\"total\":3}";
                return Content(strJson);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
