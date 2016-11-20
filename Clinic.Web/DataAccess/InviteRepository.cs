using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class InviteRepository : BaseRepository<Models.Invite>
    {
        /// <summary>
        /// 加载邀请记录文章列表
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Invite> LoadPageList(string szTel, string szName, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Invite>()
                         join a in db.Set<Users>() on new { Tel = p.InviteTel } equals new { Tel = a.Tel } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Users>() on new { Tel = p.BeInviteTel } equals new { Tel = b.Tel } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { EmployeeID = b.EmployeeID } equals new { EmployeeID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         select new
                         {
                             p.ID,
                             p.InviteTel,
                             InviteName = a.Name,
                             InviteSchool = a.School,
                             p.BeInviteTel,
                             BeInviteName = b.Name,
                             BeInviteSchool = b.School,
                             EmployeeName=c.Name,
                             p.CreateTime
                         };
            if (!string.IsNullOrEmpty(szTel))
            {
                result = result.Where(m => m.InviteTel.Contains(szTel) || m.BeInviteTel == szTel);
            }

            if (!string.IsNullOrEmpty(szName))
                result = result.Where(m => m.InviteName == szName || m.BeInviteName == szName);

            rowCount = result.Count();
            result = result.OrderByDescending(m => m.ID).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Invite()
            {
                ID = m.ID,
                BeInviteName = m.BeInviteName,
                BeInviteSchool = m.BeInviteSchool,
                BeInviteTel = m.BeInviteTel,
                InviteName = m.InviteName,
                InviteSchool = m.InviteSchool,
                InviteTel = m.InviteTel,
                EmployeeName = m.EmployeeName,
                CreateTime = m.CreateTime
            });
        }
    }
}