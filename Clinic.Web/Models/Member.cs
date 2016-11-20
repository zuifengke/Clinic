using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Models
{
    /// <summary>
    /// 会员实体类
    /// </summary>
    [Table("Member")]
    public class Member
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(TypeName = "varchar")]
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column(TypeName = "varchar")]
        public string RealName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Password { get; set; }
        /// <summary>
        /// 性别 1:男性 2:女性
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// qq
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 0:正常 1:删除
        /// </summary>
        public int Deleted { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
    }
}
