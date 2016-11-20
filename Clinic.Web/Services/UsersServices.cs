using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class UsersServices
    {
        public static IEnumerable<Models.Employee> GetStaffEmployee(int id)
        {
            var lstEmployees = EnterRepository.GetRepositoryEnter().EmployeeRepository.GetStaffEmpployee(id);

            return lstEmployees;
        }

        public static bool Import(string filePath, string szTemplateValue)
        {
            try
            {
                GlobalMethod.log.Info("开始导入");
                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.AutoDetectChangesEnabled = false;
                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.ValidateOnSaveEnabled = false;
                Hashtable htUsers = new Hashtable();
                var lstUsers = EnterRepository.GetRepositoryEnter().UsersRepository.LoadEntities().ToList();
                foreach (var item in lstUsers)
                {
                    if (!htUsers.ContainsKey(item.Tel))
                        htUsers.Add(item.Tel, item.ID);
                }
                var lstEmployee = EnterRepository.GetRepositoryEnter().EmployeeRepository.LoadEntities().ToList();
                Hashtable htEmployee = new Hashtable();
                foreach (var item in lstEmployee)
                {
                    if (htEmployee.Contains(item.Name))
                        continue;
                    htEmployee.Add(item.Name, item.ID);
                }
                string[] values = szTemplateValue.Split(',');
                ExcelProvider excelProvider = ExcelProvider.Create(filePath, "Sheet1");
                int passCount = 0;
                int importCount = 0;
                foreach (ExcelRow row in excelProvider)
                {
                    int ID = 0;
                    Models.Users user = new Models.Users();
                    for (int index = 0; index < values.Length; index++)
                    {
                        switch (values[index])
                        {
                            case "网报序号":
                                if (!string.IsNullOrEmpty(row.GetString("网报序号")))
                                    user.ID = int.Parse(row.GetString(values[index]));
                                break;
                            case "多退少补":
                                user.MoneyBack = row.GetString(values[index]);
                                break;
                            case "酒店房价":
                                user.HotelExpense = row.GetString(values[index]);
                                break;
                            case "酒店":
                                user.Hotel = row.GetString(values[index]);
                                break;
                            case "房号":
                                user.Room = row.GetString(values[index]);
                                break;
                            case "已交款额":
                                user.PayMoney = row.GetString(values[index]);
                                break;
                            case "备注":
                                user.Baks = row.GetString(values[index]);
                                break;
                            case "联系方式":
                                user.Tel = row.GetString(values[index]);
                                break;
                            case "网报密码":
                                user.Pwd = row.GetString(values[index]);
                                break;
                            case "所在学校":
                                user.School = row.GetString(values[index]);
                                break;
                            case "性别":
                                user.Gender = row.GetString(values[index]);
                                break;
                            case "报名次序":
                                string sequences = row.GetString(values[index]);
                                if (string.IsNullOrEmpty(sequences))
                                    sequences = "0";
                                user.Sequences = int.Parse(sequences);
                                break;
                            case "业务员":
                                user.EmployeeName = row.GetString(values[index]);
                                break;
                            case "意向同住人":
                                user.ExceptRoomie = row.GetString(values[index]);
                                break;
                            case "报考类型":
                                user.Template = row.GetString(values[index]);
                                break;
                            case "收缴余款所在地":
                                user.PayPlace = row.GetString(values[index]);
                                break;
                            case "所报学校":
                                user.ExamSchool = row.GetString(values[index]);
                                break;
                            case "姓名":
                                user.Name = row.GetString(values[index]);
                                break;
                            case "提交考点":
                                user.ExamPlace = row.GetString(values[index]);
                                break;
                            default:
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(user.Tel))
                    {
                        passCount++;
                        GlobalMethod.log.Warn(string.Format("导入excel,考生{0}号码为空，跳过", user.Name));
                        continue;
                    }
                    object szEmployeeID = htEmployee[user.EmployeeName];
                    if (szEmployeeID == null)
                    {
                        passCount++;
                        GlobalMethod.log.Warn(string.Format("导入excel,考生{0}的业务员姓名未能在系统内找到，跳过", user.Name));
                        continue;
                    }
                    user.EmployeeID = szEmployeeID == null ? 0 : int.Parse(szEmployeeID.ToString());
                    if (!htUsers.ContainsKey(user.Tel))
                    {
                        user.CreateTime = DateTime.Now;
                        EnterRepository.GetRepositoryEnter().UsersRepository.AddEntity(user);
                    }
                    else
                    {
                        user.ID = int.Parse(htUsers[user.Tel].ToString());
                        EnterRepository.GetRepositoryEnter().UsersRepository.EditEntity(user, new string[] { "EmployeeID","Name", "School", "ExamSchool", "Sequences", "Tel", "Baks", "Pwd", "PayMoney", "ExamPlace", "Room", "Hotel", "HotelExpense", "MoneyBack", "Gender", "Template", "PayPlace", "ExceptRoomie", "Status" });
                    }
                }
                importCount = EnterRepository.GetRepositoryEnter().SaveChange();
                GlobalMethod.log.Info(string.Format("本次导入成功，共导入{0}考生,应数据异常跳过{1}个"
                       , importCount.ToString()
                       , passCount.ToString()));
                return true;

            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
            finally
            {

                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.AutoDetectChangesEnabled = true;
                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.ValidateOnSaveEnabled = true;
            }
        }
    }
}