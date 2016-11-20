
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Windy.WebMVC.Web2
{
    public static class GlobalMethod
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly log4net.ILog menulog = log4net.LogManager.GetLogger("menulog");
    }
}