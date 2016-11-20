using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class OrgnizationController : Controller
    {
        //
        // GET: /Admin/Menu/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryEmployee()
        {

            try
            {
                int OrgID = Request.Form["OrgID"] != "" ? int.Parse(Request.Form["OrgID"].ToString()) : 0;
                //查询所有用户信息
                //List<Employee> lstEmployee = new List<Employee>();
                //short shRet = SystemContext.Instance.EmployeeService.GetEmployeeList("", "", "", ref lstEmployee);
                var lstEmployee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities().ToList();
                //List<EmpOrg> lstEmpOrg = new List<EmpOrg>();
                //shRet = SystemContext.Instance.OrgnizationServices.GetEmpOrgByOrgID(OrgID, ref lstEmpOrg);
                var lstEmpOrg = EnterRepository.GetRepositoryEnter().EmpOrgRepository.LoadEntities(m => m.OrgID == OrgID);
                string writeMsg = "";
                foreach ( Employee item in lstEmployee)
                {
                    bool b = false;
                    foreach ( EmpOrg item1 in lstEmpOrg)
                    {
                        if (item1.EmpID == item.ID)
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b)
                        writeMsg = writeMsg + string.Format("<input type=\"checkbox\"  checked=true id=\"{0}\" class=\"empid\" />{1}&nbsp", item.ID, item.EmpNo + "|" + item.Name);
                    else
                        writeMsg = writeMsg + string.Format("<input type=\"checkbox\"  id=\"{0}\" class=\"empid\"  />{1}&nbsp", item.ID, item.EmpNo + "|" + item.Name);
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }

        public ActionResult SaveByOrgID()
        {
            try
            {
                string OrgID = Request.Form["OrgID"] != "" ? Request.Form["OrgID"].ToString() : "";
                string EmpID = Request.Form["EmpID"] != "" ? Request.Form["EmpID"].ToString() : "";
                string[] arrEmpID = EmpID.Split(',');
                string writeMsg = "保存成功！";
                if (EmpID == "")
                    writeMsg = "保存失败！";
                else
                {
                    //删除
                    EnterRepository.GetRepositoryEnter().EmpOrgRepository.DeleteByOrgId(int.Parse(OrgID));
                    foreach (string item in arrEmpID)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                         EmpOrg model = new  EmpOrg();
                        model.OrgID = int.Parse(OrgID);
                        model.EmpID = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().EmpOrgRepository.AddEntity(model);
                    }
                    int result = EnterRepository.GetRepositoryEnter().SaveChange();
                    if (result == 0)
                        writeMsg = "保存失败";
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult GetEmpName()
        {
            string result = "";
            string szOrgID = Request.Form["OrgID"] == null ? Request.QueryString["OrgID"].ToString() : Request.Form["OrgID"];
            int nOrgID = int.Parse(szOrgID);
            //List<EmpOrg> lstEmpOrg = new List<EmpOrg>();
            //short shRet = SystemContext.Instance.OrgnizationServices.GetEmpOrgByOrgID(szOrgID, ref lstEmpOrg);
            var lstEmpOrg = EnterRepository.GetRepositoryEnter().EmpOrgRepository.LoadEntities(m => m.OrgID == nOrgID).ToList();
            string szEmpIDs = string.Empty;
            if (lstEmpOrg.Count > 0)
            {
                foreach ( EmpOrg item in lstEmpOrg)
                {
                    var employee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.ID == item.EmpID).FirstOrDefault();
                    if (result != "")
                        result = "," + result;
                    if (employee == null)
                        continue;
                    result = employee.Name + result;
                }
            }
            return Content(result);
        }

        public ActionResult SaveByEmpID()
        {
            try
            {
                string OrgID = Request.Form["OrgID"] != "" ? Request.Form["OrgID"].ToString() : "";
                string EmpID = Request.Form["EmpID"] != "" ? Request.Form["EmpID"].ToString() : "";
                string[] arrOrgID = OrgID.Split(',');
                string writeMsg = "保存成功！";
                if (EmpID == "")
                    writeMsg = "保存失败！";
                else
                {
                    int nEmpID = int.Parse(EmpID);
                    EnterRepository.GetRepositoryEnter().EmpOrgRepository.DeleteByEmpId(nEmpID);
                    List<EmpOrg> lstEmpRog = new List<EmpOrg>();
                    foreach (string item in arrOrgID)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                         EmpOrg model = new  EmpOrg();
                        model.EmpID = int.Parse(EmpID);
                        model.OrgID = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().EmpOrgRepository.AddEntity(model);
                    }
                    if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                        writeMsg = "保存失败";
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult GetOrgTree()
        {
            try
            {
                var lstOrgnization = EnterRepository.GetRepositoryEnter().OrgnizationRepository.LoadEntities().ToList();
                DataTable dtOrgnization = GlobalMethods.Table.GetDataTable(lstOrgnization);
                //得到ID号
                string szCheckItems = string.Empty;
                if (Request.QueryString["EmpID"] != null && Request.QueryString["EmpID"] != "")
                {
                    string EmpID = Request.QueryString["EmpID"];
                    int nEmpID = int.Parse(EmpID);

                    var lstEmpOrg = EnterRepository.GetRepositoryEnter().EmpOrgRepository.LoadEntities(m => m.EmpID == nEmpID).ToList();
                    foreach ( EmpOrg item in lstEmpOrg)
                    {
                        szCheckItems += "," + item.OrgID;
                    }
                }
                string strJson = JsonHelper.GetTreeJsonByTable(dtOrgnization, "ID", "OrgName", "ParentID", "0", szCheckItems);
                //string strJson = "[{\"id\":\"1\",\"text\":\"hello1\",\"checked\":\"true\",\"state\":\"open\",\"children\":[{\"id\":\"2\",\"text\":\"hello2\",\"state\":\"open\"}]},{\"id\":\"1\",\"text\":\"hello1\",\"state\":\"open\",\"children\":[{\"id\":\"2\",\"text\":\"hello2\",\"state\":\"open\"}]}]";
                return Content(strJson);
            }
            catch (Exception ex)
            {

                throw;
            }

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
                //Orgnization item = new Orgnization();
                //short shRet = SystemContext.Instance.OrgnizationServices.GetOrgnizationByID(id, ref item);
                var item = EnterRepository.GetRepositoryEnter().OrgnizationRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item != null)
                {
                    if (item.ParentID!=0)
                    {
                        item.ParentName = EnterRepository.GetRepositoryEnter().OrgnizationRepository.LoadEntities(m => m.ID == item.ParentID).FirstOrDefault().OrgName;
                    }
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("OrgName", item.OrgName);
                    json.AddItem("ParentID", item.ParentID.ToString());
                    json.AddItem("ParentName", item.ParentName);
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
            try
            {
                var lstOrgnization = EnterRepository.GetRepositoryEnter().OrgnizationRepository.LoadEntities().ToList();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                foreach ( Orgnization item in lstOrgnization)
                {
                    json.AddItem("id", item.ID.ToString());
                    json.AddItem("name", item.OrgName);
                    json.AddItem("pid", item.ParentID.ToString());
                    if (item.ParentID!=0)
                    {
                        var org = lstOrgnization.Find(m => m.ID == item.ParentID);
                        if (org != null)
                            item.ParentName = org.OrgName;
                    }
                    json.AddItem("ParentName", item.ParentName);
                    json.AddItem("Description", item.Description);
                    json.ItemOk();
                }
                if (lstOrgnization.Count() > 0)
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
                 Orgnization model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (id < 1)
                    {
                        EnterRepository.GetRepositoryEnter().OrgnizationRepository.AddEntity(model);

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
                        EnterRepository.GetRepositoryEnter().OrgnizationRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().OrgnizationRepository.EditEntity(model, new string[] { "OrgName", "ParentID",  "Description" });
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

                throw;
            }
        }

        /// <summary>
        /// 取得数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private  Orgnization GetData(int id)
        {
             Orgnization model = new  Orgnization();
            if (id > 0)
            {
                model = EnterRepository.GetRepositoryEnter().OrgnizationRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            model.ParentID = Request.Form["ParentID"] != "" ? int.Parse(Request.Form["ParentID"]) : 0;
            model.ParentName = Request.Form["ParentName"] != "" ? Request.Form["ParentName"] : "";
            model.OrgName = Request.Form["OrgName"] != "" ? Request.Form["OrgName"] : "";
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

                string selectID = Request.Form["id"] != "" ? Request.Form["id"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    EnterRepository.GetRepositoryEnter().OrgnizationRepository.DeleteEntity(new  Orgnization() { ID = int.Parse(selectID) });

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

                throw;
            }
        }
        #endregion
    }
}
