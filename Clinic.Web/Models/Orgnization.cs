using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
namespace Windy.WebMVC.Web2.Models
{
    [Table("Orgnization")]
    public class Orgnization
    {
        [Key]
        public int ID { get; set; }
        public string OrgName { get; set; }
        public int ParentID { get; set; }
        [NotMapped]
        public string ParentName { get; set; }
        public string Description { get; set; }
    }

}
