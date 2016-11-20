using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Controllers
{
    /// <summary>
    /// 考试订房
    /// </summary>
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            string Name = WebCookieHelper.GetUserInfo(1);
            //未缓存手机号
            if (string.IsNullOrEmpty(Tel))
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
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
            {
                result = new Models.Users();
                result.Tel = Tel;
                result.Name = Name;
            }
            if (result.EmployeeID != 0)
            {
                var employee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.ID == result.EmployeeID).FirstOrDefault();
                ViewBag.employee = employee;
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult UserInfo(Models.Users user)
        {
            GlobalMethod.log.Info(string.Format("{0}{1}提交报名信息", DateTime.Now.ToString(), user.Name));
            string Tel = WebCookieHelper.GetUserInfo(2);
            var u = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == user.Tel).FirstOrDefault();
            if (u == null)
            {
                user.Pwd = SystemContext.Instance.GetPwd(Tel);
                user.CreateTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(user);
            }
            else 
            {
                if (user.ID == 0)
                    user.ID = u.ID;
                EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(user, new string[] { "Name", "Gender", "School", "ExamSchool", "ExamPlace", "ExceptRoomie", "Baks" });
            }
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                return Content("");
            }
            else
                return Content("提交成功");

        }

        public ActionResult ExamPlace()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            string Name = WebCookieHelper.GetUserInfo(1);
            //未缓存手机号
            if (string.IsNullOrEmpty(Tel))
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
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
            {
                return Redirect("/users/userinfo");
            }
            return View(result);
        }
        public ActionResult Map()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ExamPlace(FormCollection fc)
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            if (string.IsNullOrEmpty(Tel))
                return Redirect("/member/login?returnurl=" + Request.Url);
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
                return Redirect("/users/userinfo");
            string ddlSchool = fc["ddlSchool"];
            string Place = fc["Place"];
            if (!string.IsNullOrEmpty(ddlSchool))
                result.ExamPlace = ddlSchool;
            else
                result.ExamPlace = Place;
            EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(result, new string[] { "ExamPlace" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                return Content("");
            }
            else
                return Content("提交失败");

        }

        /// <summary>
        /// 酒店安排房间信息
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomInfo()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            if (string.IsNullOrEmpty(Tel))
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
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
                return Redirect("/users/userinfo");
            if (!string.IsNullOrEmpty(result.Hotel)
                && !string.IsNullOrEmpty(result.Room))
            {
                var roomies = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Hotel == result.Hotel && m.Room == result.Room).ToList();
                if (roomies != null)
                    ViewData["roomies"] = roomies;
            }
            var lottery = EnterRepository.GetRepositoryEnter().LotteryRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            ViewBag.lottery = lottery;
            return View(result);
        }
        public ActionResult RoomieInfo(string id)
        {
            if (!WebCookieHelper.UserCheckLogin())
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
            if (string.IsNullOrEmpty(id))
            {
                Models.Users result = new Models.Users();
                return View(result);
            }
            int nid = int.Parse(id);
            var user = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
            return View(user);
        }
    }
}
