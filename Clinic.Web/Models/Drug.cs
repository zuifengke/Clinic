using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Drug")]
    public class Drug
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
    }

}
