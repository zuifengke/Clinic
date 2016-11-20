using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("RoomOrder")]
    public class RoomOrder
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        public string Telphone { get; set; }

        [Column(TypeName = "varchar")]
        public string School { get; set; }

        [Column(TypeName = "varchar")]
        public string ExamType { get; set; }
        public DateTime? SubmitTime { get; set; }

        [Column(TypeName = "varchar")]
        public string ShortTel { get; set; }
    }

}
