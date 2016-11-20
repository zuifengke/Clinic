using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Demand")]
    public class Demand
    {
        [Key]
        public int ID { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }
        public string Creater { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Owener { get; set; }
        public DateTime SoluteTime { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string FileAttach { get; set; }
        public string Expense { get; set; }
        public string Question { get; set; }
        public string Solution { get; set; }
    }

}
