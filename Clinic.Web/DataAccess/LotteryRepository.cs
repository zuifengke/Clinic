using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class LotteryRepository : BaseRepository<Models.Lottery>
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
        public IEnumerable<Lottery> LoadPageList(string tel, string name, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Lottery>()
                         join a in db.Set<Users>() on new {  Tel = p.Tel} equals new { Tel= a.Tel} into a_into
                         from a in a_into.DefaultIfEmpty()
                         select new
                         {
                            p.ID,
                            p.Prize,
                            p.Tel,
                            Name = a.Name,
                            School=a.School,
                            p.CreateTime
                         };
            if (!string.IsNullOrEmpty(tel))
            {
                result = result.Where(m => m.Tel.Contains(tel));
            }
            if (!string.IsNullOrEmpty(name))
                result = result.Where(m => m.Name.Contains(name));
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.CreateTime).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Lottery()
            {
                ID = m.ID,
                Name = m.Name,
                Prize = m.Prize,
                Tel = m.Tel,
                School=m.School,
                CreateTime = m.CreateTime
            });
        }
    }
}