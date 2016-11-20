using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Utility
{
    public class StringOperation
    {
        /// <summary>
        /// 將ASCⅡ碼轉為字符
        /// </summary>
        /// <param name="asc">ASCⅡ碼</param>
        /// <returns></returns>
        public static string IntToChar(int asc)
        {
            return ((char)asc).ToString();
        }

        /// <summary>
        /// 獲取文件後綴名
        /// </summary>
        /// <param name="fileName">fileName文件名</param>
        /// <returns>文件後綴名(例:"zip")</returns>
        public static string GetFileExtension(string fileName)
        {
            return fileName.Split('.')[fileName.Split('.').Length - 1];
        }

        /// <summary>
        /// 從文件完整路徑獲取文件名
        /// </summary>
        /// <param name="fileFullName">文件完整路徑</param>
        /// <returns>文件名</returns>
        public static string GetFileName(string fileFullName)
        {
            int lastSeparateIndex =
                fileFullName.LastIndexOf('\\') > fileFullName.LastIndexOf('/') ?
                fileFullName.LastIndexOf('\\') : fileFullName.LastIndexOf('/');
            return fileFullName.Substring(lastSeparateIndex + 1);
        }

        /// <summary>
        /// 獲取文件所在文件夾
        /// </summary>
        /// <param name="filePath">文件完整路徑</param>
        /// <returns></returns>
        public static string GetFileDirectory(string filePath)
        {
            filePath = filePath.Replace('/', '\\');
            return filePath.Substring(0, filePath.LastIndexOf("\\"));
        }

        /// <summary>
        /// 計算文件大小,將字節數轉化為字符串輸出
        /// </summary>
        /// <param name="contentLength">文件大小(字節)</param>
        /// <returns>文件大小</returns>
        public static string CountFileSize(int contentLength)
        {
            if (contentLength < 1024)
            {
                return contentLength.ToString() + "B";
            }
            else if (contentLength / 1024 < 1024)
            {
                return (contentLength / 1024).ToString() + "KB";
            }
            else
            {
                return (contentLength / 1024 / 1024).ToString() + "MB";
            }
        }


        public static string IntToStringTime(int num)
        {
            if (num == 0)
            {
                return "00:00:00";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //xiaoshi
                int h = (num / 60) / 60;
                if (h >= 1)
                {
                    sb.Append(Temp(h.ToString()) + ":");
                }
                else
                {
                    sb.Append("00:");
                }

                //fengzhong 
                int m = num / 60;
                if (m >= 1)
                {
                    sb.Append(Temp((m - h * 60).ToString()) + ":");
                }
                else
                {
                    sb.Append("00:");
                }
                //miao
                int s = num % 60;
                if (s == 0)
                {
                    sb.Append("00");
                }
                else
                {
                    sb.Append(Temp(s.ToString()));
                }
                return sb.ToString();
            }
        }


        private static string Temp(string temp)
        {
            if (temp.Length == 1)
            {
                return "0" + temp;
            }
            return temp;
        }
    }
}
