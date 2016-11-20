using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class BlogRepository : BaseRepository<Models.Blog>
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
        public IEnumerable<Blog> LoadPageList(string title, int categoryID, string categoryCode, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Blog>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Member>() on new { MemberID = p.MemberID } equals new { MemberID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         select new
                         {
                             p.CategoryID,
                             MemberName = b.RealName,
                             p.Content,
                             p.MemberID,
                             CategoryName = a.Name,
                             p.CreateTime,
                             p.ID,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             CategoryCode = a.Code,
                             p.ViewCount,
                             p.Keywords,
                             p.ArticleID,
                             p.ReprintUrl
                         };
            if (!string.IsNullOrEmpty(title))
            {
                result = result.Where(m => m.Title.Contains(title)||m.Keywords.Contains(title));
            }
            if (categoryID != 0)
                result = result.Where(m => m.CategoryID == categoryID);
            if (!string.IsNullOrEmpty(categoryCode))
            {
                result = result.Where(m => m.CategoryCode.Contains(categoryCode));
            }
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.ModifyTime).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Blog()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName,
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content,
                MemberID=m.MemberID,
                MemberName = m.MemberName,
                CreateTime = m.CreateTime,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                CategoryCode = m.CategoryCode,
                ViewCount = m.ViewCount,
                Keywords = m.Keywords,
                ArticleID = m.ArticleID,
                 ReprintUrl =m.ReprintUrl
            });
        }
        public Blog GetBlog(int id)
        {
            var result = from p in db.Set<Blog>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Member>() on new { MemberID = p.MemberID } equals new { MemberID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()
                      
                         select new
                         {
                             p.CategoryID,
                             MemberName = b.RealName,
                             p.Content,
                             p.MemberID,
                             CategoryName = a.Name,
                             CategoryCode = a.Code,
                             p.CreateTime,
                             p.ID,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             p.ViewCount,
                             p.IsPublic,
                             p.ReprintUrl,
                             p.ArticleID,
                             p.Keywords,
                             p.Zhuanzai
                         };
            result = result.Where(m => m.ID == id);


            return result.ToList().Select(m => new Blog()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName,
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content,
                ArticleID = m.ArticleID,
                IsPublic = m.IsPublic,
                MemberID = m.MemberID,
                MemberName = m.MemberName,
                ReprintUrl = m.ReprintUrl,
                CreateTime = m.CreateTime,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                ViewCount = m.ViewCount,
                CategoryCode = m.CategoryCode,
                Keywords = m.Keywords,
                Zhuanzai=m.Zhuanzai
            }).FirstOrDefault();
        }
        public IEnumerable<Blog> GetBlogs(string keyword,int memberID,string categoryCode, string order, int startNum, int pageSize, out int totalcount)
        {
            var result = from p in db.Set<Blog>()
                         join a in db.Set<Category>() on new { CategoryID = p.CategoryID } equals new { CategoryID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<Member>() on new { MemberID = p.MemberID } equals new { MemberID = b.ID } into b_into
                         from b in b_into.DefaultIfEmpty()

                         select new
                         {
                             p.CategoryID,
                             MemberName = b.RealName,
                             p.Content,
                             p.MemberID,
                             CategoryName = a.Name,
                             CategoryCode = a.Code,
                             p.CreateTime,
                             p.ID,
                             p.ModifyTime,
                             p.Title,
                             p.ImagePath,
                             p.Summary,
                             p.ViewCount,
                             p.IsPublic,
                             p.ReprintUrl,
                             p.ArticleID,
                             p.Keywords

                         };
            if (!string.IsNullOrEmpty(categoryCode))
            {
                result = result.Where(m => m.CategoryCode.Contains(categoryCode));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(m => m.Title.Contains(keyword) || m.Keywords.Contains(keyword));
            }
            if (memberID != 0)
            {
                result = result.Where(m => m.MemberID==memberID);
            }
            if (order == "latest")
                result = result.OrderByDescending(m => m.ModifyTime);
            else if (order == "hot")
                result = result.OrderByDescending(m => m.ViewCount);
            else
                result = result.OrderByDescending(m => m.ModifyTime);
            totalcount = result.Count();
            if (pageSize != 0)
               result= result.Skip(startNum).Take(pageSize);
            string sql = result.ToString();
            return result.ToList().Select(m => new Blog()
            {
                ID = m.ID,
                CategoryID = m.CategoryID,
                CategoryName = m.CategoryName, 
                Title = m.Title,
                ModifyTime = m.ModifyTime,
                Content = m.Content, 
                ArticleID = m.ArticleID,
                IsPublic = m.IsPublic,
                MemberID = m.MemberID,
                MemberName = m.MemberName,
                ReprintUrl = m.ReprintUrl,
                CreateTime = m.CreateTime,
                ImagePath = m.ImagePath,
                Summary = m.Summary,
                ViewCount = m.ViewCount,
                CategoryCode = m.CategoryCode,
                Keywords = m.Keywords
            });
        }
    }
}