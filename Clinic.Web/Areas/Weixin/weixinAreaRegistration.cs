using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.weixin
{
    public class weixinAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "weixin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "weixin_default",
                "weixin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
