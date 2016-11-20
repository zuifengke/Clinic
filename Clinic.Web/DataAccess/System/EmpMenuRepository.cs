using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class EmpMenuRepository : BaseRepository<Models.EmpMenu>
    {
        /// <summary>
        /// 根据角色Id删除管理员对应页面按钮
        /// </summary>
        /// <param name="authoryId">角色Id</param>
        /// <returns></returns>
        public void DeleteByEmpId(int empId)
        {
            var result = from empmenu in db.Set<EmpMenu>()
                         where

                                empmenu.EmpID == empId

                         select new
                         {
                             empmenu.EmpID,
                             empmenu.MenuID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new EmpMenu() { EmpID = item.EmpID, MenuID = item.MenuID });
            }
            //return db.SaveChanges() > 0;
        }
    }
}