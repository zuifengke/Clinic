using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class MenuRepository : BaseRepository<Models.Menu>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> GetPageMenuList()
        {
            var result = from p in db.Set<Menu>()
                         join a in db.Set<Menu>() on new { PageId = p.ParentID } equals new { PageId = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         select new
                         {
                             p.ID,
                             p.MenuName,
                             p.MenuType,
                             p.ParentID,
                             p.Url,
                             p.Description,
                             p.Icon,
                             ParentName = a.MenuName
                         };
            return result.ToList().Select(m => new Menu() { Description = m.Description, ID = m.ID, ParentID = m.ParentID, MenuName = m.MenuName, ParentName = m.ParentName, MenuType = m.MenuType, Icon = m.Icon, Url = m.Url });
        }

        /// <summary>
        /// 获取用户及角色权限
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> GetMenuByEmpID(int empid)
        {
            var result1 = from p in db.Set<Menu>()
                           join a in db.Set<EmpMenu>() on new { MenuID = p.ID } equals new { MenuID = a.MenuID } into a_into
                           from a in a_into.DefaultIfEmpty()
                           where a.EmpID == empid
                           select new
                           {
                               p.ID,
                               p.MenuName,
                               p.MenuType,
                               p.ParentID,
                               p.Url,
                               p.Description,
                               p.Icon
                           };
            var result2 = from p in db.Set<Menu>()
                          join a in db.Set<RoleMenu>() on new { MenuID = p.ID } equals new { MenuID = a.MenuID } into a_into
                          from a in a_into.DefaultIfEmpty()
                          join b in db.Set<EmpRole>() on new { RoleID = a.RoleID } equals new { RoleID = b.RoleID } into b_into
                          from b in b_into.DefaultIfEmpty()
                          where b.EmpID == empid
                          select new
                          {
                              p.ID,
                              p.MenuName,
                              p.MenuType,
                              p.ParentID,
                              p.Url,
                              p.Description,
                              p.Icon
                          };
            return result1.Union(result2).ToList().Select(m => new Menu() { Description = m.Description, ID = m.ID, ParentID = m.ParentID, MenuName = m.MenuName, MenuType = m.MenuType, Icon = m.Icon, Url = m.Url }).ToList();
        }
    }
}