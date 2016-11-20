// ***********************************************************
// 封装一些无法归类的方法集合
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;

namespace Windy.WebMVC.Web2.Utility
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装一些无法归类的方法
        /// </summary>
        public struct Misc
        {
            private static Random m_random = null;
            /// <summary>
            /// 生成一个随机数
            /// </summary>
            /// <param name="min">最小值</param>
            /// <param name="max">最大值</param>
            /// <returns>值</returns>
            public static int Random(int min, int max)
            {
                if (m_random == null)
                    m_random = new Random();
                return m_random.Next(min, max);
            }

            /// <summary>
            /// 判断字符串对象是否为空串
            /// </summary>
            /// <param name="value">目标字符串</param>
            /// <returns>true:为空串;false:非空串</returns>
            public static bool IsEmptyString(object value)
            {
                if (value == null)
                    return true;
                return IsEmptyString(value.ToString());
            }

            /// <summary>
            /// 判断字符串是否为空串
            /// </summary>
            /// <param name="value">目标字符串</param>
            /// <returns>true:为空串;false:非空串</returns>
            public static bool IsEmptyString(string value)
            {
                if (value == null)
                    return true;
                if (value.Trim() == string.Empty)
                    return true;
                return false;
            }

            private static string m_workPath = string.Empty;

            /// <summary>
            /// 得到Libraries动态库运行目录
            /// </summary>
            /// <returns>string</returns>
            public static string GetWorkingPath()
            {
                return GlobalMethods.Misc.GetWorkingPath(typeof(GlobalMethods));
            }

            /// <summary>
            /// 得到指定类型所在的动态库的运行目录
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>string</returns>
            public static string GetWorkingPath(Type type)
            {
                if (type == null || m_workPath.Trim() != string.Empty)
                    return m_workPath;
                try
                {
                    string szDllPath = type.Assembly.Location;
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(szDllPath);
                    m_workPath = fileInfo.DirectoryName;
                    return m_workPath;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
