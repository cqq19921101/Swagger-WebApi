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

namespace SFTP
{
    public class SFTP 
    {
        private Session m_session;
        private Channel m_channel;
        private ChannelSftp m_sftp;
        System.Threading.Thread tMonitor = null;
        public delegate void UpdateProgressStatus(object o,string Content, decimal iProgress);
        
        string Name = "", Site = "", UserID = "", Password = "", Port = "22";
        string Renamefile = "N", ActionWhileFileExist = "Override";

        private string Uploaddownloadpauseflag = "N";

        public SFTP(string _SiteName, string _SiteIP, string _UserID, string _Password, string _Port)
        {
            Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = _Port;
            SetSFTPPara(UserID, Site, Password, Convert.ToInt32(Port));
        }
        public SFTP(string _SiteName, string _SiteIP, string _UserID, string _Port)
        {
            Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Port = _Port;
            SetSFTPPara(UserID, Site,Convert.ToInt32(Port));
        }

        public SFTP(string _SiteName, string _SiteIP, string _UserID, string _Password, string _Port, string _Renamefilewhileuploadingdowloading, string _ActionWhileFileExist)
        {
            Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Password = _Password;
            Port = _Port;
            Renamefile = _Renamefilewhileuploadingdowloading;
            ActionWhileFileExist = _ActionWhileFileExist;
            SetSFTPPara(UserID, Site, Password, Convert.ToInt32(Port));
        }
        public SFTP(string _SiteName, string _SiteIP, string _UserID, string _Port, string _Renamefilewhileuploadingdowloading, string _ActionWhileFileExist)
        {
            Name = _SiteName;
            Site = _SiteIP;
            UserID = _UserID;
            Port = _Port;
            Renamefile = _Renamefilewhileuploadingdowloading;
            ActionWhileFileExist = _ActionWhileFileExist;
            SetSFTPPara(UserID, Site, Convert.ToInt32(Port));
        }

        private void SetSFTPPara(string user,string ip,string pwd,int port)
        {
            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);
            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);
        }
        private void SetSFTPPara(string user, string ip, int port)
        {
            JSch jsch = new JSch();
            jsch.addIdentity(Application.StartupPath+@"/id_rsa");
            m_session = jsch.getSession(user, ip, port);
            MyUserInfo ui = new MyUserInfo();
            m_session.setUserInfo(ui);
        }

        public SFTP(string host, string user, string pwd)
        {
            string[] arr = host.Split(':');
            string ip = arr[0];
            int port = 22;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);
            
            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);

        }
        public SFTP(string host, string user)
        {
            string[] arr = host.Split(':');
            string ip = arr[0];
            int port = 22;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            JSch jsch = new JSch();
            jsch.addIdentity(Application.StartupPath + @"/id_rsa");
            m_session = jsch.getSession(user, ip, port);

            MyUserInfo ui = new MyUserInfo();
            m_session.setUserInfo(ui);

        }

        public string UploadDownloadPause
        {
            set { Uploaddownloadpauseflag = value; }
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
            catch
            {
                //return false;
            }
        }

        //断开SFTP          
        public void Disconnect()
        {
            if (Connected)
            {
                m_channel.disconnect();
                m_session.disconnect();
            }
        }

        //测试连接状态
        public bool CheckFtpConnectionStatus(out string errmsg)
        {
            errmsg = "";
            try
            {
                if (!Connected)
                {
                    try
                    {
                        m_session.connect();
                        m_channel = m_session.openChannel("sftp");
                        m_channel.connect();
                        m_sftp = (ChannelSftp)m_channel;
                        return true;
                    }
                    catch (Exception exConn)
                    {
                        errmsg = "Connect error:" + exConn.Message;
                        return false;
                    }
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                errmsg = "Connect error:" + ex.Message.ToString();
                return false;
            }
        }

        //SFTP存放文件          
        public bool Upload(string localPath, string remotePath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(localPath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(remotePath);
                m_sftp.put(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath)
        {
            FileInfo fileInf = new FileInfo(Filename);
            //如果ActionWhileFileExist是Rename
            string urifordeleteexistfile = "";
            string uri = "";
            string filename = fileInf.FullName.Substring(initialpath.Length + 1, fileInf.FullName.Length - initialpath.Length - 1).Replace("\\", "/");

            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }
            if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
            {
                uri = filename;
                urifordeleteexistfile = filename;
            }
            else
            {
                uri = FtpPathWithoutIP + filename;
                urifordeleteexistfile = FtpPathWithoutIP + filename;
            }
            if (uri.StartsWith("/"))
            {
                uri = uri.Substring(1, uri.Length - 1);
            }
            
            //上传已有文件是覆盖还是重命名待上传文件，如果设置是Rename，查询一下文件是否存在，存在则给待上传文件加上时间戳
            string trueFileName = "";
            if (ActionWhileFileExist == "Rename")
            {
                if (CheckFileExist(uri, fileInf.Name.ToString()) == true)
                {
                    //文件uri重新设置
                    string extname = fileInf.Extension.ToString();
                    filename = filename.Substring(0, filename.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
                    {
                        uri =  filename;
                    }
                    else
                    {
                        uri = FtpPathWithoutIP + filename;
                    }
                }
            }
            
            //上传文件是否要使用临时文件名，如果要，加上后缀.tmp
            trueFileName = uri;
            if (Renamefile == "Y")
            {
                uri = uri + ".tmp";
            }

            uri = uri.Replace(@"\", "/");
            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen;
            int uploadedlen;
            int irefreshflag = 0;

            FileStream fs = fileInf.OpenRead();
            //Tamir.SharpSsh.java.io.InputStream iptStream = new Tamir.SharpSsh.java.io.FileInputStream(Filename);/

            Tamir.SharpSsh.java.io.OutputStream optStream = m_sftp.put(uri);
            try
            {
                contentLen=fs.Read(buff, 0, buffLength);
                uploadedlen = contentLen;

                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                decimal d_persent = 0;
                // 流内容没有结束
                
                while (contentLen != 0)
                {
                    optStream.Write(buff, 0, contentLen);
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

                d_persent = Convert.ToDecimal(1);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Uploaded " + fileInf.Name, d_persent });

                //如果是上传过程用临时文件名的，到这里已经上传完毕了，把文件名改回去
                optStream.close();
                if (Renamefile == "Y")
                {
                    try
                    {
                        Delete(urifordeleteexistfile);
                    }
                    catch { }
                    Rename(uri, trueFileName);
                }
                //filenamewithotpath = null;
                trueFileName = null;
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

        public void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath, int offset)
        {
            FileInfo fileInf = new FileInfo(Filename);
            //如果ActionWhileFileExist是Rename
            string uri = "";
            string urifordeleteexistfile = "";
            string filename = fileInf.FullName.Substring(initialpath.Length + 1, fileInf.FullName.Length - initialpath.Length - 1).Replace("\\", "/");

            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }
            if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
            {
                uri = filename;
                urifordeleteexistfile = filename;
            }
            else
            {
                uri = FtpPathWithoutIP + filename;
                urifordeleteexistfile = FtpPathWithoutIP + filename;
            }
            if (uri.StartsWith("/"))
            {
                uri = uri.Substring(1, uri.Length - 1);
            }

            //上传已有文件是覆盖还是重命名待上传文件，如果设置是Rename，查询一下文件是否存在，存在则给待上传文件加上时间戳
            string trueFileName = "";
            if (ActionWhileFileExist == "Rename")
            {
                if (CheckFileExist(uri, fileInf.Name.ToString()) == true)
                {
                    //文件uri重新设置
                    string extname = fileInf.Extension.ToString();
                    filename = filename.Substring(0, filename.Length - extname.Length) + "_" + DateTime.Now.ToString("yyyyMMddHHssmm") + extname;
                    if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
                    {
                        uri = filename;
                    }
                    else
                    {
                        uri = FtpPathWithoutIP + filename;
                    }
                }
            }


            //上传文件是否要使用临时文件名，如果要，加上后缀.tmp
            trueFileName = uri;
            if (Renamefile == "Y")
            {
                uri = uri + ".tmp";
            }

            uri = uri.Replace(@"\", "/");
            int buffLength = 2048;// 缓冲大小设置为kb
            byte[] buff = new byte[buffLength];
            int contentLen = 0;
            int uploadedlen = 0;
            int irefreshflag = 0;

            FileStream fs = fileInf.OpenRead();
            //Tamir.SharpSsh.java.io.InputStream iptStream = new Tamir.SharpSsh.java.io.FileInputStream(Filename);/

            Tamir.SharpSsh.java.io.OutputStream optStream = null;
            if (offset > 0)
            {
                optStream = m_sftp.put(uri,2);
                fs.Seek(offset, System.IO.SeekOrigin.Current);
                uploadedlen = offset;
            }
            else
            {
                optStream = m_sftp.put(uri);
            }
            try
            {
                contentLen = fs.Read(buff, 0, buffLength);
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

                    optStream.Write(buff, 0, contentLen);
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
                optStream.close();
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
                    Rename(uri, trueFileName);
                }
                //filenamewithotpath = null;
                trueFileName = null;
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

        public bool CheckFileExist(string FtpFileFullPath, string FileNameWithoutPath)
        {
            try
            {
                string filecheckexistfolder = FtpFileFullPath.Substring(0, FtpFileFullPath.Length - FileNameWithoutPath.Length);
                //filecheckexistfolder = filecheckexistfolder.Substring(("ftp://" + Site + ":" + Port).Length, filecheckexistfolder.Length - ("ftp://" + Site + ":" + Port).Length);

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

        //SFTP获取文件   
        public void ResumeDownload()
        {
            Connect();
            m_sftp.Stop = false;
        }

        public void StopDownload()
        {
            if (m_sftp != null)
            {
                m_sftp.Stop = true;

            }
            if (tMonitor != null)
            {

                if (tMonitor.ThreadState == System.Threading.ThreadState.Running)
                {
                    tMonitor.Abort();
                    
                }
            }
            GC.Collect();
        }

        public bool Download(string remotePath, string localPath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(remotePath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(localPath);
                m_sftp.get(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Download(object o, int iRow, string localPath, string remotePath)
        {
            //Stream ftpStream = null;
            //FileStream outputStream = null;
            try
            {
                if (remotePath.StartsWith("/"))
                {
                    remotePath = remotePath.Substring(1, remotePath.Length - 1);
                }
                string newFileName = "";
                localPath = localPath + "\\" + remotePath.Substring(remotePath.LastIndexOf("/") + 1, remotePath.Length - remotePath.LastIndexOf("/") - 1);
                if (Renamefile == "Y")
                {
                    newFileName = localPath + ".tmp";
                }
                else
                {
                    newFileName = localPath;
                }
                if (File.Exists(newFileName))
                {
                    File.SetAttributes(newFileName, FileAttributes.Normal);
                    File.Delete(newFileName);

                    //throw new Exception(string.Format("The file {0} is already exist, can't download", newFileName));
                }

                //SftpATTRS attr =;
                long cl = m_sftp.stat(remotePath).getSize();//;GetFileSize(remotePath);

                //ftpStream = m_sftp.get(remotePath);
                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", "??") + "--->Downloading " + newFileName, Convert.ToDecimal(0) });



                System.Threading.Thread tMonitor = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RefreshDownloadProgress));

                clsParas cp = new clsParas();
                cp.callType = "Normal";
                cp.o = o;
                cp.iRow = iRow;
                cp.mi = mi;
                cp.totalSize = cl;
                cp.localFileName = newFileName;
                tMonitor.IsBackground = true;
                tMonitor.Start(cp);

                try
                {
                    m_sftp.get(remotePath, newFileName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    tMonitor.Abort();
                }
                #region 速度慢
                //int bufferSize = 2048;
                //int readCount;
                //int downloadcount;
                //int irefreshflag = 0;

                //byte[] buffer = new byte[bufferSize];
                //readCount = ftpStream.Read(buffer, 0, bufferSize);
                //downloadcount = readCount;

                //MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                //decimal d_persent = 0;

                //outputStream = new FileStream(newFileName, FileMode.Create);
                //while (readCount > 0)
                //{
                //    outputStream.Write(buffer, 0, readCount);

                //    if (cl > 0)
                //    {
                //        d_persent = Convert.ToDecimal(Convert.ToDecimal(downloadcount) / Convert.ToDecimal(cl));
                //        mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloading " + remotePath, d_persent });
                //        irefreshflag++;
                //        if (irefreshflag == 15)
                //        {
                //            //循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                //            irefreshflag = 0;
                //            Application.DoEvents();
                //        }
                //    }
                //    readCount = ftpStream.Read(buffer, 0, bufferSize);

                //    downloadcount = downloadcount + readCount;
                //}
                //ftpStream.Close();
                //outputStream.Close();

                //d_persent = Convert.ToDecimal(1);
                #endregion

                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(1 * 100)) + "--->Downloaded " + remotePath, Convert.ToDecimal(1) });

                if (Renamefile == "Y")
                {
                    if (File.Exists(localPath))
                    {
                        File.SetAttributes(localPath, FileAttributes.Normal);
                        File.Delete(localPath);
                    }
                    File.Move(newFileName, localPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (ftpStream != null)
                //{
                //    ftpStream.Close();
                //    ftpStream = null;
                //}
                //if (outputStream != null)
                //{
                //    outputStream.Close();
                //    outputStream.Dispose();
                //}
            }
        }

        
        public void Download(object o, int iRow, string localPath, string FileNameWithoutPath, string remotePath, int offset)
        {
            //Tamir.SharpSsh.java.io.InputStream ftpStream = null;
            //Tamir.SharpSsh.java.io.OutputStream ftpStream = null;
            //FileStream outputStream = null;
            try
            {
                string trueFileName = localPath + "\\" + FileNameWithoutPath;

                if(remotePath.StartsWith("."))
                {
                    remotePath = remotePath.Substring(1, remotePath.Length - 1);
                }
                if (remotePath.StartsWith("/"))
                {
                    remotePath = remotePath.Substring(1, remotePath.Length - 1);
                }
                string newFileName = "";
                localPath = localPath + "\\" + remotePath.Substring(remotePath.LastIndexOf("/") + 1, remotePath.Length - remotePath.LastIndexOf("/") - 1);
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
                //SftpATTRS attr =;
                //long cl = m_sftp.stat(remotePath).getSize();//;GetFileSize(remotePath);
                long cl = GetFileSize(remotePath);
                if (cl == 0)
                {
                    
                }

                MethodInfo mi = getFormMethod.getMethod(o.GetType().FullName);
                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", "??") + "--->Downloading " + FileNameWithoutPath, 0 });



                tMonitor = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RefreshDownloadProgress));

                clsParas cp = new clsParas();
                cp.callType = "Breakpoint";
                cp.o = o;
                cp.iRow = iRow;
                cp.mi = mi;
                cp.totalSize = cl;
                cp.localFileName = newFileName;
                tMonitor.IsBackground = true;
                tMonitor.Start(cp);

                try
                {
                    if (offset > 0)
                    {
                        m_sftp.get(remotePath, newFileName, null, 1);
                    }
                    else
                    {
                        m_sftp.get(remotePath, newFileName);
                    }

                    if (m_sftp.Stop)
                    {
                        m_sftp.exit();

                        m_sftp = null;
                        Disconnect();
                    }
                    //    //下面这个方法太慢
                        #region
                        //ftpStream = m_sftp.get(remotePath);
                        ////m_sftp.get(remotePath, ftpStream, null, 1, offset);

                        //int bufferSize = 2048;
                        //int readCount;
                        //int downloadcount = 0;
                        //int irefreshflag = 0;
                        ////if (offset != 0)
                        ////{
                        ////    m_sftp.get(
                        ////    ftpStream.Seek(offset, System.IO.SeekOrigin.Current);
                        ////}
                        //downloadcount = offset;

                        //byte[] buffer = new byte[bufferSize];
                        ////ftpStream.Seek(offset, System.IO.SeekOrigin.Begin);
                        //readCount = ftpStream.Read(buffer, 0, bufferSize);
                        //downloadcount = downloadcount + readCount;


                        //decimal d_persent = 0;

                        //if (offset == 0)
                        //{
                        //    outputStream = new FileStream(newFileName, FileMode.Create);
                        //}
                        //else
                        //{
                        //    outputStream = new FileStream(newFileName, FileMode.Append);
                        //}

                        //while (readCount > 0)
                        //{
                        //    if (Uploaddownloadpauseflag == "Y")
                        //    {
                        //        break;
                        //    }
                        //    outputStream.Write(buffer, 0, readCount);

                        //    if (cl > 0)
                        //    {
                        //        d_persent = Convert.ToDecimal(Convert.ToDecimal(downloadcount) / Convert.ToDecimal(cl));
                        //        mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(d_persent * 100)) + "--->Downloading " + FileNameWithoutPath, downloadcount });
                        //        irefreshflag++;
                        //        if (irefreshflag == 15)
                        //        {
                        //            //循环15次刷新一下界面，防止因为DoEvents而减慢上传速度
                        //            irefreshflag = 0;
                        //            Application.DoEvents();
                        //        }
                        //    }
                        //    readCount = ftpStream.Read(buffer, 0, bufferSize);

                        //    downloadcount = downloadcount + readCount;
                        //}
                        //ftpStream.Close();
                        //outputStream.Close();
                        #endregion


                    //if (Uploaddownloadpauseflag == "Y")
                    //{
                    //    return;
                    //}
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return;
                }
                finally
                {
                   
                    tMonitor.Abort();
  
                }
                if (m_sftp==null)
                {
                    return;
                }

                mi.Invoke(o, new object[] { iRow, String.Format("{0}" + "%", Convert.ToInt32(1 * 100)) + "--->Downloaded " + FileNameWithoutPath, -1 });

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
                //if (ftpStream != null)
                //{
                //    ftpStream.Close();
                //    ftpStream = null;
                //}
                //if (outputStream != null)
                //{
                //    outputStream.Close();
                //    outputStream.Dispose();
                //}
            }
        }

        public void CallDelegate(object o, string Content, decimal iProgress)
        {
            UpdateProgressStatus ups = new UpdateProgressStatus(this.RefreshProgress);
            ups.Invoke(o, Content, iProgress);
        }

        private void RefreshProgress(object o,string per,decimal progress)
        {
            MethodInfo mi = getFormMethod.getMethod("FTPTool.frmDownloadStatusWindow");
            mi.Invoke(o, new object[] { 1, String.Format("{0}" + "%", per) + "--->Downloading ", progress });
        }

        protected class clsParas
        {
            public string callType;
            public object o;
            public int iRow;
            public MethodInfo mi;
            public long totalSize;
            public string localFileName;
        }

        private void RefreshDownloadProgress(object Paras)
        {
            int refreshcycle = 0;
            clsParas cp = (clsParas)Paras;
            long currentSize=0;
            while (true)
            {
                if (File.Exists(cp.localFileName))
                {
                    FileInfo fi = new FileInfo(cp.localFileName);
                    currentSize = fi.Length;
                    while (currentSize < cp.totalSize)
                    {
                        decimal d_persent = Convert.ToDecimal(Convert.ToDecimal(currentSize) / Convert.ToDecimal(cp.totalSize));
                        fi = new FileInfo(cp.localFileName);
                        currentSize = fi.Length;
                        if (d_persent == 0)
                        {
                            continue;
                        }
                        refreshcycle++;
                        if (refreshcycle > 15)
                        {
                            refreshcycle = 0;
                            try
                            {
                                string per = Convert.ToInt32(d_persent * 100).ToString();
                                if (cp.callType == "Normal")
                                {
                                    //CallDelegate(cp.o, per, d_persent);
                                   // MethodInfo mi2 = getFormMethod.getMethod(cp.o.GetType().FullName);
                                    cp.mi.Invoke(cp.o, new object[] { cp.iRow, String.Format("{0}" + "%", per) + "--->Downloading ", d_persent });
                                }
                                else
                                {
                                    cp.mi.Invoke(cp.o, new object[] { cp.iRow, String.Format("{0}" + "%", per) + "--->Downloading ", 0 });
                                }
                                Application.DoEvents();
                            }
                            catch { }
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }


        //删除SFTP文件  
        public bool Delete(string remoteFile)
        {
            try
            {
                if (remoteFile.StartsWith("/"))
                {
                    remoteFile = remoteFile.Substring(1, remoteFile.Length - 1);
                }
                m_sftp.rm(remoteFile);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool DeleteFolder(string remoteFolder)
        {
            try
            {
                if (remoteFolder.StartsWith("/"))
                {
                    remoteFolder = remoteFolder.Substring(1, remoteFolder.Length - 1);
                }
                m_sftp.rmdir(remoteFolder);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //创建文件夹
        public void MakeFolder(string folderPath)
        {
            Connect();
            if (folderPath.StartsWith("/"))
            {
                folderPath = folderPath.Substring(1, folderPath.Length - 1);
            }
            m_sftp.mkdir(folderPath);
        }

        public void MakeFolder(string folder, string FtpPathWithoutIP, string initialpath)
        {
            try
            {
                Connect();
                string uri = "";
                folder = folder.Substring(initialpath.Length, folder.Length - initialpath.Length).Replace(@"\", "/");

                //folder = folder.Replace(initialpath, "").Replace(@"\", "/");
                if (folder.StartsWith("/"))
                {
                    folder = folder.Substring(1, folder.Length - 1);
                }

                if (FtpPathWithoutIP == "/" || FtpPathWithoutIP == "")
                {
                    uri = folder;
                }
                else
                {
                    uri = FtpPathWithoutIP + folder;
                }
                if (uri.StartsWith("/"))
                {
                    uri = uri.Substring(1, uri.Length - 1);
                }
                m_sftp.mkdir(uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //重命名
        public void Rename(string oldFileName, string newFileName)
        {
            Connect();
            m_sftp.rename(oldFileName, newFileName);
        }

        //获取SFTP文件列表          
        public ArrayList GetFileList(string remotePath, string fileType)
        {
            try
            {
                if (remotePath == "")
                {
                    remotePath = "/";
                }
                
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string sss = qqq.getFilename();
                    if (sss.Length > (fileType.Length + 1) && fileType == sss.Substring(sss.Length - fileType.Length))
                    { objList.Add(sss); }
                    else { continue; }
                }

                return objList;
            }
            catch
            {
                return null;
            }
        }

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

                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                if (vvv.Count == 0)
                {
                    return null;
                }

                DataTable dt = GetFileListDateTable();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string filename = qqq.getFilename();//System.Text.Encoding.UTF8.GetString(qqq.getFilename().getBytes());
                    if (filename == ".." || filename==".")
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
            catch
            {
                return null;
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
