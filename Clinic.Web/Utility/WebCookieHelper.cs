using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2
{
    public class WebCookieHelper
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region 管理员cookie帮助
        public const string adminCookieName = "AuthorDesignAdminCookie";
        /// <summary>
        /// 设置管理员的信息
        /// </summary>
        /// <param name="adminId">管理员Id</param>
        /// <param name="adminUserName">管理员的用户名</param>
        /// <param name="adminLastLoginTime">上次登录时间</param>
        /// <param name="adminLastLoginIP">上次登录IP</param>
        /// <param name="adminLastLoginAddress">上次登录地址</param>
        /// <param name="isSurperAdmin">是否超级管理员</param>
        /// <param name="authoryId">角色Id</param>
        /// <param name="day">cookie有效时间</param>
        public static void SetCookie(int adminId, string adminUserName, DateTime adminLastLoginTime, string adminLastLoginIP, string adminLastLoginAddress, int isSurperAdmin, int authoryId, int day)
        {
            string cookieValue = string.Join("|*&^%$#@!", adminId, HttpUtility.UrlEncode(adminUserName, System.Text.Encoding.UTF8), adminLastLoginTime, adminLastLoginIP, HttpUtility.UrlEncode(adminLastLoginAddress, System.Text.Encoding.UTF8), isSurperAdmin, authoryId);
            if (day == 0)
            {
                Utility.CookieHelper.SetCookie(adminCookieName, cookieValue, "");
            }
            else {
                Utility.CookieHelper.SetCookie(adminCookieName, cookieValue, "", day);
            }
        }
        /// <summary>
        /// 获取管理员基本信息
        /// </summary>
        /// <param name="index">【0：adminId，1：adminUserName，2：adminLastLoginTime，3：adminLastLoginIP，4：adminLastLoginAddress,5:超级管理员,6:角色Id】</param>
        /// <returns></returns>
        public static string GetAdminInfo(int index)
        {
            string value = string.Empty;
            string cookieValue = Utility.CookieHelper.GetCookie(adminCookieName);
            if (!string.IsNullOrEmpty(cookieValue))
            {
                string[] adminInfo = cookieValue.Split(new string[] { "|*&^%$#@!" }, StringSplitOptions.None);
                if (adminInfo.Length >= index)
                {
                    if (index == 1 || index == 4)
                    {
                        value = HttpUtility.UrlDecode(adminInfo[index], System.Text.Encoding.UTF8);
                    }
                    else {
                        value = adminInfo[index];
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 获取管理员Id或者角色Id或者是否超级管理员
        /// </summary>
        /// <param name="index">[0:管理员Id;5:超级管理员;6:角色Id]</param>
        /// <returns></returns>
        public static int GetAdminId(int index)
        {
            string adminId = GetAdminInfo(index);
            return string.IsNullOrEmpty(adminId) ? 0 : int.Parse(adminId);
        }
        /// <summary>
        /// 判断管理员是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool AdminCheckLogin()
        {
            if (Utility.CookieHelper.ExistCookie(adminCookieName))
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 注销管理员Cookie
        /// </summary>
        public static void AdminLoginOut()
        {
            if (Utility.CookieHelper.ExistCookie(adminCookieName))
            {
                Utility.CookieHelper.ExpireCookie(adminCookieName);
            }
        }
        #endregion
        #region 普通会员cookie帮助
        public enum UserInfo
        {
            ID = 0,
            Name = 1,
            Tel = 2
        }
        public const string userCookieName = "UserCookie";
        /// <summary>
        /// 设置会员的信息
        /// </summary>
        public static void SetUserCookie(int id, string name, string tel,string loginIP, string picture, int day)
        {
            string cookieValue = string.Join("|*&^%$#@!", id, HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8), tel, loginIP, HttpUtility.UrlEncode(picture, System.Text.Encoding.UTF8));
            if (day == 0)
            {
                Utility.CookieHelper.SetCookie(userCookieName, cookieValue, "");
            }
            else {
                Utility.CookieHelper.SetCookie(userCookieName, cookieValue, "", day);
            }
        }
        /// <summary>
        /// 获取登录基本信息
        /// </summary>
        /// <param name="index">【0：id，1：name，2:tel,3：loginIP，4：picture</param>
        /// <returns></returns>
        public static string GetUserInfo(int index)
        {
            string value = string.Empty;
            string cookieValue = Utility.CookieHelper.GetCookie(userCookieName);
            if (!string.IsNullOrEmpty(cookieValue))
            {
                string[] userInfo = cookieValue.Split(new string[] { "|*&^%$#@!" }, StringSplitOptions.None);
                if (userInfo.Length > index)
                {
                    if (index == 1 || index == 4)
                    {
                        value = HttpUtility.UrlDecode(userInfo[index], System.Text.Encoding.UTF8);
                    }
                    else {
                        value = userInfo[index];
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 获取管理员Id或者角色Id或者是否超级管理员
        /// </summary>
        /// <param name="index">[0:用户Id;]</param>
        /// <returns></returns>
        public static int GetUserId(int index)
        {
            string userId = GetUserInfo(index);
            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }
        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool UserCheckLogin()
        {
            if (Utility.CookieHelper.ExistCookie(userCookieName))
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 注销用户Cookie
        /// </summary>
        public static void UserLoginOut()
        {
            if (Utility.CookieHelper.ExistCookie(userCookieName))
            {
                Utility.CookieHelper.ExpireCookie(userCookieName);
            }
        }
        #endregion

        #region 业务员cookie帮助
        public enum EmployeeInfo
        {
            ID = 0,
            Name = 1,
            EmpNo = 2,
            Tel = 3
        }
        public const string employeeCookieName = "EmployeeCookie";
        /// <summary>
        /// 设置会员的信息
        /// </summary>
        public static void SetEmployeeCookie(int id, string name, string EmpNo, string tel,  int day)
        {
            string cookieValue = string.Join("|*&^%$#@!", id, HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8), HttpUtility.UrlEncode(EmpNo, System.Text.Encoding.UTF8), tel);
            if (day == 0)
            {
                Utility.CookieHelper.SetCookie(employeeCookieName, cookieValue, "");
            }
            else {
                Utility.CookieHelper.SetCookie(employeeCookieName, cookieValue, "", day);
            }
        }
        /// <summary>
        /// 获取管理员基本信息
        /// </summary>
        /// <param name="index">【0：id，1：name，2:EmpNo,3:tel</param>
        /// <returns></returns>
        public static string GetEmployeeInfo(int index)
        {
            string value = string.Empty;
            string cookieValue = Utility.CookieHelper.GetCookie(employeeCookieName);
            if (!string.IsNullOrEmpty(cookieValue))
            {
                string[] info = cookieValue.Split(new string[] { "|*&^%$#@!" }, StringSplitOptions.None);
                if (info.Length >= index)
                {
                    if (index == 1 || index == 4 || index == 2)
                    {
                        value = HttpUtility.UrlDecode(info[index], System.Text.Encoding.UTF8);
                    }
                    else {
                        value = info[index];
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 获取管理员Id或者角色Id或者是否超级管理员
        /// </summary>
        /// <param name="index">[0:用户Id;]</param>
        /// <returns></returns>
        public static int GetEmployeeId()
        {
            string id = GetEmployeeInfo((int)EmployeeInfo.ID);
            return string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
        }
        /// <summary>
        /// 判断业务员是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool EmployeeCheckLogin()
        {
            if (Utility.CookieHelper.ExistCookie(employeeCookieName))
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 注销业务员Cookie
        /// </summary>
        public static void EmployeeLoginOut()
        {
            if (Utility.CookieHelper.ExistCookie(employeeCookieName))
            {
                Utility.CookieHelper.ExpireCookie(employeeCookieName);
            }
        }
        #endregion
    }
}