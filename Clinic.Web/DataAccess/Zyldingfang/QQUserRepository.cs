using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class QQUserRepository : BaseRepository<Models.QQUser>
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
        public IQueryable<dynamic> LoadPageList(string Name, string Tel, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Models.QQUser>()
                         select p;
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.NickName.Contains(Name) || m.NickName.Contains(Name));
            }
            result = result.OrderByDescending(m => m.CreateTime);

            rowCount = result.Count();
            return result.Skip(startNum).Take(pageSize);
        }
    }
}