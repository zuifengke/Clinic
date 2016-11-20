using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    /// <summary>
    /// 广告推广
    /// </summary>
    [Table("Advert")]
    public class Advert
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string Title { get; set; }
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
        /// 点击后跳转路径
        /// </summary>
        [Column(TypeName = "varchar")]
        public string ClickUrl { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Content { get; set; }



    }
}
