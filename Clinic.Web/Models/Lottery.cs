using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    /// <summary>
    /// 抽奖
    /// </summary>
    [Table("Lottery")]
    public class Lottery
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string Tel { get; set; }
        /// <summary>
        /// 中奖情况
        /// </summary>
        public string Prize { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string School { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
