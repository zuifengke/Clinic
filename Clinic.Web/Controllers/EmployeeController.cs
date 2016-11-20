using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.ViewsModels;

namespace Windy.WebMVC.Web2.Controllers
{
    [Filters.MyException]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index(int? id)
        {
            if (!WebCookieHelper.EmployeeCheckLogin())
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Utility.CookieHelper.SetCookie("returnurl", Request.Url.ToString(), "", 1);
                    return Redirect("/weixin/account");
                }
                else
                    return Redirect("/member/login?returnurl=" + Request.Url);

            }
            string tel = string.Empty;
            string name = string.Empty;

            string keyword = Request.QueryString["keyword"];
            name = keyword;
            Pagination pagination = new Pagination();
            int page = 1;
            if (id != null)
                page = int.Parse(id.ToString());

            int totalCount = 0;
            pagination.Size = 30;
            pagination.ActionUrl = "employee/index";

            pagination.CurrentPageIndex = page;
            DateTime startTime = DateTime.Now;
            int szEmployeeIDs = 0;
            int empid = WebCookieHelper.GetEmployeeId();
            if (!RightServices.CheckAuthority(SystemContext.RightPoint.ViewAllUsers, empid))
            {
                szEmployeeIDs = empid;
            }
            var lstUsers = EnterRepository.GetRepositoryEnter().UsersRepository.LoadPageList(szEmployeeIDs, empid, tel, name, (page - 1) * pagination.Size, pagination.Size, out totalCount).ToList();
            string time = (DateTime.Now - startTime).TotalSeconds.ToString();
            pagination.TotalCount = totalCount;
            pagination.Order = "Name";
            pagination.Keyword = keyword;
            ViewBag.time = time;
            ViewBag.keyword = keyword;
            ViewBag.users = lstUsers;
            ViewBag.Pagination = pagination;
            return View();
        }
        public ActionResult Order()
        {
            if (!WebCookieHelper.EmployeeCheckLogin())
            {
                string userAgent = Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    Utility.CookieHelper.SetCookie("returnurl", Request.Url.ToString(), "", 1);
                    return Redirect("/weixin/account");
                }
                else
                    return Redirect("/member/login?returnurl=" + Request.Url);
            }
            var users = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.EmployeeID == 0).OrderByDescending(m => m.ID).ToList();
            ViewBag.users = users;
            return View();
        }
        public ActionResult edituser()
        {
           
            return View();
        }

    }
}
