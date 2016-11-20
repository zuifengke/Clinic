using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Config;

namespace Windy.WebMVC.Web2
{
    public static class HtmlContentHelper
    {

        //效果 http://tool.hovertree.com/a/zz/img/
        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string[] GetHvtImgUrls(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex m_hvtRegImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //参考:http://hovertree.com/hvtart/bjae/e4pya1x0.htm


            // 搜索匹配的字符串 
            MatchCollection matches = m_hvtRegImg.Matches(sHtmlText);
            int m_i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[m_i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        public static string GetFirstImgUrl(string sHtmlText)
        {
            string path = string.Empty;
            string[] sUrlList = GetHvtImgUrls(sHtmlText);
            if (sUrlList.Length > 0)
                path = sUrlList[0].Replace("&amp;", "&");
            if (string.IsNullOrEmpty(path))
                path = SystemContext.Instance.GetDefaultImg();//设置默认图片
            return path;
        }
        public static string GetFirstUrl(string sHtmlText)
        {
            string path = string.Empty;
            MatchCollection ms = Regex.Matches(sHtmlText, @"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>"
, RegexOptions.IgnoreCase);
            if (ms.Count > 0)
                path = ms[0].Groups["url"].Value;
            return path;
        }
        /// <summary>
        /// 获取摘要
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        public static string GetSummary(string sHtmlText)
        {
            string szText = Regex.Replace(sHtmlText, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase).Replace("\n", "").Replace("\t", "");
            if (szText.Length <= 150)
                return szText.Substring(0, szText.Length);
            else
                return szText.Substring(0, 150);
        }
        /// <summary>
        /// 获取访问者客户端IP
        /// </summary>
        public static string IP
        {
            get
            {
                string _ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (_ip == null || _ip == "" || _ip == "unknown") { _ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
                if (_ip == null || _ip == "" || _ip == "unknown") { _ip = System.Web.HttpContext.Current.Request.UserHostAddress; }
                if (_ip.Contains(",")) { _ip = _ip.Split(',')[0]; }

                return _ip;
            }
        }

        public static string GetImgBoxUrl(this HtmlHelper htmlHelper, int ImageSize, string ImageUrl)
        {
            string result = string.Empty;
            if (ImageUrl.IndexOf("_") > -1)
            {
                result = ImageUrl.Substring(0, ImageUrl.LastIndexOf("_"))
                    + "_" + ImageSize +
                    ImageUrl.Substring(ImageUrl.LastIndexOf("."));
            }
            else
            {
                result = ImageUrl.Substring(0, ImageUrl.LastIndexOf("."))
                      + "_" + ImageSize +
                      ImageUrl.Substring(ImageUrl.LastIndexOf("."));
            }
            return result;
        }

        /// <summary>
        /// 抓取网页内容
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns></returns>
        public static string WebPageContentGet(string url)
        {
            if (url.IndexOf("taobao") > 0)
                return WebPageContentGet(url, "gb2312");
            else
                return WebPageContentGet(url, "utf-8");
        }

        /// <summary>
        /// 抓取网页内容
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="charset">网页编码</param>
        /// <returns></returns>
        public static string WebPageContentGet(string url, string charset)
        {
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Encoding coding;
                if (charset == "gb2312")
                {
                    coding = System.Text.Encoding.GetEncoding("gb2312");
                }
                else
                {
                    coding = System.Text.Encoding.UTF8;
                }
                System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding);
                string s = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
                reader = null;
                response = null;
                request = null;
                return s;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 抓取网页内容
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="charset">网页编码</param>
        /// <returns></returns>
        public static string WebPageContentGet(string url, Encoding code)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            Encoding coding = code;
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding);
            string s = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            response.Close();
            reader = null;
            response = null;
            request = null;
            return s;
        }

        public static string WebPagePostGet(string url, string data, string charset)
        {
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byte1 = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byte1.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(byte1, 0, byte1.Length);    //写入参数
                newStream.Close();

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                Encoding coding;
                if (charset == "gb2312")
                {
                    coding = System.Text.Encoding.GetEncoding("gb2312");
                }
                else
                {
                    coding = System.Text.Encoding.UTF8;
                }
                System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding);
                string s = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
                reader = null;
                response = null;
                request = null;
                return s;
            }
            catch (Exception e)
            {
                return "ERROR";
            }
        }


        public static string WebPagePostGet(string url, string data, Encoding code)
        {
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byte1 = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byte1.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(byte1, 0, byte1.Length);    //写入参数
                newStream.Close();
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Encoding coding = code;
                System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding);
                string s = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
                reader = null;
                response = null;
                request = null;
                return s;
            }
            catch (Exception e)
            {
                return "ERROR";
            }
        }
        /// <summary>
        /// 抓取网页标题
        /// </summary>
        /// <param name="content">网页内容</param>
        /// <returns></returns>
        public static string WebPageTitleGet(string content)
        {
            Match match = Regex.Match(content, "<title>(.*)</title>");
            string result = match.Groups[1].Value.ToString();
            return result;
        }
        /// <summary>
        /// 抓取网页描述
        /// </summary>
        /// <param name="content">网页内容</param>
        /// <returns></returns>
        public static string WebPageDescriptionGet(string content)
        {
            string regex = "<meta" + @"\s+" + "name=\"description\"" + @"\s+" + "content=\"(?<content>[^\"" + @"\<\>" + "]*)\"";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string result = match.Groups[1].Value.ToString();
            return result;
        }
        /// <summary>
        /// 抓取网页关键词
        /// </summary>
        /// <param name="content">网页内容</param>
        /// <returns></returns>
        public static string WebPageKeywordsGet(string content)
        {
            string regex = "<meta" + @"\s+" + "name=\"keywords\"" + @"\s+" + "content=\"(?<content>[^\"" + @"\<\>" + "]*)\"";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string result = match.Groups[1].Value.ToString();
            return result;
        }

        /// <summary>
        /// 抓取网页文章内容
        /// </summary>
        /// <param name="content">网页内容</param>
        /// <returns></returns>
        public static string WebPageArticleGet(string url, string content)
        {
            string regex = string.Empty;
            if (url.IndexOf("toutiao") > 0)
            {
                regex = @"(?is)<div\s+class=""article-content"">(?><div[^>]*>(?<o>)|</div>(?<-o>)|(?:(?!</?div\b).)*)*(?(o)(?!))</div>";

            }
            if (url.IndexOf("cnblogs") > 0)
            {
                regex = @"<div[^>]*?id=""news_body""[^>]*>((?>(?<o><div[^>]*>)|(?<-o></div>)|(?:(?!</?div)[\s\S]))*)(?(o)(?!))</div>";
            }
            if (content.IndexOf("taobao") > 0)
            {
                regex= @"<div[^>]*?id=""description""[^>]*>((?>(?<o><div[^>]*>)|(?<-o></div>)|(?:(?!</?div)[\s\S]))*)(?(o)(?!))</div>";
            }
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string result = match.Value.ToString();
            return result;
        }

        /// <summary>
        /// 抓取头条相关阅读链接
        /// </summary>
        /// <param name="content">网页内容</param>
        /// <returns></returns>
        public static List<string> WebPageRelationUrlGet(string url, string sHtmlText)
        {
            List<string> path = new List<string>();
            MatchCollection ms = Regex.Matches(sHtmlText, @"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>"
, RegexOptions.IgnoreCase);

            if (ms.Count > 0)
            {
                for (int i = 0; i < ms.Count; i++)
                {
                    path.Add(ms[i].Groups["url"].Value);
                }
            }
            path = path.Where(m => m.Contains("http://toutiao.com/group")).Distinct().ToList();
            return path;
        }


        public static void CreateIndex(SiteConfig _siteConfig)
        { //创建主页
            //SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as SiteConfig;

            string url = "http://" + _siteConfig.SiteUrl + "/Home/Index";//请求地址
            string path = string.Format("{0}\\{1}", _siteConfig.ServerPath, "index.html");
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
                GlobalMethod.log.Info("主页文件保存"+DateTime.Now.ToString());
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