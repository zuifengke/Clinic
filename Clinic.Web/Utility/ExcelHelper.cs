using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace Windy.WebMVC.Web2.Utility
{

    public class ExcelRow
    {
        Dictionary<string, object> columns;

        public ExcelRow()
        {
            columns = new Dictionary<string, object>();
        }

        internal void AddColumn(string key, object value)
        {
            columns.Add(key, value);
        }

        public object this[int index]
        {
            get { return columns.Values.Skip(index).First(); }
        }

        public string GetString(int index)
        {
            if (columns.Values.Skip(index).First() is DBNull)
            {
                return null;
            }
            return columns.Values.Skip(index).First().ToString();
        }

        public object this[string colunmName]
        {
            get { return columns[colunmName]; }
        }

        public string GetString(string colunmName)
        {
            if (columns[colunmName] is DBNull)
            {
                return null;
            }
            return columns[colunmName].ToString();
        }

        public int Count
        {
            get { return this.columns.Count; }
        }
    }


    public class ExcelProvider : IEnumerable<ExcelRow>
    {
        private string sheet;
        private string filePath;
        private List<ExcelRow> rows;


        public ExcelProvider()
        {
            rows = new List<ExcelRow>();
        }

        public static ExcelProvider Create(string filePath, string sheet)
        {
            ExcelProvider provider = new ExcelProvider();
            provider.sheet = sheet;
            provider.filePath = filePath;
            return provider;
        }

        private void Load()
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;""";
            connectionString = string.Format(connectionString, filePath);
            rows.Clear();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from [" + sheet + "$]";
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExcelRow newRow = new ExcelRow();
                            for (int count = 0; count < reader.FieldCount; count++)
                            {
                                newRow.AddColumn(reader.GetName(count), reader[count]);
                            }
                            rows.Add(newRow);
                        }
                    }
                }
            }
        }

        public IEnumerator<ExcelRow> GetEnumerator()
        {
            Load();
            return rows.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Load();
            return rows.GetEnumerator();
        }

        /// <summary>
        /// 創建表單
        /// </summary>
        /// <param name="createText"></param>
        /// <returns></returns>
        public bool CreateTable(string createText)
        {
            if (File.Exists(this.filePath))
            {
                File.Delete(this.filePath);
            }
            return ExecuteCommand(createText);
        }

        /// <summary>
        /// 執行語句
        /// </summary>
        /// <param name="createText"></param>
        /// <returns>是否執行成功</returns>
        public bool ExecuteCommand(string commandText)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;""";
            connectionString = string.Format(connectionString, filePath);
            OleDbConnection oleDBconn = new OleDbConnection(connectionString);
            OleDbCommand oleDBcomm = new OleDbCommand(commandText, oleDBconn);

            try
            {
                oleDBconn.Open();
                oleDBcomm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oleDBconn.Close();
                oleDBconn.Dispose();
            }
        }
    }

    /// <summary>
    /// Excel操作類
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="columnName">列名</param>
        /// <param name="values">值</param>
        /// <param name="valuesType">值的數據類型</param>
        /// <returns>是否生成成功</returns>
        public bool ToExcel(string path, string[] columnName, List<string[]> values)
        {
            // NPOI基本對象
            HSSFWorkbook hsswk = new HSSFWorkbook();
            ISheet eSheet;          // 表單
            IRow eRow;              // 數據行
            ICell eCell;            // 單元格

            // 每張sheet最多65535條數據
            int maxData = 65534;
            int sheetCount = (values.Count - 1) / maxData + 1;
            int dateIndex = 0;

            for (int i = 1; i <= sheetCount; i++)
            {
                #region 生成表單
                eSheet = hsswk.CreateSheet("Sheet" + i.ToString());
                #endregion 生成表單
                #region 生成表頭
                eRow = eSheet.CreateRow(0);

                for (int j = 0; j < columnName.Length; j++)
                {
                    eCell = eRow.CreateCell(j, CellType.String);
                    eCell.SetCellValue(columnName[j]);
                }
                #endregion 生成表頭
                #region 插入數據
                // 數據行的處理
                for (int j = 0; j < maxData && dateIndex < values.Count; j++)
                {
                    // 如果取到值的上限,則退出循環
                    if (j == values.Count)
                    {
                        break;
                    }
                    eRow = eSheet.CreateRow(j + 1);
                    // 單元格的處理
                    for (int k = 0; k < values[dateIndex].Length; k++)
                    {
                        eCell = eRow.CreateCell(k, CellType.String);
                        eCell.SetCellValue(values[dateIndex][k]);
                    }
                    dateIndex++;
                }
                #endregion 插入數據
            }
            string directoryPath = StringOperation.GetFileDirectory(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            FileStream file = new FileStream(path, FileMode.Create);
            try
            {
                hsswk.Write(file);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                file.Close();
            }
            return true;
        }
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="columnName">列名</param>
        /// <param name="values">值</param>
        /// <param name="valuesType">值的數據類型</param>
        /// <returns>是否生成成功</returns>
        public bool ToExcel(string path, string[] columnName, List<ArrayList> values)
        {
            // NPOI基本對象
            HSSFWorkbook hsswk = new HSSFWorkbook();
            ISheet eSheet;          // 表單
            IRow eRow;              // 數據行
            ICell eCell;            // 單元格

            // 每張sheet最多65535條數據
            int maxData = 65534;
            int sheetCount = (values.Count - 1) / maxData + 1;
            int dateIndex = 0;

            for (int i = 1; i <= sheetCount; i++)
            {
                #region 生成表單
                eSheet = hsswk.CreateSheet("Sheet" + i.ToString());
                #endregion 生成表單
                #region 生成表頭
                eRow = eSheet.CreateRow(0);

                for (int j = 0; j < columnName.Length; j++)
                {
                    eCell = eRow.CreateCell(j, CellType.String);
                    eCell.SetCellValue(columnName[j]);
                }
                #endregion 生成表頭
                #region 插入數據
                // 數據行的處理
                for (int j = 0; j < maxData && dateIndex < values.Count; j++)
                {
                    // 如果取到值的上限,則退出循環
                    if (j == values.Count)
                    {
                        break;
                    }
                    eRow = eSheet.CreateRow(j + 1);
                    // 單元格的處理
                    for (int k = 0; k < values[dateIndex].Count; k++)
                    {

                        try
                        {
                            eCell = eRow.CreateCell(k, CellType.String);
                            if (values[dateIndex][k] == null)
                            {
                                eCell.SetCellValue(""); continue;
                            }

                            eCell.SetCellValue(values[dateIndex][k].ToString());
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    dateIndex++;
                }
                #endregion 插入數據
            }
            string directoryPath = StringOperation.GetFileDirectory(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            FileStream file = new FileStream(path, FileMode.Create);
            try
            {
                hsswk.Write(file);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                file.Close();
            }
            return true;
        }
    }
}
