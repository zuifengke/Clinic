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
    public class ClearFileJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;

            string file = string.Format("{0}\\{1}", _siteConfig.ServerPath, "index.htm");
            if (File.Exists(file))
            {
                Utility.GlobalMethods.IO.DeleteFile(file);
                GlobalMethod.log.Info("删除病毒文件-" + file);
            }
            file = string.Format("{0}\\{1}", _siteConfig.ServerPath, "index.php");
            if (File.Exists(file))
            {
                Utility.GlobalMethods.IO.DeleteFile(file);
                GlobalMethod.log.Info("删除病毒文件-" + file);
            }
            file = string.Format("{0}\\{1}", _siteConfig.ServerPath, "index.asp");
            if (File.Exists(file))
            {
                Utility.GlobalMethods.IO.DeleteFile(file);
                GlobalMethod.log.Info("删除病毒文件-" + file);
            }
            //Console.WriteLine("hello1");
        }
    }
}