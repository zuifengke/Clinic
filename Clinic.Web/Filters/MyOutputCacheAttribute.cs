using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Filters
{
    public class MyOutputCacheAttribute : OutputCacheAttribute
    {
        public MyOutputCacheAttribute()
        {
#if DEBUG
            this.Duration = int.Parse(ConfigurationManager.AppSettings["cache.page.time.debug"]);

#else
            this.Duration = int.Parse(ConfigurationManager.AppSettings["cache.page.time"]);
#endif
        }
    }
}