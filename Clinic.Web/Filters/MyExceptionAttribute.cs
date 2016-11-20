using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Windy.WebMVC.Web2.Filters
{
    public class MyExceptionAttribute : FilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {

            if (!filterContext.ExceptionHandled ||
                filterContext.Exception is NullReferenceException)
            {
                GlobalMethod.log.Error(filterContext.HttpContext.Request.Url, filterContext.Exception);
                filterContext.Result = new RedirectResult("/error.html");
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                //filterContext.Result = new ContentResult { Content="出错啦" };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}