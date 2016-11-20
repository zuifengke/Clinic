using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;
using Windy.WebMVC.Web2.Filters;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    [MyException]
    public class MemberController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region 查询数据

        /// <summary>
        /// 查询数据
        /// </summary>
        public ActionResult QueryData()
        {
            int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
            int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
            string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
            string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
            string tel = Request.Form["tel"] != "" ? Request.Form["tel"] : "";
            string name = Request.Form["name"] != "" ? Request.Form["name"] : "";

            if (page < 1) return Content("");
            
            int totalCount = 0;
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadPageList(tel, name, (page - 1) * size, size, out totalCount).ToList();

            JsonHelper json = new JsonHelper();
            string strJson = string.Empty;
            return Json(new
            {
                total = totalCount,
                rows = result
            }, JsonRequestBehavior.AllowGet); 
        }
        #endregion

        [HttpPost]
        public ActionResult GetDetail()
        {
            int id = Request.Form["id"] != "" ? int.Parse(Request.Form["id"]) : 0;
            var result = EnterRepository.GetRepositoryEnter().MemberRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Member member)
        {
            string writeMsg = string.Empty;
            EnterRepository.GetRepositoryEnter().MemberRepository.Get(m => m.ID == member.ID);
            bool result = EnterRepository.GetRepositoryEnter().MemberRepository.EditEntity(member, new string[] { "UserName", "RealName", "Tel", "Mail", "Sex" });
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                writeMsg = "修改成功!";
            }
            else
            {
                writeMsg = "修改失败!";
            }
            return Content(writeMsg);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Member item)
        {
            string writeMsg = string.Empty;
            item.Password = Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd);
            EnterRepository.GetRepositoryEnter().MemberRepository.AddEntity(item);
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                writeMsg = "{msg:\"保存成功!\",id:" + item.ID.ToString() + "}";
            }
            else
            {
                writeMsg = "{msg:\"保存失败!\",id:0}";
            }
            return Content(writeMsg);
        }
        /// <summary>
        /// 密码重置为111111
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(Member item)
        {
            if ( item.ID == 0)
                return Content("密码重置失败");
            item.Password = Utility.MD5Helper.MD5(SystemContext.Instance.DefaultPwd);
            EnterRepository.GetRepositoryEnter().MemberRepository.Get(m=>m.ID==item.ID);
            EnterRepository.GetRepositoryEnter().MemberRepository.EditEntity(item, new string[] { "Password"});
            string writeMsg = string.Empty;
            if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
            {
                writeMsg = "密码重置成功!";
            }
            else
            {
                writeMsg = "密码重置失败";
            }
            return Content(writeMsg);
        }


        public ActionResult Delete()
        {
            string writeMsg = "删除失败！";
            try
            {
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().MemberRepository.DeleteEntity(new Member() { ID = id });
                    }
                    if (EnterRepository.GetRepositoryEnter().SaveChange() > 0)
                    {
                        writeMsg = string.Format("删除成功");
                    }
                    else
                    {
                        writeMsg = "删除失败！";
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                return Content(writeMsg);
            }
        }
    }
}
