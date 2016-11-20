using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    
    [Table("Article")]
    public class Article
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
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }
    }
}
