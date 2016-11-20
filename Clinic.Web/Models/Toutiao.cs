using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    
    [Table("Toutiao")]
    public class Toutiao
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
        /// 转载地址
        /// </summary>
        public string ReprintUrl { get; set; }
        /// <summary>
        /// 是否公开 0:不公开 1:公开
        /// </summary>
        public int IsPublic { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 文章ID号
        /// </summary>
        public int ArticleID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public int MemberID { get; set; }
        [NotMapped]
        public string MemberName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 是否转载 0：否 1：是
        /// </summary>
        public int Zhuanzai { get; set; }
    }
}
