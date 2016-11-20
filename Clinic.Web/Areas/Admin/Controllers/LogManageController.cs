using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class LogManageController : Controller
    {
        //
        // GET: /Admin/LogManage/

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
            try
            {
                int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
                int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
                string Message = Request.Form["Message"];
                string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
                string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
                if (page < 1) return Content("");

                int totalCount = 0;
                var lstDemand = EnterRepository.GetRepositoryEnter().LogRepository.LoadPageList(Message, (page - 1) * size, size, out totalCount).ToList();

                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach (Log item in lstDemand)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("Thread", item.Thread);
                    json.AddItem("Level", item.Level);
                    json.AddItem("Logger", item.Logger.ToString());
                    json.AddItem("Date", item.Date.ToString());
                    json.AddItem("Message", item.Message==null?"":item.Message.Replace("\r\n", "").Replace("\\", "/"));
                    json.AddItem("Exception", item.Exception==null?"": item.Exception.Replace("\r\n", "").Replace("\r","").Replace("\\", "/"));
                    if (item.ID == 4558)
                    {
                        string s="2";
                    }
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
                // json = "{ \"rows\":[ { \"ID\":\"48\",\"NewsTitle\":\"mr\",\"NewsContent\":\"mrsoft\",\"CreateTime\":\"2013-12-23\",\"CreateUser\":\"ceshi\",\"ModifyTime\":\"2013-12-23\",\"ModifyUser\":\"ceshi\"} ],\"total\":3}";
                return Content(strJson);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        /// <summary>
        /// 删除数据
        /// </summary>
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
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().LogRepository.DeleteEntity(new Log() { ID = id });
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
                throw;
            }
        }
    }
}
