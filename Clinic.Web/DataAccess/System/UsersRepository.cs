using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class UsersRepository : BaseRepository<Models.Users>
    {

        /// <summary>
        /// 加载管理员列表
        /// </summary>
        /// <param name="roleId">权限Id</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Users> LoadPageList(int searchEmpid, int curEmpID, string Tel, string Name, int startNum, int pageSize, out int rowCount)
        {
            rowCount = 0;
            var result = from p in db.Set<Users>()
                         join a in db.Set<EmpOrg>() on new { EmpID = p.EmployeeID } equals new { EmpID = a.EmpID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<EmpOrg>() on new { OrgID = a.OrgID } equals new { OrgID = b.OrgID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { EmpID = p.EmployeeID } equals new { EmpID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         join d in db.Set<Lottery>() on new { Tel=p.Tel}equals new { Tel=d.Tel} into d_into
                         from d in d_into.DefaultIfEmpty()
                         where b.EmpID == curEmpID
                         select new
                         {
                             p.Baks,
                             p.EmployeeID,
                             p.ExamPlace,
                             p.ExamSchool,
                             p.ExceptRoomie,
                             p.Gender,
                             p.CreateTime,
                             p.Hotel,
                             p.HotelExpense,
                             p.ID,
                             p.MoneyBack,
                             p.Name,
                             p.PayMoney,
                             p.PayPlace,
                             p.Pwd,
                             p.Room,
                             p.School,
                             p.Sequences,
                             p.Status,
                             p.Tel,
                             p.Template,
                             EmployeeName = c.Name,
                             Prize =d.Prize
                         };
            if (searchEmpid > 0)
            {
                result = result.Where(m => m.EmployeeID == searchEmpid);
            }
            if (!string.IsNullOrEmpty(Tel))
            {
                result = result.Where(m => m.Tel.Contains(Tel));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.Name.Contains(Name));
            }
            rowCount = result.Count();
            result = result.OrderBy(m => m.EmployeeID).ThenBy(m=>m.Sequences).Skip(startNum).Take(pageSize);

            string sql = result.ToString();
            return result.ToList().Select(m => new Users() { Baks = m.Baks, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, ExamPlace = m.ExamPlace, ExamSchool = m.ExamSchool, ExceptRoomie = m.ExceptRoomie, Gender = m.Gender, Hotel = m.Hotel, HotelExpense = m.HotelExpense, ID = m.ID, MoneyBack = m.MoneyBack, Name = m.Name, PayMoney = m.PayMoney, PayPlace = m.PayPlace, Pwd = m.Pwd, Room = m.Room, School = m.School, Sequences = m.Sequences, Status = m.Status, Tel = m.Tel, Template = m.Template,CreateTime=m.CreateTime,Prize=m.Prize }); 
        }
        /// <summary>
        /// 加载管理员列表
        /// </summary>
        /// <param name="roleId">权限Id</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startNum">起始数字</param>
        /// <param name="pageSize">页长</param>
        /// <param name="IsDesc">是否倒序排列</param>
        /// <param name="rowCount">总个数</param>
        /// <returns></returns>
        public IEnumerable<Users> GetUsersList(int searchEmpid, int curEmpID, string Tel, string Name)
        {
            var result = from p in db.Set<Users>()
                         join a in db.Set<EmpOrg>() on new { EmpID = p.EmployeeID } equals new { EmpID = a.EmpID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         join b in db.Set<EmpOrg>() on new { OrgID = a.OrgID } equals new { OrgID = b.OrgID } into b_into
                         from b in b_into.DefaultIfEmpty()
                         join c in db.Set<Employee>() on new { EmpID = p.EmployeeID } equals new { EmpID = c.ID } into c_into
                         from c in c_into.DefaultIfEmpty()
                         join d in db.Set<Lottery>() on new { Tel = p.Tel } equals new { Tel = d.Tel } into d_into
                         from d in d_into.DefaultIfEmpty()
                         where b.EmpID == curEmpID
                         select new
                         {
                             p.Baks,
                             p.EmployeeID,
                             p.ExamPlace,
                             p.ExamSchool,
                             p.ExceptRoomie,
                             p.Gender,
                             p.Hotel,
                             p.HotelExpense,
                             p.ID,
                             p.MoneyBack,
                             p.Name,
                             p.PayMoney,
                             p.PayPlace,
                             p.Pwd,
                             p.Room,
                             p.School,
                             p.Sequences,
                             p.Status,
                             p.Tel,
                             p.Template,
                             p.CreateTime,
                             EmployeeName = c.Name,
                             Prize=d.Prize
                         };
            if (searchEmpid > 0)
            {
                result = result.Where(m => m.EmployeeID == searchEmpid);
            }
            if (!string.IsNullOrEmpty(Tel))
            {
                result = result.Where(m => m.Tel.Contains(Tel));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(m => m.Name.Contains(Name));
            }

            result = result.OrderBy(m => m.EmployeeID).ThenBy(m => m.Sequences);
            
            return result.ToList().Select(m => new Users() { Baks = m.Baks, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, ExamPlace = m.ExamPlace, ExamSchool = m.ExamSchool, ExceptRoomie = m.ExceptRoomie, Gender = m.Gender, Hotel = m.Hotel, HotelExpense = m.HotelExpense, ID = m.ID, MoneyBack = m.MoneyBack, Name = m.Name, PayMoney = m.PayMoney, PayPlace = m.PayPlace, Pwd = m.Pwd, Room = m.Room, School = m.School, Sequences = m.Sequences, Status = m.Status, Tel = m.Tel, Template = m.Template,CreateTime=m.CreateTime,Prize=m.Prize });
        }
    }
}