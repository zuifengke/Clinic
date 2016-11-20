using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Areas.Admin.Models;
using Windy.WebMVC.Web2.Config;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /Admin/Config/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 站点设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteConfig()
        {
            SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;
            return View(_siteConfig);
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SiteConfig(FormCollection form)
        {
            SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;
            if (TryUpdateModel<SiteConfig>(_siteConfig))
            {
                _siteConfig.CurrentConfiguration.Save();
                return View("Prompt", new Prompt() { Title = "修改成功", Message = "成功修改了网站设置", Buttons = new List<string> { "<a href='" + Url.Action("SiteConfig") + "' class='btn btn-default'>返回</a>" } });
            }
            else return View(_siteConfig);
        }
    }
}
