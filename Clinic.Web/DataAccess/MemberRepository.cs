using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class MemberRepository : BaseRepository<Models.Member>
    {

        /// <summary>
        /// 加载文章列表
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Member> LoadPageList(string tel, string name, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Member>()
                         select new
                         {
                             p.Age,
                             p.CreateTime,
                             p.Deleted,
                             p.ID,
                             p.LoginTime,
                             p.Mail,
                             p.Password,
                             p.QQ,
                             p.RealName,
                             p.Sex,
                             p.Tel,
                             p.UserName
                         };
            if (!string.IsNullOrEmpty(tel))
            {
                result = result.Where(m => m.Tel.Contains(tel));
            }
            if (!string.IsNullOrEmpty(name))
                result = result.Where(m => m.RealName.Contains(name) || m.UserName.Contains(name));
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.CreateTime).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Member()
            {
                ID = m.ID,
                Age = m.Age,
                UserName = m.UserName,
                RealName = m.RealName,
                Deleted = m.Deleted,
                LoginTime = m.LoginTime,
                Mail = m.Mail,
                Password = m.Password,
                CreateTime=m.CreateTime,
                QQ = m.QQ,
                Sex = m.Sex,
                Tel = m.Tel
            });
        }
    }
}