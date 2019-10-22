using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 導出和讀取CSV文件
    /// </summary>
    public class CSV
    {

        /// <summary>
        /// 將DataTable寫入到CSV文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath">文件名，帶路徑</param>
        public static void SaveCSV(DataTable dt, string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //列名
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //填數據
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替換英文冒號為兩個冒號
                                                    //逗號、冒號、換行符放入引號中                                
                    if (str.Contains(",") || str.Contains("\"")
                        || str.Contains("\r") || str.Contains("\n"))
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 將CSV文件讀取到DataTable中
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="hasTitle">首行是否是列名</param>
        /// <returns></returns>
        public static DataTable OpenCSV(string fileName, bool hasTitle)
        {
            Encoding encoding = Encoding.UTF8;
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool isFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (isFirst)
                {
                    columnCount = strLine.Split(',').Length;

                    if (hasTitle)
                    {
                        tableHead = strLine.Split(',');
                    }
                    else
                    {
                        tableHead = new string[columnCount];
                        for (int i = 0; i < columnCount; i++)
                        {
                            tableHead[i] = "Column" + (i + 1).ToString();
                        }
                    }
                    foreach (string colName in tableHead)
                    {
                        DataColumn dc = new DataColumn(colName);
                        dt.Columns.Add(dc);
                    }
                    isFirst = false;

                    if (hasTitle == false)
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }

                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            //if (aryLine != null && aryLine.Length > 0)
            //{
            //    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            //}

            sr.Close();
            fs.Close();
            return dt;
        }
    }    
}
