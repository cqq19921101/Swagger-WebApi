using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// DataSet/DataTable導出Html
    /// </summary>
    public class Html
    {
        /// <summary>
        /// 將DataTable導出到Html
        /// </summary>
        /// <param name="title">標題</param>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">文件路徑</param>
        public static void ExportDataTableToHtml(string title, DataTable dt, string filePath)
        {
            string content = ExportDataTableToHtmlString(title, dt);
            if (string.IsNullOrEmpty(filePath)) { filePath = Path.GetTempPath(); }
            string fileName = filePath + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + title + ".html";
            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }

            RunIE(fileName);
        }

        /// <summary>
        /// 將DataSet導出到Html
        /// </summary>
        /// <param name="title">標題</param>
        /// <param name="ds">DataSet</param>
        /// <param name="filePath">文件路徑</param>
        public static void ExportDataSetToHtml(string title, DataSet ds, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) { filePath = Path.GetTempPath(); }
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];

                string content = ExportDataTableToHtmlString(title + Convert.ToString(i + 1), dt);
                string fileName = filePath + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + title + Convert.ToString(i + 1) + ".html";
                using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }

                RunIE(fileName);
            }
        }

        /// <summary>
        /// 打開IE，顯示導出的數據
        /// </summary>
        /// <param name="fileName">文件路徑</param>
        private static void RunIE(string fileName)
        {
            Process process = new Process();
            process.StartInfo.FileName = "iexplore.exe";
            process.StartInfo.Arguments = fileName;

            process.Start();
            Thread.Sleep(300);
        }

        /// <summary>
        /// 導出html的主方法
        /// </summary>
        /// <param name="title">標題</param>
        /// <param name="dt">DataTable</param>
        /// <returns>返回Html字符串</returns>
        private static string ExportDataTableToHtmlString(string title, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"/>");
            sb.Append("<title>" + title + "</title>");
            sb.Append("<style type='text/css'>");
            sb.Append("table.t1 {margin-top:10px;margin-left:20px;margin-right:20px;margin-bottom:5px;#background-color: #FFF;#background:#EEF4F9;#border: none;border: 1;#color:#003755;border-collapse:collapse;font: 14px  \"宋体\"; }   ");
            sb.Append("table.t1 th{background:#7CB8E2;color:#fff; padding:6px 4px; text-align:center;}");
            sb.Append("table.t1 td{background:#C7DDEE none repeat-x scroll center left; color:#000;padding:4px 2px;}</style>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<table border='1' class='t1'><tr align=center bgcolor=#FFFF00>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append("<th>");
                sb.Append(dt.Columns[i].ColumnName);
                sb.Append("</th>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb.Append("<td>");
                    string value = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                    sb.Append(value);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
