using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("News")]
    public class News
    {
        [Key]
        public int ID { get; set; }
        public  string NewsTitle { get; set; }
        public string NewsContent  { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string ModifyUser { get; set; }
        public DateTime ModifyTime { get; set; }
        public string CategoryName { get; set; }

    }

}
