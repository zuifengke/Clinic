using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Filters;

namespace Windy.WebMVC.Web2.Controllers
{
    [MyException]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            int totalCount = 0;
            string order = "latest";
            ViewBag.NewsGonggao = ArticleServices.GetArticles(SystemConst.CategoryCode.NewsGongGao, order, 1, 10, out totalCount);
            ViewBag.NewsExamData = ArticleServices.GetArticles(SystemConst.CategoryCode.NewsExamData, order, 1, 10, out totalCount);
            ViewBag.NewsReview = ArticleServices.GetArticles(SystemConst.CategoryCode.NewsReview, order, 1, 10, out totalCount);
            ViewBag.NewsScore= ArticleServices.GetArticles(SystemConst.CategoryCode.NewsScore, order, 1, 10, out totalCount);
            ViewBag.Book = BookServices.GetBooks(SystemConst.CategoryCode.Book, order, 1, 10, out totalCount);
            ViewBag.Train = TrainServices.GetTrains(SystemConst.CategoryCode.Train, order, 1, 8, out totalCount);
            ViewBag.Hotel = HotelServices.GetHotels(SystemConst.CategoryCode.Hotel, order, 1, 20, out totalCount);
            ViewBag.Blog = BlogServices.GetBlogs(null,0,SystemConst.CategoryCode.Blog, order, 1, 10, out totalCount);
            ViewBag.Product = ProductServices.GetProducts(null,SystemConst.CategoryCode.Advert, order, 1, 30, out totalCount);
            return View();
        }
        public ActionResult Advise()
        {
            return View();
        }
        public ActionResult Help()
        {
            var categorys = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code.Contains(SystemConst.CategoryCode.Help)).ToList();
            ViewBag.Categorys = categorys;
            var articles = EnterRepository.GetRepositoryEnter().ArticleRepository.GetArticles(SystemConst.CategoryCode.Help, "latest").ToList();
            ViewBag.Articles = articles;
            return View();
        }
        public ActionResult About()
        {
            var about = ArticleServices.GetArticles(SystemConst.CategoryCode.About);
            ViewBag.About = about;
            return View();
        }

        public ActionResult CreateIndexView()
        {
            //return Redirect("/amazeui");
            CreateIndex();
            return Redirect("/index.html");
        }
        public void CreateIndex()
        { //创建主页

            string url = "http://" + Request.Url.Authority + "/Home/Index";//请求地址
            string path = Server.MapPath("/index.html");//生成静态页文件
            WebRequest request = WebRequest.Create(url);
            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string htmlcontent = sr.ReadToEnd();
            stream.Close();
            sr.Close();
            SaveFile(path, htmlcontent);
        }
        private static void SaveFile(string path, string content)//保存静态页
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(content);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
