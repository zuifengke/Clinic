using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class DrugPurchaseRepository : BaseRepository<Models.DrugPurchase>
    {
        /// <summary>
        /// 加载管理员列表
        /// </summary>
        /// <param name="roleId">权限Id</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<DrugPurchase> LoadPageList(string Name, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Models.DrugPurchase>()
                         join a in db.Set<Drug>() on new { DrugID = p.DrugID } equals new { DrugID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         select new
                         {
                             p.AcceptanceConclusion,
                             DrugName = a.Name,
                             p.Amount,
                             p.Batch,
                             p.Buyer,
                             p.ID,
                             p.DrugID,
                             p.Examiner,
                             p.Factory,
                             p.InspectionReport,
                             p.Instructions,
                             p.License,
                             p.PurchaseDate,
                             p.Quality,
                             p.Specification,
                             p.Supplier,
                             p.Unit,
                             p.ValidityTerm
                         };
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.DrugName.Contains(Name));
            }
            rowCount = result.Count();
            result = result.OrderByDescending(m => m.PurchaseDate).Skip(startNum).Take(pageSize);
            return result.ToList().Select(m => new DrugPurchase()
            {
                ID = m.ID,
                AcceptanceConclusion = m.AcceptanceConclusion,
                Amount = m.Amount,
                Batch = m.Batch,
                Buyer = m.Buyer,
                PurchaseDate = m.PurchaseDate,
                ValidityTerm = m.ValidityTerm,
                DrugID = m.DrugID,
                DrugName = m.DrugName,
                Examiner = m.Examiner,
                Factory = m.Factory,
                InspectionReport = m.InspectionReport,
                Instructions = m.Instructions,
                License = m.License,
                Quality = m.Quality,
                Specification = m.Specification,
                Supplier = m.Supplier,
                Unit = m.Unit
            });
        }
    }
}