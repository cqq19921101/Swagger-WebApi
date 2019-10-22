using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
//using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace Liteon.Mes.WebUtility
{
    public class File
    {
        public static string GetWebConfigSetting(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string GetBaseUrl()
        {
            try
            {
                string url = string.Format("{0}://{1}{2}",
                    HttpContext.Current.Request.Url.Scheme,
                    HttpContext.Current.Request.Url.Authority,
                    HttpRuntime.AppDomainAppVirtualPath);
                if (url.EndsWith("/") == false)
                {
                    url = url + "/";
                }
                return url;
            }
            catch { return ""; }
        }


        /// <summary>
        /// 将DataTable的第一行转化为Json格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonOnlyFirstRow(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[0][j].ToString());
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }

        /// <summary>  
        /// DataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            //jsonBuilder.Append("{\"");
            //jsonBuilder.Append(dt.TableName==""?"DT":dt.TableName);
            //jsonBuilder.Append("\":");
            jsonBuilder.Append("[");

            jsonBuilder.Append(Jsonbody(dt));

            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }


        /// <summary>  
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string DataSetToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            int i = 0;
            foreach (DataTable dt in ds.Tables)
            {
                json.Append("\"");
                //json.Append(dt.TableName==""?"DT"+i.ToString():dt.TableName);
                json.Append("Table" + i.ToString());
                json.Append("\":");
                json.Append("[");
                //json.Append(DataTableToJson(dt));
                json.Append(Jsonbody(dt));
                json.Append("],");
                i++;
            }
            json.Remove(json.Length - 1, 1);
            json.Append("}");
            return json.ToString();
        }

        private static string Jsonbody(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(StringZhuanyiJSON(dt.Rows[i][j].ToString()));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// DataTable转换成Json格式，指定名称
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + StringZhuanyiJSON(dt.Rows[i][j].ToString()) + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        private static string StringZhuanyiJSON(string str)
        {
            str = str == null ? "" : str;

            StringBuilder sb = new StringBuilder();

            int i = 0;
            while (i < str.Length)
            {
                string tmpchar = str.Substring(i, 1);
                if (tmpchar == "\"" || tmpchar == "\\")
                //|| tmpchar == "\t" || tmpchar == "\r" || tmpchar == "\n")
                {
                    sb.Append("\\" + tmpchar);
                }
                else if (tmpchar == "\t" || tmpchar == "\r" || tmpchar == "\n")
                {
                    i++;
                    continue;
                }
                else
                {
                    sb.Append(tmpchar);
                }
                i++;
            }
            return sb.ToString();
        }


        public static string ClassToJsonString<T>(T objClass)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(objClass.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, objClass);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                return szJson;
            }
        }

        public static T JsonStringToClass<T>(string JsonString)
        {
            if (JsonString.StartsWith("["))
            {
                JsonString = JsonString.Substring(1, JsonString.Length - 1);
            }
            if (JsonString.EndsWith("]"))
            {
                JsonString = JsonString.Substring(0, JsonString.Length - 1);
            }
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(JsonString)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
        }

        //public static IEnumerable<SelectListItem> CreateSelectList(DataTable dt, string ColumnName)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        SelectListItem item = new SelectListItem();
        //        item.Text = dr[ColumnName].ToString();
        //        item.Value = dr[ColumnName].ToString();
        //        list.Add(item);
        //    }
        //    return list;
        //}

        /// <summary>
        /// 用DataTable返回一個Json數組，EXTJS專用
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonExtJS(DataTable dt)
        {
            if (dt == null)
            {
                return "";
            }
            var jobj = new JObject { 
                    new JProperty("total",dt.Rows.Count),
                    new JProperty("success",true),
                    new JProperty("data",new JArray(
                            from c in dt.AsEnumerable() 
                            
                            select new JObject(
                                    createJPropertyArray((DataRow)c)
                                )
                        ))
                };
            return jobj.ToString();
        }

        /// <summary>
        /// 分頁顯示用，傳入DataSet，然後返回一個Json數組，EXTJS專用
        /// </summary>
        /// <param name="ds">傳入一個DataSet，第一個Table只有一行一列，內容是所有記錄的行數，第二個Table包含當前頁所要呈現的記錄明細</param>
        /// <returns></returns>
        public static string DataSetToJsonExtJSPaging(DataSet ds)
        {
            var jobj = new JObject { 
                    new JProperty("total",Int32.Parse(ds.Tables[0].Rows[0][0].ToString())),
                    new JProperty("success",true),
                    new JProperty("data",new JArray(
                            from c in ds.Tables[1].AsEnumerable() 
                            select new JObject(
                                    createJPropertyArray((DataRow)c)
                                )
                        ))
                };
            return jobj.ToString();
        }

        private static JProperty[] createJPropertyArray(DataRow dr)
        {
            JProperty[] jP = new JProperty[dr.ItemArray.Count()];
            for(int i=0; i<jP.Length;i++)
            {
                jP[i] = new JProperty(dr.Table.Columns[i].Caption, dr[i]);
            }
            return jP;
        }

        public static string DataTableToJsonEasyUITree(DataTable dt)
        {
            if (dt == null)
            {
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            string lastModule = "";
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isNewModule = false;
                    if (lastModule == "")
                    {
                        isNewModule = true;
                        lastModule = dt.Rows[i][0].ToString();
                    }
                    if (lastModule.ToUpper() != dt.Rows[i][0].ToString().ToUpper())
                    {
                        isNewModule = true;
                        lastModule = dt.Rows[i][0].ToString();
                    }

                    if (isNewModule)
                    {
                        if (i > 0)
                        {
                            Json.AppendLine("]},");
                        }
                        Json.AppendLine("{");
                        Json.AppendLine("\"id\":\"" + i.ToString() + "\",");
                        Json.AppendLine("\"text\":\"" + StringZhuanyiJSON(dt.Rows[i][0].ToString()) + "\",");
                        Json.AppendLine("\"children\":[");
                    }
                    else
                    {
                        Json.Append(",");
                    }
                    Json.AppendLine("{\"text\":\"" + StringZhuanyiJSON(dt.Rows[i][1].ToString()) + "\",\"state\":\"open\",\"attributes\":{\"url\":\"" + StringZhuanyiJSON(dt.Rows[i][2].ToString()) + "\",\"menuId\":\"" + StringZhuanyiJSON(dt.Rows[i][3].ToString()) + "\"}}");
                }
            }
            Json.AppendLine("]");
            Json.AppendLine("}");
            Json.AppendLine("]");
            return Json.ToString();
        }
        //public static string DataTableToJsonEasyUITree(DataTable dt)
        //{
        //    if (dt == null)
        //    {
        //        return "";
        //    }
        //    if (dt.Rows.Count == 0)
        //    {
        //        return "";
        //    }
        //    string lastModule = "";
        //    StringBuilder Json = new StringBuilder();
        //    Json.Append("[");
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            bool isNewModule = false;
        //            if (lastModule == "")
        //            {
        //                isNewModule = true;
        //                lastModule = dt.Rows[i][0].ToString();
        //            }
        //            if (lastModule.ToUpper() != dt.Rows[i][0].ToString().ToUpper())
        //            {
        //                isNewModule = true;
        //                lastModule = dt.Rows[i][0].ToString();
        //            }

        //            if (isNewModule)
        //            {
        //                if (i > 0)
        //                {
        //                    Json.AppendLine("]},");
        //                }
        //                Json.AppendLine("{");
        //                Json.AppendLine("\"id\":\"" + i.ToString() + "\",");
        //                Json.AppendLine("\"text\":\"" + StringZhuanyiJSON(dt.Rows[i][0].ToString()) + "\",");
        //                Json.AppendLine("\"children\":[");
        //            }
        //            else
        //            {
        //                Json.Append(",");
        //            }
        //            Json.AppendLine("{\"text\":\"" + StringZhuanyiJSON(dt.Rows[i][1].ToString()) + "\",\"state\":\"open\",\"attributes\":{\"url\":\"" + StringZhuanyiJSON(dt.Rows[i][2].ToString()) + "\"}}");
        //        }
        //    }
        //    Json.AppendLine("]");
        //    Json.AppendLine("}");
        //    Json.AppendLine("]");
        //    return Json.ToString();
        //}

        public static string DataTableToJsonEasyUIDataGrid(DataTable dt)
        {
            if (dt == null)
            {
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            StringBuilder Json = new StringBuilder();

            string[] columns = new string[dt.Columns.Count];
            for(int i = 0; i < columns.Length; i++)
            {
                columns[i] = dt.Columns[i].ColumnName;
            }

            Json.Append("{\"total\":" + dt.Rows.Count.ToString() + ",\"rows\":[");


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j=0;j<columns.Length;j++)
                {
                    Json.Append("\"" + columns[j] + "\":\"" + StringZhuanyiJSON(dt.Rows[i][columns[j]].ToString()) + "\"");
                    if (j < columns.Length - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
                Json.Append("\r\n");
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        /// EasyUI datagrid 分頁
        /// </summary>
        /// <param name="ds">2個table，第一個只有1行1列，提示所有行數，第二個table返回當前頁碼的數據</param>
        /// <returns></returns>
        public static string DataTableToJsonEasyUIDataGridPaging(DataSet ds)
        {
            if (ds == null)
            {
                return "";
            }
            if (ds.Tables.Count == 0)
            {
                return "";
            }
            if (ds.Tables[1].Rows.Count == 0)
            {
                return "";
            }
            StringBuilder Json = new StringBuilder();

            string[] columns = new string[ds.Tables[1].Columns.Count];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = ds.Tables[1].Columns[i].ColumnName;
            }

            Json.Append("{\"total\":" + ds.Tables[0].Rows[0][0].ToString() + ",\"rows\":[");


            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < columns.Length; j++)
                {
                    Json.Append("\"" + columns[j] + "\":\"" + StringZhuanyiJSON(ds.Tables[1].Rows[i][columns[j]].ToString()) + "\"");
                    if (j < columns.Length - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < ds.Tables[1].Rows.Count - 1)
                {
                    Json.Append(",");
                }
                Json.Append("\r\n");
            }
            Json.Append("]}");
            return Json.ToString();
        }

        public static void WriteCookie(string Key, string Value)
        {
            try
            {
                HttpCookie myCookie = System.Web.HttpContext.Current.Request.Cookies["liteonCookie"];
                //检验Cookie是否已经存在 
                if (null == myCookie)
                {
                    myCookie = new HttpCookie("liteonCookie");
                }
                myCookie.Expires = DateTime.Now.AddMonths(1);
                myCookie[Key] = HttpUtility.UrlEncode(Value);
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            catch { }
        }

        public static string ReadCookie(string Key)
        {
            try
            {
                HttpCookie myCookie = System.Web.HttpContext.Current.Request.Cookies["liteonCookie"];
                //检验Cookie是否已经存在 
                if (null == myCookie)
                {
                    return "";
                }
                else
                {
                    return myCookie.Values[Key] == null ? "" : HttpUtility.UrlDecode(myCookie.Values[Key].ToString());
                }
            }
            catch { return ""; }
        }

        public static string GetWebClientIp()
        {

            string userIP = "";

            try
            {
                if (System.Web.HttpContext.Current == null
                 || System.Web.HttpContext.Current.Request == null
                 || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;

        }

    }
}
