using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    [Table("Invite")]
    public class Invite
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "varchar")]
        public string InviteTel { get; set; }
        [NotMapped]
        public string InviteName { get; set; }
        [NotMapped]
        public string InviteSchool { get; set; }
        [Column(TypeName = "varchar")]
        public string BeInviteTel { get; set; }
        [NotMapped]
        public string BeInviteName { get; set; }
        [NotMapped]
        public string BeInviteSchool { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        public DateTime? CreateTime { get; set; }
    }

}
