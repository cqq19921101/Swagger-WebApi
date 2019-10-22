using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 讀取和寫入Txt文件
    /// </summary>
    public class Txt
    {
        /// <summary>
        /// 讀取TXT文件中的所有內容
        /// </summary>
        /// <param name="fileName">完整的文件名，包含路徑（最後不能帶\）</param>
        /// <returns>返回一個字符串，包含文件中的內容</returns>
        public static string GetAllDataFromTxt(string fileName)
        {
            string rel = "";
            StreamReader sr = null;

            if (!File.Exists(fileName))
            {
                return "";
            }
            else
            {
                try
                {
                    sr = File.OpenText(fileName);
                    rel = sr.ReadToEnd();
                }
                catch
                {
                    return "";
                }
                finally
                {
                    sr.Close();
                }
                return rel;
            }
        }

        /// <summary>
        /// 讀取TXT文件第X行的數據
        /// </summary>
        /// <param name="fileName">完整的文件名，包含路徑（最後不能帶\）</param>
        /// <param name="lineNo">行號，從1開始</param>
        /// <returns>返回指定行號的文字內容</returns>
        public static string GetSpecificLineFromTxt(string fileName, Int32 lineNo)
        {
            string rel = null;
            Int32 i = 1;
            StreamReader sr = null;
            sr = File.OpenText(fileName);
            rel = sr.ReadLine();

            try
            {
                if (lineNo != 1)
                {
                    while (rel != null)
                    {
                        i++;
                        rel = sr.ReadLine();

                        if (i == lineNo)
                        {
                            return rel;
                        }
                    }
                }

                if (lineNo > i)
                {
                    return "";
                }
                else
                {
                    return rel;
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                sr.Close();
            }
        }

        /// <summary>
        /// 向TXT文件中寫入內容
        /// </summary>
        /// <param name="content">要寫入的內容</param>
        /// <param name="fileName">完整的文件名，包含路徑（最後不能帶\）</param>
        /// <param name="createIfNotExist">文件不存在是否創建</param>
        /// <param name="returnMsg">返回信息，成功返回OK，否則返回Error:msg</param>
        /// <returns>寫入成功返回OK，否則返回false</returns>
        public static bool AppendDataToTxt(string content, string fileName, bool createIfNotExist, out string returnMsg)
        {
            bool bFlag;
            bFlag = Dir.CreateFolders(fileName, out returnMsg);

            if (!bFlag)
            {
                return false;
            }

            FileAttributes attr = FileAttributes.Normal;
            StreamWriter sw;

            if (File.Exists(fileName))
            {
                //記錄當前文檔屬性，寫完以後恢復原來的屬性
                attr = File.GetAttributes(fileName);
                File.SetAttributes(fileName, FileAttributes.Normal);
            }
            else
            {
                if (createIfNotExist)
                {
                    File.Create(fileName).Close();
                }
                else
                {
                    returnMsg = "Error:File not exist";
                    return false;
                }
            }
            sw = File.AppendText(fileName);
            try
            {
                sw.WriteLine(content);
                returnMsg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                returnMsg = "Error:" + ex.Message.ToString();
                return false;
            }
            finally
            {
                sw.Close();
                File.SetAttributes(fileName, attr);
            }
        }
        
        /// <summary>
        /// 在程序運行目錄\LogFolder下面增加以日期為文件名的的TXT文件，將logMsg記錄進去
        /// </summary>
        /// <param name="logMsg">要記錄的log信息</param>
        public static void SaveLogToTxt(string logMsg)
        {
            string filePath = Application.StartupPath + "\\LogFolder";
            string fileName= DateTime.Today.ToString("yyyyMMdd") + ".txt";
            SaveLogToTxt(filePath, fileName, logMsg);
        }

        /// <summary>
        /// 在指定目錄生成LOG文件
        /// </summary>
        /// <param name="filePath">路徑，不要以\\結尾</param>
        /// <param name="fileName">TXT文件名</param>
        /// <param name="logMsg">要記錄的log信息</param>
        public static void SaveLogToTxt(string filePath, string fileName, string logMsg)
        {
            string m = "";
            Dir.CreateFolders(filePath, out m);
            string logFile =  filePath + "\\" + fileName;
            Txt.AppendDataToTxt("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]-->" + logMsg, logFile, true, out m);
        }
    }
}
