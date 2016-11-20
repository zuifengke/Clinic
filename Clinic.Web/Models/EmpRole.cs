using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("EmpRole")]
    public class EmpRole
    {
        [Key]
        [ColumnAttribute(Order = 0)]
        public int EmpID { get; set; }
        [Key]
        [ColumnAttribute(Order =1)]
        public int RoleID { get; set; }
       
    }

}
