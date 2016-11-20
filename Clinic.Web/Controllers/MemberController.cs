
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Models;
namespace Windy.WebMVC.Web2.Controllers
{
    [Filters.MyException]
    public class MemberController : Controller
    {
        //
        // GET: /Member/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary> 
        /// 回调页面 
        /// </summary>
        public ActionResult QQConnect()
        {
            return View();
        }

        /// <summary> 
        /// 回调页面 
        /// </summary>
        [HttpPost]
        public ActionResult QQConnect(QQUser qquser)
        {
            var result = EnterRepository.GetRepositoryEnter().QQUserRepository.LoadEntities(m => m.OpenID == qquser.OpenID).FirstOrDefault();
            if (result == null)
            {
                result = new QQUser();
                result.OpenID = qquser.OpenID;
                result.NickName = qquser.NickName;
                result.Level = qquser.Level;
                result.Gender = qquser.Gender;
                result.Figureurl = qquser.Figureurl;
                result.CreateTime = DateTime.Now;
                result.Vip = qquser.Vip;
                EnterRepository.GetRepositoryEnter().QQUserRepository.AddEntity(result);
                EnterRepository.GetRepositoryEnter().SaveChange();
                return Json(new
                {
                    msg = "注册",
                    ID = result.ID
                });
            }
            if (result.MemberID == 0)
            {
                return Json(new
                {
                    msg = "注册",
                    ID = result.ID
                });
            }
            var member = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.ID == result.MemberID).FirstOrDefault();
            WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, "", member.Picture, 15);
            return Content("");
        }

        /// <summary>
        /// qq第一次授权绑定或注册账号
        /// </summary>
        /// <returns></returns>
        public ActionResult QQRegister(QQUser user)
        {
            ViewBag.QQUser = user;
            return View();
        }

        /// <summary>
        /// qq第一次授权绑定或注册账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QQRegister(Member member)
        {
            int qquserID = int.Parse(Request.Form["QQUserID"]);
            var result = EnterRepository.GetRepositoryEnter().QQUserRepository.LoadEntities(m => m.ID == qquserID).FirstOrDefault();
            var item = EnterRepository.GetRepositoryEnter().MemberRepository
                .LoadEntities(m => m.UserName == member.UserName || m.Tel == member.Tel || m.Mail == member.Mail).FirstOrDefault();
            if (item != null)
                return Content("用户名、邮箱或手机号已存在");
            //创建会员信息
            member.CreateTime = DateTime.Now;
            member.Sex = Request.Form["Gender"] == "男" ? 1 : 2;
            member.Password = string.IsNullOrEmpty(member.Password)? Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd): Utility.MD5Helper.MD5(member.Password);
            member.Picture = result.Figureurl;
            EnterRepository.GetRepositoryEnter().MemberRepository.AddEntity(member);
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
            {
                return Content("注册失败");
            }
            //更新qquser的memberID

            result.MemberID = member.ID;
            EnterRepository.GetRepositoryEnter().QQUserRepository.Get(m => m.ID == qquserID);
            EnterRepository.GetRepositoryEnter().QQUserRepository.EditEntity(result, new string[] { "MemberID" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
            {
                return Content("绑定失败");
            }
            WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, "", member.Picture, 15);
            return Content("");
        }
        /// <summary>
        /// qq第一次授权绑定或注册账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QQBind(Member member)
        {
            var result = EnterRepository.GetRepositoryEnter().MemberRepository
                .LoadEntities(m => m.UserName == member.UserName || m.Tel == member.UserName || m.Mail == member.UserName).FirstOrDefault();
            if (result == null)
                return Content("用户名、邮箱或手机号不存在");
            if (result.Password != Utility.MD5Helper.MD5(member.Password))
                return Content("密码错误");
            int qquserID = int.Parse(Request.Form["QQUserID"]);

            //更新qquser的memberID
            var qquser = EnterRepository.GetRepositoryEnter().QQUserRepository.LoadEntities(m => m.ID == qquserID).FirstOrDefault();
            qquser.MemberID = result.ID;
            EnterRepository.GetRepositoryEnter().QQUserRepository.Get(m => m.ID == qquserID);
            EnterRepository.GetRepositoryEnter().QQUserRepository.EditEntity(qquser, new string[] { "MemberID" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
            {
                return Content("绑定失败");
            }
            WebCookieHelper.SetUserCookie(result.ID, result.UserName, result.Tel, "", "", 15);
            return Content("");
        }
        public ActionResult Login()
        {
            var returnurl = Request.QueryString["returnurl"];
            ViewBag.returnurl = returnurl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string name = form["name"];
            string pwd = form["pwd"];
            var member = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.UserName == name
             || m.Tel == name || m.Mail == name).FirstOrDefault();
            if (member == null)
            {
                //从考试订房中获取考生资料
                var users = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == name).FirstOrDefault();
                if (users == null)
                    return Content("用户不存在");
                else
                {
                    //自动创建会员
                    member = new Member();
                    member.CreateTime = DateTime.Now;
                    member.Tel = users.Tel;
                    member.Picture = SystemContext.Instance.GetDefaultImg();
                    member.Password =string.IsNullOrEmpty(users.Pwd)? Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd): Utility.MD5Helper.MD5(member.Password);
                    member.UserName = users.Name + "_" + users.Sequences.ToString();
                    EnterRepository.GetRepositoryEnter().MemberRepository.AddEntity(member);
                    if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                    {
                        return Content("用户不存在");
                    }
                }
            }
            else if (Utility.MD5Helper.MD5(pwd) != member.Password)
                return Content("密码错误");
            WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, "", member.Picture, 15);
            string tel = WebCookieHelper.GetUserInfo(2);
            var employee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.Tel == tel).FirstOrDefault();
            if (employee != null)
            {
                WebCookieHelper.SetEmployeeCookie(employee.ID, employee.Name, employee.EmpNo, employee.Tel, 15);
            }
            return Content(string.Empty);
        }
        public ActionResult LoginOut()
        {
            WebCookieHelper.UserLoginOut();
            return Redirect("/member/login?returnurl=/member/personalinfo");
        }
        /// <summary>
        /// 前端调用判断是否会员已登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckLogin()
        {
            var username = WebCookieHelper.GetUserInfo(1);
            var picture = WebCookieHelper.GetUserInfo(4);
            if (string.IsNullOrEmpty(username))
                return Json(new
                {
                    state = "login",
                    username = "",
                    picture = ""
                }, JsonRequestBehavior.AllowGet);
            else
            {
                if (string.IsNullOrEmpty(picture))
                    picture = SystemContext.Instance.GetDefaultImg();
                return Json(new
                {
                    state = "logined",
                    username = username,
                    picture = picture
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Member member)
        {
            //验证用户名唯一
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.UserName == member.UserName).FirstOrDefault();
            if (result != null)
                return Content("已存在相同的用户名");
            //验证手机号唯一
            result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Tel == member.Tel).FirstOrDefault();
            if (result != null)
                return Content("手机号已注册");
            //验证邮箱唯一
            result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Mail == member.Mail).FirstOrDefault();
            if (result != null)
                return Content("邮箱已注册");
            //密码MD5加密,默认密码为111111
            member.Password = string.IsNullOrEmpty(member.Password) ? Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd): Utility.MD5Helper.MD5(member.Password);
            member.Picture = SystemContext.Instance.GetDefaultImg();
            member.LoginTime = DateTime.Now;
            EnterRepository.GetRepositoryEnter().MemberRepository.AddEntity(member);
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                return Content("系统错误，注册失败");
            //放到cookie里
            WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, "", "", 15);
            return Content("");
        }
        public ActionResult Reset()
        {
            return View();
        }
        public ActionResult SendMail()
        {
            string email = Request.QueryString["email"];
            string refresh_token = Guid.NewGuid().ToString();
            Utility.CacheHelper.AddCache(refresh_token, email, 2);
            var url = string.Format("http://{0}:{1}/member/resetpwd?refresh_token={2}"
              , Request.Url.Host, Request.Url.Port, refresh_token);

            var alink = string.Format("<a href='{0}'>{0}</a>", url);
            var content = string.Format(" 您好！" +
                                        "<br /> 请点击下面链接进行用户密码重置：<br />{0}(如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。该链接使用后将立即失效。)<br />" +
                                        "注意:请您在收到邮件1个小时内({1}前)使用，否则该链接将会失效。<br /><br />",
                 alink, DateTime.Now.AddHours(1));
            var res = Helpers.SendMailHelper.SendMail(email, content, "找回密码");
            ViewBag.Email = email;
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(FormCollection form)
        {
            string toMail = form["name"];
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Mail == toMail).FirstOrDefault();
            string msg = "";
            if (result == null)
                msg = "邮箱未注册";
            if (msg != string.Empty)
                return Json(new { error = true, msg = result });
            else
                return Json(new { error = false, msg = toMail });
        }
        public ActionResult ResetPwd()
        {
            string refresh_token = Request.QueryString["refresh_token"];
            var email = Utility.CacheHelper.GetCache(refresh_token);
            if (email == null)
                return Redirect("/");
            email = email.ToString();
            var member = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Mail == email).FirstOrDefault();
            return View(member);

        }
        [HttpPost]
        public ActionResult ResetPwd(Models.Member member)
        {
            member.Password = Utility.MD5Helper.MD5(member.Password);
            EnterRepository.GetRepositoryEnter().MemberRepository.Get(m => m.ID == member.ID);
            EnterRepository.GetRepositoryEnter().MemberRepository.EditEntity(member, new string[] { "Password" });
            string result = string.Empty;
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                result = "重置密码失败";
            return Content(result);

        }
        public ActionResult PersonalInfo()
        {
            int id = WebCookieHelper.GetUserId(0);
            var member = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
           
            if (member == null)
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
            var oAuthUser = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.MemberID == id).FirstOrDefault();
            int blogsum = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.MemberID == id).Count();
            int blogvideosum = 0;
            int blogpicturesum = 0;
            EnterRepository.GetRepositoryEnter().BlogRepository.GetBlogs(null, id, SystemConst.CategoryCode.BlogVideo, null,0,0, out blogvideosum);
            EnterRepository.GetRepositoryEnter().BlogRepository.GetBlogs(null, id, SystemConst.CategoryCode.BlogPicture, null, 0, 0, out blogvideosum);

            if (oAuthUser != null)
            {
                ViewBag.FigureUrl = oAuthUser.Headimgurl;
            }
            string tel = WebCookieHelper.GetUserInfo(2);
            var employee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities(m => m.Tel == tel).FirstOrDefault();
            if (employee != null)
            {
                WebCookieHelper.SetEmployeeCookie(employee.ID, employee.Name, employee.EmpNo, employee.Tel, 15);
            }
            ViewBag.employee = employee;
            ViewBag.blogsum = blogsum;
            ViewBag.blogvideosum = blogvideosum;
            ViewBag.blogpicturesum = blogpicturesum;
            GlobalMethod.log.Info(string.Format("{0}进入个人主页", member.UserName));
            return View(member);
        }
        [HttpPost]
        public ActionResult Update(Member member)
        {
            //验证用户名唯一
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.UserName == member.UserName && m.ID != member.ID).FirstOrDefault();
            if (result != null)
                return Content("已存在相同的用户名");
            //验证手机号唯一
            result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Tel == member.Tel && m.ID != member.ID).FirstOrDefault();
            if (result != null)
                return Content("手机号已注册");
            //验证邮箱唯一
            result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.Mail == member.Mail && m.ID != member.ID).FirstOrDefault();
            if (result != null)
                return Content("邮箱已注册");
            EnterRepository.GetRepositoryEnter().MemberRepository.Get(m => m.ID == member.ID);
            EnterRepository.GetRepositoryEnter().MemberRepository.EditEntity(member, new string[] { "UserName", "RealName", "Sex", "Tel", "Mail", "QQ", "Age" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                return Content("系统错误，修改失败");
            //放到cookie里
            WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, "", "", 15);
            return Content("");
        }

        [HttpPost]
        public ActionResult updatePassword()
        {

            string oldpwd = Request.Form["oldpwd"];
            string pwd = Request.Form["pwd"];
            int id = WebCookieHelper.GetUserId(0);
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            //if (result == null)
            //    return Content("用户不存在，请重新登录");
            //if (result.Password != Utility.MD5Helper.MD5(oldpwd))
            //    return Content("原始密码不对");
            result.Password =string.IsNullOrEmpty(pwd)?Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd): Utility.MD5Helper.MD5(pwd);
            EnterRepository.GetRepositoryEnter().MemberRepository.Get(m => m.ID == id);
            EnterRepository.GetRepositoryEnter().MemberRepository.EditEntity(result, new string[] { "Password" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                return Content("系统错误，修改密码失败");

            return Content("");
        }

        /// <summary>
        /// qq第一次授权绑定或注册账号
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinRegister(OAuthUser user)
        {
            ViewBag.OAuthUser = user;
            return View();
        }
        /// <summary>
        /// 微信第一次授权绑定或注册账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeixinRegister(Member member)
        {
            var result = EnterRepository.GetRepositoryEnter().MemberRepository
                .LoadEntities(m => m.Tel == member.Tel).FirstOrDefault();
            int oauthuserID = int.Parse(Request.Form["OAuthUserID"]);
            var oauthuser = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.ID == oauthuserID).FirstOrDefault();
            if (result == null)
            {
                //member不存在，创建member
                member.UserName = oauthuser.NickName;
                member.CreateTime = DateTime.Now;
                member.Password = Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd);
                EnterRepository.GetRepositoryEnter().MemberRepository.AddEntity(member);
                if (EnterRepository.GetRepositoryEnter().SaveChange() < 0)
                {
                    return Content("绑定失败");
                }
                oauthuser.MemberID = member.ID;
            }
            else
            {
                oauthuser.MemberID = result.ID;
                member.UserName = result.UserName;
            }

            //更新qquser的memberID
            EnterRepository.GetRepositoryEnter().OAuthUserRepository.Get(m => m.ID == oauthuserID);
            EnterRepository.GetRepositoryEnter().OAuthUserRepository.EditEntity(oauthuser, new string[] { "MemberID" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
            {
                return Content("绑定失败");
            }
            WebCookieHelper.SetUserCookie(oauthuser.MemberID, member.UserName, member.Tel, "", "", 15);
            return Content("");
        }
    }
}
