using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class EmployeeRepository : BaseRepository<Models.Employee>
    {
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
        public IQueryable<dynamic> LoadPageList(string Name,string Tel,int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Models.Employee>()
                         select p;
            if (!string.IsNullOrEmpty(Tel))
            {
                result = result.Where(m => m.Tel.Contains(Tel));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.Name.Contains(Name));
            }
            result = result.OrderBy(m => m.EmpNo);

            rowCount = result.Count();
            return result.Skip(startNum).Take(pageSize);
        }
        /// <summary>
        /// 得到下属员工
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> GetStaffEmpployee(int empid)
        {
            var result = from p in db.Set<Employee>()
                         join a in db.Set<EmpOrg>() on new { EmpID = p.ID } equals new { EmpID = a.EmpID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<EmpOrg>() on new { OrgID = a.OrgID } equals new { OrgID = b.OrgID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         where b.EmpID==empid 
                         select new
                         {
                             p.ID,
                             p.EmpNo,
                             p.Pwd,
                             p.Tel,
                             p.Name
                         };
            return result.Distinct().ToList().Select(m => new Models.Employee() {  EmpNo = m.EmpNo, ID = m.ID,  Tel = m.Tel, Name=m.Name,  Pwd=m.Pwd,  });
        }
    }
}