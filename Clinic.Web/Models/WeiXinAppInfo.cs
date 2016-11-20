using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.Models
{
    public class WeiXinAppInfo
    {
        /// <summary>
        /// 微信应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
        public string OpenID { get; set; }
        public string AccessToken { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string URL { get; set; }
    }
}