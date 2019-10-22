using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace ActionBLL
{
   
    public class Action
    {
        /// <summary>
        /// RunAction
        /// </summary>
        /// <param name="o">Main form=frmMain</param>
        /// <param name="iRow">Current run schedule row number, start with zero</param>
        /// <param name="type">FTP or SFTP</param>
        /// <param name="SiteName">FTP site profile name</param>
        /// <param name="SiteIP">FTP server IP</param>
        /// <param name="UserID">UserID</param>
        /// <param name="Password">Password</param>
        /// <param name="Port">Connect port</param>
        /// <param name="Action">Run action</param>
        /// <param name="RemoteFileFolder">Remote file or folder</param>
        /// <param name="RemoteIsFolder">Is remote a folder</param>
        /// <param name="LocalFileFolder">Local file or folder</param>
        /// <param name="LocalIsFolder">Is local a folder</param>
        public virtual void RunAction(object o, int iRow,string type,string SiteName,string SiteIP,string UserID,string Password,string Port,
            string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
        }

        public virtual void RunAction(object o, int iRow, string type,string SiteName, string SiteIP, string UserID, string Password,string Port,string Renamefilewhileuploading, string ActionWhileFileExist,
            string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
        }

        public virtual string RunAction(object o,string type, string SiteName, string SiteIP, string UserID, string Password,string Port)
        {
            return "";
        }

        public virtual void RunAction(object o,int iRow, string localFileFolder,string localIsFolder,string backupFolder,string byDay)
        {

        }
    }

    public class GetAction
    {
        public static Action GetActionType(string runtype)
        {
            switch (runtype)
            {
                case "TestFtpConnection":
                     return new TestFtpConnection();
                    break;
                case "Run External Program":
                    return new RunExternalProgram();
                    break;
                case "Upload":
                    return new Upload();
                    break;
                case "Download":
                    return new Download();
                    break;
                case "Delete":
                    return new Delete();
                    break;
                case "DeleteRemoteExceptParentFolder":
                    return new DeleteRemoteExceptParentFolder();
                    break;
                case "Rename":
                    return new Rename();
                    break;
                case "MoveLocalFileToBackupFolder":
                    return new MoveLocalFileToBackupFolder();
                default:
                    return null;
            }
        }
    }

    class TestFtpConnection:Action
    {
        public override string RunAction(object o, string type,string SiteName, string SiteIP, string UserID, string Password,string Port)
        {
            try
            {
                string errmsg = "";
                bool connectstatus = true;
                if (type == "FTP")
                {
                    FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);
                    connectstatus = ftp.CheckFtpConnectionStatus(out errmsg);
                }
                else if (type == "SSH")
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
                }
                else
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                }
                if (connectstatus)
                {
                    return "OK";
                }
                else
                {
                    return errmsg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    class MoveLocalFileToBackupFolder :Action
    {
        public override void RunAction(object o,int iRow,string localFileFolder, string localIsFolder, string backupFolder, string byDay)
        {
            try
            {
                if (localIsFolder.EndsWith("\\"))
                {
                    localIsFolder = localIsFolder.Substring(0, localIsFolder.Length - 1);
                }
                if (backupFolder.EndsWith("\\"))
                {
                    backupFolder = backupFolder.Substring(0, backupFolder.Length - 1);
                }
                if (!Directory.Exists(backupFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(backupFolder);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Backup folder create error");
                    }
                }
                if (byDay == "Y")
                {
                    backupFolder = backupFolder + "\\" + DateTime.Today.ToString("yyyyMMdd");
                    if (!Directory.Exists(backupFolder))
                    {
                        try
                        {
                            Directory.CreateDirectory(backupFolder);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Backup folder create error");
                        }
                    }
                }
                if (localIsFolder == "Y")
                {
                    DoMoveWhileLocalIsFolder(o, iRow, localFileFolder, backupFolder);
                }
                else
                {
                    int last = localFileFolder.LastIndexOf("\\");
                    string ext = localFileFolder.Substring(last + 1, localFileFolder.Length - last - 1);
                    string truelocalfilefolder = localFileFolder.Substring(0, localFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        DoMoveWhileLocalIsFolder(o, iRow, localFileFolder, backupFolder);
                    }
                    else if (ext.IndexOf('*') >= 0)
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                MoveFile(backupFolder, fi);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        if (File.Exists(localFileFolder))
                        {
                            MoveFile(backupFolder, new FileInfo(localFileFolder));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoMoveWhileLocalIsFolder(object o, int iRow, string currentfolder,string backupFolder)
        {

            DirectoryInfo di = new DirectoryInfo(currentfolder);


            foreach (FileInfo fi in di.GetFiles())
            {
                string bkPath = backupFolder + "\\" + fi.DirectoryName.Substring(3, fi.DirectoryName.Length - 3);
                MoveFile(bkPath, fi);
            }

            DirectoryInfo[] dis = di.GetDirectories();

            foreach (DirectoryInfo dii in dis)
            {                
                DoMoveWhileLocalIsFolder(o, iRow,dii.FullName,backupFolder);
            }
        }

        private void MoveFile(string bkPath,FileInfo fi)
        {
            try
            {
                if (!Directory.Exists(bkPath))
                {
                    Directory.CreateDirectory(bkPath);
                }
                if (File.Exists(bkPath + "\\" + fi.Name))
                {
                    fi.MoveTo(bkPath + "\\" + fi.Name.Replace(fi.Extension, "") + "_" + DateTime.Now.ToString("HHmmssfff") + fi.Extension);
                }
                else
                {
                    fi.MoveTo(bkPath + "\\" + fi.Name);
                }
            }
            catch (Exception ex)
            {
                bkPath = null;
            }
        }
    }

   

    class RunExternalProgram : Action
    {
        public override void RunAction(object o, int iRow, string type,string SiteName, string SiteIP, string UserID, string Password,string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            try
            {

                MethodInfo mi = getFormMethod.getMethod();

                mi.Invoke(o, new object[] { iRow, "Running " + RemoteFileFolder, Convert.ToDecimal(Convert.ToDecimal(50) / 100) });
                Application.DoEvents();

                Process p = Process.Start(RemoteFileFolder);
                p.WaitForExit();
                
                mi.Invoke(o, new object[] { iRow, "Finished " + RemoteFileFolder, Convert.ToDecimal(Convert.ToDecimal(100) / 100) });
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    class Upload : Action
    {
        /// <summary>
        /// Upload without rename file
        /// </summary>
        /// <param name="o"></param>
        /// <param name="iRow"></param>
        /// <param name="type">FTP or SFTP</param>
        /// <param name="SiteName"></param>
        /// <param name="SiteIP"></param>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <param name="Port"></param>
        /// <param name="Action"></param>
        /// <param name="RemoteFileFolder"></param>
        /// <param name="RemoteIsFolder"></param>
        /// <param name="LocalFileFolder"></param>
        /// <param name="LocalIsFolder"></param>
        public override void RunAction(object o, int iRow, string type,string SiteName, string SiteIP, string UserID, string Password,string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            if (type == "FTP")
            {
                FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        ftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder(o, iRow, ftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder(o, iRow, ftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*')>=0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    ftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                ftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        ftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
            else if (type == "SSH")
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                sftp.Connect();
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        sftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        sftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
            else
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                sftp.Connect();
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        sftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        sftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
        }


        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port,
            string Renamefilewhileuploadingdownloading, string ActionWhileFileExist, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            if (type == "FTP")
            {
                FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        ftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder(o, iRow, ftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder(o, iRow, ftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    ftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                ftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        ftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
             else if (type == "SSH")
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);
                sftp.Connect();
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        sftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        sftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
            else
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);
                sftp.Connect();
                if (LocalFileFolder.EndsWith("\\"))
                {
                    LocalFileFolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - 1);
                }
                if (LocalIsFolder == "Y")
                {
                    string ipath = LocalFileFolder.Substring(0, LocalFileFolder.LastIndexOf("\\"));
                    try
                    {
                        sftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                    }
                    catch { }
                    DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, LocalFileFolder, ipath);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要上传
                        DoWhileLocalIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder, truelocalfilefolder, truelocalfilefolder);
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只上传源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            //{
                            //    sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            //}
                            if (matchFile.CheckMatchFile(fi.Name, ext))
                            {
                                sftp.Upload(o, iRow, RemoteFileFolder, fi.FullName, truelocalfilefolder);
                            }
                        }
                    }
                    else
                    {
                        //只上传指定文件
                        sftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);
                    }

                }
            }
        }

        private void DoWhileLocalIsFolder(object o, int iRow, FTP.FTP ftp, string FtpPathWithoutIP, string currentfolder, string initialpath)
        {

            DirectoryInfo di = new DirectoryInfo(currentfolder);


            foreach (FileInfo fi in di.GetFiles())
            {
                ftp.Upload(o, iRow, FtpPathWithoutIP, fi.FullName, initialpath);
            }

            DirectoryInfo[] dis = di.GetDirectories();

            foreach (DirectoryInfo dii in dis)
            {
                try
                {
                    ftp.MakeFolder(dii.FullName, FtpPathWithoutIP,initialpath);
                }
                catch { }
                DoWhileLocalIsFolder(o, iRow, ftp, FtpPathWithoutIP,dii.FullName.ToString(), initialpath);
            }

        }

        private void DoWhileLocalIsFolder_SFTP(object o, int iRow, SFTP.SFTP sftp, string FtpPathWithoutIP, string currentfolder, string initialpath)
        {

            DirectoryInfo di = new DirectoryInfo(currentfolder);


            foreach (FileInfo fi in di.GetFiles())
            {
                sftp.Upload(o, iRow, FtpPathWithoutIP, fi.FullName, initialpath);
            }

            DirectoryInfo[] dis = di.GetDirectories();

            foreach (DirectoryInfo dii in dis)
            {
                try
                {
                    sftp.MakeFolder(dii.FullName, FtpPathWithoutIP, initialpath);
                }
                catch { }
                DoWhileLocalIsFolder_SFTP(o, iRow, sftp, FtpPathWithoutIP, dii.FullName.ToString(), initialpath);
            }

        }

    }

    class Download : Action
    {
        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            if (type == "FTP")
            {
                FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);

                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder(o, iRow, ftp, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    ftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
            }
            else if (type == "SSH")
            {
                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder_SSH(o, iRow, SiteName, SiteIP, UserID,Password, Port, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                    sftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
            }
            else
            {
                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder_SFTP(o, iRow, SiteName, SiteIP, UserID, Password, Port, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                    sftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
            }
        }

        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port,
            string Renamefilewhileuploadingdownloading, string ActionWhileFileExist, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            if (type == "FTP")
            {
                FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);

                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder(o, iRow, ftp, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    ftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
            }
            else if (type == "SSH")
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);
                sftp.Connect();
                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder_SSH(o, iRow, SiteName, SiteIP, UserID,Password, Port, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    sftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
                sftp.Disconnect();
                sftp = null;
            }
            else
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port, Renamefilewhileuploadingdownloading, ActionWhileFileExist);
                sftp.Connect();
                if (RemoteIsFolder == "Y")
                {
                    //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                    int ilast;
                    string tmpremotefolder;
                    if (RemoteFileFolder.EndsWith("/"))
                    {
                        tmpremotefolder = RemoteFileFolder.Substring(0, RemoteFileFolder.Length - 1);
                    }
                    else
                    {
                        tmpremotefolder = RemoteFileFolder;
                    }
                    ilast = tmpremotefolder.LastIndexOf("/");
                    string ipath = RemoteFileFolder.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                    if (Directory.Exists(LocalFileFolder + "\\" + ipath) == false)
                    {
                        Directory.CreateDirectory(LocalFileFolder + "\\" + ipath);
                    }
                    DoWhileRemoteIsFolder_SFTP(o, iRow, SiteName, SiteIP, UserID, Password, Port, RemoteFileFolder, LocalFileFolder + "\\" + ipath);
                }
                else
                {
                    sftp.Download(o, iRow, LocalFileFolder, RemoteFileFolder);
                }
                sftp.Disconnect();
                sftp = null;
            }
        }


        private void DoWhileRemoteIsFolder(object o, int iRow, FTP.FTP ftp, string currentftpfolder, string targetfolder)
        {
            if (targetfolder.EndsWith("\\"))
            {
                targetfolder = targetfolder.Substring(0, targetfolder.Length - 1);
            }

            //获取文件列表 
            if(currentftpfolder=="/")
            {
                currentftpfolder="";
            }

            DataTable dtfilelist = ftp.GetFileList(currentftpfolder);
            if (dtfilelist != null)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {
                    
                    string type = (dr["IsFolder"].ToString()=="Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        ftp.Download(o, iRow, targetfolder, currentftpfolder + "/" + dr["File Name"].ToString());
                    }
                    else
                    {
                        string temptargetfolder = targetfolder + "\\" + dr["File Name"].ToString();
                        if (Directory.Exists(temptargetfolder) == false)
                        {
                            Directory.CreateDirectory(temptargetfolder);
                        }
                        DoWhileRemoteIsFolder(o, iRow, ftp, currentftpfolder + "/" + dr["File Name"].ToString(), temptargetfolder);
                    }
                }
            }        
            //string[] filelist = ftp.GetFileList(currentftpfolder);
            
            //if (filelist != null)
            //{
            //    //对al分析，是文件就下载，不是文件就访问里层目录
            //    foreach (string s in filelist)
            //    {
            //        ArrayList al = new ArrayList();
            //        if (s != "")
            //        {
            //            string[] Original = s.Split(' ');

            //            foreach (string s1 in Original)
            //            {
            //                if (s1 != "")
            //                {
            //                    al.Add(s1);
            //                }
            //            }
            //        }
            //        string filefoldername = al[8].ToString();
            //        string type = (al[0].ToString().IndexOf("d") >= 0) ? "Folder" : "File";

            //        if (type == "File")
            //        {
            //            ftp.Download(o, iRow, targetfolder, currentftpfolder + "/" + filefoldername);
            //        }
            //        else
            //        {
            //            string temptargetfolder = targetfolder + "\\" + filefoldername;
            //            if (Directory.Exists(temptargetfolder) == false)
            //            {
            //                Directory.CreateDirectory(temptargetfolder);
            //            }
            //            DoWhileRemoteIsFolder(o, iRow, ftp, currentftpfolder + "/" + filefoldername, temptargetfolder);
            //        }
            //    }
            //}        
        }

        private void DoWhileRemoteIsFolder_SFTP(object o, int iRow,string SiteName,string SiteIP,string UserID,string Password,string Port, string currentftpfolder, string targetfolder)
        {
            if (targetfolder.EndsWith("\\"))
            {
                targetfolder = targetfolder.Substring(0, targetfolder.Length - 1);
            }

            //获取文件列表 
            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
            sftp.Connect();
            DataTable dtfilelist = sftp.GetFileList(currentftpfolder);
            if (dtfilelist != null && dtfilelist.Rows.Count > 0)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string type = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        SFTP.SFTP sftpX = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                        sftpX.Connect();
                        sftpX.Download(o, iRow, targetfolder, currentftpfolder + "/" + dr["File Name"].ToString());
                        sftpX.Disconnect();
                        sftpX = null;
                    }
                    else
                    {
                        string temptargetfolder = targetfolder + "\\" + dr["File Name"].ToString();
                        if (Directory.Exists(temptargetfolder) == false)
                        {
                            Directory.CreateDirectory(temptargetfolder);
                        }
                        DoWhileRemoteIsFolder_SFTP(o, iRow, SiteName,SiteIP,UserID,Password,Port, currentftpfolder + "/" + dr["File Name"].ToString(), temptargetfolder);
                    }
                }
            }
            sftp.Disconnect();
            sftp = null;
        }
    //}
    private void DoWhileRemoteIsFolder_SSH(object o, int iRow,string SiteName,string SiteIP,string UserID,string Password,string Port, string currentftpfolder, string targetfolder)
        {
            if (targetfolder.EndsWith("\\"))
            {
                targetfolder = targetfolder.Substring(0, targetfolder.Length - 1);
            }

            //获取文件列表 
            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
            sftp.Connect();
            DataTable dtfilelist = sftp.GetFileList(currentftpfolder);
            if (dtfilelist != null && dtfilelist.Rows.Count > 0)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string type = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        SFTP.SFTP sftpX = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
                        sftpX.Connect();
                        sftpX.Download(o, iRow, targetfolder, currentftpfolder + "/" + dr["File Name"].ToString());
                        sftpX.Disconnect();
                        sftpX = null;
                    }
                    else
                    {
                        string temptargetfolder = targetfolder + "\\" + dr["File Name"].ToString();
                        if (Directory.Exists(temptargetfolder) == false)
                        {
                            Directory.CreateDirectory(temptargetfolder);
                        }
                        DoWhileRemoteIsFolder_SSH(o, iRow, SiteName,SiteIP,UserID,Password,Port, currentftpfolder + "/" + dr["File Name"].ToString(), temptargetfolder);
                    }
                }
            }
            sftp.Disconnect();
            sftp = null;
        }
    }
    class Delete : Action
    {
        bool b_delelefolderwithnofile = false;

        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {

                if (RemoteFileFolder != "")
                {
                    if (type == "FTP")
                    {
                        FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);

                        if (RemoteIsFolder == "Y")
                        {
                            DoWhileRemoteIsFolder(o, iRow, ftp, RemoteFileFolder);
                            try
                            {
                                ftp.DeleteFolder(RemoteFileFolder);
                            }
                            catch { }
                        }
                        else
                        {
                            ftp.Delete(RemoteFileFolder);
                        }
                    }
                    else if (type == "SSH")
                    {
                        SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
                        sftp.Connect();
                        if (RemoteIsFolder == "Y")
                        {
                            if (RemoteFileFolder.StartsWith("/") == false)
                            {
                                RemoteFileFolder = "/" + RemoteFileFolder;
                            }
                            DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder);
                            try
                            {
                                sftp.DeleteFolder("." + RemoteFileFolder);
                            }
                            catch { }
                        }
                        else
                        {
                            sftp.Delete(RemoteFileFolder);
                        }
                    }
                    else
                    {
                        SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                        sftp.Connect();
                        if (RemoteIsFolder == "Y")
                        {
                            if (RemoteFileFolder.StartsWith("/") == false)
                            {
                                RemoteFileFolder = "/" + RemoteFileFolder;
                            }
                            DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder);
                            try
                            {
                                sftp.DeleteFolder("." + RemoteFileFolder);
                            }
                            catch { }
                        }
                        else
                        {
                            sftp.Delete(RemoteFileFolder);
                        }
                    }
                }

                if (LocalFileFolder != "")
                {
                    if (LocalIsFolder == "Y")
                    {
                        Directory.Delete(LocalFileFolder, true);
                    }
                    else
                    {
                        int last = LocalFileFolder.LastIndexOf("\\");
                        string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                        string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                        if (ext == "*.*")
                        {
                            //除了源文件夹本身以外的所有内容都需要删除
                            string[] files = Directory.GetFiles(truelocalfilefolder);
                            foreach (string file in files)
                            {
                                try
                                {
                                    File.SetAttributes(file, FileAttributes.Normal);
                                    File.Delete(file);
                                }
                                catch { }
                            }
                            string[] dirs = Directory.GetDirectories(truelocalfilefolder);
                            foreach (string dir in dirs)
                            {
                                try
                                {
                                    Directory.Delete(dir, true);
                                }
                                catch { }
                            }
                        }
                        else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                        {
                            //只删除源文件夹当前层次的文件，无需递归
                            DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                            foreach (FileInfo fi in di.GetFiles())
                            {
                                //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                                //{
                                if (matchFile.CheckMatchFile(fi.Name, ext))
                                {
                                    try
                                    {
                                        fi.Attributes = FileAttributes.Normal;
                                        fi.Delete();
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            //只删除指定文件
                            try
                            {
                                File.Delete(LocalFileFolder);
                            }
                            catch { }
                        }
                    }
                }
           
        }

        private void DoWhileRemoteIsFolder(object o, int iRow, FTP.FTP ftp, string currentftpfolder)
        {
            //获取文件列表 
            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            DataTable dtfilelist = ftp.GetFileList(currentftpfolder);

            if (dtfilelist != null)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string filefoldername = dr["File Name"].ToString();
                    string type = (dr["IsFolder"].ToString()=="Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        ftp.Delete(currentftpfolder + "/" + filefoldername);
                    }
                    else
                    {
                        DoWhileRemoteIsFolder(o, iRow, ftp, currentftpfolder + "/" + filefoldername);
                        if (b_delelefolderwithnofile == false)
                        {
                            ftp.DeleteFolder(currentftpfolder + "/" + filefoldername);
                        }
                        b_delelefolderwithnofile = false;
                    }
                }
            }
            else
            {
                ftp.DeleteFolder(currentftpfolder);
                b_delelefolderwithnofile = true;
            }
        }

        private void DoWhileRemoteIsFolder_SFTP(object o, int iRow, SFTP.SFTP sftp, string currentftpfolder)
        {
            //获取文件列表 
            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            DataTable dtfilelist = sftp.GetFileList(currentftpfolder);

            if (dtfilelist != null && dtfilelist.Rows.Count>0)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string filefoldername = dr["File Name"].ToString();
                    string type = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        sftp.Delete(currentftpfolder + "/" + filefoldername);
                    }
                    else
                    {
                        DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, currentftpfolder + "/" + filefoldername);
                        if (b_delelefolderwithnofile == false)
                        {
                            sftp.DeleteFolder(currentftpfolder + "/" + filefoldername);
                        }
                        b_delelefolderwithnofile = false;
                    }
                }
            }
            else
            {
                sftp.DeleteFolder(currentftpfolder);
                b_delelefolderwithnofile = true;
            }
        }
    }

    class DeleteRemoteExceptParentFolder : Action
    {
        bool b_delelefolderwithnofile = false;

        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {

            if (RemoteFileFolder != "")
            {
                if (type == "FTP")
                {
                    FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);

                    if (RemoteIsFolder == "Y")
                    {
                        DoWhileRemoteIsFolder(o, iRow, ftp, RemoteFileFolder,true);
                    }
                    else
                    {
                        ftp.Delete(RemoteFileFolder);
                    }
                }
                else if (type == "SSH")
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
                    sftp.Connect();
                    if (RemoteIsFolder == "Y")
                    {
                        if (RemoteFileFolder.StartsWith("/") == false)
                        {
                            RemoteFileFolder = "/" + RemoteFileFolder;
                        }
                        DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder,true);
                    }
                    else
                    {
                        sftp.Delete(RemoteFileFolder);
                    }
                }
                else
                {
                    SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                    sftp.Connect();
                    if (RemoteIsFolder == "Y")
                    {
                        if (RemoteFileFolder.StartsWith("/") == false)
                        {
                            RemoteFileFolder = "/" + RemoteFileFolder;
                        }
                        DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, RemoteFileFolder,true);
                    }
                    else
                    {
                        sftp.Delete(RemoteFileFolder);
                    }
                }
            }

            if (LocalFileFolder != "")
            {
                if (LocalIsFolder == "Y")
                {
                    Directory.Delete(LocalFileFolder, true);
                }
                else
                {
                    int last = LocalFileFolder.LastIndexOf("\\");
                    string ext = LocalFileFolder.Substring(last + 1, LocalFileFolder.Length - last - 1);
                    string truelocalfilefolder = LocalFileFolder.Substring(0, LocalFileFolder.Length - ext.Length - 1);
                    if (ext == "*.*")
                    {
                        //除了源文件夹本身以外的所有内容都需要删除
                        string[] files = Directory.GetFiles(truelocalfilefolder);
                        foreach (string file in files)
                        {
                            try
                            {
                                File.SetAttributes(file, FileAttributes.Normal);
                                File.Delete(file);
                            }
                            catch { }
                        }
                        string[] dirs = Directory.GetDirectories(truelocalfilefolder);
                        foreach (string dir in dirs)
                        {
                            try
                            {
                                Directory.Delete(dir, true);
                            }
                            catch { }
                        }
                    }
                    else if (ext.IndexOf('*') >= 0)//(ext.StartsWith("*."))
                    {
                        //只删除源文件夹当前层次的文件，无需递归
                        DirectoryInfo di = new DirectoryInfo(truelocalfilefolder);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            //if ("*" + fi.Extension.ToUpper() == ext.ToUpper())
                            if(matchFile.CheckMatchFile(fi.Name,ext))
                            {
                                try
                                {
                                    fi.Attributes = FileAttributes.Normal;
                                    fi.Delete();
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //只删除指定文件
                        try
                        {
                            File.Delete(LocalFileFolder);
                        }
                        catch { }
                    }
                }
            }

        }

        private void DoWhileRemoteIsFolder(object o, int iRow, FTP.FTP ftp, string currentftpfolder,bool parentRemoteFolder)
        {
            //获取文件列表 

            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            DataTable dtfilelist = ftp.GetFileList(currentftpfolder);

            if (dtfilelist != null)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string filefoldername = dr["File Name"].ToString();
                    string type = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        ftp.Delete(currentftpfolder + "/" + filefoldername);
                    }
                    else
                    {
                        DoWhileRemoteIsFolder(o, iRow, ftp, currentftpfolder + "/" + filefoldername,false);
                        if (b_delelefolderwithnofile == false)
                        {
                            ftp.DeleteFolder(currentftpfolder + "/" + filefoldername);
                        }
                        b_delelefolderwithnofile = false;
                    }
                }
            }
            else
            {
                if (parentRemoteFolder == false)
                {
                    ftp.DeleteFolder(currentftpfolder);
                }
                b_delelefolderwithnofile = true;
            }

        }

        private void DoWhileRemoteIsFolder_SFTP(object o, int iRow, SFTP.SFTP sftp, string currentftpfolder, bool parentRemoteFolder)
        {
            //获取文件列表 
            if (currentftpfolder == "/")
            {
                currentftpfolder = "";
            }

            DataTable dtfilelist = sftp.GetFileList(currentftpfolder);

            if (dtfilelist != null && dtfilelist.Rows.Count > 0)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string filefoldername = dr["File Name"].ToString();
                    string type = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (type == "File")
                    {
                        sftp.Delete(currentftpfolder + "/" + filefoldername);
                    }
                    else
                    {
                        DoWhileRemoteIsFolder_SFTP(o, iRow, sftp, currentftpfolder + "/" + filefoldername,false);
                        if (b_delelefolderwithnofile == false)
                        {
                            sftp.DeleteFolder(currentftpfolder + "/" + filefoldername);
                        }
                        b_delelefolderwithnofile = false;
                    }
                }
            }
            else
            {
                if (parentRemoteFolder == false)
                {
                    sftp.DeleteFolder(currentftpfolder);
                }
                b_delelefolderwithnofile = true;
            }
        }
    }

    class Rename : Action
    {
        public override void RunAction(object o, int iRow, string type, string SiteName, string SiteIP, string UserID, string Password, string Port, string Action, string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            string oldfilename = RemoteFileFolder;
            string newfilename = LocalFileFolder;
            if (type == "FTP")
            {
                FTP.FTP ftp = new FTP.FTP(SiteName, SiteIP, UserID, Password, Port);

                oldfilename = "ftp://" + SiteIP + ":" + Port + "/" + oldfilename;
                //newfilename = "ftp://" + SiteIP + ":" + Port + "/" + newfilename;
                ftp.Rename(oldfilename, newfilename);
            }
            else if (type == "SSH")
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID,Password, Port);
                sftp.Rename(oldfilename, newfilename);
            }
            else
            {
                SFTP.SFTP sftp = new SFTP.SFTP(SiteName, SiteIP, UserID, Password, Port);
                sftp.Rename(oldfilename, newfilename);
            }
        }
    }

    static class getFormMethod
    {
        public static MethodInfo getMethod()
        {
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\FTPTool.exe");
            //加载DLL
            System.Type t = ass.GetType("FTPTool.frmMain");//获得类型
            //object o = System.Activator.CreateInstance(t);//创建实例

            System.Reflection.MethodInfo mi = t.GetMethod("SetMessage");//获得方法
            return mi;
        }
    }

    static class matchFile
    {
        public static bool CheckMatchFile(string fileName, string ext)
        {
            try
            {
                ext = "^" + ext.ToUpper().Replace(".", "\\.").Replace("*", @"([\u4e00-\u9fa5]|\w|\.)+") + "$";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(ext);
                if (regex.IsMatch(fileName.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
