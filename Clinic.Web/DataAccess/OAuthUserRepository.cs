using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class OAuthUserRepository : BaseRepository<Models.OAuthUser>
    {
        /// <summary>
        /// 加载微信授权用户列表
        /// </summary>
        /// <param name="roleId">权限Id</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<OAuthUser> LoadPageList(string Name, string Tel, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Models.OAuthUser>()
                         join a in db.Set<Member>() on new { ID = p.MemberID } equals new { ID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         select new
                         {
                             p.ID,
                             p.City,
                             p.Country,
                             p.CreateTime,
                             p.Headimgurl,
                             p.Mail,
                             p.MemberID,
                             p.NickName,
                             Name= a.RealName,
                             p.OpenID,
                             p.Pwd,
                             p.Province,
                             p.Sex,
                             Tel = a.Tel
                         };
            if (!string.IsNullOrEmpty(Tel))
            {
                result = result.Where(m => m.Tel.Contains(Tel));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.Name.Contains(Name) || m.NickName.Contains(Name));
            }
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.CreateTime).Skip(startNum).Take(pageSize);

            return result.ToList().Select(m => new OAuthUser()
            {
                ID = m.ID,
                Name = m.Name,
                City = m.City,
                Country = m.Country,
                Sex = m.Sex,
                Province = m.Province,
                Headimgurl = m.Headimgurl,
                Mail = m.Mail,
                MemberID = m.MemberID,
                NickName = m.NickName,
                OpenID = m.OpenID,
                Pwd = m.Pwd,
                Tel = m.Tel,
                CreateTime = m.CreateTime
            });
        }
    }
}