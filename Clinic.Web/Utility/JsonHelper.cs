using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;

namespace Windy.WebMVC.Web2.Utility

{
    /// <summary>
    /// JSONHelper 的摘要说明
    /// </summary>
    public class JsonHelper
    {
        //对应JSON的singleInfo成员
        public string singleInfo = string.Empty;
        protected string _error = string.Empty;
        protected bool _success = true;
        protected int _totalCount = 0;
        protected System.Collections.ArrayList arrData = new ArrayList();
        protected System.Collections.ArrayList arrDataItem = new ArrayList();


        public JsonHelper()
        {

        }

        //对应于JSON的success成员
        public bool success
        {
            get
            {
                return _success;
            }
            set
            {
                //如设置为true则清空error
                if (success) _error = string.Empty;
                _success = value;
            }
        }

        //对应于JSON的error成员
        public string error
        {
            get
            {
                return _error;
            }
            set
            {
                //如设置error，则自动设置success为false
                if (value != "") _success = false;
                _error = value;
            }
        }

        public int totlalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }


        //重置，每次新生成一个json对象时必须执行该方法
        public void Reset()
        {
            _success = true;
            _error = string.Empty;
            singleInfo = string.Empty;
            arrData.Clear();
            arrDataItem.Clear();
        }



        public void AddItem(string name, string value)
        {
            arrData.Add("\"" + name + "\":" + "\"" + value + "\"");
        }



        public void ItemOk()
        {
            arrData.Add("<BR>");
            totlalCount++;
        }

        //序列化JSON对象，得到返回的JSON代码
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("totalCount:" + totlalCount.ToString() + ",");
            sb.Append("success:" + _success.ToString().ToLower() + ",");
            sb.Append("error:\"" + _error.Replace("\"", "\\\"") + "\",");
            sb.Append("singleInfo:\"" + singleInfo.Replace("\"", "\\\"") + "\",");
            sb.Append("data:[");

            int index = 0;
            sb.Append("{");
            if (arrData.Count <= 0)
            {
                sb.Append("}]");
            }
            else
            {
                foreach (string val in arrData)
                {
                    index++;

                    if (val != "<BR>")
                    {
                        sb.Append(val + ",");
                    }
                    else
                    {
                        sb = sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("},");
                        if (index < arrData.Count)
                        {
                            sb.Append("{");
                        }
                    }

                }
                sb = sb.Replace(",", "", sb.Length - 1, 1);
                sb.Append("]");
            }

            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 生成easyui grid所需要的json数据格式
        /// </summary>
        /// <returns></returns>
        public string ToEasyuiGridJsonString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"total\":" + totlalCount.ToString() + ",");
            sb.Append("\"rows\":[");
            int index = 0;
            sb.Append("{");
            if (arrData.Count <= 0)
            {
                sb.Append("}]");
            }
            else
            {
                foreach (string val in arrData)
                {
                    index++;

                    if (val != "<BR>")
                    {
                        sb.Append(val + ",");
                    }
                    else
                    {
                        sb = sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("},");
                        if (index < arrData.Count)
                        {
                            sb.Append("{");
                        }
                    }

                }
                sb = sb.Replace(",", "", sb.Length - 1, 1);
                sb.Append("]");
            }

            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 生成easyui grid所需要的json数据格式
        /// </summary>
        /// <returns></returns>
        public string ToEasyuiListJsonString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int index = 0;
            sb.Append("{");
            if (arrData.Count <= 0)
            {
                sb.Append("}]");
            }
            else
            {
                foreach (string val in arrData)
                {
                    index++;

                    if (val != "<BR>")
                    {
                        sb.Append(val + ",");
                    }
                    else
                    {
                        sb = sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("},");
                        if (index < arrData.Count)
                        {
                            sb.Append("{");
                        }
                    }

                }
                sb = sb.Replace(",", "", sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 生成easyui 对象所需要的json数据格式
        /// </summary>
        /// <returns></returns>
        public string ToEasyuiOneModelJsonString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            int index = 0;
            sb.Append("{");
            if (arrData.Count <= 0)
            {
                sb.Append("}]");
            }
            else
            {
                foreach (string val in arrData)
                {
                    index++;

                    if (val != "<BR>")
                    {
                        sb.Append(val + ",");
                    }
                    else
                    {
                        sb = sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("},");
                        if (index < arrData.Count)
                        {
                            sb.Append("{");
                        }
                    }

                }
                sb = sb.Replace(",", "", sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public T Deserialize<T>(string sJson) where T : class
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sJson));
            T obj = (T)ds.ReadObject(ms);

            ms.Close();
            return obj;
        }

        /// <summary>
        /// 得到json树
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="szCheckItems">选中的ID号集合</param>
        /// <returns></returns>
        public static string GetCategoryTreeJson(List<Models.Category> categorys, int parentID)
        {
            StringBuilder sb = new StringBuilder();
            if (categorys.Count > 0)
            {
                sb.Append("[");
                string filer = string.Empty;
                var result = categorys.FindAll(m => m.ParentID == parentID).ToList();
                foreach (var item in result)
                {
                    sb.Append("{\"id\":\"" + item.ID + "\",\"text\":\"" + item.Name + "\"");
                    if (categorys.FindAll(m => m.ParentID == item.ID).Count > 0)
                    {
                        sb.Append(",\"children\":");
                        sb.Append(GetCategoryTreeJson(categorys, item.ID));
                    }
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);

                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到json树
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="szCheckItems">选中的ID号集合</param>
        /// <returns></returns>
        public static string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string szCheckItems)
        {
            string[] arrCheckItem = szCheckItems.Split(',');

            StringBuilder sb = new StringBuilder();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Empty;
                filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        bool bchecked = false;
                        foreach (string item in arrCheckItem)
                        {
                            if (item != "")
                                if (int.Parse(item) == int.Parse(row[idCol].ToString()))
                                {
                                    bchecked = true;
                                    break;
                                }

                        }
                        if (bchecked && tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length <= 0)
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\",\"checked\":\"true\"");
                        else
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            sb.Append(GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], szCheckItems));

                        }

                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到json树
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="szCheckItems">选中的ID号集合</param>
        /// <returns></returns>
        public static string GetTreeGridJsonByTable(DataTable dt, string idCol, string txtCol, string rela, object pId, string szCheckItems)
        {
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.AppendFormat("\"total\":{0}, ", dt.Rows.Count);
            jsonBuilder.Append("\"rows\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 得到json树
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="szCheckItems">选中的ID号集合</param>
        /// <returns></returns>
        public static string GetMenuJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, string url, string menutype, string icon, object pId, string szCheckItems)
        {
            string[] arrCheckItem = szCheckItems.Split(',');

            StringBuilder sb = new StringBuilder();

            if (tabel.Rows.Count > 0)
            {
                sb.Append("\"menus\":[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {

                    foreach (DataRow row in rows)
                    {
                        bool bchecked = false;
                        foreach (string item in arrCheckItem)
                        {
                            if (item != "")
                                if (int.Parse(item) == int.Parse(row[idCol].ToString()))
                                {
                                    bchecked = true;
                                    break;
                                }

                        }
                        if (bchecked)
                        {
                            sb.Append("{\"menuid\":\"" + row[idCol] + "\",\"menuname\":\"" + row[txtCol] + "\",\"icon\":\"" + row[icon] + "\",\"url\":\"" + row[url] + "\"");
                            if (row[menutype].ToString() == "页签" && tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                            {
                                sb.Append(",");
                                sb.Append(GetMenuJsonByTable(tabel, idCol, txtCol, rela, url, menutype, icon, row[idCol], szCheckItems));
                            }
                            sb.Append("},");
                        }

                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到json树
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="szCheckItems">选中的ID号集合</param>
        /// <returns></returns>
        public static string GetMenuJson(List<Models.Menu> menus, int pId)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("\"menus\":[");
            for (int i = 0; i < menus.Count; i++)
            {
                var item = menus[i];
                if (item.ParentID == pId)
                {
                    sb.Append("{\"menuid\":\"" + item.ID + "\",\"menuname\":\"" + item.MenuName + "\",\"icon\":\"" + item.Icon + "\",\"url\":\"" + item.Url + "\"");
                    if (item.MenuType == "页签" && menus.Where(m => m.ParentID == item.ID).ToList().Count > 0)
                    {
                        sb.Append(",");
                        sb.Append(GetMenuJson(menus, item.ID));
                    }
                    sb.Append("},");
                }
            }
            sb = sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return sb.ToString();
        }
    }

}