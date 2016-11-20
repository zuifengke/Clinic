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
    public class EmployeeController : Controller
    {
        //
        // GET: /Admin/Employee/

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
                string Tel = Request.Form["Tel"] != null ? Request.Form["Tel"] : "";
                if (page < 1) return Content("");
                
                int totalCount = 0;

                var lstEmployee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadPageList(Name,Tel,(page - 1) * size, size, out totalCount).ToList();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach (Employee item in lstEmployee)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("EmpNo", item.EmpNo);
                    json.AddItem("Pwd", item.Pwd);
                    json.AddItem("Name", item.Name);
                    json.AddItem("Tel", item.Tel);
                    item.RoleNames = EmployeeServices.GetRoleNames(item.ID);
                    json.AddItem("RoleNames", item.RoleNames);
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
                        EnterRepository.GetRepositoryEnter().EmployeeRepository.DeleteEntity(new Employee() { ID = id });
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
                Employee model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (id < 1)
                    {
                        EnterRepository.GetRepositoryEnter().EmployeeRepository.AddEntity(model);
                        
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
                        {
                            if (!string.IsNullOrEmpty(model.RoleIDs))
                            {
                                string[] roleIds = model.RoleIDs.Split(',');
                                foreach (var item in roleIds)
                                {
                                    if (item == "")
                                        continue;
                                    EmpRole empRole = new EmpRole();
                                    empRole.EmpID = model.ID;
                                    empRole.RoleID = int.Parse(item);
                                    EnterRepository.GetRepositoryEnter().EmpRoleRepository.AddEntity(empRole);
                                }
                                EnterRepository.GetRepositoryEnter().SaveChange();
                            }
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
                        EnterRepository.GetRepositoryEnter().EmployeeRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().EmployeeRepository.EditEntity(model, new string[] { "EmpNo", "Pwd", "Name", "Tel" });
                        EnterRepository.GetRepositoryEnter().EmpRoleRepository.DeleteByEmpId(model.ID);
                        
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
                        {
                            if (!string.IsNullOrEmpty(model.RoleIDs))
                            {
                                string[] roleIds = model.RoleIDs.Split(',');
                                foreach (var item in roleIds)
                                {
                                    if (item == "")
                                        continue;
                                    EmpRole empRole = new EmpRole();
                                    empRole.EmpID = model.ID;
                                    empRole.RoleID = int.Parse(item);
                                    EnterRepository.GetRepositoryEnter().EmpRoleRepository.AddEntity(empRole);
                                }
                                EnterRepository.GetRepositoryEnter().SaveChange();
                            }
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
        private Employee GetData(int id)
        {
            Employee model = new Employee();
            if (id > 0)
            {
                //SystemContext.Instance.EmployeeService.GetEmployeeByID(id.ToString(), ref model);
                model = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            //model.Name = Request.Form["ipt_Name"] != "" ? Request.Form["ipt_Name"] : "";
            //model.Url = Request.Form["ipt_Url"] != "" ? Request.Form["ipt_Url"] : "";
            model.EmpNo = Request.Form["EmpNo"] != "" ? Request.Form["EmpNo"] : "";
            model.Pwd = Request.Form["Pwd"] != "" ? Request.Form["Pwd"] : "";
            model.Name = Request.Form["Name"] != "" ? Request.Form["Name"] : "";
            model.Tel = Request.Form["Tel"] != "" ? Request.Form["Tel"] : "";
            model.RoleIDs = Request.Form["RoleIDs[]"] != "" ? Request.Form["RoleIDs[]"] : "";
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
                var item = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                item.RoleIDs = EmployeeServices.GetRoleIDs(item.ID);
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item!=null)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("EmpNo", item.EmpNo);
                    json.AddItem("Pwd", item.Pwd);
                    json.AddItem("Name", item.Name);
                    json.AddItem("Tel", item.Tel);
                    json.AddItem("RoleIDs", item.RoleIDs);
                    json.ItemOk();
                }
                strJson = json.ToEasyuiOneModelJsonString();
                // strJson = "[{\"ID\":\"81\",\"EmpNo\":\"jxdhlgljp\",\"Name\":\"hello\",\"Pwd\":\"111111\",\"Tel\":\"18720081979\"}]";
                return Content(strJson);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        #endregion

        public ActionResult QueryRoleComboJson()
        {
            try
            {

                var result = EnterRepository.GetRepositoryEnter().RoleRepository.LoadEntities().ToList();
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
