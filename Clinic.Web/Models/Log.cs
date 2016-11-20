using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int ID { get; set; }
        
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }

}
