using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class ExamPlaceController : Controller
    {
        //
        // GET: /Admin/ExamPlace/

        public ActionResult Index()
        {
            return View();
        }
        #region 查询数据
        /// <summary>
        /// 获取指定ID的数据
        /// </summary>
        public ActionResult QueryOneData()
        {
            try
            {
                int id = Request.Form["id"] != "" ? int.Parse(Request.Form["id"]) : 0;
                //ExamPlace item = new ExamPlace();
                //short shRet = SystemContext.Instance.ExamPlaceServices.GetExamPlaceByID(id, ref item);
                var item = EnterRepository.GetRepositoryEnter().ExamPlaceRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item != null)
                {
                    if (item.ParentID!=0)
                    {
                       
                        item.ParentName = EnterRepository.GetRepositoryEnter().ExamPlaceRepository.LoadEntities(m => m.ID == item.ParentID).FirstOrDefault().PlaceName;
                    }
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("PlaceName", item.PlaceName);
                    json.AddItem("ParentID", item.ParentID.ToString());
                    json.AddItem("ParentName", item.ParentName);
                    json.AddItem("PlaceType", item.PlaceType);
                    json.AddItem("Description", item.Description);
                    json.ItemOk();
                }
                strJson = json.ToEasyuiListJsonString();
                return Content(strJson);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }

        }
        /// <summary>
        /// 查询数据
        /// </summary>
        public ActionResult QueryData()
        {
            //List<ExamPlace> lstExamPlace = new List<ExamPlace>();
            //short shRet = SystemContext.Instance.ExamPlaceServices.GetExamPlaceList("", "", "", ref lstExamPlace);
            try
            {
                var lstExamPlace = EnterRepository.GetRepositoryEnter().ExamPlaceRepository.LoadEntities().ToList();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach ( ExamPlace item in lstExamPlace)
                {
                    if (item.ParentID!=0)
                    {
                        var parent = lstExamPlace.Find(m => m.ID == item.ParentID);
                        if (parent != null)
                            item.ParentName = parent.PlaceName;
                    }
                    json.AddItem("id", item.ID.ToString());
                    json.AddItem("name", item.PlaceName);
                    json.AddItem("pid", item.ParentID.ToString());
                    //json.AddItem("NewsContent", item.NewsContent);
                    json.AddItem("ParentName", item.ParentName);
                    json.AddItem("Description", item.Description);
                    json.ItemOk();
                }
                if (lstExamPlace.Count > 0)
                {
                    strJson = json.ToEasyuiListJsonString();
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

        #region 添加或修改提交的数据
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        public ActionResult UpdateData()
        {
            try
            {
                int id = Request.Form["id"] != "" ? Convert.ToInt32(Request.Form["id"]) : 0;
                 ExamPlace model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (id < 1)
                    {
                        EnterRepository.GetRepositoryEnter().ExamPlaceRepository.AddEntity(model);
                        if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                        {
                            writeMsg = "增加成功!";
                        }
                        else
                        {
                            writeMsg = "增加失败!";
                        }
                    }
                    else
                    {
                        //清楚context中result对象
                        EnterRepository.GetRepositoryEnter().ExamPlaceRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().ExamPlaceRepository.EditEntity(model, new string[] { "ParentID", "PlaceName", "PlaceType", "Description" });
                        if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                        {
                            writeMsg = "更新成功!";
                        }
                        else
                        {
                            writeMsg = "更新失败!";
                        }

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

        /// <summary>
        /// 取得数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private  ExamPlace GetData(int id)
        {
             ExamPlace model = new  ExamPlace();
            if (id > 0)
            {
                //SystemContext.Instance.EmployeeService.GetEmployeeByID(id.ToString(), ref model);
                model = EnterRepository.GetRepositoryEnter().ExamPlaceRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            model.ParentID = Request.Form["ParentID"] != "" ? int.Parse(Request.Form["ParentID"]) : 0;
            model.ParentName = Request.Form["ParentName"] != "" ? Request.Form["ParentName"] : "";
            model.PlaceName = Request.Form["PlaceName"] != "" ? Request.Form["PlaceName"] : "";
            model.PlaceType = Request.Form["PlaceType"] != "" ? Request.Form["PlaceType"] : "";
            model.Description = Request.Form["Description"] != "" ? Request.Form["Description"] : "";

            return model;
        }
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
                        EnterRepository.GetRepositoryEnter().ExamPlaceRepository.DeleteEntity(new  ExamPlace() { ID = id });
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
        #endregion
    }
}
