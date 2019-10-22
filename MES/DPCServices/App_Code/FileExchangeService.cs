using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.IO;
using System.Security.Permissions;

/// <summary>
/// Summary description for FileExchangeService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class FileExchangeService : System.Web.Services.WebService
{

    public FileExchangeService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool IsExistsFile(string filePath)
    {
        WriteLog("IsExistsFile", "Start.");
        WriteLog("IsExistsFile", "End.");
        return IOHelper.FileExists(filePath);
    }

    [WebMethod]
    public DataTable GetFile(string filePath, ref string errorMessage)
    {
        WriteLog("GetFile", "Start.");
        DataTable dtResult = null;
        try
        {
            if (IOHelper.FileExists(filePath))
            {
                string fileContent = IOHelper.Read(filePath);

                using (System.IO.StringReader sr = new System.IO.StringReader(fileContent))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr);
                    dtResult = ds.Tables[0];
                    dtResult.Rows.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog("GetFile", "Error: " + ex.ToString());
            errorMessage = ex.Message;
        }

        WriteLog("GetFile", "End.");
        return dtResult;
    }

    [WebMethod]
    public DataTable GetFileByFormat(string filePath, string fileFormatPath, ref string errorMessage)
    {
        WriteLog("GetFileByFormat", "Start.");
        DataTable dtResult = null;
        DataTable dtFormat = null;
        try
        {
            if (IOHelper.FileExists(fileFormatPath))
            {
                string fileFormat = IOHelper.Read(fileFormatPath);
                using (System.IO.StringReader sr = new System.IO.StringReader(fileFormat))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr);
                    dtFormat = ds.Tables[0];
                }

                dtResult = dtFormat.Clone();

                if (IOHelper.FileExists(filePath))
                {
                    string[] fileRows = IOHelper.ReadLines(filePath);
                    for (int i = 0; i < fileRows.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(fileRows[i].Trim()))
                        {

                            string[] colValues = fileRows[i].Split(new Char[] { '\t' });
                            DataRow dr = dtResult.NewRow();

                            ////根據長度截取
                            //int columnStartIndex = 0;
                            //int columnLen = 0;
                            //foreach (DataColumn dc in dtFormat.Columns)
                            //{
                            //    columnLen = Convert.ToInt32(dtFormat.Rows[0][dc].ToString());
                            //    dr[dc.ColumnName] = CutStringByte(fileRows[i], columnStartIndex, columnLen).Trim();
                            //    columnStartIndex += columnLen;
                            //}

                            //根據tab鍵截取
                            for (int j = 0; j < dtFormat.Columns.Count; j++)
                            {
                                dr[dtFormat.Columns[j].ColumnName] = colValues[j].Trim();
                            }

                            dtResult.Rows.Add(dr);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog("GetFileByFormat", "Error: " + ex.ToString());
            errorMessage = ex.Message;
        }

        WriteLog("GetFileByFormat", "End.");
        return dtResult;
    }

    [WebMethod]
    public bool UploadFile(string filePath, string fileFormatPath,DataTable fileSource, ref string errorMessage)
    {
        WriteLog("UploadFile", "Start.");
        bool result = false;
        DataTable dtFormat = null;
        try
        {
            if (!string.IsNullOrEmpty(fileFormatPath))
            {
                string fileFormat = IOHelper.Read(fileFormatPath);
                using (System.IO.StringReader sr = new System.IO.StringReader(fileFormat))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr);
                    dtFormat = ds.Tables[0];
                }
            }
            else
            {
                dtFormat = fileSource.Clone();
            }

            string fileDirectory = filePath.Substring(0, filePath.LastIndexOf(@"\"));
            string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

            if (fileSource != null && fileSource.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < fileSource.Rows.Count; i++)
                {
                    DataRow dr = fileSource.Rows[i];
                    string row = "";
                    foreach (DataColumn dc in dtFormat.Columns)
                    {
                        string colValue = "";
                        ////根據長度組合
                        //int columnLen = Convert.ToInt32(dtFormat.Rows[0][dc].ToString());
                        //if (!fileSource.Columns[dc.ColumnName].DataType.Equals(typeof(DateTime)))
                        //{
                        //    colValue = dr[dc.ColumnName].ToString().PadRight(columnLen);
                        //}
                        //else
                        //{
                        //    colValue = Convert.ToDateTime(dr[dc.ColumnName]).ToString("yyyy/MM/dd HH:mm:ss").PadRight(columnLen);
                        //}

                        //row += CutStringByte(colValue, 0, columnLen);

                        //根據tab鍵組合
                        if (!fileSource.Columns[dc.ColumnName].DataType.Equals(typeof(DateTime)))
                        {
                            colValue = dr[dc.ColumnName].ToString().Trim();
                        }
                        else
                        {
                            colValue = Convert.ToDateTime(dr[dc.ColumnName]).ToString("yyyy/MM/dd HH:mm:ss").Trim();
                        }
                        row += colValue + "\t";

                    }
                    
                    row = row.Substring(0, row.Length - 1); //去掉行最後一個tab鍵

                    sb.AppendLine(row);
                }

                //若目錄不存在，首先創建目錄
                IOHelper.CreateDir(fileDirectory);
                //刪除相同的文件
                IOHelper.DeleteFile(filePath);
                //創建新的文件
                IOHelper.CreateFile(filePath);
                //寫入內容
                IOHelper.Write(filePath, sb.ToString());
                
                result = true;
            }
        }
        catch (Exception ex)
        {
            WriteLog("UploadFile", "Error: " + ex.ToString());
            errorMessage = ex.Message;   
        }

        WriteLog("UploadFile", "End.");
        return result;
    }

    [WebMethod]
    public List<string> FindFiles(string dir, string fileName)
    {
        WriteLog("FindFiles", "Start.");
        List<string> fileNames = new List<string>();

        FileInfo[] files = IOHelper.FindFiles(dir, fileName);

        if (files != null && files.Length > 0)
        {
            foreach (FileInfo file in files)
            {
                fileNames.Add(file.Name);
            }
        }

        WriteLog("FindFiles", "End.");
        return fileNames;
    }

    [WebMethod]
    public bool MoveFile(string sourceFileName, string destFileName)
    {
        WriteLog("MoveFile", "Start.");
        WriteLog("MoveFile", "End.");
        return IOHelper.MoveFile(sourceFileName, destFileName);
    }

    [WebMethod]
    public bool CopyFile(string sourceFileName, string destFileName)
    {
        WriteLog("CopyFile", "Start.");
        WriteLog("CopyFile", "End.");
        return IOHelper.CopyFile(sourceFileName, destFileName);
    }

    [WebMethod]
    public bool DeleteFile(string fileName)
    {
        WriteLog("DeleteFile", "Start.");
        WriteLog("DeleteFile", "End.");
        return IOHelper.DeleteFile(fileName);
    }

    /// <summary> /// 按指定长度(单字节)截取字符串 
    /// </summary> /// <param name="str">源字符串</param> 
    /// <param name="startIndex">开始索引</param> 
    /// <param name="len">截取字节数</param> 
    /// <returns>string</returns> 
    public string CutStringByte(string str, int startIndex, int len)
    {
        WriteLog("CutStringByte", "Start.");
        if (str == null || str.Trim() == "") 
        { 
            return ""; 
        }
        if (Encoding.UTF8.GetByteCount(str) < startIndex + 1 + len)
        { 
            return str; 
        }

        int i = 0;//字节数 
        int j = 0;//实际截取长度 
        foreach (char newChar in str)
        {
            if ((int)newChar > 127)
            { //汉字 
                i += 2;
            }
            else 
            { 
                i++; 
            } 

            if (i > startIndex + len) 
            { 
                str = str.Substring(startIndex, j); 
                break; 
            }

            if (i > startIndex)
            { 
                j++; 
            }
        }
        WriteLog("CutStringByte", "End.");
        return str;
    }

    private void WriteLog(string method, string message)
    {
        string filePath = HttpContext.Current.Request.PhysicalApplicationPath +
                "\\Log\\" + DateTime.Today.ToString("yyyyMM") +
                "\\";

        if (!Directory.Exists(filePath))
        {
            (new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, filePath)).Demand();
            Directory.CreateDirectory(filePath);
        }

        using (FileStream fileStream = File.Open(filePath + DateTime.Today.ToString("yyyyMMdd") + "FileExchangeService.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.WriteLine("LogDateTime : " + DateTime.Now.ToString());
                writer.WriteLine("Method : " + method);
                writer.WriteLine("LogMessage");
                writer.WriteLine(message);
                writer.WriteLine("----------------------------------------------------------------------------------------");
            }
        }
    }

}

