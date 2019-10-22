using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Data;
using System.Collections;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 讀取和寫入Ini文件
    /// </summary>
    public class Ini
    {
        /// <summary>
        /// Import Windows API for writing INI file
        /// </summary>
        /// <param name="section">Section Name</param>
        /// <param name="key">Key</param>
        /// <param name="val">Key value</param>
        /// <param name="filepath">FileName and its absolute path</param>
        /// <returns>Long type</returns>
        [DllImport("kernel32")] // Return 0 means Fail,otherwise,means Success
        protected static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        /// <summary>
        /// Import windows API for reading INI file
        /// </summary>
        /// <param name="section">Setion Name</param>
        /// <param name="key">Key</param>
        /// <param name="def">string type</param>
        /// <param name="retval">StringBuilder: return a stream</param>
        /// <param name="size">Return string size</param>
        /// <param name="filepath">Filename and its absolute path</param>
        /// <returns>Return result</returns>
        [DllImport("kernel32")] //Return the string length
        protected static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filepath);

        /// <summary>
        /// 讀取Ini文件內容
        /// </summary>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name</param>
        /// <param name="filePath">Ini文件完整路徑</param>
        /// <returns>返回讀取到的內容，如果失敗，返回空</returns>
        public static string ReadIniValue(string section, string key, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    StringBuilder SB = new StringBuilder(1024);
                    GetPrivateProfileString(section, key, string.Empty, SB, 1024, filePath);

                    return SB.ToString();
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 寫內容到Ini文件
        /// </summary>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name</param>
        /// <param name="value">The value for Key</param>
        /// <param name="filePath">INI filename and full path</param>
        /// <returns>執行成功返回true，否則返回false</returns>
        public static bool WriteIniFile(string section, string key, string value, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    long lRTN = WritePrivateProfileString(section, key, value, filePath);
                    if (lRTN == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 讀取System Manager傳入的字符串參數
        /// </summary>
        /// <param name="argsString">參數字符串</param>
        /// <returns>返回一個Key/value的DataTable</returns>
        public static DataTable ReadSysManagerArgs(string argsString)
        {
            try
            {
                argsString = argsString.Substring(1, argsString.Length - 1);
                string[] aArgs = argsString.Split('<');
                System.Data.DataTable dtReturn = new System.Data.DataTable();
                dtReturn.Columns.Add("Key");
                dtReturn.Columns.Add("Value");
                for (int j = 0; j < aArgs.Length; j++)
                {
                    string stmp = aArgs[j].Substring(0, aArgs[j].Length - 1);
                    if (stmp == "" || stmp == "=")
                    {
                        continue;
                    }
                    else
                    {
                        int iFirstEqual = stmp.IndexOf("=");
                        if (iFirstEqual < 0)
                        {
                            continue;
                        }

                        System.Data.DataRow dr = dtReturn.NewRow();
                        dr["Key"] = stmp.Substring(0, iFirstEqual);
                        dr["Value"] = stmp.Substring(iFirstEqual + 1, stmp.Length - iFirstEqual - 1);

                        dtReturn.Rows.Add(dr);
                    }
                }

                return dtReturn;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 從參數列表DataTable中查詢指定的值
        /// </summary>
        /// <param name="argsDataTable">一個KEY/VALUE結構的DataTable</param>
        /// <param name="key">要查詢的參數，比如LINE</param>
        /// <returns>如果從查詢到，返回值，否則返回空白</returns>
        public static string GetOneValueFromArgsDataTable(DataTable argsDataTable, string key)
        {
            try
            {
                return (argsDataTable.Select("Key='" + key.Trim() + "'"))[0][1].ToString();
            }
            catch
            {
                return "";
            }
        }

    }
}
