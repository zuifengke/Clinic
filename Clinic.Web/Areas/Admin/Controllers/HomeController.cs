using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;


using Windy.WebMVC.Web2.Models;
using Windy.WebMVC.Web2.EFDao;
using Windy.WebMVC.Web2.Models;
using Windy.WebMVC.Areas.Admin.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    [Filters.MyException]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (!WebCookieHelper.EmployeeCheckLogin())
            {
                return Redirect("/Admin/Account/Login");
            }

            ViewBag.Name = WebCookieHelper.GetEmployeeInfo((int)WebCookieHelper.EmployeeInfo.Name);
            return View();
        }
        public ActionResult Static()
        {
            return View();
        }

        public ActionResult StaticUnSub()
        {
            return View();
        }
        public ActionResult StaticData()
        {
            try
            {
                string sql = "select t.count,t.name FROM (select count(users.Tel) as count, employee.Name from users , employee where users.EmployeeID = employee.ID group by employee.Name) t order by count";
                DataTable ds = null;
                ds = EFDao.EFSqlHelper.SqlQueryForDataTatable(sql);

                List<DataChat> lstDataChat = new List<DataChat>();
                if (ds != null && ds.Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        lstDataChat.Add(new DataChat(int.Parse(ds.Rows[i][0].ToString()), ds.Rows[i][1].ToString()));
                    }
                }
                return Json(lstDataChat, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public ActionResult StaticUnSubExamPlace()
        {
            string sql = "select t.count,t.name FROM (select count(users.Tel) as count, employee.`Name`from users , employee where users.EmployeeID = employee.ID and users.ExamPlace ='' group by employee.`Name`  ) t  order by count";
            DataTable ds = null;
            ds = EFDao.EFSqlHelper.SqlQueryForDataTatable( sql);

            List<DataChat> lstDataChat = new List<DataChat>();
            if (ds != null && ds.Rows.Count > 0)
            {

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstDataChat.Add(new DataChat(int.Parse(ds.Rows[i][0].ToString()), ds.Rows[i][1].ToString()));
                } 
            }
            return Json(lstDataChat, JsonRequestBehavior.AllowGet);
        }

        public ActionResult test()
        {
            return View();
        }
        public ActionResult ChangePwd()
        {
            string szPwdOld = Request.Form["pwdold"] != "" ? Request.Form["pwdold"] : "";
            string szPwdNew = Request.Form["pwdnew"] != "" ? Request.Form["pwdnew"] : "";
            string szPwdConfirm = Request.Form["pwdconfirm"] != "" ? Request.Form["pwdconfirm"] : "";
            int id = WebCookieHelper.GetEmployeeId();
            var curUser = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.ID==id).FirstOrDefault();
            if (curUser == null)
                return RedirectToAction("Login");
            string writeMsg = "更改失败！";
            if (curUser.Pwd != szPwdOld)
            {
                writeMsg = "原始密码错误，无法更改密码！";
            }
            else {
                curUser.Pwd = szPwdNew;
                EnterRepository.GetRepositoryEnter().EmployeeRepository.EditEntity(curUser, new string[] { "Pwd" });
                if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
                {
                    writeMsg = "更改成功!";
                }
                else
                {
                    writeMsg = "更改失败!";
                }
            }
            return Content(writeMsg);
        }
        // GET api/admin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        public ActionResult GetEmpMenu()
        {
          
            string strJson = string.Empty;
            try
            {
               
                if (!WebCookieHelper.EmployeeCheckLogin())
                {
                    return RedirectToAction("Admin/Account/Login");
                }
               
                int nEmpID =WebCookieHelper.GetEmployeeId();
                
                var menus = MenuServices.GetMenuByEmpID(nEmpID);
               

                if (menus.ToList().Count>0)
                {
                    strJson = JsonHelper.GetMenuJson(menus,0);
                    strJson = "{" + strJson + "}";
                }
                else
                    strJson = "\"menus\":[]";
                //string strJson = "[{\"id\":\"1\",\"text\":\"hello1\",\"checked\":\"true\",\"state\":\"open\",\"children\":[{\"id\":\"2\",\"text\":\"hello2\",\"state\":\"open\"}]},{\"id\":\"1\",\"text\":\"hello1\",\"state\":\"open\",\"children\":[{\"id\":\"2\",\"text\":\"hello2\",\"state\":\"open\"}]}]";
            }
            catch (Exception ex)
            {

                throw;
            }
            return Content(strJson);

        }

        public ActionResult TestSession()
        {
            try
            {
               
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return View();
        }

        public ActionResult TestForm()
        {
            ViewData["Name"] = Request.Form["Name"];
            ViewData["Age"] = Request.Form["Age"];
            ViewData["count"] = Request.Form.Count;
            return View();
        }

        public ActionResult TestLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["userName"] = User.Identity.Name;
                return View("Admin");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Admin()
        {
            if (User.IsInRole("Admin"))
            {
                return View("Admin");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ViewResult Details()
        {
            ViewData["PageSize"] = Request.QueryString["PageSize"];
            ViewData["CurrentPage"] = Request.QueryString["CurrentPage"];
            ViewData["count"] = Request.QueryString.Count;

            return View();
        }
        public ViewResult TestCookie()
        {
            ViewData["key"] = Request.Cookies["key"].Value;
            return View();
        }
    }
}
