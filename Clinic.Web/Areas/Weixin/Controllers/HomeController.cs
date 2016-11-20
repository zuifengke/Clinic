using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Windy.WebMVC.Web2.Areas.weixin.Controllers
{

    public class HomeController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HomeController));
        //
        // GET: /weixin/Home/

        public ActionResult Index()
        {
            //判断用户是否登陆
            if(!WebCookieHelper.UserCheckLogin())
            {
                //跳转到微信授权登录界面
                Redirect("/weixin/Account");
            }
            AccessTokenContainer.Register(SystemContext.Instance.WeiXinAppInfo.AppID, SystemContext.Instance.WeiXinAppInfo.AppSecret);
            var accessToken = AccessTokenContainer.GetAccessToken(SystemContext.Instance.WeiXinAppInfo.AppID);

            var result = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(accessToken, SystemContext.Instance.WeiXinAppInfo.OpenID);

            return View(result);
        }
        //
        // GET: /weixin/OAuth/

        public ActionResult OAuth()
        {
            //ViewData["UrlUserInfo"] =Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(
            //    SystemContext.Instance.WeiXinAppInfo.AppID
            //    , SystemContext.Instance.WeiXinAppInfo.URL+"weixin/Home/UserInfoCallback"
            //    , SystemContext.Instance.WeiXinAppInfo.Token
            //    , OAuthScope.snsapi_userinfo);
            //ViewData["UrlBase"] = OAuthApi.GetAuthorizeUrl(
            //    SystemContext.Instance.WeiXinAppInfo.AppID
            //    , SystemContext.Instance.WeiXinAppInfo.URL + "weixin/Home/BaseCallback"
            //    ,SystemContext.Instance.WeiXinAppInfo.Token
            //    , OAuthScope.snsapi_base);
            var redictUrl= Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(
                SystemContext.Instance.WeiXinAppInfo.AppID
                , SystemContext.Instance.WeiXinAppInfo.URL + "weixin/Home/UserInfoCallback"
                , SystemContext.Instance.WeiXinAppInfo.Token
                , OAuthScope.snsapi_userinfo);
            return Redirect(redictUrl);
        }
        

        public ActionResult SendText()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendText(FormCollection collection)
        {
            string text = collection["text"];
            Response.Write("你发送消息" + text);
            AccessTokenContainer.Register(SystemContext.Instance.WeiXinAppInfo.AppID, SystemContext.Instance.WeiXinAppInfo.AppSecret);
            var accessToken = AccessTokenContainer.GetAccessToken(SystemContext.Instance.WeiXinAppInfo.AppID);
            try
            {
                var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(accessToken, SystemContext.Instance.WeiXinAppInfo.OpenID,text);
               
            }
            catch (ErrorJsonResultException ex)
            {
                Response.Write("发送失败");

                return View();
            }

            Response.Write("发送成功");
            return View();
        }
       

    }
}
