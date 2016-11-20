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
    [Table("OAuthUser")]
    public class OAuthUser
    {
        [Key]
        public int ID { get; set; }
        public string OpenID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public string NickName { get; set; }
        public string Province { get; set; }
        public int Sex { get; set; }
        [NotMapped]
        public string Tel { get; set; }
        public string Mail { get; set; } 
        public string Name { get; set; }
        public string Pwd { get; set; }
        public DateTime CreateTime { get; set; }
        public int MemberID { get; set; }
    }

}
