using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Windy.WebMVC.Web2.Config;

namespace Windy.WebMVC.Web2.Job
{
    /// <summary>
    /// 更新主页
    /// </summary>
    public class UpdateIndexJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;

                GlobalMethod.log.Info("主页刷新-" + DateTime.Now.ToString());
                HtmlContentHelper.CreateIndex(_siteConfig);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
            //Console.WriteLine("hello1");
        }
    }
}