using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windy.WebMVC.Web2.Utility;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.Areas.Admin.Controllers
{
    public class DrugPurchaseController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryData()
        {
            try
            {
                int page = Request.Form["page"] != "" ? Convert.ToInt32(Request.Form["page"]) : 0;
                int size = Request.Form["rows"] != "" ? Convert.ToInt32(Request.Form["rows"]) : 0;
                string sort = Request.Form["sort"] != "" ? Request.Form["sort"] : "";
                string order = Request.Form["order"] != "" ? Request.Form["order"] : "";
                string Name = Request.Form["Name"] != null ? Request.Form["Name"] : "";
                if (page < 1) return Content("");
                
                int totalCount = 0;

                var result = EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.LoadPageList(Name,(page - 1) * size, size, out totalCount).ToList();
                return Json(new
                {
                    total = totalCount,
                    rows = result
                }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }

        #region 删除指定ID 的数据
        /// <summary>
        /// 删除数据
        /// </summary>
        public ActionResult DelData()
        {
            try
            {
                string writeMsg = "删除失败！";
                string selectID = Request.Form["cbx_select"] != "" ? Request.Form["cbx_select"] : "";
                if (selectID != string.Empty && selectID != "0")
                {
                    string[] ids = selectID.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.DeleteEntity(new DrugPurchase() { ID = id });
                    }
                    //short shRet = SystemContext.Instance.EmployeeService.Delete(selectID);
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
                throw;
            }
        }
        #endregion

        #region 添加或修改提交的数据
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        [HttpPost]
        public ActionResult UpdateData(DrugPurchase model)
        {
            try
            {
                bool blResult = false;
                int id = Request.Form["id"] != "" ? Convert.ToInt32(Request.Form["id"]) : 0;
                //DrugPurchase model = GetData(id);

                string writeMsg = "操作失败！";
                if (model != null)
                {
                    if (model.ID < 1)
                    {
                        EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.AddEntity(model);
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
                        {
                            writeMsg = "增加成功!";
                        }
                        else
                        {
                            writeMsg = "增加失败!";
                        }
                    }
                    else
                    {
                        //清楚context中result对象
                        EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.Get(m => m.ID == id);
                        EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.EditEntity(model, new string[] { "DrugID", "PurchaseDate", "Supplier", "Specification", "Amount", "Unit", "Factory", "Batch", "ValidityTerm", "License", "InspectionReport", "Instructions", "Quality", "AcceptanceConclusion", "Buyer", "Examiner" });
                        if (EnterRepository.GetRepositoryEnter().SaveChange()>0)
                        {
                            writeMsg = "更新成功!";
                        }
                        else
                        {
                            writeMsg = "更新失败!";
                        }
                    }
                }
                return Content(writeMsg);
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 取得数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private DrugPurchase GetData(int id)
        {
            DrugPurchase model = new DrugPurchase();
            if (id > 0)
            {
                model = EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
            }
            model.AcceptanceConclusion = Request.Form["AcceptanceConclusion"] != "" ? Request.Form["AcceptanceConclusion"] : "";
            model.Amount = Request.Form["Amount"] != "" ? int.Parse(Request.Form["Amount"]) : 0;

            return model;
        }
        #endregion

        #region 查询指定ID 的数据
        /// <summary>
        /// 获取指定ID的数据
        /// </summary>
        public ActionResult QueryOneData()
        {
            try
            {
                int id = Request.Form["id"] != "" ? int.Parse(Request.Form["id"]) : 0;
                var item = EnterRepository.GetRepositoryEnter().DrugPurchaseRepository.LoadEntities(m => m.ID == id).FirstOrDefault();
                JsonHelper json = new JsonHelper();
                string strJson = string.Empty;
                if (item != null)
                {
                    json.AddItem("ID", item.ID.ToString());
                    json.AddItem("AcceptanceConclusion", item.AcceptanceConclusion==null?"":item.AcceptanceConclusion);
                    json.AddItem("Amount", item.Amount.ToString());
                    json.AddItem("Batch",item.Batch==null?"": item.Batch.ToString());
                    json.AddItem("Buyer", item.Buyer == null ? "" : item.Buyer.ToString());
                    json.AddItem("DrugID",  item.DrugID.ToString());
                    json.AddItem("DrugName", item.DrugName == null ? "" : item.DrugName.ToString());
                    json.AddItem("Examiner", item.Examiner == null ? "" : item.Examiner.ToString());
                    json.AddItem("Factory", item.Factory == null ? "" : item.Factory.ToString());
                    json.AddItem("InspectionReport", item.InspectionReport == null ? "" : item.InspectionReport.ToString());
                    json.AddItem("Instructions", item.Instructions == null ? "" : item.Instructions.ToString());
                    json.AddItem("License", item.License == null ? "" : item.License.ToString());
                    json.AddItem("PurchaseDate", item.PurchaseDate == null ? "" : item.PurchaseDate.ToString());
                    json.AddItem("Quality", item.Quality == null ? "" : item.Quality.ToString());
                    json.AddItem("Specification", item.Specification == null ? "" : item.Specification.ToString());
                    json.AddItem("Supplier", item.Supplier == null ? "" : item.Supplier.ToString());
                    json.AddItem("Unit", item.Unit == null ? "":item.Unit.ToString());
                    json.AddItem("ValidityTerm", item.ValidityTerm == null ? "" : item.ValidityTerm.ToString());
                    json.ItemOk();
                }
                strJson = json.ToEasyuiOneModelJsonString();
                return Content(strJson);
                
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
        }
        #endregion
       
    }
}
