using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Windy.WebMVC.Web2.EFDao
{
    public class DbContextFactory
    {
        /// <summary>
        /// 获取当前线程内的数据上下文，如果当前线程内没有上下文，那么创建一个上下文，
        /// </summary>
        /// <returns>当前线程内的数据上下文</returns>
        public static DbContext GetCurrentDbContext()
        {
            DbContext currentContext = CallContext.GetData("CurrentDbContext") as DbContext;
            if (currentContext == null)
            {
#if DEBUG
                currentContext = new ZyldingfangContext("ZyldingfangContext_debug");
#else

            currentContext = new ZyldingfangContext("ZyldingfangContext");
#endif
                CallContext.SetData("CurrentDbContext", currentContext);
            }
            return currentContext;
        }
        
    }
}