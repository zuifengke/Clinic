using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class RightServices
    {
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public static bool CheckAuthority(string rightPoint,int empid)
        {
            string key = "empid_" + empid.ToString();
            
            if (!CacheHelper.IsExistCache(key))
            {
                //获取用户及角色已授权的权限
                var result = MenuServices.GetMenuByEmpID(empid);
                CacheHelper.AddCache(key, result, 1);
              
            }
            var menus  = CacheHelper.GetCache(key) as List<Models.Menu>;
            if (menus == null)
                return false;
            if (menus.FindAll(m => m.Url == rightPoint).Count > 0)
                return true;
            return false;
        }
    }
}