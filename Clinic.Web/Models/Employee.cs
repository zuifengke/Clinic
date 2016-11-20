using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string EmpNo { get; set; }
        [Column(TypeName = "varchar")]
        public string Pwd { get; set; }
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        [Column(TypeName = "varchar")]
        public string Tel { get; set; }
        [NotMapped]
        public string RoleIDs { get; set; }
        [NotMapped]
        public string RoleNames { get; set; }
    }

}
