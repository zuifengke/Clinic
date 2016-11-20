using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Filters;

namespace Windy.WebMVC.Web2.Areas.weixin.Controllers
{
    /// <summary>
    /// 微信授权账号信息统一处理控制
    /// </summary>
    [MyException]
    public class AccountController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AccountController));
        //
        // GET: /weixin/Account/

        public ActionResult Index()
        {
            return Redirect("/weixin/Account/OAuth");
        }
        public ActionResult OAuth()
        {
            var redictUrl = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(
               SystemContext.Instance.WeiXinAppInfo.AppID
               , SystemContext.Instance.WeiXinAppInfo.URL + "weixin/Account/UserInfoCallback"
               , SystemContext.Instance.WeiXinAppInfo.Token
               , OAuthScope.snsapi_userinfo);
            return Redirect(redictUrl);
        }
        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != SystemContext.Instance.WeiXinAppInfo.Token)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                result = OAuthApi.GetAccessToken(SystemContext.Instance.WeiXinAppInfo.AppID, SystemContext.Instance.WeiXinAppInfo.AppSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            //Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            //Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                //将用户资料保存进自己的库
                var oauthuser = EnterRepository.GetRepositoryEnter().OAuthUserRepository.LoadEntities(m => m.OpenID == userInfo.openid).FirstOrDefault();
                //微信公开信息记录数据库
                if (oauthuser == null)
                {
                    DateTime createTime = DateTime.Now;
                    string pwd = Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd);
                    oauthuser = new Models.OAuthUser() { City = userInfo.city, Country = userInfo.country, Headimgurl = userInfo.headimgurl, NickName = userInfo.nickname, OpenID = userInfo.openid, Province = userInfo.province, Sex = userInfo.sex, CreateTime = createTime, Pwd = pwd };

                    EnterRepository.GetRepositoryEnter().OAuthUserRepository.AddEntity(oauthuser);
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        logger.Info("新增关注用户" + userInfo.nickname);
                    }
                }
                if (oauthuser.MemberID == 0)
                {
                    //绑定手机号，查找是否有相同号码的member，如果没有则创建member账号
                    return Redirect("/member/weixinregister?ID=" + oauthuser.ID.ToString());
                }
                var member = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.ID == oauthuser.MemberID).FirstOrDefault();
                if (member == null)
                {
                    GlobalMethod.log.Error("微信绑定的用户不存在了,重新绑定手机号");
                 
                    //重新绑定手机号
                    return Redirect("/member/weixinregister?ID=" + oauthuser.ID.ToString());
                }
                WebCookieHelper.SetUserCookie(member.ID, member.UserName, member.Tel, string.Empty, string.Empty, 7);
                if (Utility.CookieHelper.ExistCookie("returnurl"))
                    return Redirect(Utility.CookieHelper.GetCookie("returnurl"));
                else
                    return Redirect("/member/personalinfo");
            }
            catch (ErrorJsonResultException ex)
            {
                logger.Error(ex);
                return Content(ex.Message);
            }
        }
    }
}
