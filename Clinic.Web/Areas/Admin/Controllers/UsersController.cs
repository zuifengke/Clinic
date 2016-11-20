using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(UsersController));
        //
        // GET: /Admin/Users/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FileUpload()
        {
            return View();
        }

        public ActionResult QueryTemplate()
        {
            JsonHelper json = new JsonHelper();
            string strJson = string.Empty;
            string[] arrTemplate = SystemContext.Template.GetArrTemplate();
            foreach (string item in arrTemplate)
            {
                json.AddItem("id", item);
                json.AddItem("text", item);
                json.ItemOk();
            }
            strJson = json.ToEasyuiListJsonString();
            return Content(strJson);
        }

        public ActionResult QueryEmployeeIDs()
        {
            try
            {
                if (!WebCookieHelper.EmployeeCheckLogin())
                {
                    return RedirectToAction("Admin/Account/Login");
                }
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                json.AddItem("id", "");
                json.AddItem("text", "所有");
                json.ItemOk();

                int empid = WebCookieHelper.GetEmployeeId();
                string Name = WebCookieHelper.GetEmployeeInfo((int)WebCookieHelper.EmployeeInfo.Name);
                if (!RightServices.CheckAuthority(SystemContext.RightPoint.ViewAllUsers, empid))
                {
                    json.AddItem("id", empid.ToString());
                    json.AddItem("text", Name);
                    json.ItemOk();
                }
                else
                {
                    var result = UsersServices.GetStaffEmployee(empid);
                    foreach (Employee item in result)
                    {
                        json.AddItem("id", item.ID.ToString());
                        json.AddItem("text", item.Name);
                        json.ItemOk();
                    }

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
        /// 移除考生
        /// </summary>
        public ActionResult Remove()
        {
            try
            {
                string writeMsg = "移除失败！";
                int id = Convert.ToInt32(Request.Form["id"]);
                var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                result.EmployeeID = 0;
                EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(result, new string[] { "EmployeeID" });
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    writeMsg = string.Format("移除成功");
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Content("移除失败！");
            }
        }

        /// <summary>
        /// 导入订房报名
        /// </summary>
        public ActionResult ImportOrder()
        {
            try
            {
                string writeMsg = "";
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                string[] ids = selectID.Split(',');
                if (ids.Count() > 0)
                {
                    int employeeid = WebCookieHelper.GetEmployeeId();
                    foreach (var item in ids)
                    {
                        int id = Convert.ToInt32(item);
                        var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                        result.EmployeeID = employeeid;
                        EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(result, new string[] { "EmployeeID" });
                    }
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        writeMsg = string.Format("导入成功");
                    }
                    else
                    {
                        writeMsg = "导入失败";
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Content("导入失败！");
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
                string[] ids = selectID.Split(',');
                if (ids.Count() > 0)
                {
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().UsersRepository.DeleteEntity(new Users() { ID = id });
                    }
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        writeMsg = string.Format("删除成功");
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
                Users model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (id < 1)
                    {
                        if (WebCookieHelper.EmployeeCheckLogin())
                        {

                            int empid = WebCookieHelper.GetEmployeeId();

                            model.EmployeeID = empid;
                            //判断是否已经有相同号码的考生
                            if (EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == model.Tel).Count() > 0)
                            {
                                writeMsg = "存在相同的电话号码无法新增，请查看考生是否在预报名列表";
                            }
                            else
                            {
                                EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(model);
                                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                                {
                                    writeMsg = "增加成功!";
                                }
                            }
                        }
                        else
                        {
                            writeMsg = "请重新登录！";

                        }

                    }
                    else
                    {
                        //EnterRepository.GetRepositoryEnter().UsersRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(model, new string[] { "Name", "School", "ExamSchool", "Sequences", "Tel", "Baks", "PayMoney", "ExamPlace", "Room", "Hotel", "HotelExpense", "MoneyBack", "Gender", "Template", "PayPlace", "ExceptRoomie", "Status" });
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
        private Users GetData(int id)
        {
            Users model = new Users();
            if (id > 0)
            {
                model = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            model.Name = Request.Form["Name"] != "" ? Request.Form["Name"] : "";
            model.School = Request.Form["School"] != "" ? Request.Form["School"] : "";
            model.ExamSchool = Request.Form["ExamSchool"] != "" ? Request.Form["ExamSchool"] : "";
            model.Sequences = Request.Form["Sequences"] != "" ? int.Parse(Request.Form["Sequences"]) : 0;
            model.Tel = Request.Form["Tel"] != "" ? Request.Form["Tel"] : "";
            model.Baks = Request.Form["Baks"] != "" ? Request.Form["Baks"] : "";
            model.Pwd = Request.Form["Pwd"] != "" ? Request.Form["Pwd"] : "111111";
            model.PayMoney = Request.Form["PayMoney"] != "" ? Request.Form["PayMoney"] : "";
            model.ExamPlace = Request.Form["ExamPlace"] != "" ? Request.Form["ExamPlace"] : "";
            model.Room = Request.Form["Room"] != "" ? Request.Form["Room"] : "";
            model.Hotel = Request.Form["Hotel"] != "" ? Request.Form["Hotel"] : "";
            model.HotelExpense = Request.Form["HotelExpense"] != "" ? Request.Form["HotelExpense"] : "";
            model.MoneyBack = Request.Form["MoneyBack"] != "" ? Request.Form["MoneyBack"] : "";
            model.Gender = Request.Form["Gender"] != "" ? Request.Form["Gender"] : "";
            model.Template = Request.Form["Template"] != "" ? Request.Form["Template"] : "";
            model.PayPlace = Request.Form["PayPlace"] != "" ? Request.Form["PayPlace"] : "";
            model.ExceptRoomie = Request.Form["ExceptRoomie"] != "" ? Request.Form["ExceptRoomie"] : "";
            model.Status = Request.Form["Status"] != "" ? Request.Form["Status"] : "";
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
                var item = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item != null)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("Name", item.Name);
                    json.AddItem("School", item.School);
                    json.AddItem("ExamSchool", item.ExamSchool);
                    json.AddItem("Sequences", item.Sequences.ToString());
                    json.AddItem("Tel", item.Tel);
                    json.AddItem("Baks", item.Baks);
                    json.AddItem("Pwd", item.Pwd);
                    json.AddItem("PayMoney", item.PayMoney);
                    json.AddItem("ExamPlace", item.ExamPlace);
                    json.AddItem("Room", item.Room);
                    json.AddItem("Hotel", item.Hotel);
                    json.AddItem("PassWord", item.Pwd);
                    json.AddItem("PayMoney", item.PayMoney);
                    json.AddItem("ExamPlace", item.ExamPlace);
                    json.AddItem("Room", item.Room);
                    json.AddItem("Hotel", item.Hotel);
                    json.AddItem("HotelExpense", item.HotelExpense);
                    json.AddItem("MoneyBack", item.MoneyBack);
                    json.AddItem("EmployeeID", item.EmployeeID.ToString());
                    json.AddItem("Gender", item.Gender);
                    json.AddItem("Template", item.Template);
                    json.AddItem("PayPlace", item.PayPlace);
                    json.AddItem("ExceptRoomie", item.ExceptRoomie);
                    json.AddItem("CreateTime", item.CreateTime==null?DateTime.Now.ToString():item.CreateTime.ToString());
                    json.AddItem("Status", item.Status);

                    json.ItemOk();
                }
                strJson = json.ToEasyuiOneModelJsonString();
                // strJson = "[{\"ID\":\"81\",\"EmpNo\":\"jxdhlgljp\",\"Name\":\"hello\",\"Pwd\":\"111111\",\"Tel\":\"18720081979\"}]";
                return Content(strJson);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }
        #endregion

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
                string Template = Request.Form["Template"] != "" ? Request.Form["Template"] : "";
                string szEmployeeIDs = !string.IsNullOrEmpty(Request.Form["EmployeeID"]) ? Request.Form["EmployeeID"] : "0";
                string Name = Request.Form["Name"] != "" ? Request.Form["Name"] : "";
                string Tel = Request.Form["Tel"] != "" ? Request.Form["Tel"] : "";
                if (page < 1) return Content("");
                if (Name == "所有")
                    Name = string.Empty;
                if (szEmployeeIDs == "所有")
                    szEmployeeIDs = "0";

                if (!WebCookieHelper.EmployeeCheckLogin())
                    return Content("");
                int empid = WebCookieHelper.GetEmployeeId();
                if (!RightServices.CheckAuthority(SystemContext.RightPoint.ViewAllUsers, empid))
                {
                    szEmployeeIDs = empid.ToString();
                }
                int rowCount = 0;
                var lstUsers = EnterRepository.GetRepositoryEnter().UsersRepository.LoadPageList(int.Parse(szEmployeeIDs), empid, Tel, Name, (page - 1) * size, size, out rowCount).ToList();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                try
                {
                    foreach (Users item in lstUsers)
                    {
                        item.Hotel = item.Hotel ==null?"":item.Hotel.Replace("\t", "");
                        item.Tel = item.Tel == null ? "" : item.Tel.Replace("\n", "");
                        item.Baks = item.Baks == null ? "" : item.Baks.Replace("\n", "");
                        json.AddItem("ID", item.ID.ToString());
                        json.AddItem("Name", item.Name);
                        json.AddItem("School", item.School);
                        json.AddItem("ExamSchool", item.ExamSchool);
                        json.AddItem("Sequences", item.Sequences.ToString());
                        json.AddItem("Tel", item.Tel);
                        json.AddItem("Baks", item.Baks);
                        json.AddItem("Pwd", item.Pwd);
                        json.AddItem("PayMoney", item.PayMoney);
                        json.AddItem("ExamPlace", item.ExamPlace);
                        json.AddItem("Hotel", item.Hotel);
                        json.AddItem("PassWord", item.Pwd);
                        json.AddItem("PayMoney", item.PayMoney);
                        json.AddItem("ExamPlace", item.ExamPlace);
                        json.AddItem("Room", item.Room);
                        json.AddItem("Hotel", item.Hotel);
                        json.AddItem("HotelExpense", item.HotelExpense);
                        json.AddItem("MoneyBack", item.MoneyBack);
                        json.AddItem("EmployeeID", item.EmployeeID.ToString());
                        json.AddItem("EmployeeName", item.EmployeeName);
                        json.AddItem("Gender", item.Gender);
                        json.AddItem("Template", item.Template);
                        json.AddItem("PayPlace", item.PayPlace);
                        json.AddItem("CreateTime", item.CreateTime==null?DateTime.Now.ToString():item.CreateTime.ToString());
                        json.AddItem("ExceptRoomie", item.ExceptRoomie);
                        json.AddItem("Status", item.Status);
                        json.AddItem("Prize", item.Prize);
                        json.ItemOk();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                json.totlalCount = rowCount;
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
                log.Error(ex);
                throw;
            }
        }
        private static string GetColumnValue(Users user, string columnName)
        {
            switch (columnName)
            {
                case "备注":
                    return user.Baks;
                case "业务员":
                    return user.EmployeeName;
                case "网报序号":
                    return user.ID.ToString();
                case "报名次序":
                    return user.Sequences.ToString();
                case "姓名":
                    return user.Name;
                case "性别":
                    return user.Gender;
                case "所在学校":
                    return user.School;
                case "报考类型":
                    return user.Template;
                case "联系方式":
                    return user.Tel;
                case "意向同住人":
                    return user.ExceptRoomie;
                case "提交考点":
                    return user.ExamPlace;
                case "已交款额":
                    return user.PayMoney;
                case "房号":
                    return user.Room;
                case "酒店":
                    return user.Hotel;
                case "酒店房价":
                    return user.HotelExpense;
                case "多退少补":
                    return user.MoneyBack;
                case "所报学校":
                    return user.ExamSchool == null ? "" : user.ExamSchool; ;
                case "网报密码":
                    return user.Pwd;
                case "状元乐抽奖福利":
                    return user.Prize==null?"":user.Prize;
                case "收缴余款所在地":
                    return user.PayPlace == null ? "" : user.PayPlace;
                default:
                    break;
            }
            return string.Empty;
        }
        public ActionResult Upload()
        {
            HttpFileCollectionBase files = Request.Files;//这里只能用<input type="file" />才能有效果,因为服务器控件是HttpInputFile类型
            string msg = string.Empty;
            string error = string.Empty;
            string imgurl = string.Empty;
            string szTemplate = Request.Form["TemplateType"] == null ? "研究生考试" : Request.Form["TemplateType"].ToString();
            if (files.Count < 0)
                return Content("");
            string res = string.Empty;
            if (files[0].FileName == "")
            {
                error = "未选择文件";
                res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
            }
            else
            {
                files[0].SaveAs(Server.MapPath(SystemContext.FilePath.Excel) + System.IO.Path.GetFileName(files[0].FileName));
                msg = " 导入成功!请关闭窗口查看导入结果";
                imgurl = "/" + files[0].FileName;

                //处理数据导入
                try
                {
                    bool result = UsersServices.Import(Server.MapPath(SystemContext.FilePath.Excel) + System.IO.Path.GetFileName(files[0].FileName), SystemContext.Template.GetTemplate(szTemplate));
                }
                catch (Exception ex)
                {

                    error = "导入失败";

                }
                res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
            }
            return Content(res);
        }
        public ActionResult Export()
        {

            string jsons = "";
            JsonHelper jsonHelper = new JsonHelper();
            var Template = Request.Form["Template"];
            var Name = Request.Form["Name"];
            var szEmployeeID = Request.Form["EmployeeID"];

            string filePath = string.Format("{0}{1}"
                , Server.MapPath(SystemContext.FilePath.Excel)
                , "test.xls");

            if (!WebCookieHelper.EmployeeCheckLogin())
                return Content("");
            int empid = WebCookieHelper.GetEmployeeId();
            if (string.IsNullOrEmpty(szEmployeeID) || szEmployeeID == "所有")
                szEmployeeID = "0";

            if (!RightServices.CheckAuthority(SystemContext.RightPoint.ViewAllUsers, empid))
            {
                szEmployeeID = empid.ToString();
            }
            try
            {
                var lstUser = EnterRepository.GetRepositoryEnter().UsersRepository.GetUsersList(int.Parse(szEmployeeID), empid, null, null);

                string templateValue = SystemContext.Template.GetTemplate(Template);
                ExcelHelper excelOpr = new ExcelHelper();
                string[] columnName = templateValue.Split(',');
                List<ArrayList> values = new List<ArrayList>();
                foreach (var user in lstUser)
                {

                    ArrayList value = new ArrayList();
                    for (int j = 0; j < columnName.Length; j++)
                    {
                        value.Add(GetColumnValue(user, columnName[j]));
                    }
                    value.Add("");
                    values.Add(value);
                }

                excelOpr.ToExcel(filePath, columnName, values);
                jsons = "[{\"success\":true,msg:\"导出成功\",filePath:\""
                    + string.Format("{0}/{1}"
                    , SystemContext.FilePath.Excel
                    , "test.xls")
                        + "\"}]";
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);

                jsons = "[{\"success\":true,msg:\"导出失败\"}]";
            }
            return Content(jsons);

        }
        #endregion

        /// <summary>
        /// 获取某［城市］的所有［市区］数据
        /// </summary>
        public ActionResult GetOrders()
        {
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.EmployeeID == 0).OrderByDescending(m => m.ID).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
