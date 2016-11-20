using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class DrugController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryData()
        {
            try
            {
                int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
                int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
                string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
                string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
                string Name = Request.Form["Name"] != null ? Request.Form["Name"] : "";
                if (page < 1) return Content("");
                
                int totalCount = 0;

                var result = EnterRepository.GetRepositoryEnter().DrugRepository.LoadPageList(Name,(page - 1) * size, size, out totalCount).ToList();
                return Json(new
                {
                    total = totalCount,
                    rows = result
                }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }

        #region 删除指定ID 的数据
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
                        EnterRepository.GetRepositoryEnter().DrugRepository.DeleteEntity(new Drug() { ID = id });
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

        #region 添加或修改提交的数据
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        public ActionResult UpdateData()
        {
            try
            {
                bool blResult = false;
                int id = Request.Form["id"] != "" ? Convert.ToInt32(Request.Form["id"]) : 0;
                Drug model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (id < 1)
                    {
                        EnterRepository.GetRepositoryEnter().DrugRepository.AddEntity(model);
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
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
                        EnterRepository.GetRepositoryEnter().DrugRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().DrugRepository.EditEntity(model, new string[] { "Name","PageNumber"});
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
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
        private Drug GetData(int id)
        {
            Drug model = new Drug();
            if (id > 0)
            {
                model = EnterRepository.GetRepositoryEnter().DrugRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            model.Name = Request.Form["Name"] != "" ? Request.Form["Name"] : "";
            model.PageNumber = Request.Form["PageNumber"] != "" ? int.Parse(Request.Form["PageNumber"]) : 0;

            return model;
        }
        #endregion

        #region 查询指定ID 的数据
        /// <summary>
        /// 获取指定ID的数据
        /// </summary>
        public ActionResult QueryOneData()
        {
            try
            {
                int id = Request.Form["id"] != "" ? int.Parse(Request.Form["id"]) : 0;
                var item = EnterRepository.GetRepositoryEnter().DrugRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item!=null)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("Name", item.Name);
                    json.AddItem("PageNumber", item.PageNumber.ToString());
                    json.ItemOk();
                }
                strJson = json.ToEasyuiOneModelJsonString();
                return Content(strJson);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }
        #endregion
        public ActionResult QueryDrugComboJson()
        {
            try
            {

                var result = EnterRepository.GetRepositoryEnter().DrugRepository.LoadEntities().OrderBy(m=>m.Name).ToList();
                //return Json(new
                //{
                //    result
                //}, JsonRequestBehavior.AllowGet);
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach (var item in result)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("Name", item.Name);
                    json.ItemOk();
                }

                //string strJson = "[{\"ID\":\"81\",\"Name\":\"jxdhlgljp\",\"Desc\":\"jxdhlgljp\"},{\"ID\":\"82\",\"Name\":\"hhh\",\"Desc\":\"jxdhlgljp\"}]";
                return Content(json.ToEasyuiListJsonString());
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }
    }
}
