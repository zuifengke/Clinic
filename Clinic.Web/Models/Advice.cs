using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    
    [Table("Advice")]
    public class Advice
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Contact { get; set; }
    }
}
