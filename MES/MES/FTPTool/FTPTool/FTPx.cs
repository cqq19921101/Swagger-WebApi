using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace FTPTool
{
    class FTPx
    {
        DB DB = new DB();

        FtpWebRequest reqFTP = null;
        string Name = "", Site = "", UserID = "", Password = "";

        private void GetFTPSiteProfile(string Name)
        {
            using (DataTable dt = DB.GetSiteProfileDetail(Name))
            {
                Site = dt.Rows[0]["SiteIP"].ToString();
                UserID = dt.Rows[0]["UserID"].ToString();
                Password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
            }
            //DataRow[] drs = Para.dsSiteProfile.Tables["Connection"].Select("Name='" + Name + "'");
            //if (drs.Length > 0)
            //{
            //    Site = drs[0]["Site"].ToString();
            //    UserID = drs[0]["UserID"].ToString();
            //    Password = Security.Decrypt(drs[0]["Password"].ToString());
            //    MessageBox.Show(Site + "/" + UserID + "/" + Password);
            //}
        }

        private void Connect(string Name)
        {
            GetFTPSiteProfile(Name);
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://"+Site+"/"));
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(UserID, Password);
        }

        public string[] GetFileList(string SiteName, string WRMethods)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            try
            {
                Connect(SiteName);
                reqFTP.Method = WRMethods;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);//中文文件名
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
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
                System.Windows.Forms.MessageBox.Show(ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        //public void Upload(object main)
        //{
        //    frmMain fm = (frmMain)main;
        //    frmMain.SetProgressStatus sps = new frmMain.SetProgressStatus(fm.setProgressStatus);
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        fm.BeginInvoke(sps, i);
        //        System.Threading.Thread.Sleep(200);
        //        Application.DoEvents();
        //    }
        //}

        public void Upload(string Name ,string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            GetFTPSiteProfile(Name);
            string uri = "ftp://" + Site + "/" + fileInf.Name;
            Connect(uri);//连接         
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            FileStream fs = fileInf.OpenRead();
            try
            {
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }
        }
    }
}


/*
最近做FTP 编程
 收录一个比较全的使用介绍 
 里面关于创建目录可能出现的问题,在BLOG的另一篇文章<<C# FTP 目录创建>>中做了我的解决方案
  
.net,C#,Ftp各种操作,上传,下载,删除文件,创建目录,删除目录,获得文件列表等
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
 
namespace ConvertData
{
    class FtpUpDown
    {
        string ftpServerIP;
        string ftpUserID;
        string ftpPassword;
        FtpWebRequest reqFTP;
        
        public FtpUpDown(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
        }
        //都调用这个
        private string[] GetFileList(string path, string WRMethods)//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            try
            {
                Connect(path);
                reqFTP.Method = WRMethods;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);//中文文件名
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }
        public string[] GetFileList(string path)//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory);
        }
        public string[] GetFileList()//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            return GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectory);
        }
        public void Upload(string filename) //上面的代码实现了从ftp服务器上载文件的功能
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
            Connect(uri);//连接         
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            FileStream fs = fileInf.OpenRead();
            try
            {
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }
        }
        public bool Download(string filePath, string fileName, out string errorinfo)////上面的代码实现了从ftp服务器下载文件的功能
        {
            try
            {
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + "\\" + onlyFileName;
                if (File.Exists(newFileName))
                {
                    errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);
                    return false;
                }
                string url = "ftp://" + ftpServerIP + "/" + fileName;
                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                errorinfo = "";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }
        //删除文件
        public void DeleteFileName(string fileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
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
                MessageBox.Show(ex.Message, "删除错误");
            }
        }
*/
