using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class ArticleRepository : BaseRepository<Models.Article>
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
        public IEnumerable<Article> LoadPageList(string title, int categoryID,string categoryCode,string keywords, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Article>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Employee>() on new { CreateID = p.CreateID } equals new { CreateID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { ModifyID = p.ModifyID } equals new { ModifyID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         select new
                         {
                             p.CategoryID,
                             CreateName = b.Name,
                             p.Content,
                             p.CreateID,
                             CategoryName = a.Name,
                             p.CreateTime,
                             p.ID,
                             p.ModifyID,
                             ModifyName = c.Name,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             CategoryCode = a.Code,
                             Keywords=p.Keywords

                         };
            if (!string.IsNullOrEmpty(title))
            {
                result = result.Where(m => m.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(keywords))
                result = result.Where(m => m.Keywords.Contains(keywords));
            if (categoryID != 0)
                result = result.Where(m => m.CategoryID == categoryID);
            if (!string.IsNullOrEmpty(categoryCode))
            {
                result = result.Where(m => m.CategoryCode.Contains(categoryCode));
            }
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.ModifyTime).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Article()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName,
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content,
                CreateID = m.CreateID,
                CreateName = m.CreateName,
                CreateTime = m.CreateTime,
                ModifyID = m.ModifyID,
                ModifyName = m.ModifyName,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                CategoryCode = m.CategoryCode,
                Keywords=m.Keywords
            });
        }

        /// <summary>
        /// 加载文章列表
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public Article GetArticle(int id)
        {
            var result = from p in db.Set<Article>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Employee>() on new { CreateID = p.CreateID } equals new { CreateID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { ModifyID = p.ModifyID } equals new { ModifyID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         select new
                         {
                             p.CategoryID,
                             CreateName = b.Name,
                             p.Content,
                             p.CreateID,
                             CategoryName = a.Name,
                             CategoryCode = a.Code,
                             p.CreateTime,
                             p.ID,
                             p.ModifyID,
                             ModifyName = c.Name,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             p.ViewCount,
                             p.Keywords
                         };
            result = result.Where(m => m.ID == id);


            return result.ToList().Select(m => new Article()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName,
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content,
                CreateID = m.CreateID,
                CreateName = m.CreateName,
                CreateTime = m.CreateTime,
                ModifyID = m.ModifyID,
                ModifyName = m.ModifyName,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                CategoryCode = m.CategoryCode,
                ViewCount = m.ViewCount,
                Keywords=m.Keywords

            }).FirstOrDefault();
        }
        /// <summary>
        /// 加载文章列表
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Article> GetArticles(string categoryCode, string order)
        {
            var result = from p in db.Set<Article>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Employee>() on new { CreateID = p.CreateID } equals new { CreateID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { ModifyID = p.ModifyID } equals new { ModifyID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         select new
                         {
                             p.CategoryID,
                             CreateName = b.Name,
                             p.Content,
                             p.CreateID,
                             CategoryName = a.Name,
                             CategoryCode = a.Code,
                             p.CreateTime,
                             p.ID,
                             p.ModifyID,
                             ModifyName = c.Name,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             p.ViewCount,
                             p.Keywords

                         };
            if (!string.IsNullOrEmpty(categoryCode))
            {
                result = result.Where(m => m.CategoryCode.Contains(categoryCode));
            }
            if (order == "latest")
                result = result.OrderByDescending(m => m.ModifyTime);
            else if (order == "hot")
                result = result.OrderByDescending(m => m.ViewCount);
            else
                result = result.OrderByDescending(m => m.ModifyTime);
            string sql = result.ToString();
            return result.ToList().Select(m => new Article()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName,
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content,
                CreateID = m.CreateID,
                CreateName = m.CreateName,
                CreateTime = m.CreateTime,
                ModifyID = m.ModifyID,
                ModifyName = m.ModifyName,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                ViewCount = m.ViewCount,
                CategoryCode = m.CategoryCode,
                Keywords=m.Keywords
            });
        }
    }
}