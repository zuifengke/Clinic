using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class ProductRepository : BaseRepository<Models.Product>
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
        public IEnumerable<Product> LoadPageList(string title, int categoryID, string categoryCode, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Product>()
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
                             p.DisCount,
                             p.Price,
                             p.Url,
                             p.DetailUrl,
                             p.ViewCount,
                             p.Keywords,
                             p.ArticleID,
                             p.Percent,
                             p.ShopName,
                             p.TaobaoID,
                             p.Income,
                             p.Sales
                         };
            if (!string.IsNullOrEmpty(title))
            {
                result = result.Where(m => m.Title.Contains(title));
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
            return result.ToList().Select(m => new Product()
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
                DisCount = m.DisCount,
                Price = m.Price,
                 DetailUrl=m.DetailUrl,
                Url = m.Url,
                ViewCount = m.ViewCount,
                Keywords = m.Keywords,
                ArticleID = m.ArticleID,
                Income = m.Income,
                TaobaoID = m.TaobaoID,
                ShopName = m.ShopName,
                Percent = m.Percent,
                Sales=m.Sales
            });
        }
        public Product GetProduct(int id)
        {
            var result = from p in db.Set<Product>()
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
                             p.DisCount,
                             p.Url,
                             p.DetailUrl,
                             p.Price,
                             p.Keywords,
                             p.Percent,
                             p.ShopName,
                             p.TaobaoID,
                             p.Income,
                              p.Sales
                         };
            result = result.Where(m => m.ID == id);


            return result.ToList().Select(m => new Product()
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
                DisCount = m.DisCount,
                Price = m.Price,
                Url = m.Url,
                DetailUrl=m.DetailUrl,
                Keywords = m.Keywords,
                Income = m.Income,
                TaobaoID = m.TaobaoID,
                ShopName = m.ShopName,
                Percent = m.Percent,
                 Sales=m.Sales
            }).FirstOrDefault();
        }
        public IEnumerable<Product> GetProducts(string keyword,string categoryCode, string order, int startNum, int pageSize, out int totalcount)
        {
            var result = from p in db.Set<Product>()
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
                             p.Price,
                             p.Url,
                             p.DetailUrl,
                             p.DisCount,
                             p.Keywords,
                             p.Percent,
                             p.ShopName,
                             p.TaobaoID,
                             p.Income,
                             p.Sales

                         };
            if (!string.IsNullOrEmpty(categoryCode))
            {
                result = result.Where(m => m.CategoryCode.Contains(categoryCode));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(m => m.Keywords.Contains(keyword));
            }
            if (order == "latest")
                result = result.OrderByDescending(m => m.ModifyTime);
            else if (order == "hot")
                result = result.OrderByDescending(m => m.ViewCount);
            else
                result = result.OrderByDescending(m => m.ModifyTime);
            string sql = result.ToString();
            totalcount = result.Count();
            if (pageSize != 0)
                result = result.Skip(startNum).Take(pageSize);
            return result.ToList().Select(m => new Product()
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
                DisCount = m.DisCount,
                Price = m.Price,
                Url = m.Url,
                 DetailUrl=m.DetailUrl,
                Keywords = m.Keywords,
                Percent = m.Percent,
                ShopName = m.ShopName,
                TaobaoID = m.TaobaoID,
                Income = m.Income,
                 Sales=m.Sales
            });
        }
    }
}