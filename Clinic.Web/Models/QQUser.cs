using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    /// <summary>
    /// 微信授权用户
    /// </summary>
    [Table("QQUser")]
    public class QQUser
    {
        [Key]
        public int ID { get; set; }
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public int Level { get; set; }
        public int Vip { get; set; }
        public string Gender { get; set; }
        public string Figureurl { get; set; } 
        public int MemberID { get; set; }
        public DateTime CreateTime { get; set; }
    }

}
