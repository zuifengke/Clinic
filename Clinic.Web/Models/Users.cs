using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
namespace Windy.WebMVC.Web2.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public string ExamSchool { get; set; }
        public int Sequences { get; set; }
        public string Tel { get; set; }
        public string Baks { get; set; }
        public string Pwd { get; set; }
        public string PayMoney { get; set; }
        public string ExamPlace { get; set; }
        public string Room { get; set; }
        public string Hotel { get; set; }
        public string HotelExpense { get; set; }
        public string MoneyBack { get; set; }
        public int EmployeeID { get; set; }
        public string Gender { get; set; }
        public string Template { get; set; }
        public string PayPlace { get; set; }
        public string ExceptRoomie { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string Prize { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "datetime")] 
        public DateTime? CreateTime { get; set; }

    }

}
