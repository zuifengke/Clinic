using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Windy.WebMVC.Web2.Utility
{
    public class MD5Helper
    {  
       /// <summary>
       /// MD5散列
       /// </summary>
        public static string MD5(string inputStr)
       {
           if (string.IsNullOrEmpty(inputStr))
               return string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashByte = md5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            StringBuilder sb = new StringBuilder();
            foreach (byte item in hashByte)
                sb.Append(item.ToString("X").PadLeft(2, '0'));
            return sb.ToString();
        }
    }
}