using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string MenuName { get; set; }
        [NotMapped]
        public string ParentName { get; set; }
        public int ParentID { get; set; }
        [Column(TypeName = "varchar")]
        public string MenuType { get; set; }
        [Column(TypeName = "varchar")]
        public string Description { get; set; }
        [Column(TypeName = "varchar")]
        public string Icon { get; set; }
        [Column(TypeName = "varchar")]
        public string Url { get; set; }
    }
}
