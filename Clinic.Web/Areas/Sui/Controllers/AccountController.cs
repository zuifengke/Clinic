using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.sui.Controllers
{
    public class AccountController : Controller
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /sui/Account/

        public ActionResult Index()
        {
            if (!WebCookieHelper.UserCheckLogin())
                return Redirect("/weixin/account/");
            try
            {
                int nid = WebCookieHelper.GetUserId(0);
                //查找用户信息
                var result = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
                if (result == null)
                {
                    return Redirect("/weixin/account/");
                }
                log.Info(string.Format("用户：{0}登录个人中心", result.NickName));
                return View(result);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Redirect("/weixin/account/");
            }
        }
        public ActionResult Detail()
        {

            if (!WebCookieHelper.UserCheckLogin())
                return Redirect("/weixin/account/");
            try
            {
                int nid = WebCookieHelper.GetUserId(0);
                //查找用户信息
                var result = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
                if (result == null)
                {
                    return Redirect("/weixin/account/");
                }
                return View(result);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Redirect("/weixin/account/");
            }

        }
        public ActionResult UserInfo()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            string Name = WebCookieHelper.GetUserInfo(1);
            //未缓存手机号
            if (string.IsNullOrEmpty(Tel))
            {
                return Redirect("/sui/account/telephone");
            }
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
            {
                result = new Models.Users();
                result.Tel = Tel;
                result.Name = Name;
            }
            return View(result);
        }
        public ActionResult ExamPlace()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            string Name = WebCookieHelper.GetUserInfo(1);
            //未缓存手机号
            if (string.IsNullOrEmpty(Tel))
            {
                return Redirect("/sui/account/telephone");
            }
            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
            {
                return Redirect("/sui/account/userinfo");
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult ExamPlace(FormCollection fc)
        {
            try
            {
                string Tel = WebCookieHelper.GetUserInfo(2);
                if (string.IsNullOrEmpty(Tel))
                    return Redirect("/weixin/account/telephone");
                var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
                if (result == null)
                    return Redirect("/weixin/account/userinfo");
                string ddlSchool = fc["ddlSchool"];
                string Place = fc["Place"];
                if (!string.IsNullOrEmpty(ddlSchool))
                    result.ExamPlace = ddlSchool;
                else
                    result.ExamPlace = Place;
                EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(result, new string[] { "ExamPlace" });
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    return Json(new { validate = "true", message = "考点提交成功" });
                }
                else
                    return Json(new { validate = "false", message = "考点提交失败" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new { validate = "false", message = "考点提交失败" });
            }
        }
        [HttpPost]
        public ActionResult UserInfo(Models.Users user)
        {
            try
            {
                string Tel = WebCookieHelper.GetUserInfo(2);
                if (user.ID == 0)
                {
                    user.Pwd = SystemContext.Instance.GetPwd(Tel);
                    EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(user);
                }
                else
                {
                    EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(user, new string[] { "Name", "Gender", "School", "PayPlace", "Template", "ExamSchool", "ExamPlace", "ExceptRoomie", "Baks" });
                }
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    return Json(new { validate = "true", message = "提交成功" });
                }
                else
                    return Json(new { validate = "false", message = "提交失败" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new { validate = "false", message = "提交失败" });
            }
        }
        public ActionResult Telephone()
        {
            int nid = WebCookieHelper.GetUserId(0);
            var result = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
            if (result == null)
                return Redirect("/weixin/account/");
            //ViewData["user"] = result;
            return View(result);
        }
        [HttpPost]
        public ActionResult Telephone(FormCollection fc)
        {
            try
            {
                string id = WebCookieHelper.GetUserInfo(0);
                if (string.IsNullOrEmpty(id))
                    return Redirect("/weixin/account/");
                int nid = int.Parse(id);
                var result = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
                if (result == null)
                    return Redirect("/weixin/account/");
                result.Name = fc["Name"];
                result.Tel = fc["Tel"];
                EnterRepository.GetRepositoryEnter().OAuthUserRepository.EditEntity(result, new string[] { "Name", "Tel" });
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    log.Info(string.Format("用户:{0}绑定手机号", result.NickName));
                    WebCookieHelper.SetUserCookie(result.ID, result.Name, result.Tel, string.Empty, string.Empty, 7);
                    return Json(new { validate = "true", message = "绑定成功" });
                }
                else
                    return Json(new { validate = "false", message = "绑定失败" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new { validate = "false", message = "绑定失败" });
            }
        }
        public ActionResult GetProvinceList()
        {
            try
            {
                var lstExamPlace = ExamPlaceServices.GetExamPlaces().FindAll(m => m.PlaceType == "省份");

                return Json(lstExamPlace, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public ActionResult GetCityList(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("请不要非法方法,这是不道德的行为！");
            }

            var lstExamPlace = ExamPlaceServices.GetExamPlaces().FindAll(m => m.ParentID == id);
            return Json(lstExamPlace, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某［城市］的所有［市区］数据
        /// </summary>
        public ActionResult GetSchoolList(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("请不要非法方法,这是不道德的行为！");
            }
            var lstExamPlace = ExamPlaceServices.GetExamPlaces().FindAll(m => m.ParentID == id).OrderBy(m=>m.PlaceName);
            return Json(lstExamPlace, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 酒店安排房间信息
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomInfo()
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            if (string.IsNullOrEmpty(Tel))
                return Redirect("/sui/account/telephone");

            var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == Tel).FirstOrDefault();
            if (result == null)
                return Redirect("/sui/account/userinfo");
            if (!string.IsNullOrEmpty(result.Hotel)
                && !string.IsNullOrEmpty(result.Room))
            {
                var roomies = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Hotel == result.Hotel && m.Room == result.Room).ToList();
                if (roomies != null)
                    ViewData["roomies"] = roomies;
            }
            return View(result);
        }
        public ActionResult RoomieInfo(string id)
        {
            if (!WebCookieHelper.UserCheckLogin())
                return Redirect("/weixin/account/");
            if (string.IsNullOrEmpty(id))
            {
                Models.Users result = new Models.Users();
                return View(result);
            }
            int nid = int.Parse(id);
            var user = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == nid).FirstOrDefault();
            return View(user);
        }
        public ActionResult RemoveCookie()
        {
            WebCookieHelper.UserLoginOut();
            return Redirect("/sui/account");
        }
        public ActionResult Advice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAdvice(Models.Advice advice)
        {
            try
            {
                advice.Name = WebCookieHelper.GetUserInfo(1);
                advice.Contact = WebCookieHelper.GetUserInfo(2);
                advice.CreateTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().AdviceRepository.AddEntity(advice);
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    return Json(new { validate = "true", message = "提交成功" });
                }
                else
                    return Json(new { validate = "false", message = "提交失败" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new { validate = "false", message = "提交失败" });
            }
        }
    }
}
