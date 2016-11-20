using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Windy.WebMVC.Web2.Helpers
{
    public static class TopSDKHelper
    {
        private const string AppKey = "23390406";
        private const string AppSecret = "7d1fc5badbdbb22f4487b745e881fe31";
        private const string Url = "http://gw.api.taobao.com/router/rest";
        private const long adzone_id_商品 = 58758697;
        /// <summary>
        /// 关键词搜索
        /// </summary>
        /// <param name="szText"></param>
        /// <returns></returns>
        public static TbkItemGetResponse GetTbkItemGetResponse(string szText)
        {
            // 定义正则表达式用来匹配 img 标签 
            ITopClient client = new DefaultTopClient(TopSDKHelper.Url, TopSDKHelper.AppKey, TopSDKHelper.AppSecret);
            TbkItemGetRequest req = new TbkItemGetRequest();
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick";
            req.Q = szText;

            req.Sort = "tk_rate_des";
            TbkItemGetResponse rsp = client.Execute(req);
            return rsp;
        }
        /// <summary>
        /// 获取淘宝联盟选品库的列表
        /// </summary>
        /// <param name="szText"></param>
        /// <returns></returns>
        public static TbkUatmFavoritesGetResponse GetTbkUatmFavoritesGetResponse(string szText)
        {
            // 定义正则表达式用来匹配 img 标签 
            ITopClient client = new DefaultTopClient(TopSDKHelper.Url, TopSDKHelper.AppKey, TopSDKHelper.AppSecret);
            TbkUatmFavoritesGetRequest req = new TbkUatmFavoritesGetRequest();
            req.PageNo = 1L;
            req.PageSize = 20L;
            req.Fields = "favorites_title,favorites_id,type";
            req.Type = 1L;
            TbkUatmFavoritesGetResponse rsp = client.Execute(req);
            Console.WriteLine(rsp.Body);
            return rsp;
        }
        /// <summary>
        /// 获取淘宝联盟选品库的宝贝信息
        /// </summary>
        /// <param name="szText"></param>
        /// <returns></returns>
        public static TbkUatmFavoritesItemGetResponse GetTbkUatmFavoritesItemGetResponse(long favoritesId )
        {
            ITopClient client = new DefaultTopClient(TopSDKHelper.Url, TopSDKHelper.AppKey, TopSDKHelper.AppSecret);
            TbkUatmFavoritesItemGetRequest req = new TbkUatmFavoritesItemGetRequest();
            req.Platform = 1L;
            req.PageSize = 200L;
            req.AdzoneId = adzone_id_商品;
            //req.Unid = "3456";
            req.FavoritesId = favoritesId;
            req.PageNo = 1L;
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick,shop_title,zk_final_price_wap,event_start_time,event_end_time,tk_rate,status,type,click_url";
            TbkUatmFavoritesItemGetResponse rsp = client.Execute(req);
            Console.WriteLine(rsp.Body);
            return rsp;
        }
    }
}