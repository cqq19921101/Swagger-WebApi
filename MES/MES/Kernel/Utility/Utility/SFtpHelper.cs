using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace Liteon.Mes.Utility
{
    /// <summary>
    /// SFTP基本方法類，需要實例化后使用
    /// </summary>
    public class SFtpHelper
    {
        private Session m_session;
        private Channel m_channel;
        private ChannelSftp m_sftp;

        string Site = "", UserID = "", Password = "", Port = "22";

        /// <summary>
        /// 構造函數，使用默認端口22
        /// </summary>
        /// <param name="_SiteIP">Sftp的IP地址</param>
        /// <param name="_UserID">連接賬號</param>
        /// <param name="_Password">連接密碼</param>
        public SFtpHelper(string _SiteIP, string _UserID, string _Password)
        {
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = "22";
            SetSFTPPara(UserID, Site, Password, Convert.ToInt32(Port));
        }

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="_SiteIP">Sftp的IP地址</param>
        /// <param name="_UserID">連接賬號</param>
        /// <param name="_Password">連接密碼</param>
        /// <param name="_Port">端口</param>
        public SFtpHelper(string _SiteIP, string _UserID, string _Password, string _Port)
        {
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = _Port;
            SetSFTPPara(UserID, Site, Password, Convert.ToInt32(Port));
        }

        private void SetSFTPPara(string user, string ip, string pwd, int port)
        {
            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);
            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);
        }

        //SFTP连接状态          
        public bool Connected { get { return m_session.isConnected(); } }

        //连接SFTP          
        public void Connect()
        {
            try
            {
                if (!Connected)
                {
                    m_session.connect();
                    m_channel = m_session.openChannel("sftp");
                    m_channel.connect();
                    m_sftp = (ChannelSftp)m_channel;
                }
                //return true;
            }
            catch (Exception ex)
            {
                throw new Exception("SFTP連接失敗：\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 斷開連接
        /// </summary>    
        public void Disconnect()
        {
            if (Connected)
            {
                m_channel.disconnect();
                m_session.disconnect();
            }
        }

        /// <summary>
        /// 上傳文件
        /// </summary>
        /// <param name="remotePath">路徑文件夾名稱，不包含sftp://ip:port，只包含路徑文件夾</param>
        /// <param name="localFileName">本地文件完整路徑</param>
        public void Upload(string remotePath, string localFileName)
        {
            Connect();
            FileInfo fileInf = new FileInfo(localFileName);
            //如果ActionWhileFileExist是Rename
            string uri = "";

            string filename = fileInf.Name.Replace("\\", "/");

            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }
            if (remotePath.EndsWith("/"))
            {
                remotePath = remotePath.Substring(0, remotePath.Length - 1);
            }
            if (remotePath == "/" || remotePath == "")
            {
                uri = filename;
            }
            else
            {
                uri = remotePath + "/" + filename;
            }
            if (uri.StartsWith("/"))
            {
                uri = uri.Substring(1, uri.Length - 1);
            }


            uri = uri.Replace(@"\", "/");
            //MessageBox.Show(uri);
            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen;

            FileStream fs = fileInf.OpenRead();
            //Tamir.SharpSsh.java.io.InputStream iptStream = new Tamir.SharpSsh.java.io.FileInputStream(Filename);/

            Tamir.SharpSsh.java.io.OutputStream optStream = m_sftp.put(uri);
            try
            {
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    optStream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                //如果是上传过程用临时文件名的，到这里已经上传完毕了，把文件名改回去
                optStream.close();

                filename = null;
                fileInf = null;
            }
            catch (Exception ex)
            {
                throw new Exception("上傳失敗：\r\n" + ex.Message);
                //MessageBox.Show(ex.Message, "Upload Error");
            }
            finally
            {
                if (optStream != null)
                {
                    optStream = null;
                }
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }
            }

        }

        /// <summary>
        /// 判斷文件是否存在
        /// </summary>
        /// <param name="remotePath">SFTP上的文件名，不包含sftp://ip:port的部分，只包含文件夾和文件名</param>
        /// <returns></returns>
        public bool CheckFileExist(string remotePath)
        {
            try
            {
                if (remotePath.EndsWith("/"))
                {
                    remotePath = remotePath.Substring(0, remotePath.Length - 1);
                }
                int idx = remotePath.LastIndexOf("/");
                string filecheckexistfolder = remotePath.Substring(0, idx);
                string FileNameWithoutPath = remotePath.Substring(idx + 1, remotePath.Length - idx - 1);

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
        /// <param name="remotePath">SFTP上的文件名，不包含sftp://ip:port的部分，只包含文件夾和文件名</param>
        /// <param name="localPath">本地路徑，不含文件名</param>
        public void Download(string remotePath, string localPath)
        {
            try
            {
                if (remotePath.StartsWith("/"))
                {
                    remotePath = remotePath.Substring(1, remotePath.Length - 1);
                }
                if (localPath.EndsWith("\\"))
                {
                    localPath = localPath.Substring(0, localPath.Length - 1);
                }
                string newFileName = localPath + "\\" + remotePath.Substring(remotePath.LastIndexOf("/") + 1, remotePath.Length - remotePath.LastIndexOf("/") - 1);
                Connect();

                if (File.Exists(newFileName))
                {
                    File.SetAttributes(newFileName, FileAttributes.Normal);
                    File.Delete(newFileName);

                    //throw new Exception(string.Format("The file {0} is already exist, can't download", newFileName));
                }

                //SftpATTRS attr =;
                //long cl = m_sftp.stat(remotePath).getSize();//;GetFileSize(remotePath);

                m_sftp.get(remotePath, newFileName);


            }
            catch (Exception ex)
            {
                throw new Exception("下載失敗：\r\n" + ex.Message);
            }
            finally
            {

            }
        }


        //删除SFTP文件  
        /// <summary>
        /// 刪除SFTP上的文件
        /// </summary>
        /// <param name="remoteFile">SFTP上的文件名，不包含sftp://ip:port的部分，只包含文件夾和文件名</param>
        /// <returns></returns>
        public void Delete(string remoteFile)
        {
            try
            {
                if (remoteFile.StartsWith("/"))
                {
                    remoteFile = remoteFile.Substring(1, remoteFile.Length - 1);
                }
                Connect();
                m_sftp.rm(remoteFile);
            }
            catch (Exception ex)
            {
                throw new Exception("刪除失敗：\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 刪除SFTP上的文件夾
        /// </summary>
        /// <param name="remoteFolder">SFTP上的文件夾，不包含sftp://ip:port的部分，只包含文件夾路徑</param>
        /// <returns></returns>
        public void DeleteFolder(string remoteFolder)
        {
            try
            {
                if (remoteFolder.StartsWith("/"))
                {
                    remoteFolder = remoteFolder.Substring(1, remoteFolder.Length - 1);
                }
                Connect();
                m_sftp.rmdir(remoteFolder);
            }
            catch (Exception ex)
            {
                throw new Exception("刪除文件夾失敗：\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 創建文件夾
        /// </summary>
        /// <param name="folderPath">FTP上的文件夾，不包含sftp://ip:port的部分，只包含文件夾路徑</param>
        public void MakeFolder(string folderPath)
        {
            Connect();
            if (folderPath.StartsWith("/"))
            {
                folderPath = folderPath.Substring(1, folderPath.Length - 1);
            }
            m_sftp.mkdir(folderPath);
        }

        //
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        public void Rename(string oldFileName, string newFileName)
        {
            Connect();
            m_sftp.rename(oldFileName, newFileName);
        }

        /// <summary>
        /// 獲取文件列表
        /// </summary>
        /// <param name="remotePath">SFTP上的文件夾，不包含sftp://ip:port的部分，只包含文件夾路徑</param>
        /// <returns></returns>
        public DataTable GetFileList(string remotePath)
        {
            try
            {
                Connect();
                if (remotePath == "")
                {
                    remotePath = ".";
                }
                if (remotePath.StartsWith("/"))
                {
                    remotePath = remotePath.Substring(1, remotePath.Length - 1);
                }
                if (remotePath.EndsWith("/"))
                {
                    remotePath = remotePath.Substring(0, remotePath.Length - 1);
                }

                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                if (vvv.Count == 0)
                {
                    return null;
                }

                DataTable dt = GetFileListDateTable();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string filename = qqq.getFilename();//System.Text.Encoding.UTF8.GetString(qqq.getFilename().getBytes());
                    if (filename == ".." || filename == ".")
                    {
                        continue;
                    }
                    DataRow dr = dt.NewRow();


                    dr["File Name"] = filename;
                    dr["FileFullPath"] = remotePath + "/" + filename;
                    dr["Size"] = qqq.getAttrs().getSize();
                    dr["Modify Date"] = qqq.getAttrs().getMtimeString();
                    if (qqq.getLongname().ToString().StartsWith("d"))
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
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("刪除文件列表失敗：\r\n" + ex.Message);
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

        private long GetFileSize(string remotePath)
        {
            try
            {
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                if (vvv.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return ((Tamir.SharpSsh.jsch.ChannelSftp.LsEntry)vvv[0]).getAttrs().getSize();
                }
            }
            catch
            {
                return 0;
            }
        }

        //登录验证信息          
        public class MyUserInfo : UserInfo
        {
            String passwd;
            public String getPassword() { return passwd; }
            public void setPassword(String passwd) { this.passwd = passwd; }

            public String getPassphrase() { return null; }
            public bool promptPassphrase(String message) { return true; }

            public bool promptPassword(String message) { return true; }
            public bool promptYesNo(String message) { return true; }
            public void showMessage(String message) { }
        }
    }
}
