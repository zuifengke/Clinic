using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Controllers
{

    /// <summary>
    /// 抽奖
    /// </summary>
    public class LotteryController : Controller
    {
        //
        // GET: /Lottery/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LuckyRoller()
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
            var invites = EnterRepository.GetRepositoryEnter().InviteRepository.LoadEntities(m => m.InviteTel == member.Tel).ToList();
            if (invites != null&&invites.Count>0)
            {
                List<string> lstBeinvite = new List<string>();
                foreach (var item in invites)
                {
                    lstBeinvite.Add(item.BeInviteTel);
                }
                //获取邀请名额
                var result = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => lstBeinvite.Contains(m.Tel) && m.EmployeeID != 0).ToList();
                ViewBag.users = result;
            }
            var lottery = EnterRepository.GetRepositoryEnter().LotteryRepository.LoadEntities(m => m.Tel == member.Tel).FirstOrDefault();
            ViewBag.lottery = lottery;
            string szName = WebCookieHelper.GetUserInfo(1);
            GlobalMethod.log.Info(string.Format("{0}进入幸运大转盘抽奖",szName));
            return View();
        }
        [HttpPost]
        public ActionResult Save()
        {
            var prize = Request.Form["prize"];
            string tel = WebCookieHelper.GetUserInfo(2);
            string name = WebCookieHelper.GetUserInfo(1);
            
            var lottery = EnterRepository.GetRepositoryEnter().LotteryRepository.LoadEntities(m => m.Tel == tel).FirstOrDefault();
            string result = string.Empty;
            var invites = EnterRepository.GetRepositoryEnter().InviteRepository.LoadEntities(m => m.InviteTel == tel).ToList();
            if (invites == null||invites.Count<=0)
            {
                result = "您未成功邀请任何人，无法参与抽奖！";
                return Content(result);
            }
            if (invites != null && invites.Count > 0)
            {
                List<string> lstBeinvite = new List<string>();
                foreach (var item in invites)
                {
                    lstBeinvite.Add(item.BeInviteTel);
                }
                //获取邀请名额
                var users = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities(m => lstBeinvite.Contains(m.Tel) && m.EmployeeID != 0).ToList();
                if (users == null || users.Count <= 0)
                {
                    result = "您未成功邀请任何人，无法参与抽奖！";
                    return Content(result);
                }
                if (lottery != null)
                {
                    var prizes = lottery.Prize.Split('、');
                    if (users.Count <= prizes.Length)
                    {
                        result = "抽奖次数限制，本次抽奖无效！";
                        return Content(result);
                    }
                }
            }
            if (lottery == null)
            {
                lottery = new Models.Lottery();
                lottery.Tel = tel;
                lottery.Prize = prize;
                lottery.CreateTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().LotteryRepository.AddEntity(lottery);
                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                {
                    result = "抽奖结果保存失败";
                    return Content(result);
                }
            }
            else {
                
                lottery.Prize =string.IsNullOrEmpty(lottery.Prize)?prize: lottery.Prize + "、" + prize;
                lottery.CreateTime = DateTime.Now;
                EnterRepository.GetRepositoryEnter().LotteryRepository.EditEntity(lottery,new string[] { "Prize", "CreateTime" });
                if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                {
                    result = "抽奖结果保存失败";
                    return Content(result);
                }
            }
            return Content(result);
        }
    }
}
