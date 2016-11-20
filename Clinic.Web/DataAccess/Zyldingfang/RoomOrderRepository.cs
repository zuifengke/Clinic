using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class RoomOrderRepository : BaseRepository<RoomOrder>
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
        public IQueryable<dynamic> LoadPageList(int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<RoomOrder>()
                         select new
                         {
                             p.ID,
                             p.Name,
                             p.School,
                             p.ShortTel,
                             p.SubmitTime,
                             p.Telphone,
                             p.ExamType
                         };
            result = result.OrderByDescending(m => m.SubmitTime);

            rowCount = result.Count();
            return result.Skip(startNum).Take(pageSize);
        }
    }
}