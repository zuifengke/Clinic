using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Filters;
using Windy.WebMVC.Web2.ViewsModels;
namespace Windy.WebMVC.Web2.Controllers
{
    [MyException]
    public class InviteController : Controller
    {
        //
        // GET: /Advise/

        public ActionResult Write(int? id)
        {
            var user = new Models.Users();
            if (id != null)
                user = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            return View(user);
        }
        public ActionResult Index()
        {
            string tel = WebCookieHelper.GetUserInfo(2);
            DateTime startTime = DateTime.Now;
            var invites = EnterRepository.GetRepositoryEnter().InviteRepository.LoadEntities(m => m.InviteTel == tel).ToList();
            var users = new List<Models.Users>();
            foreach (var item in invites)
            {
                var user = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == item.BeInviteTel).FirstOrDefault();
                if (user != null)
                    users.Add(user);
            }
            ViewBag.users = users;
            return View();
        }
        [HttpPost]
        public ActionResult UserInfo(Models.Users user)
        {
            string Tel = WebCookieHelper.GetUserInfo(2);
            var userexist = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => m.Tel == user.Tel).FirstOrDefault();
            var invite = EnterRepository.GetRepositoryEnter().InviteRepository.LoadEntities(m => m.InviteTel == Tel && m.BeInviteTel == user.Tel).FirstOrDefault();
            if (invite == null && userexist == null)
            {
                user.Pwd = SystemContext.Instance.GetPwd(Tel);
                user.CreateTime = DateTime.Now;
                //考生不存在，并且未有邀请记录，则新增考生订房记录和邀请记录
                EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(user);
                if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                {
                    invite = new Models.Invite();
                    invite.InviteTel = Tel;
                    invite.BeInviteTel = user.Tel;
                    invite.CreateTime = DateTime.Now;
                    EnterRepository.GetRepositoryEnter().InviteRepository.AddEntity(invite);
                    EnterRepository.GetRepositoryEnter().SaveChange();
                   
                }
                else
                {
                    return Content("邀请失败，创建考生预报名记录失败");
                }
            }
            else if (invite == null && userexist != null)
            {
                return Content("好友已经参加状元乐订房报名，无法重复邀请");
            }
            else if (invite != null && userexist != null)
            {
                //考生存在，并且有邀请记录，则允许修改考生订房信息
                EnterRepository.GetRepositoryEnter().UsersRepository.Get(m => m.ID == userexist.ID);
                user.ID = userexist.ID;
                EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(user, new string[] { "Name", "Gender", "School", "ExamSchool", "ExceptRoomie", "Baks" });
                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                {
                    return Content("好友订房信息修改失败");
                }
            }
            else if (invite != null && userexist == null)
            {
                user.Pwd = SystemContext.Instance.GetPwd(Tel);
                user.CreateTime = DateTime.Now;
                //考生不存在，并且未有邀请记录，则新增考生订房记录和邀请记录
                EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(user);
                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                {
                    return Content("邀请失败，创建考生预报名记录失败");
                }
            }
            GlobalMethod.log.Info(string.Format("{0}邀请{1}", WebCookieHelper.GetUserInfo(1), user.Name));
            return Content("");

        }
        /// <summary>
        /// 浏览文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var blog = BlogServices.GetBlog(id);
            Random ran = new Random();
            int RandKey = ran.Next(1, 500);
            if (blog == null)
            {
                blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.Zhuanzai == 1).OrderBy(m => m.CreateTime).Skip(RandKey).FirstOrDefault();
            }
            blog.ViewCount++;
            EnterRepository.GetRepositoryEnter().BlogRepository.EditEntity(blog, new string[] { "ViewCount" });
            EnterRepository.GetRepositoryEnter().SaveChange();
            var next = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ModifyTime > blog.ModifyTime).FirstOrDefault();
            var pre = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ModifyTime < blog.ModifyTime).OrderByDescending(m => m.ModifyTime).FirstOrDefault();
            ViewBag.Next = next;
            ViewBag.Pre = pre;
            var others = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities().OrderByDescending(m => m.ModifyTime).Skip(RandKey).Take(8).ToList();
            ViewBag.others = others;
            return View(blog);
        }


        public ActionResult Delete(int id)
        {
            string mine = string.IsNullOrEmpty(Request.QueryString["mine"])
               ? "false" : Request.QueryString["mine"];
            string pageindex = string.IsNullOrEmpty(Request.QueryString["pageindex"])
               ? "1" : Request.QueryString["pageindex"];
            string categoryCode = string.IsNullOrEmpty(Request.QueryString["CategoryCode"])
               ? SystemConst.CategoryCode.Blog : Request.QueryString["CategoryCode"];
            string order = string.IsNullOrEmpty(Request.QueryString["order"])
                ? "default" : Request.QueryString["order"];

            var blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            EnterRepository.GetRepositoryEnter().BlogRepository.DeleteEntity(blog);
            int result = EnterRepository.GetRepositoryEnter().SaveChange();
            return Redirect("/blog/index/" + pageindex + "?mine=" + mine + "&categorycode=" + categoryCode + "&order=" + order);
        }
    }
}
