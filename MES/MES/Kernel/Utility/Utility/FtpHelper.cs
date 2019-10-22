using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Collections;


namespace Liteon.Mes.Utility
{
    /// <summary>
    /// FTP基本方法類，需要實例化后使用
    /// </summary>
    public class FtpHelper
    {
        FtpWebRequest reqFTP = null;
        FtpWebRequest reqFTPGetFileSize = null;
        FtpWebRequest reqFTPGetLastModifyDate = null;
        string Site = "", UserID = "", Password = "", Port = "21";

        /// <summary>
        /// 構造函數，使用默認端口21
        /// </summary>
        /// <param name="_SiteIP">ftp的IP地址</param>
        /// <param name="_UserID">連接賬號</param>
        /// <param name="_Password">連接密碼</param>
        public FtpHelper(string _SiteIP, string _UserID, string _Password)
        {
            //Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = "21";
        }

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="_SiteIP">ftp的IP地址</param>
        /// <param name="_UserID">連接賬號</param>
        /// <param name="_Password">連接密碼</param>
        /// <param name="_Port">端口</param>
        public FtpHelper(string _SiteIP, string _UserID, string _Password, string _Port)
        {
            //Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = _Port;
        }

        /// <summary>
        /// 連接FTP
        /// </summary>
        public void Connect()
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + Site + ":" + Port + "/"));
            reqFTP.UseBinary = true;

            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(UserID, Password);
            reqFTP.Timeout = 200000;
        }

        /// <summary>
        /// 連接到FTP的指定路徑
        /// </summary>
        /// <param name="url"></param>
        public void Connect(string url)
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(UserID, Password);
            reqFTP.Timeout = 200000;
        }

        private void ConnectGetFileSize(string url)
        {
            reqFTPGetFileSize = null;
            reqFTPGetFileSize = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFTPGetFileSize.UseBinary = true;
            // ftp用户名和密码
            reqFTPGetFileSize.Credentials = new NetworkCredential(UserID, Password);
        }

        private void ConnectGetLastModifyDate(string url)
        {
            reqFTPGetLastModifyDate = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFTPGetLastModifyDate.UseBinary = true;
            // ftp用户名和密码
            reqFTPGetLastModifyDate.Credentials = new NetworkCredential(UserID, Password);
        }

        /// <summary>
        /// 獲取FTP默認路徑下的文件列表
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList()
        {
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            string line = "";
            try
            {
                Connect();
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                if (result.Length != 0)
                {
                    // to remove the trailing '\n'
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    return result.ToString().Split('\n');
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("獲取文件列表錯誤：\r\n" + ex.Message);
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //return null;
            }
            finally
            {
                reader.Close();
                response.Close();
                reader = null;
                response = null;
                line = null;
                reqFTP = null;
            }
        }
        
        /// <summary>
        /// 獲取指定路徑下的文件列表
        /// </summary>
        /// <param name="folder">FTP上的文件夾，比如ftp://ip:port/folder1/folder2，也可以只寫文件夾folder1/folder2</param>
        /// <returns></returns>
        public DataTable GetFileList(string folder)
        {
            StringBuilder result = new StringBuilder();
            FtpWebResponse response = null;
            StreamReader reader = null;
            DataTable dt = null;
            string filename = "";
            string lastmodifydate = "";
            string line = "";
            try
            {
                if (folder == "/")
                {
                    folder = "";
                }
                if (folder.StartsWith("/"))
                {
                    folder = folder.Substring(1, folder.Length - 1);
                }
                string uri = "";
                if (folder.ToUpper().StartsWith("FTP://"))
                {
                    uri = folder;
                }
                else
                {
                    uri = "ftp://" + Site + ":" + Port + "/" + folder;
                }

                if (uri.EndsWith("/"))
                {
                    uri = uri.Substring(0, uri.Length - 1);
                }

                Connect(uri);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                response = (FtpWebResponse)reqFTP.GetResponse();

                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名

                line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                string[] filelist = null;
                if (result.Length != 0)
                {
                    // to remove the trailing '\n'
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    filelist = result.ToString().Split('\n');
                }
                dt = GetFileListDateTable();
                if (filelist != null && filelist.Length > 0)
                {
                    foreach (string s in filelist)
                    {
                        string[] filesplit = s.Split(' ');
                        ArrayList al = GetFTPFileListInfoWithoutSpace(filesplit);
                        DataRow dr = dt.NewRow();
                        filename = "";
                        for (int i = 8; i < al.Count; i++)
                        {
                            filename = filename + " " + al[i].ToString();
                        }
                        if (filename.StartsWith(" "))
                        {
                            filename = filename.Substring(1, filename.Length - 1);
                        }
                        if (uri.EndsWith("/"))
                        {
                            uri = uri.Substring(0, uri.Length - 1);
                        }
                        lastmodifydate = "";
                        if (folder.ToUpper().StartsWith("FTP://"))
                        {
                            lastmodifydate = GetLastModifyDate(folder);
                        }
                        else
                        {
                            lastmodifydate = GetLastModifyDate(uri + "/" + filename);
                        }
                        dr["File Name"] = filename;// al[8].ToString();
                        dr["FileFullPath"] = uri + "/" + filename;// al[8].ToString();
                        dr["Size"] = al[4].ToString();
                        dr["Modify Date"] = lastmodifydate;
                        if (al[0].ToString().IndexOf("d") >= 0)
                        {
                            dr["Type"] = "File Folder";
                            dr["IsFolder"] = "Y";
                        }
                        else
                        {
                            if (filename.LastIndexOf(".") > 0)
                            {
                                dr["Type"] = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                            }
                            else
                            {
                                dr["Type"] = "";
                            }
                            //dr["Type"] = Tool.GetFileType(al[8].ToString().Substring(al[8].ToString().Length - 4, 4));
                            dr["IsFolder"] = "N";
                        }
                        dt.Rows.Add(dr);
                        al = null;
                    }
                    return dt;
                }
                if (dt != null)
                {
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("獲取指定路徑文件列表錯誤：\r\n" + ex.Message);
                //return null;
            }
            finally
            {
                line = null;
                filename = null;
                lastmodifydate = null;
                dt = null;
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
            }
        }

        private DataTable GetFileListDateTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("File Name");
            dt.Columns.Add("Size");
            dt.Columns.Add("Type");
            dt.Columns.Add("Modify Date");
            dt.Columns.Add("FileFullPath");
            dt.Columns.Add("IsFolder");

            return dt;
        }

        private ArrayList GetFTPFileListInfoWithoutSpace(string[] OriginalArray)
        {
            ArrayList al = new ArrayList();
            foreach (string s in OriginalArray)
            {
                if (s != "")
                {
                    al.Add(s);
                }
            }
            return al;
        }
        
        /// <summary>
        /// 上傳文件
        /// </summary>
        /// <param name="FtpPath">FTP路径，如果是FTP根目錄可以設/或者空字符串（不是null）</param>
        /// <param name="Filename">本地文件全文件名</param>
        public void Upload(string FtpPath, string Filename)
        {
            FileInfo fileInf = new FileInfo(Filename);
            //FtpWebResponse response = null;
            string uri = "";
            //string filename = fileInf.FullName.Replace(initialpath, "").Replace(@"\", "/");
            string filename = fileInf.Name.Replace("\\", "/");
            if (FtpPath.ToUpper().StartsWith("FTP://"))
            {
                FtpPath = FtpPath.Substring(("ftp://" + Site + ":" + Port).Length, FtpPath.Length - ("ftp://" + Site + ":" + Port).Length);
            }
            
            if (FtpPath.StartsWith("/"))
            {
                FtpPath = FtpPath.Substring(1, FtpPath.Length - 1);
            }
            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }

            if (FtpPath == "/" || FtpPath == "")
            {
                uri = "ftp://" + Site + ":" + Port + "/" + filename;
            }
            else
            {
                uri = "ftp://" + Site + ":" + Port + "/" + FtpPath + filename;
            }
            uri = uri.Replace(@"\", "/");
            Connect(uri);//连接 

            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen;
            Stream strm = null;
            FileStream fs = fileInf.OpenRead();

            try
            {
                reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 在一个命令之后被执行
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令
                reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小
                                                      //response = (FtpWebResponse)reqFTP.GetResponse();
                strm = reqFTP.GetRequestStream();// 把上传的文件写入流

                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的kb                
                decimal d_persent = 0;
                // 流内容没有结束
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);// 把内容从file stream 写入upload stream

                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();// 关闭流
                
                filename = null;
                fileInf = null;
            }
            catch (Exception ex)
            {
                //SaveLog(logname,"Upload fail-->"+ex.Message+"\r\n"+ex.ToString());
                throw new Exception("上傳失敗：\r\n" + ex.Message + ". " + uri);
                //MessageBox.Show(ex.Message, "Upload Error");
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                    strm = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (fileInf != null)
                {
                    fileInf = null;
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
            }
        }

        /// <summary>
        /// 判斷文件是否存在
        /// </summary>
        /// <param name="FtpPath">FTP完整路徑，含文件名</param>
        /// <returns></returns>
        public bool CheckFileExist(string FtpFullFileName)
        {
            try
            {
                if (FtpFullFileName.EndsWith("/"))
                {
                    FtpFullFileName = FtpFullFileName.Substring(0, FtpFullFileName.Length - 1);
                }
                int idx = FtpFullFileName.LastIndexOf("/");
                string FtpPath = FtpFullFileName.Substring(0, idx);
                string FileNameWithoutPath = FtpFullFileName.Substring(idx + 1, FtpFullFileName.Length - idx - 1);
                string filecheckexistfolder = FtpPath;
                if (filecheckexistfolder.ToUpper().StartsWith("FTP://"))
                {
                    filecheckexistfolder = filecheckexistfolder.Substring(("ftp://" + Site + ":" + Port).Length, filecheckexistfolder.Length - ("ftp://" + Site + ":" + Port).Length);
                }                

                using (DataTable dtfilelist = GetFileList(filecheckexistfolder))
                {
                    //dtfilelist.CaseSensitive = true;//区分大小写
                    string filter = "[File Name]='" + FileNameWithoutPath + "'";
                    DataRow[] drs = dtfilelist.Select(filter);
                    if (drs.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 下載文件
        /// </summary>
        /// <param name="LocalFilePath">要保存的本地路径</param>
        /// <param name="fileName">FTP上的文件名，包含路径</param>
        public void Download(string LocalFilePath, string fileName)
        {
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;
            if (LocalFilePath.EndsWith("\\"))
            {
                LocalFilePath = LocalFilePath.Substring(0, LocalFilePath.Length - 1);
            }
            try
            {
                if (fileName.ToUpper().StartsWith("FTP://"))
                {
                    fileName = fileName.Substring(("ftp://" + Site + ":" + Port).Length, fileName.Length - ("ftp://" + Site + ":" + Port).Length);
                }
                if (fileName.StartsWith("/"))
                {
                    fileName = fileName.Substring(1, fileName.Length - 1);
                }
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = LocalFilePath + "\\" + onlyFileName;
                
                if (File.Exists(newFileName))
                {
                    File.SetAttributes(newFileName, FileAttributes.Normal);
                    File.Delete(newFileName);

                    //throw new Exception(string.Format("The file {0} is already exist, can't download", newFileName));
                }
                string url = "ftp://" + Site + ":" + Port + "/" + fileName;

                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                int bufferSize = 2048;
                int readCount;

                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);                    
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("下載文件失敗：\r\n" + ex.Message);
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                    ftpStream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                    outputStream.Dispose();
                }
            }
        }
        
        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="fileName">FTP上的文件名，包含路径</param>
        public void Delete(string fileName)
        {
            try
            {
                if (fileName.ToUpper().StartsWith("FTP://"))
                {
                    fileName = fileName.Substring(("ftp://" + Site + ":" + Port).Length, fileName.Length - ("ftp://" + Site + ":" + Port).Length);
                }
                if (fileName.StartsWith("/"))
                {
                    fileName = fileName.Substring(1, fileName.Length - 1);
                }
                //FileInfo fileInf = new FileInfo(fileName);
                string uri = "ftp://" + Site + ":" + Port + "/" + fileName;
                Connect(uri);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("刪除文件失敗：\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 刪除文件夾，文件夾裡面是空的才可以
        /// </summary>
        /// <param name="folder">FTP上的文件夹名，包含路径</param>
        public void DeleteFolder(string folder)
        {
            try
            {                
                if (folder.ToUpper().StartsWith("FTP://"))
                {
                    folder = folder.Substring(("ftp://" + Site + ":" + Port).Length, folder.Length - ("ftp://" + Site + ":" + Port).Length);
                }
                if (folder.StartsWith("/"))
                {
                    folder = folder.Substring(1, folder.Length - 1);
                }
                string uri = "ftp://" + Site + ":" + Port + "/" + folder;
                Connect(uri);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("刪除文件夾失敗：\r\n" + ex.Message);
            }
        }
        
        /// <summary>
        /// 創建FTP目錄
        /// </summary>
        /// <param name="folder">文件夾</param>
        public void MakeFolder(string folder)
        {
            try
            {
                if (folder.ToUpper().StartsWith("FTP://"))
                {
                    folder = folder.Substring(("ftp://" + Site + ":" + Port).Length, folder.Length - ("ftp://" + Site + ":" + Port).Length);
                }
                if (folder.StartsWith("/"))
                {
                    folder = folder.Substring(1, folder.Length - 1);
                }
                string uri = "ftp://" + Site + ":" + Port + "/" + folder;
                Connect(uri);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("創建文件夾失敗：\r\n" + ex.Message + "，" + folder);
            }
        }

        private int GetFileSize(string url)
        {
            try
            {
                ConnectGetFileSize(url);
                //reqFTPGetFileSize.Credentials = new NetworkCredential(UserID, Password);
                reqFTPGetFileSize.Method = WebRequestMethods.Ftp.GetFileSize;
                int dataLength = (int)reqFTPGetFileSize.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        private string GetLastModifyDate(string url)
        {
            try
            {
                //if (url.StartsWith("/"))
                //{
                //    url = "ftp://" + Site + url;
                //}
                //else
                //{
                //    url = "ftp://" + Site + "/" + url;
                //}
                ConnectGetLastModifyDate(url);
                reqFTPGetLastModifyDate.Credentials = new NetworkCredential(UserID, Password);
                reqFTPGetLastModifyDate.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response = (FtpWebResponse)reqFTPGetLastModifyDate.GetResponse();
                return response.LastModified.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch { return ""; }
        }

        /// <summary>
        /// 重命名FTP上的文件名
        /// </summary>
        /// <param name="CurrentFileWithFtpPath">現在的文件名完整路徑</param>
        /// <param name="newFilenameWithoutPath">新的文件名，不含路徑</param>
        public void Rename(string CurrentFileWithFtpPath, string newFilenameWithoutPath)
        {
            try
            {
                //if (currentFilename.StartsWith("/"))
                //{
                //    currentFilename = currentFilename.Substring(1, currentFilename.Length - 1);
                //}
                ////FileInfo fileInf = new FileInfo(fileName);
                //string uri = "ftp://" + Site + "/" + currentFilename;
                //uri = currentFilename;

                Connect(CurrentFileWithFtpPath);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilenameWithoutPath;
                reqFTP.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("重命名文件失敗：\r\n" + ex.Message);
            }
        }


    }
}
