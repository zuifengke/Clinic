// ***********************************************************
// 封装对DataSet及DataTable两个对象的操作方法集合
// Creator:YangMingkun  Date:2013-8-5
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Windy.WebMVC.Web2.Utility
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装对象转换方法
        /// </summary>
        public struct Table
        {
            /// <summary>
            /// 从指定的DataTable复制一份同样结构的对象.
            /// 但不包含数据
            /// </summary>
            /// <param name="source">源DataTable</param>
            /// <returns>DataTable</returns>
            public static DataTable CloneTable(DataTable source)
            {
                DataTable table = new DataTable();
                if (source != null)
                {
                    foreach (DataColumn column in source.Columns)
                        table.Columns.Add(column.ColumnName, column.DataType);
                }
                return table;
            }

            /// <summary>
            /// 在指定的数据集中,获取指定行和列索引的数据
            /// </summary>
            /// <param name="data">数据集</param>
            /// <param name="row">行索引</param>
            /// <param name="column">列索引</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataSet data, int row, int column)
            {
                if (data == null || data.Tables.Count <= 0)
                    return string.Empty;
                return GlobalMethods.Table.GetFieldValue(data.Tables[0], row, column);
            }

            /// <summary>
            /// 在指定的数据集中,获取指定行和列名称的数据
            /// </summary>
            /// <param name="data">数据集</param>
            /// <param name="row">行索引</param>
            /// <param name="column">列名称</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataSet data, int row, string column)
            {
                if (data == null || data.Tables.Count <= 0)
                    return string.Empty;
                return GlobalMethods.Table.GetFieldValue(data.Tables[0], row, column);
            }

            /// <summary>
            /// 在指定的数据行和列索引的数据
            /// </summary>
            /// <param name="row">行索引</param>
            /// <param name="column">列索引</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataRow row, int column)
            {
                if (row == null)
                    return string.Empty;
                object value = null;
                try
                {
                    if (!row.IsNull(column))
                        value = row[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// 在指定的数据行和列名称的数据
            /// </summary>
            /// <param name="row">行索引</param>
            /// <param name="column">列名称</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataRow row, string column)
            {
                if (row == null)
                    return string.Empty;
                object value = null;
                try
                {
                    if (!row.IsNull(column))
                        value = row[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// 在指定的数据集中,获取指定行和列索引的数据
            /// </summary>
            /// <param name="table">数据集</param>
            /// <param name="row">行索引</param>
            /// <param name="column">列索引</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataTable table, int row, int column)
            {
                if (table == null)
                    return string.Empty;
                if (row < 0 || row >= table.Rows.Count)
                    return string.Empty;
                if (column < 0 || column >= table.Columns.Count)
                    return string.Empty;

                object value = null;
                try
                {
                    DataRow dataRow = table.Rows[row];
                    if (!dataRow.IsNull(column))
                        value = dataRow[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

            /// <summary>
            /// 在指定的数据集中,获取指定行和列名称的数据
            /// </summary>
            /// <param name="table">数据集</param>
            /// <param name="row">行索引</param>
            /// <param name="column">列名称</param>
            /// <returns>单元格数据</returns>
            public static object GetFieldValue(DataTable table, int row, string column)
            {
                if (table == null)
                    return string.Empty;
                if (column == null)
                    column = "";
                if (row < 0 || row >= table.Rows.Count)
                    return string.Empty;

                object value = null;
                try
                {
                    DataRow dataRow = table.Rows[row];
                    if (!dataRow.IsNull(column))
                        value = dataRow[column];
                }
                catch { return string.Empty; }
                if (value == DBNull.Value || GlobalMethods.Misc.IsEmptyString(value))
                    return string.Empty;
                return value;
            }

           

            /// <summary>
            /// 将指定的对象实例的属性列表转换为DataTable对象
            /// </summary>
            /// <param name="instance">对象实例</param>
            /// <returns>DataTable对象</returns>
            public static DataTable GetDataTable(object instance)
            {
                if (instance == null)
                    return null;

                IEnumerable enumerableObject = instance as IEnumerable;
                if (enumerableObject != null)
                    return GetDataTableFromList(enumerableObject);

                DataTable table = new DataTable();
                PropertyInfo[] properties = instance.GetType().GetProperties();
                List<object> values = new List<object>();
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanRead)
                        continue;
                    if (table.Columns.Contains(property.Name))
                        continue;
                    table.Columns.Add(property.Name, property.PropertyType);
                    values.Add(GlobalMethods.Reflect.GetPropertyValue(instance, property));
                }
                table.Rows.Add(values.ToArray());
                return table;
            }

            /// <summary>
            /// 将指定的对象实例的属性列表转换为DataTable对象
            /// </summary>
            /// <param name="list">对象实例</param>
            /// <returns>DataTable对象</returns>
            private static DataTable GetDataTableFromList(IEnumerable list)
            {
                if (list == null)
                    return null;

                IEnumerator enumerator = list.GetEnumerator();
                if (enumerator == null)
                    return null;

                DataTable table = new DataTable();
                while (enumerator.MoveNext())
                {
                    object instance = enumerator.Current;
                    PropertyInfo[] properties = instance.GetType().GetProperties();
                    List<object> values = new List<object>();
                    foreach (PropertyInfo property in properties)
                    {
                        if (!property.CanRead)
                            continue;
                        if (table.Rows.Count <= 0 && !table.Columns.Contains(property.Name))
                            table.Columns.Add(property.Name, property.PropertyType);
                        values.Add(GlobalMethods.Reflect.GetPropertyValue(instance, property));
                    }
                    table.Rows.Add(values.ToArray());
                }
                return table;
            }

            /// <summary>
            /// 将指定的DataRow对象里保存的数据转换到对象实例中
            /// </summary>
            /// <param name="row">DataRow对象</param>
            /// <param name="instance">对象实例</param>
            /// <returns>是否成功</returns>
            public static bool DataRowToObject(DataRow row, object instance)
            {
                if (row == null || row.Table == null || instance == null)
                    return false;
                DataColumnCollection columns = row.Table.Columns;
                if (columns == null || columns.Count <= 0)
                    return false;
                PropertyInfo[] properties = instance.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanWrite)
                        continue;

                    foreach (DataColumn column in columns)
                    {
                        if (string.Compare(column.ColumnName, property.Name, false) != 0)
                            continue;
                        object propertyValue = null;
                        if (!row.IsNull(column))
                            propertyValue = row[column];
                        GlobalMethods.Reflect.SetPropertyValue(instance, property, propertyValue);
                        break;
                    }
                }
                return true;
            }

            /// <summary>
            /// 把DataTable的数据导出到为CSV文件输出
            /// </summary>
            /// <param name="table">需要转换的Table数据</param>
            /// <param name="fileName">CSV文件输出全路径</param>
            /// <returns>是否转换成功</returns>
            public static bool ExportCsvFile(DataTable table, string fileName)
            {
                if (table == null || GlobalMethods.Misc.IsEmptyString(fileName))
                    return false;

                if (!GlobalMethods.IO.DeleteFile(fileName))
                    return false;

                StringBuilder sbRowColumnText = new StringBuilder();

                //导出列集合 
                for (int index = 0; index < table.Columns.Count; index++)
                {
                    if (index > 0)
                        sbRowColumnText.Append(",");
                    string columnName = table.Columns[index].ColumnName;
                    columnName = GlobalMethods.Convert.ReplaceText(columnName, new string[] { "\"" }, new string[] { "\"\"" });
                    sbRowColumnText.AppendFormat("\"{0}\"", columnName);
                }
                sbRowColumnText.AppendLine();

                //导出行数据   
                foreach (DataRow row in table.Rows)
                {
                    for (int index = 0; index < table.Columns.Count; index++)
                    {
                        if (index > 0)
                            sbRowColumnText.Append(",");
                        string cellValue = GlobalMethods.Table.GetFieldValue(row, index).ToString();
                        cellValue = GlobalMethods.Convert.ReplaceText(cellValue, new string[] { "\"" }, new string[] { "\"\"" });
                        sbRowColumnText.AppendFormat("\"{0}\"", cellValue);
                    }
                    sbRowColumnText.AppendLine();
                }
                return GlobalMethods.IO.WriteFileText(fileName, sbRowColumnText.ToString());
            }
        }
    }
}
