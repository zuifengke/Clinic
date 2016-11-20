using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Linq;
using System.Web;
using System.Configuration;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    /// <summary>
    /// 系统全局上下文环境变量
    /// </summary>
    public class SystemContext
    {
        private static SystemContext m_instance = null;

        /// <summary>
        /// 获取系统运行上下文实例
        /// </summary>
        public static SystemContext Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SystemContext();
                return m_instance;
            }
        }

        private SystemContext()
        {
        }
        public string State
        {
            get { return ConfigurationManager.AppSettings["state"]; }
        }

        private Models.WeiXinAppInfo m_WeiXinAppInfo = null;
        public Models.WeiXinAppInfo WeiXinAppInfo
        {
            get
            {
                if (this.m_WeiXinAppInfo == null)
                {
                    this.m_WeiXinAppInfo = new Models.WeiXinAppInfo();
#if DEBUG   
                    this.m_WeiXinAppInfo.AppID = ConfigurationManager.AppSettings["weixin.debug.app_id"];
                    this.m_WeiXinAppInfo.AppSecret = ConfigurationManager.AppSettings["weixin.debug.app_secret"];
                    this.m_WeiXinAppInfo.URL = ConfigurationManager.AppSettings["weixin.debug.url"];
                    this.m_WeiXinAppInfo.Token = ConfigurationManager.AppSettings["weixin.debug.token"];
#else
                    this.m_WeiXinAppInfo.AppID = ConfigurationManager.AppSettings["weixin.app_id"];
                    this.m_WeiXinAppInfo.AppSecret = ConfigurationManager.AppSettings["weixin.app_secret"];
                    this.m_WeiXinAppInfo.URL = ConfigurationManager.AppSettings["weixin.url"];
                    this.m_WeiXinAppInfo.Token = ConfigurationManager.AppSettings["weixin.token"];
#endif

                }
                return this.m_WeiXinAppInfo;
            }
            set
            {
                this.m_WeiXinAppInfo = value;
            }
        }

        private List<Models.Advert> m_adverts = null;
        public List<Models.Advert> Adverts
        {
            get
            {
                if (CacheHelper.IsExistCache(SystemConst.CategoryCode.Advert))
                {
                    this.m_adverts = CacheHelper.GetCache(SystemConst.CategoryCode.Advert) as List<Models.Advert>;
                }
                else
                {
                    this.m_adverts = EnterRepository.GetRepositoryEnter().AdvertRepository.GetAdverts(null, null).ToList();
                    CacheHelper.AddCache(SystemConst.CategoryCode.Advert, this.m_adverts, 1);
                }
                return m_adverts;
            }
        }

        private Models.PageInfo m_PageInfo = null;
        public Models.PageInfo PageInfo
        {
            get
            {
                if (this.m_PageInfo == null)
                {
                    this.m_PageInfo = new Models.PageInfo();
                    this.PageInfo.Keywords = ConfigurationManager.AppSettings["keywords"];
                    this.PageInfo.Description = ConfigurationManager.AppSettings["description"];
                    this.PageInfo.Title = ConfigurationManager.AppSettings["title"];
                    this.PageInfo.Author = ConfigurationManager.AppSettings["author"];
                }
                return this.m_PageInfo;
            }
            set
            {
                this.m_PageInfo = value;
            }
        }
        public string DefaultPwd
        {
            get { return "111111"; }
        }
        private string m_szWorkPath = string.Empty;
        /// <summary>
        /// 获取或设置程序工作路径
        /// </summary>
        public string WorkPath
        {
            set { this.m_szWorkPath = value; }
            get
            {
                if (string.IsNullOrEmpty(this.m_szWorkPath))
                {

                    string szDllPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                    this.m_szWorkPath = szDllPath;
                    return m_szWorkPath;
                }
                return this.m_szWorkPath;
            }
        }
        public string CreatIDs(string szTableName)
        {
            Random rand = new Random();
            return string.Format("{0}_{1}{2}", szTableName, DateTime.Now.ToString("yyyyMMddHHmmss"), rand.Next(0, 9999).ToString().PadLeft(4, '0'));
        }

        public string GetDefaultImg()
        {
            return "http://www.zyldingfang.com/Content/images/sui/defaultpic.gif";
        }
        /// <summary>
        /// 默认密码 六个1，得到考生登陆默认密码
        /// </summary>
        /// <param name="Tel"></param>
        /// <returns></returns>
        public string GetPwd(string Tel)
        {
            if (Tel == null)
                return string.Empty;
            if (Tel.Length >= 6)
            {
                // return Tel.Substring(Tel.Length - 6, 6);
                return "111111";  //改成默认密码 六个1
            }
            return string.Empty;
        }
        public struct Template
        {
            public const string SiFa = "业务员,报名次序,姓名,性别,收缴余款所在地,所在学校,报考类型,网报序号,网报密码,联系方式,备注,意向同住人,已交款额,提交考点,房号,酒店,酒店房价,多退少补";
            public const string ChengRen = "业务员,报名次序,姓名,性别,收缴余款所在地,所在学校,报考类型,网报序号,网报密码,联系方式,备注,意向同住人,已交款额,提交考点,房号,酒店,酒店房价,多退少补";
            public const string YanjiuSheng = "业务员,报名次序,姓名,性别,所在学校,所报学校,报考类型,网报序号,网报密码,联系方式,备注,意向同住人,已交款额,提交考点,房号,酒店,酒店房价,多退少补,状元乐抽奖福利";
            public static string GetTemplate(string Name)
            {
                if (Name == "司法考试")
                    return SiFa;
                else if (Name == "研究生考试")
                    return YanjiuSheng;
                else if (Name == "成人考试")
                    return ChengRen;
                else
                    return string.Empty;
            }
            public static string[] GetArrTemplate()
            {
                return new string[] {
                    "司法考试",
                    "研究生考试",
                    "成人考试"
                };
            }
        }
        public const string ProductTemplate= "商品详情页链接地址,商品id,商品名称,商品主图,店铺名称,商品价格(单位：元),收入比率(%),佣金,淘宝客链接,商品月销量";
        /// <summary>
        /// 考点类型
        /// </summary>
        public struct PlaceType
        {

            /// <summary>
            /// 考点
            /// </summary>
            public const string SCHOOL = "考点";
            /// <summary>
            /// 城市
            /// </summary>
            public const string CITY = "城市";
            /// <summary>
            /// 省份
            /// </summary>
            public const string PROVINCE = "省份";

            public static string[] GetPlaceTypes()
            {
                return new string[] {
                    "省份",
                    "城市",
                    "考点"
                };
            }

        }
        /// <summary>
        /// 权限点
        /// </summary>
        public struct RightPoint
        {

            /// <summary>
            /// 查看相同机构下的考生
            /// </summary>
            public const string ViewAllUsers = "ViewAllUsers";

            public static string[] GetPlaceTypes()
            {
                return new string[] {
                    "ViewAllUsers",
                    "城市",
                    "考点"
                };
            }

        }
        public struct FilePath
        {
            public const string Excel = "~/Content/temp/excel/";
            public const string Demand = "~/Content/temp/demand/";
            public const string News = "~/Content/temp/news/";
        }
        /// <summary>
        /// 项目名
        /// </summary>
        public struct ProductName
        {
            /// <summary>
            /// 状元乐后台管理
            /// </summary>
            public const string ZYLManage = "状元乐后台管理";

            /// <summary>
            /// 状元乐手机应用
            /// </summary>
            public const string AppPhone = "状元乐手机应用";

            /// <summary>
            /// 状元乐网站
            /// </summary>
            public const string ZYLWebSit = "状元乐网站";


            public static string[] GetProductNames()
            {
                return new string[] {
                    "状元乐后台管理",
                    "状元乐手机应用",
                    "状元乐网站"};
            }
        }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public struct MenuType
        {
            public const string Tab = "页签";

            public const string Page = "页面";

            public const string Button = "按钮";

            public const string Perssion = "权限";



            public static string[] GetMenuTypeNames()
            {
                return new string[] {
                    "页签",
                    "页面",
                    "按钮",
                    "权限"};
            }
        }
        /// <summary>
        /// 需求状态
        /// </summary>
        public struct DemandState
        {
            /// <summary>
            /// 已提交
            /// </summary>
            public const string Submit = "已提交";

            /// <summary>
            /// 处理中
            /// </summary>
            public const string Process = "处理中";

            /// <summary>
            /// 已解决
            /// </summary>
            public const string Solute = "已解决";
            /// <summary>
            /// 已确认
            /// </summary>
            public const string Complete = "已确认";


            public static string[] GetDemandStates()
            {
                return new string[] {
                    "已提交",
                    "处理中",
                    "已解决",
                    "已确认"};
            }
        }
    }
}