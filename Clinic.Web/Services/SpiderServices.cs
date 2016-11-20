using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class SpiderServices
    {
        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static bool SpiderRelationPage(string sourceUrl)
        {
            string html = HtmlContentHelper.WebPageContentGet(sourceUrl);
            List<string> urls = HtmlContentHelper.WebPageRelationUrlGet(sourceUrl, html);

            if (urls.Count > 0)
            {
                foreach (var item in urls)
                {
                    string htmlstr = HtmlContentHelper.WebPageContentGet(item);
                    string title = HtmlContentHelper.WebPageTitleGet(htmlstr).Replace("- 今日头条(TouTiao.com)", "").Replace("- 今日头条(TouTiao.org)", ""); ;
                    string description = HtmlContentHelper.WebPageDescriptionGet(htmlstr);
                    string keywords = HtmlContentHelper.WebPageKeywordsGet(htmlstr);
                    string article = HtmlContentHelper.WebPageArticleGet(item, htmlstr);
                    if (string.IsNullOrEmpty(article))
                        continue;
                    //if (article.IndexOf("转载") > 0 ||article.IndexOf("抄袭")>0)
                    //    continue;
                    var blog = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities(m => m.ReprintUrl == item).FirstOrDefault();
                    if (blog == null)
                    {
                        blog = new Models.Blog();
                        blog.ReprintUrl = item;
                        blog.MemberID = WebCookieHelper.GetUserId(0);
                        blog.CategoryID = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == "blog").FirstOrDefault().ID;
                        blog.ImagePath = HtmlContentHelper.GetFirstImgUrl(article);
                        blog.IsPublic = 1;
                        blog.Keywords = keywords;
                        blog.Summary = description;
                        blog.Zhuanzai = 1;
                        blog.ModifyTime = DateTime.Now;
                        blog.CreateTime = DateTime.Now;
                        blog.Content = article;
                        blog.Title = title;
                        EnterRepository.GetRepositoryEnter().BlogRepository.AddEntity(blog);
                        if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                        {
                            return false;
                        }
                    }
                    else if(blog.ViewCount==0)
                    {
                        blog.ModifyTime = DateTime.Now;
                        EnterRepository.GetRepositoryEnter().BlogRepository.EditEntity(blog, new string[] {
                     "ModifyTime"  });
                        if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static bool SpiderRelationPageToutiao(string sourceUrl)
        {
            string html = HtmlContentHelper.WebPageContentGet(sourceUrl);
            List<string> urls = HtmlContentHelper.WebPageRelationUrlGet(sourceUrl, html);

            if (urls.Count > 0)
            {
                foreach (var item in urls)
                {
                    string htmlstr = HtmlContentHelper.WebPageContentGet(item);
                    string title = HtmlContentHelper.WebPageTitleGet(htmlstr).Replace("- 今日头条(TouTiao.com)", "").Replace("- 今日头条(TouTiao.org)", ""); ;
                    string description = HtmlContentHelper.WebPageDescriptionGet(htmlstr);
                    string keywords = HtmlContentHelper.WebPageKeywordsGet(htmlstr);
                    string article = HtmlContentHelper.WebPageArticleGet(item, htmlstr);
                    if (string.IsNullOrEmpty(article))
                        continue;
                    //if (article.IndexOf("转载") > 0 ||article.IndexOf("抄袭")>0)
                    //    continue;
                    var result = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities(m => m.ReprintUrl == item).FirstOrDefault();
                    if (result == null)
                    {
                        result = new Models.Toutiao();
                        result.ReprintUrl = item;
                        result.MemberID = WebCookieHelper.GetUserId(0);
                        result.CategoryID = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == "toutiao").FirstOrDefault().ID;
                        result.ImagePath = HtmlContentHelper.GetFirstImgUrl(article);
                        result.IsPublic = 1;
                        result.Keywords = keywords;
                        result.Summary = description;
                        result.Zhuanzai = 1;
                        result.ModifyTime = DateTime.Now;
                        result.CreateTime = DateTime.Now;
                        result.Content = article;
                        result.Title = title;
                        EnterRepository.GetRepositoryEnter().ToutiaoRepository.AddEntity(result);
                        if (EnterRepository.GetRepositoryEnter().SaveChange() <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
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