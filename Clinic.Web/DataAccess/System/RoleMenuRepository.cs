using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class RoleMenuRepository : BaseRepository<Models.RoleMenu>
    {
        
        /// <summary>
        /// 根据角色Id删除管理员对应页面按钮
        /// </summary>
        /// <param name="authoryId">角色Id</param>
        /// <returns></returns>
        public void DeleteByRoleId(int roleId)
        {
            var result = from rolemenu in db.Set<RoleMenu>()
                         where

                                rolemenu.RoleID == roleId

                         select new
                         {
                             rolemenu.RoleID,
                             rolemenu.MenuID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new RoleMenu() { RoleID = item.RoleID, MenuID = item.MenuID });
            }
            //return db.SaveChanges() > 0;
        }
    }
}