using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class EmpOrgRepository : BaseRepository<Models.EmpOrg>
    {/// <summary>
     /// 根据角色Id删除管理员对应页面按钮
     /// </summary>
     /// <param name="authoryId">角色Id</param>
     /// <returns></returns>
        public void DeleteByEmpId(int empId)
        {
            var result = from empmenu in db.Set<EmpOrg>()
                         where

                                empmenu.EmpID == empId

                         select new
                         {
                             empmenu.EmpID,
                             empmenu.OrgID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new EmpOrg() { EmpID = item.EmpID, OrgID = item.OrgID });
            }
            //return db.SaveChanges() > 0;
        }
        /// <summary>
        /// 根据角色Id删除管理员对应页面按钮
        /// </summary>
        /// <param name="authoryId">角色Id</param>
        /// <returns></returns>
        public void DeleteByOrgId(int orgId)
        {
            var result = from empmenu in db.Set<EmpOrg>()
                         where

                                empmenu.OrgID == orgId

                         select new
                         {
                             empmenu.EmpID,
                             empmenu.OrgID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new EmpOrg() { EmpID = item.EmpID, OrgID = item.OrgID });
            }
            //return db.SaveChanges() > 0;
        }

       
    }
}