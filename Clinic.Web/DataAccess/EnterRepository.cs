using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Windy.WebMVC.Web2.EFDao;

namespace Windy.WebMVC.Web2
{
    public class EnterRepository
    {
        ///// <summary>
        ///// 获取DAL入口类
        ///// </summary>
        ///// <returns></returns>
        public static RepositoryEnter GetRepositoryEnter()
        {
            RepositoryEnter _enter = CallContext.GetData("CurrentRepositoryEnter") as RepositoryEnter;
            if (_enter == null)
            {
                _enter = new RepositoryEnter();
                CallContext.SetData("CurrentRepositoryEnter", _enter);
            }
            return _enter;
        }
    }
}