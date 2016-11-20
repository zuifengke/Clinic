using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class ZyldingfangContext : DbContext
    {
        public ZyldingfangContext(string connectionString)
            : base(connectionString) {
            this.Database.Initialize(false);
            this.Database.CommandTimeout = 180;
        }

        public DbSet<RoomOrder> RoomOrder { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<EmpMenu> EmpMenu { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<ExamPlace> ExamPlace { get; set; }
        public DbSet<EmpOrg> EmpOrg { get; set; }
        public DbSet<EmpRole> EmpRole { get; set; }
        public DbSet<Orgnization> Orgnization { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Demand> Demand { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<OAuthUser> OAuthUser { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Advice> Advice { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<QQUser> QQUser { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Toutiao> Toutiao { get; set; }
        public DbSet<Train> Train { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Advert> Advert { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Invite> Invite { get; set; }
        public DbSet<Lottery> Lottery { get; set; }
        public DbSet<Drug> Drug { get; set; }
        public DbSet<DrugPurchase> DrugPurchase { get; set; }
    }
}