using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Areas.Topic
{
    public class TopicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Topic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            

            context.MapRoute(
                "Topic_default",
                "Topic/{controller}/{action}/{id}",
                new
                {
                    controller = "Home",
                    action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
