using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Windy.WebMVC.Web2.Config;
using Windy.WebMVC.Web2.Job;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            try
            {
                AreaRegistration.RegisterAllAreas();

                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/log4net.config")));

                System.Data.Entity.Database.SetInitializer<EFDao.ZyldingfangContext>(null);
                //获取根目录，保存到siteconfig
                SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;
                _siteConfig.ServerPath = System.Web.HttpContext.Current.Server.MapPath("~/");
                _siteConfig.CurrentConfiguration.Save();

                GlobalMethod.log.Info("任务启动：" + DateTime.Now);
                JobScheduler.Start();
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            GlobalMethod.log.Info("应用回收内存：" + DateTime.Now);
            Response.Redirect("/aspx/index.aspx");
        }
    }
}