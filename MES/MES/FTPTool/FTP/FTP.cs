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


namespace FTP
{
    public class FTP
    {
        
        FtpWebRequest reqFTP = null;
        FtpWebRequest reqFTPGetFileSize = null;
        FtpWebRequest reqFTPGetLastModifyDate = null;
        string Name = "", Site = "", UserID = "", Password = "", Port = "21";
        string Renamefile = "N", ActionWhileFileExist = "Override";

        int reconnecttimes = 20;

        private string Uploaddownloadpauseflag = "N";

        public FTP(string _SiteName,string _SiteIP,string _UserID,string _Password,string _Port)
        {
            Name=_SiteName;
            Site=_SiteIP;
            UserID=_UserID;
            Password=_Password;
            Port = _Port;
        }

        public FTP(string _SiteName, string _SiteIP, string _UserID, string _Password, string _Port,string _Renamefilewhileuploadingdowloading, string _ActionWhileFileExist)
        {
            Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = _Port;
            Renamefile = _Renamefilewhileuploadingdowloading;
            ActionWhileFileExist = _ActionWhileFileExist;
        }

        public string UploadDownloadPause
        {
            set { Uploaddownloadpauseflag = value; }
        }

        private static void SaveLog(string Msg)
        {
            string logname = "Log_Connect_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string NORMALLOGPATH = Application.StartupPath + "\\Logs\\Normal";
            string ERRORLOGFILEPATH = Application.StartupPath + "\\Logs\\Error";
            string path = "";
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
               
                if (Directory.Exists(NORMALLOGPATH) == false)
                {
                    Directory.CreateDirectory(NORMALLOGPATH);
                }
                path = NORMALLOGPATH + "\\" + logname;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.Write(DateTime.Now.ToString("HH:mm:ss\t") + Msg + "\r\n");
            }
            catch { }
            finally
            {
                sw.Flush();
                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
                path = null;
                logname = null;
                NORMALLOGPATH = null;
                ERRORLOGFILEPATH = null;
            }
        }

        public static void SaveLog(string FileName, string Msg)
        {
            string NORMALLOGPATH = Application.StartupPath + "\\Logs\\Normal";
            string path = "";
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                
                if (Directory.Exists(NORMALLOGPATH) == false)
                {
                    Directory.CreateDirectory(NORMALLOGPATH);
                }
                path = path = NORMALLOGPATH + "\\" + FileName;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.Write(DateTime.Now.ToString("HH:mm:ss\t") + Msg + "\r\n");
            }
            catch { }
            finally
            {
                sw.Flush();
                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
                path = null;
                NORMALLOGPATH = null;
            }
        }

        public bool CheckFtpConnectionStatus(out string errmsg)
        {
            FtpWebResponse response = null;
            try
            {                
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + Site + "/"));
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                response = (FtpWebResponse)reqFTP.GetResponse();
                if (response.StatusCode == FtpStatusCode.LoggedInProceed || response.StatusCode == FtpStatusCode.OpeningData || response.StatusCode == FtpStatusCode.DataAlreadyOpen)
                {
                    //SaveLog("Test connect ok");
                    errmsg = "";
                    reqFTP.Abort();
                    return true;
                }
                else
                {
                    //SaveLog("Test connect fail");
                    errmsg = "Connect error:" + response.StatusCode.ToString();
                    return false;
                }
            }
            catch (Exception ex)
            {
                //SaveLog("Test connect fail, errmsg=" + ex.Message);
                errmsg = "Connect error:" + ex.Message.ToString();
                return false;
            }
            finally
            {
                reqFTP = null;
                response = null;
            }

        }

        //public bool CheckFtpConnectionStatus(out string errmsg)
        //{
        //    FtpWebResponse response=null;
        //    int i=1;
        //    while (i <= 15)
        //    {
        //        try
        //        {
        //            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + Site + "/"));
        //            reqFTP.UseBinary = true;
        //            // ftp用户名和密码
        //            reqFTP.Credentials = new NetworkCredential(UserID, Password);

        //            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

        //            response = (FtpWebResponse)reqFTP.GetResponse();

        //        }
        //        catch (Exception ex)
        //        {
        //            response = null;
        //        }
        //        if (response != null)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            reqFTP = null;
        //            SaveLog("reconnect test-->" + i.ToString());
        //            Application.DoEvents();
        //        }
        //        i++;
        //    }
        //    try
        //    {
        //        if (response.StatusCode == FtpStatusCode.LoggedInProceed || response.StatusCode == FtpStatusCode.OpeningData || response.StatusCode == FtpStatusCode.DataAlreadyOpen)
        //        {
        //            SaveLog("Test connect ok");
        //            errmsg = "";
        //            reqFTP.Abort();
        //            return true;
        //        }
        //        else
        //        {
        //            SaveLog("Test connect fail");
        //            errmsg = "Connect error:" + response.StatusCode.ToString();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //SaveLog("Test connect fail, errmsg=" + ex.Message);
        //        errmsg = "Connect error:" + ex.Message.ToString();
        //        return false;
        //    }
        //    finally
        //    {
        //        reqFTP = null;
        //    }

        //}

        public void Connect()
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://"+Site+"/"));
            reqFTP.UseBinary = true;

            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(UserID, Password);
            reqFTP.Timeout = 200000;
        }

        public void Connect(string url)
        {
            //SaveLog("Connect start before upload");
            //SaveLog("url=" + url);
            //SaveLog("UserID=" + UserID + ",Password=" + Password);
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(UserID, Password);
            reqFTP.Timeout=200000;
            //SaveLog("Connect OK before upload");
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

        public string[] GetFileList()
        {
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            string line = "";
            try
            {
                //string logname = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                //SaveLog(logname, "begin connect");
                Connect();
                //SaveLog(logname, "end connect");
                //SaveLog(logname, "get response begin");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                response = reqFTP.GetResponse();
                //SaveLog(logname, "get response end");
                //SaveLog(logname, "get response stream begin");
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                //SaveLog(logname, "get response stream end");
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
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
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

        //public string[] GetFileList(string folder)
        //{
        //    string[] downloadFiles;
        //    StringBuilder result = new StringBuilder();
        //    try
        //    {
        //        if(folder=="/")
        //        {
        //            folder = "";
        //        }
        //        if (folder.StartsWith("/"))
        //        {
        //            folder = folder.Substring(1, folder.Length - 1);
        //        }
        //        string uri =  "ftp://" + Site + "/" + folder;
        //        //uri = System.Web.HttpUtility.UrlDecode(uri, System.Text.Encoding.GetEncoding("GB2312"));
        //        Connect(uri);
        //        reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        //        WebResponse response = reqFTP.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                
        //        string line = reader.ReadLine();

        //        while (line != null)
        //        {
        //            result.Append(line);
        //            result.Append("\n");
        //            line = reader.ReadLine();
        //        }
        //        reader.Close();
        //        response.Close();
        //        if (result.Length != 0)
        //        {
        //            // to remove the trailing '\n'
        //            result.Remove(result.ToString().LastIndexOf('\n'), 1);
        //            return result.ToString().Split('\n');
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //System.Windows.Forms.MessageBox.Show(ex.Message);
        //        downloadFiles = null;
        //        return downloadFiles;
        //    }
        //}

        public DataTable GetFileList(string folder)
        {
            //string logname = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
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

                //SaveLog(logname, "begin connect");
                //uri = System.Web.HttpUtility.UrlDecode(uri, System.Text.Encoding.GetEncoding("GB2312"));
                //SaveLog(logname, uri);
                Connect(uri);
                //SaveLog(logname, "end connect");
                //SaveLog(logname, "get response begin");

                int retryqty = 1;
                while (retryqty <= reconnecttimes)
                {
                    try
                    {
                        reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                        //SaveLog(logname, "get response 2222222222");
                        response = (FtpWebResponse)reqFTP.GetResponse();
                    }
                    catch(Exception ex)
                    {
                        //SaveLog(logname, "get response 2222222222"+ex.Message);
                        response = null;
                    }
                    
                    if (response != null)
                    {
                        break;
                    }
                    else
                    {
                        reqFTP = null;
                        //SaveLog(logname, uri);
                        Connect(uri);
                        //SaveLog(logname, "=======================");
                        //SaveLog(logname, "retry get response-->" + retryqty.ToString());
                    }
                    retryqty++;
                }
                //SaveLog(logname, "get response end");
                //SaveLog(logname, "get response stream begin");
               
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                 
                //SaveLog(logname, "get response stream end");

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
                //SaveLog(logname, "set file list begin======================");
                dt = GetFileListDateTable();
                if (filelist != null && filelist.Length > 0)
                {
                    foreach (string s in filelist)
                    {
                        //listBox1.Items.Add(s);

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
                            //SaveLog(logname, "get modify date begin-->" + folder);
                            lastmodifydate = GetLastModifyDate(folder);
                            //SaveLog(logname, "get modify date end-->" + folder);
                        }
                        else
                        {
                            //SaveLog(logname, "get modify date begin-->" + uri + "/" + filename);
                            lastmodifydate = GetLastModifyDate(uri + "/" + filename);
                            //SaveLog(logname, "get modify date end-->" + uri + "/" + filename);
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
                //SaveLog(logname, "set file list end=====================");
                if (dt != null)
                {
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                //SaveLog(logname, ex.Message);
                return null;
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
            dt.Columns.Add("Icon");

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


        //private string[] GetFileList(string uri)
        //{
        //    string[] downloadFiles;
        //    StringBuilder result = new StringBuilder();
        //    try
        //    {
        //        Connect(uri);
        //        reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        //        WebResponse response = reqFTP.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);//中文文件名
        //        string line = reader.ReadLine();
        //        while (line != null)
        //        {
        //            result.Append(line);
        //            result.Append("\n");
        //            line = reader.ReadLine();
        //        }
        //        reader.Close();
        //        response.Close();
        //        if (result.Length != 0)
        //        {
        //            // to remove the trailing '\n'
        //            result.Remove(result.ToString().LastIndexOf('\n'), 1);
        //            return result.ToString().Split('\n');
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //System.Windows.Forms.MessageBox.Show(ex.Message);
        //        downloadFiles = null;
        //        return downloadFiles;
        //    }
        //}

        //public bool CheckFolderExist(string folder)
        //{
        //    string uri = "ftp://" + Site + "/" + folder + "/";
        //    if (GetFileList(uri) == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}


        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="FtpPathWithoutIP">FTP路径，但不包含[ftp://IP]</param>
        /// <param name="Filename">本地文件全文件名</param>
        /// <param name="initialpath">本地文件起始路径</param>
        public void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath)
        {
            //string logname = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            FileInfo fileInf = new FileInfo(Filename);
            //FtpWebResponse response = null;
            //如果ActionWhileFileExist是Rename
            string filenamewithotpath = "";
            string uri = "";
            //string filename = fileInf.FullName.Replace(initialpath, "").Replace(@"\", "/");
            string filename = fileInf.FullName.Substring(initialpath.Length + 1, fileInf.FullName.Length - initialpath.Length - 1).Replace("\\", "/");

            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }

            if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
            {
                uri = "ftp://" + Site + ":" + Port + "/" + filename;
            }
            else
            {
                uri = "ftp://" + Site + ":" + Port + FtpPathWithoutIP + filename;
            }

            //上传已有文件是覆盖还是重命名待上传文件，如果设置是Rename，查询一下文件是否存在，存在则给待上传文件加上时间戳
            if (ActionWhileFileExist == "Rename")
            {
                if (CheckFileExist(uri, fileInf.Name.ToString()) == true)
                {
                    //文件uri重新设置
                    string extname = fileInf.Extension.ToString();
                    filename = filename.Substring(0, filename.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    filenamewithotpath = fileInf.Name.Substring(0, fileInf.Name.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
                    {
                        uri = "ftp://" + Site + ":" + Port + "/" + filename;
                    }
                    else
                    {
                        uri = "ftp://" + Site + ":" + Port + FtpPathWithoutIP + filename;
                    }
                }
                else
                {
                    filenamewithotpath = fileInf.Name.ToString();
                }
            }
            else
            {
                filenamewithotpath = fileInf.Name.ToString();
            }

            //上传文件是否要使用临时文件名，如果要，加上后缀.tmp
            if (Renamefile == "Y")
            {
                uri = uri + ".tmp";
            }

            uri = uri.Replace(@"\", "/");
            Connect(uri);//连接 
            //SaveLog( logname,"Upload step 1");


            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen;
            int uploadedlen;
            int irefreshflag = 0;

            Stream strm = null;
            FileStream fs = fileInf.OpenRead();// 打开一个文件流(System.IO.FileStream) 去读上传的文件
            string errmsg = "";
            try
            {
                //SaveLog(logname,"Upload step 2");
                int retryqty = 1;
                while (retryqty <= reconnecttimes)
                {
                    try
                    {
                        //SaveLog(logname, uri);
                        //SaveLog(logname, "================================");

                        reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 在一个命令之后被执行
                        reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令
                        reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小
                        //response = (FtpWebResponse)reqFTP.GetResponse();
                        strm = reqFTP.GetRequestStream();// 把上传的文件写入流
                    }
                    catch(Exception ex)
                    {
                        //SaveLog(logname, ex.Message);
                        errmsg = "Upload fail: " + ex.Message + ". " + uri;
                        strm = null;
                    }
                    if (strm != null)
                    {
                        errmsg = "";
                        break;
                    }
                    else
                    {
                        reqFTP = null;
                        //SaveLog(logname, uri);
                        Connect(uri);
                        //SaveLog(logname, "============Re===========");
                        //SaveLog(logname, "retry get response-->" + retryqty.ToString());
                    }
                    retryqty++;
                }
                //SaveLog(logname, "Upload step3");
                if (errmsg != "")
                {
                    throw new Exception(errmsg);
                }
                //SaveLog(logname,"Upload step 4");
                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的kb
                //SaveLog("Upload step 5");
                uploadedlen = contentLen;
                //SaveLog("Upload step 6");
                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                decimal d_persent = 0;
                // 流内容没有结束
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);// 把内容从file stream 写入upload stream

                    d_persent = Convert.ToDecimal(Convert.ToDecimal(uploadedlen) / Convert.ToDecimal(fs.Length));
                    mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploading " + fileInf.Name, d_persent });
                    irefreshflag++;
                    if (irefreshflag == 15)
                    {
                        irefreshflag = 0;//循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                        Application.DoEvents();
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                    uploadedlen = uploadedlen + contentLen;
                }
                strm.Close();// 关闭流
                //SaveLog(logname,"Upload step 7");
                d_persent = Convert.ToDecimal(1);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploaded " + fileInf.Name, d_persent });

                //如果是上传过程用临时文件名的，到这里已经上传完毕了，把文件名改回去
                //SaveLog(logname,"Upload step 8");
                if (Renamefile == "Y")
                {
                    Rename(uri, filenamewithotpath);
                }
                filenamewithotpath = null;
                filename = null;
                fileInf = null;
                //SaveLog(logname,"Upload step 9");
            }
            catch (Exception ex)
            {
                //SaveLog(logname,"Upload fail-->"+ex.Message+"\r\n"+ex.ToString());
                throw ex;
                //MessageBox.Show(ex.Message, "Upload Error");
            }
            finally
            {
                errmsg = null;
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

                //SaveLog(logname,"Upload step 10");
                System.Threading.Thread.Sleep(200);
                //SaveLog(logname,"Upload step 11");
            }
        }

        public void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath, int offset)
        {
            FileInfo fileInf = new FileInfo(Filename);
            //如果ActionWhileFileExist是Rename
            string filenamewithotpath = "";
            string uri = "";
            string urifordeleteexistfile = "";
            //string filename = fileInf.FullName.Replace(initialpath, "").Replace(@"\", "/");
            string filename = fileInf.FullName.Substring(initialpath.Length + 1, fileInf.FullName.Length - initialpath.Length - 1).Replace("\\","/");

            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }

            if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
            {
                uri = "ftp://" + Site + ":" + Port + "/" + filename;
                urifordeleteexistfile = filename;
            }
            else
            {
                uri = "ftp://" + Site + ":" + Port + FtpPathWithoutIP + filename;
                urifordeleteexistfile = FtpPathWithoutIP + filename;
            }

            //上传已有文件是覆盖还是重命名待上传文件，如果设置是Rename，查询一下文件是否存在，存在则给待上传文件加上时间戳
            if (ActionWhileFileExist == "Rename")
            {
                if (CheckFileExist(uri, fileInf.Name.ToString()) == true)
                {
                    //文件uri重新设置
                    string extname = fileInf.Extension.ToString();
                    filename = filename.Substring(0, filename.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    filenamewithotpath = fileInf.Name.Substring(0, fileInf.Name.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
                    {
                        uri = "ftp://" + Site + ":" + Port + "/" + filename;
                    }
                    else
                    {
                        uri = "ftp://" + Site + ":" + Port + FtpPathWithoutIP + filename;
                    }
                }
                else
                {
                    filenamewithotpath = fileInf.Name.ToString();
                }
            }
            else
            {
                filenamewithotpath = fileInf.Name.ToString();
            }

            //上传文件是否要使用临时文件名，如果要，加上后缀.tmp
            if (Renamefile == "Y")
            {
  
                uri = uri + ".tmp";
            }

            uri = uri.Replace(@"\", "/");
            Connect(uri);//连接         
            reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 在一个命令之后被执行
            if (offset > 0)
            {
                reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
            }
            else
            {
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令
            }
            reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小

            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen;
            int uploadedlen = 0;
            int irefreshflag = 0;

            Stream strm = null;

            FileStream fs = fileInf.OpenRead();// 打开一个文件流(System.IO.FileStream) 去读上传的文件
            try
            {
                if (offset > 0)
                {
                    fs.Seek(offset, System.IO.SeekOrigin.Current);
                    uploadedlen = offset;
                }
                strm = reqFTP.GetRequestStream();// 把上传的文件写入流
                contentLen = fs.Read(buff,0, buffLength);// 每次读文件流的kb
                uploadedlen = uploadedlen + contentLen;

                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                decimal d_persent = 0;
                // 流内容没有结束
                while (contentLen != 0)
                {
                    if (Uploaddownloadpauseflag == "Y")
                    {
                        break;
                    }

                    strm.Write(buff, 0, contentLen);// 把内容从file stream 写入upload stream

                    d_persent = Convert.ToDecimal(Convert.ToDecimal(uploadedlen) / Convert.ToDecimal(fs.Length));
                    mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploading " + fileInf.Name, uploadedlen });
                    irefreshflag++;
                    if (irefreshflag == 15)
                    {
                        irefreshflag = 0;//循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                        Application.DoEvents();
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                    uploadedlen = uploadedlen + contentLen;
                }
                strm.Close();// 关闭流

                if (Uploaddownloadpauseflag == "Y")
                {
                    return;
                }
                d_persent = Convert.ToDecimal(1);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploaded " + fileInf.Name, -1 });

                //如果是上传过程用临时文件名的，到这里已经上传完毕了，把文件名改回去
                if (Renamefile == "Y")
                {
                    try
                    {
                        Delete(urifordeleteexistfile);
                    }
                    catch { }
                    Rename(uri, filenamewithotpath);
                }
                filenamewithotpath = null;
                filename = null;
                fileInf = null;
            }
            catch (Exception ex)
            {
                throw ex;
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
            }
        }

        public bool CheckFileExist(string FtpFileFullPath, string FileNameWithoutPath)
        {
            try
            {
                string filecheckexistfolder = FtpFileFullPath.Substring(0, FtpFileFullPath.Length - FileNameWithoutPath.Length);
                filecheckexistfolder = filecheckexistfolder.Substring(("ftp://" + Site + ":" + Port).Length, filecheckexistfolder.Length - ("ftp://" + Site + ":" + Port).Length);

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


        #region
        ///// <summary>
        ///// 上传
        ///// </summary>
        ///// <param name="o">主窗体frmMain，或其他调用母窗体</param>
        ///// <param name="iRow">当前操作的DataGridView行，起始为0，若没有，也写0</param>
        ///// <param name="initialPath">中间路径，比如1/2/3，可以为空</param>
        ///// <param name="filename">操作的文件名</param>
        //public void Upload(object o,int iRow,string initialPath,string filename)
        //{
        //    FileInfo fileInf = new FileInfo(filename);
        //    string uri="";
        //    if (String.IsNullOrEmpty(initialPath) == false)
        //    {
        //        if (initialPath.EndsWith("\\") == false)
        //        {
        //            initialPath = initialPath + "\\";
        //        }
        //        uri = "ftp://" + Site + "/" + fileInf.FullName.Replace(initialPath, "");
        //        uri = uri.Replace(@"\", "/");
        //    }
        //    else
        //    {
        //        uri = "ftp://" + Site + "/" + fileInf.Name;
        //    }
        //    Connect(uri);//连接         
        //    // 默认为true，连接不会被关闭
        //    // 在一个命令之后被执行
        //    reqFTP.KeepAlive = false;
        //    // 指定执行什么命令
        //    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
        //    // 上传文件时通知服务器文件的大小
        //    reqFTP.ContentLength = fileInf.Length;
        //    // 缓冲大小设置为kb
        //    int buffLength = 2048;
        //    byte[] buff = new byte[buffLength];
        //    int contentLen;
        //    int uploadedlen;
        //    int irefreshflag = 0;
        //    // 打开一个文件流(System.IO.FileStream) 去读上传的文件
        //    FileStream fs = fileInf.OpenRead();
        //    try
        //    {
        //        // 把上传的文件写入流
        //        Stream strm = reqFTP.GetRequestStream();

        //        // 每次读文件流的kb
        //        contentLen = fs.Read(buff, 0, buffLength);
        //        uploadedlen = contentLen;

        //        MethodInfo mi = getFormMethod.getMethod();
        //        decimal d_persent = 0;
        //        // 流内容没有结束
        //        while (contentLen != 0)
        //        {
        //            // 把内容从file stream 写入upload stream
        //            strm.Write(buff, 0, contentLen);


        //            d_persent = Convert.ToDecimal(Convert.ToDecimal(uploadedlen) / Convert.ToDecimal(fs.Length));
        //            mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploading " + fileInf.Name, d_persent });
        //            irefreshflag++;
        //            if (irefreshflag == 15)
        //            {
        //                //循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
        //                irefreshflag = 0;
        //                Application.DoEvents();
        //            }
        //            contentLen = fs.Read(buff, 0, buffLength);
        //            uploadedlen = uploadedlen + contentLen;
        //        }
        //        // 关闭两个流
        //        strm.Close();

        //        d_persent = Convert.ToDecimal(1);
        //        mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploaded " + fileInf.Name, d_persent });

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //MessageBox.Show(ex.Message, "Upload Error");
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }
        //}
        #endregion

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="LocalFilePath">要保存的本地路径，最后一码不能是\</param>
        /// <param name="fileName">FTP上的文件名，包含路径，但不包含[ftp://IP]</param>
        public void Download(object o, int iRow, string LocalFilePath, string fileName)
        {
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;
            try
            {
                if (fileName.StartsWith("/"))
                {
                    fileName = fileName.Substring(1, fileName.Length - 1);
                }
                String onlyFileName = Path.GetFileName(fileName);
                string trueFileName = LocalFilePath + "\\" + onlyFileName;
                string newFileName = "";
                if (Renamefile == "Y")
                {
                    newFileName = trueFileName + ".tmp";
                }
                else
                {
                    newFileName = trueFileName;
                }
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
                int cl = GetFileSize(url);
                int bufferSize = 2048;
                int readCount;
                int downloadcount;
                int irefreshflag = 0;

                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                downloadcount = readCount;

                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                decimal d_persent = 0;

                outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {


                    outputStream.Write(buffer, 0, readCount);

                    if (cl > 0)
                    {
                        d_persent = Convert.ToDecimal(Convert.ToDecimal(downloadcount) / Convert.ToDecimal(cl));
                        mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloading " + fileName, d_persent });
                        irefreshflag++;
                        if (irefreshflag == 15)
                        {
                            //循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                            irefreshflag = 0;
                            Application.DoEvents();
                        }
                    }
                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                    downloadcount = downloadcount + readCount;
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                d_persent = Convert.ToDecimal(1);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloaded " + fileName, d_persent });

                if (Renamefile == "Y")
                {
                    if (File.Exists(trueFileName))
                    {
                        File.SetAttributes(trueFileName, FileAttributes.Normal);
                        File.Delete(trueFileName);
                    }
                    File.Move(newFileName, trueFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// Download
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="LocalFilePath">要保存的本地路径，最后一码不能是\</param>
        /// <param name="fileName">文件名，不包含路径</param>
        public void Download(object o, int iRow, string LocalFilePath, string FileNameWithoutPath, string FtpFileFullPath, int offset)
        {
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;
            try
            {
                //if (FileNameWithoutPath.StartsWith("/"))
                //{
                //    FileNameWithoutPath = FileNameWithoutPath.Substring(1, FileNameWithoutPath.Length - 1);
                //}
                //String onlyFileName = Path.GetFileName(FileNameWithoutPath);
                string trueFileName = LocalFilePath + "\\" + FileNameWithoutPath;

                string newFileName = "";
                if (Renamefile == "Y")
                {
                    newFileName = trueFileName + ".tmp";
                }
                else
                {
                    newFileName = trueFileName;
                }
                if (File.Exists(newFileName) == false)//文件没有了，重新下载
                {
                    offset = 0;
                }
                //string url = "ftp://" + Site + "/" + FileNameWithoutPath;
                string url = FtpFileFullPath;

                int cl = GetFileSize(url);
                reqFTPGetFileSize = null;


                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = false;
                reqFTP.UsePassive = false;

                if (offset != 0)
                {
                    reqFTP.ContentOffset = offset;
                }

                int bufferSize = 2048;
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();

                int readCount;
                int downloadcount;
                int irefreshflag = 0;

                downloadcount = offset;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                downloadcount = downloadcount + readCount;

                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                decimal d_persent = 0;


                if (offset == 0)
                {
                    outputStream = new FileStream(newFileName, FileMode.Create);
                }
                else
                {
                    outputStream = new FileStream(newFileName, FileMode.Append);
                }

                while (readCount > 0)
                {
                    if (Uploaddownloadpauseflag == "Y")
                    {
                        break;
                    }
                    outputStream.Write(buffer, 0, readCount);

                    if (cl > 0)
                    {
                        d_persent = Convert.ToDecimal(Convert.ToDecimal(downloadcount) / Convert.ToDecimal(cl));
                        mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloading " + FileNameWithoutPath, downloadcount });
                        irefreshflag++;
                        if (irefreshflag == 15)
                        {
                            //循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                            irefreshflag = 0;
                            Application.DoEvents();
                        }
                    }
                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                    downloadcount = downloadcount + readCount;
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                if (Uploaddownloadpauseflag == "Y")
                {
                    return;
                }
                d_persent = Convert.ToDecimal(1);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloaded " + FileNameWithoutPath, -1 });

                if (Renamefile == "Y")
                {
                    if (File.Exists(trueFileName))
                    {
                        File.SetAttributes(trueFileName, FileAttributes.Normal);
                        File.Delete(trueFileName);
                    }
                    File.Move(newFileName, trueFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// Delete
        /// </summary>
        /// <param name="fileName">FTP上的文件名，包含路径，但不包含[ftp://IP]</param>
        public void Delete(string fileName)
        {
            try
            {
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
                throw ex;
            }
        }

        /// <summary>
        /// Delete folder
        /// </summary>
        /// <param name="folder">FTP上的文件夹名，包含路径，但不包含[ftp://IP]</param>
        public void DeleteFolder(string folder)
        {
            try
            {
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
                throw ex;
            }
        }


        public void MakeFolder(string folder)
        {
            try
            {
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
                throw ex;
            }
        }

        /// <summary>
        /// Make folder
        /// </summary>
        /// <param name="folder">本地文件夹路径</param>
        /// <param name="FtpPathWithoutIP">FTP路径，但不包含[ftp://IP]</param>
        /// <param name="initialpath">本地文件夹起始路径</param>
        public void MakeFolder(string folder, string FtpPathWithoutIP, string initialpath)
        {
            try
            {
                string uri = "";
                folder = folder.Substring(initialpath.Length, folder.Length - initialpath.Length).Replace(@"\", "/");

                //folder = folder.Replace(initialpath, "").Replace(@"\", "/");
                if (folder.StartsWith("/"))
                {
                    folder = folder.Substring(1, folder.Length - 1);
                }
               
                if (FtpPathWithoutIP == "/" || FtpPathWithoutIP=="")
                {
                    uri = "ftp://" + Site + ":" + Port + "/" + folder;
                }
                else
                {
                    uri = "ftp://" + Site + ":" + Port + FtpPathWithoutIP + folder;
                }

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
                throw ex;
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
            catch(Exception ex)
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
                throw ex;
            }
        }
       
        static class getFormMethod
        {
            public static MethodInfo getMethod(string FormName)
            {
                if (FormName == null || FormName == "")
                {
                    FormName = "FTPTool.frmMain";
                }
                Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\FTPTool.exe");
                //加载DLL
                System.Type t = ass.GetType(FormName);//获得类型
                //object o = System.Activator.CreateInstance(t);//创建实例

                System.Reflection.MethodInfo mi = t.GetMethod("SetMessage");//获得方法
                return mi;
            }
        }
    }
    
}
