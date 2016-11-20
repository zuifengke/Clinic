using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class BlogServices
    {
        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<Models.Blog> GetBlogs(string keyword,int memberID,string categoryCode,string order, int page, int size, out int totalCount)
        {
            int skip = (page - 1) * size;
            var result = EnterRepository.GetRepositoryEnter().BlogRepository.GetBlogs(keyword,memberID, categoryCode,order,skip,size,out totalCount).ToList();
            return result;
        }
        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<Models.Book> GetBooks(string categoryCode)
        {
            var result = EnterRepository.GetRepositoryEnter().BookRepository.GetBooks(categoryCode,string.Empty).ToList();
            return result;
        }
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Models.Blog GetBlog(int id)
        {

            //获取用户及角色已授权的权限
            var result = EnterRepository.GetRepositoryEnter().BlogRepository.GetBlog(id);

            return result;
        }

    }
}