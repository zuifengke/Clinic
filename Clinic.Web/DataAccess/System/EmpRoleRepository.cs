using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class EmpRoleRepository : BaseRepository<Models.EmpRole>
    {/// <summary>
     /// 根据用户ID删除用户角色
     /// </summary>
     /// <param name="authoryId">角色Id</param>
     /// <returns></returns>
        public void DeleteByEmpId(int empId)
        {
            var result = from empmenu in db.Set<EmpRole>()
                         where

                                empmenu.EmpID == empId

                         select new
                         {
                             empmenu.EmpID,
                             empmenu.RoleID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new EmpRole() { EmpID = item.EmpID, RoleID = item.RoleID });
            }
            //return db.SaveChanges() > 0;
        }
        /// <summary>
        /// 根据角色Id删除用户角色
        /// </summary>
        /// <param name="authoryId">角色Id</param>
        /// <returns></returns>
        public void DeleteByRoleId(int roleId)
        {
            var result = from empmenu in db.Set<EmpRole>()
                         where

                                empmenu.RoleID == roleId

                         select new
                         {
                             empmenu.EmpID,
                             empmenu.RoleID
                         };

            foreach (var item in result)
            {
                DeleteEntity(new EmpRole() { EmpID = item.EmpID, RoleID = item.RoleID });
            }
            //return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 加载管理员列表
        /// </summary>
        /// <param name="roleId">权限Id</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Role> GetRolesByEmpID(int EmpID)
        {
            var result = from p in db.Set<Role>()
                         join a in db.Set<EmpRole>() on new { RoleID = p.ID } equals new { RoleID = a.RoleID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         where a.EmpID == EmpID
                         select new
                         {
                             p.ID,
                             p.Name,
                             p.Description
                         };
            

            return result.ToList().Select(m => new Role() {  Description = m.Description,ID=m.ID,Name=m.Name });
        }

    }
}