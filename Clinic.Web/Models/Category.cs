using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public int ParentID { get; set; }
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        [NotMapped]
        public string ParentName { get; set; }
        [Column(TypeName = "varchar")]
        public string Code { get; set; }
    }

}
