using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    /// <summary>
    /// 药械购进验收记录
    /// </summary>
    [Table("DrugPurchase")]
    public class DrugPurchase
    {
        [Key]
        public int ID { get; set; }
        public int DrugID { get; set; }
        [NotMapped]
        public string DrugName { get; set; }
        /// <summary>
        /// 购货日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }
        /// <summary>
        /// 供货单位
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 规格（型号）
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Factory { get; set; }
        /// <summary>
        /// 批号（编号）（生产日期）（灭菌批号）
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidityTerm { get; set; }
        /// <summary>
        /// 批准文号（注册证号）
        /// </summary>
        public string License { get; set; }
        /// <summary>
        /// 检验报告书/通关单
        /// </summary>
        public string InspectionReport { get; set; }
        /// <summary>
        /// 中文说明书
        /// </summary>
        public string Instructions { get; set; }
        /// <summary>
        /// 质量状况
        /// </summary>
        public string Quality { get; set; }
        /// <summary>
        /// 验收结论
        /// </summary>
        public string AcceptanceConclusion { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public string Buyer { get; set; }
        /// <summary>
        /// 验收员
        /// </summary>
        public string Examiner { get; set; }
    }

}
