using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    
    [Table("product")]
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string Title { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }
        public int CategoryID { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public string CategoryCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public int CreateID { get; set; }
        [NotMapped]
        public string CreateName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifyTime { get; set; }
        public int ModifyID { get; set; }
        [NotMapped]
        public string ModifyName { get; set; }
        /// <summary>
        /// 内容中的第一张图
        /// </summary>
        [Column(TypeName = "varchar")]
        public string ImagePath { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Summary { get; set; }
        /// <summary>
        /// 推广链接
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Url { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public float DisCount { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; }
        public int ArticleID { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }
        public string TaobaoID { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public float Income { get; set;}
        public float Percent { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        public int Sales { get; set; }
        /// <summary>
        /// 宝贝详情描述链接
        /// </summary>
        [Column(TypeName = "varchar")]
        public string DetailUrl { get; set; }


    }
}
