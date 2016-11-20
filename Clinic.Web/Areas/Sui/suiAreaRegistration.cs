using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.sui
{
    public class suiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "sui";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "sui_default",
                "sui/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
