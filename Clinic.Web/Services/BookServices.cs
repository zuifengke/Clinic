using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class BookServices
    {
        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<Models.Book> GetBooks(string categoryCode,string order, int page, int size, out int totalCount)
        {
            int skip = (page - 1) * size;
            totalCount = EnterRepository.GetRepositoryEnter().BookRepository.GetBooks(categoryCode,order).Count();
            var result = EnterRepository.GetRepositoryEnter().BookRepository.GetBooks(categoryCode,order).Skip(skip).Take(size).ToList();
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
        public static Models.Book GetBook(int id)
        {

            //获取用户及角色已授权的权限
            var result = EnterRepository.GetRepositoryEnter().BookRepository.GetBook(id);

            return result;
        }

    }
}