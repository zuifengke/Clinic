using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Windy.WebMVC.Web2.EFDao
{
    public class EFSqlHelper
    {
#if DEBUG
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZyldingfangContext_debug"].ToString();

#else
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZyldingfangContext"].ToString();

#endif
        /// <summary>
        /// 通过EF执行sql语句获得Table
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable SqlQueryForDataTatable(
                string sql,
                SqlParameter[] parameters)
        {
            SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = connectionString;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            if (parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn.Close();
            return table;
        }
        /// <summary>
        /// 通过EF执行sql语句获得Table
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static DataTable SqlQueryForDataTatable(
        string sql)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionString;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn.Close();
            return table;
        }
        /// <summary>
        /// 执行sql增删改语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static bool ExecuteSqlCommand(DbContext db, string sql, SqlParameter[] parameters)
        {
            return db.Database.ExecuteSqlCommand(sql, parameters) > 0;
        }
        /// <summary>
        /// 执行sql增删改语句
        /// </summary>
        /// <param name="db">要执行的sql语句</param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSqlCommand(DbContext db, string sql)
        {
            return db.Database.ExecuteSqlCommand(sql) > 0;
        }
    }
}