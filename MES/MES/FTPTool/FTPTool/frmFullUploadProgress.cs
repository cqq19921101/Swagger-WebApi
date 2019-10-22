using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace FTPTool
{
    public partial class frmFullUploadProgress : Form
    {
        DB DB = new DB();
        FTP.FTP Ftp = null;
        SFTP.SFTP sFtp = null;

        bool callbyPause = false;
        bool clickbychkall = false;
        DataTable dtAllResumeFiles;
        DataTable dtAllFiles;
        DataTable dtChoosedList;
        private string sitename;
        private string ftppathwithoutip;
        private string initiallocalpath;
        private string type, siteip, userid, pwd, renamefile, port;
        private string isCalcel = "Y";
        private struct CurrentUploadInfo
        {
            public string SiteName;
            public string FtpPath;
            public string FileNameWithoutPath;
            public string LocalPath;
            public string Type;
            public int FileSize;
            public string FtpFullFileName;
            public string CurrentUsedFtpFullFileName;
        }

        CurrentUploadInfo currentuploadinfo;

        //定义刷新任务Status状态值的委托
        public delegate void SetScheduleStatus(int i, string Content, int Downloadcount);

        public frmFullUploadProgress(string _sitename, string _ftppathwithoutip, DataTable _dtChoosedList,string _initiallocalpath)
        {
            sitename = _sitename;
            ftppathwithoutip = _ftppathwithoutip;
            dtChoosedList = new DataTable();
            //如果dtChoosedList是null，则说明之前有上传列表没有完成，需要继续上传
            dtChoosedList = _dtChoosedList;
            initiallocalpath = _initiallocalpath;
            InitializeComponent();
        }

        public string IsCancel
        {
            set { isCalcel = value; }
            get { return isCalcel; }
        }

        private void frmFullUploadProgress_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Waiting...";
            lblSite.Text = sitename;
            lblFtpPath.Text = ftppathwithoutip;
            dtAllFiles = GetFileListDateTable();
            btnUpload.Enabled = false;
        }

        private DataTable GetFileListDateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LocalPath");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Size");
            dt.Columns.Add("Type");
            dt.Columns.Add("Modify Date");
            dt.Columns.Add("FileFullPath");
            dt.Columns.Add("IsFolder");
            dt.Columns.Add("FtpFullFilePath");

            return dt;
        }

        private void UpdateDownloadStatus(int iRow, string Content, int Downloadcount)
        {
            dgvList.Rows[iRow].Cells["Status"].Value = Content;
            //dgvList.Rows[iRow].Cells["FinishedSize"].Value = Downloadcount.ToString();
            if (Downloadcount == -1)//下完了
            {
                DB.DeleteUploaditem(currentuploadinfo.SiteName, currentuploadinfo.LocalPath, currentuploadinfo.FileNameWithoutPath);
                dgvList.Rows[iRow].Cells[0].Value = res.ok;
                dgvList.Rows[iRow].Cells["Status"].Value = "<OK>Finished";
            }
        }

        public void SetMessage(int iRow, string Content, int Downloadcount)
        {
            SetScheduleStatus sss = new SetScheduleStatus(this.UpdateDownloadStatus);
            sss.Invoke(iRow, Content, Downloadcount);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            lblFinished.Text = "0";
            if (clickbychkall == false)
            {
                clickbychkall = true;
                return;
            }
            if (chkAll.Checked == true)
            {
                foreach (DataGridViewRow dgvr in dgvList.Rows)
                {
                    dgvr.Cells["Image"].Value = res.wait;
                    dgvr.Cells["Selected"].Value = true;
                }
                lblSelected.Text = dgvList.Rows.Count.ToString();
            }
            else
            {
                foreach (DataGridViewRow dgvr in dgvList.Rows)
                {
                    dgvr.Cells["Image"].Value = res.remove;
                    dgvr.Cells["Selected"].Value = false;
                }
                lblSelected.Text = "0";
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if ((bool)dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == true)
                {
                    dgvList.Rows[e.RowIndex].Cells["Image"].Value = res.remove;
                    dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    lblSelected.Text = ((Convert.ToInt32(lblSelected.Text)) - 1).ToString();
                    clickbychkall = false;
                    chkAll.Checked = false;
                    clickbychkall = true;
                }
                else
                {
                    dgvList.Rows[e.RowIndex].Cells["Image"].Value = res.wait;
                    dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    lblSelected.Text = ((Convert.ToInt32(lblSelected.Text)) + 1).ToString();
                    if ((Convert.ToInt32(lblSelected.Text) == dgvList.Rows.Count))
                    {
                        chkAll.Checked = true;
                    }
                }
                lblFinished.Text = "0";
            }
        }



        private void t_prepare_Tick(object sender, EventArgs e)
        {
            try
            {
                t_prepare.Enabled = false;
                this.Cursor = Cursors.AppStarting;
                
                clickbychkall = true;
                //在读取文件的时候读取空文件夹，因为读取文件列表的时候空文件夹不在文件列表中，会造成空文件夹上传丢失

                lblStatus.Text = "Getting site profile...";
                Application.DoEvents();
                using (DataTable dt = DB.GetSiteProfileDetail(sitename))
                {
                    type = dt.Rows[0]["Type"].ToString();
                    siteip = dt.Rows[0]["SiteIP"].ToString();
                    userid = dt.Rows[0]["UserID"].ToString();
                    pwd = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                    renamefile = dt.Rows[0]["RenameFile"].ToString();
                    port = dt.Rows[0]["Port"].ToString();
                }

                lblStatus.Text = "Creating " + type + " instance...";
                if (type == "FTP")
                {
                    Ftp = new FTP.FTP(sitename, siteip, userid, pwd, port, renamefile, "Override");
                }
                else
                {
                    sFtp = new SFTP.SFTP(sitename, siteip, userid, pwd, port, renamefile, "Override");
                    sFtp.Connect();
                }
                Thread.Sleep(300);
                Application.DoEvents();

                if (dtChoosedList == null)
                {
                    #region Continue upload
                    btnUpload.Text = "Continue upload";
                    btnUpload.Width = 129;
                    btnUpload.Left = 702;
                    btnPause.Left = 640;
                    lblStatus.Text = "Getting breakpoint resume files list...";
                    Thread.Sleep(300);
                    Application.DoEvents();

                    dtAllResumeFiles = DB.GetUploadBreakpointResumeFileList(sitename);
                    if (dtAllResumeFiles.Rows.Count > 0)
                    {
                        lblFtpPath.Text = dtAllResumeFiles.Rows[0]["FtpPath"].ToString();
                    }
                    foreach (DataRow dr in dtAllResumeFiles.Rows)
                    {
                        //取得文件列表后，检索服务器上文件上传载情况。
                        if (dr["Type"].ToString() == "File Folder")
                        {
                            bool bChkResult = true;
                            if (type == "FTP")
                            {
                                bChkResult = Ftp.CheckFileExist(dr["FtpFullFilePath"].ToString(), dr["FileNameWithoutPath"].ToString());
                            }
                            else
                            {
                                bChkResult = sFtp.CheckFileExist(dr["FtpFullFilePath"].ToString(), dr["FileNameWithoutPath"].ToString());
                            }
                            if (bChkResult == false)
                            {
                                DB.SaveUploadlist(sitename, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblFtpPath.Text, dr["Type"].ToString(), dr["FileSize"].ToString(), dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["ModifyDate"].ToString());
                                dgvList.Rows.Add(res.wait, false, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<!>Can't find the folder, need recreat",
                                    dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                            }
                        }
                        else
                        {
                            string filename = null;
                            if (renamefile == "Y")
                            {
                                filename = dr["FtpFullFilePath"].ToString() + ".tmp";
                            }
                            else
                            {
                                filename = dr["FtpFullFilePath"].ToString();
                            }

                            //查询FTP上的文件大小和现在的上次下载的是否一致，一致则继续下载，不一致则重新下载
                            

                                if (File.Exists(dr["LocalFullFilePath"].ToString())==false)
                                {
                                    dgvList.Rows.Add(res.error, false, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<ERROR>Can't find the source file, can't download",
                                        dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                                }
                                else
                                {
                                    FileInfo currlocalfileinfo = new FileInfo(dr["LocalFullFilePath"].ToString());

                                    if (currlocalfileinfo.Length.ToString() != dr["FileSize"].ToString()
                                         || currlocalfileinfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") != dr["ModifyDate"].ToString())
                                    {
                                        DB.SaveUploadlist(sitename, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblFtpPath.Text, dr["Type"].ToString(), currlocalfileinfo.Length.ToString(), dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), currlocalfileinfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        dgvList.Rows.Add(res.wait, false, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), currlocalfileinfo.Length.ToString(), "<!>The source file has been changed, need reupload",
                                            dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["Type"].ToString(), currlocalfileinfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    else
                                    {
                                        DataTable dtFtpFileInfo = (type == "FTP") ? Ftp.GetFileList(filename) : sFtp.GetFileList(filename);
                                        if (dtFtpFileInfo!=null && dtFtpFileInfo.Rows.Count>0)
                                        {
                                            //fi的长度就是offset
                                            dgvList.Rows.Add(res.wait, false, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(),
                                                "<!>Upload finished " + Convert.ToInt32((Convert.ToDouble(dtFtpFileInfo.Rows[0]["Size"].ToString()) / Convert.ToDouble(currlocalfileinfo.Length)) * 100).ToString() + "%",
                                                dr["LocalFullFilePath"].ToString(),dr["FtpFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                                        }
                                        else
                                        {
                                            DB.SaveUploadlist(sitename, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblFtpPath.Text, dr["Type"].ToString(), dr["FileSize"].ToString(), dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["ModifyDate"].ToString());
                                            dgvList.Rows.Add(res.wait, false, dr["LocalPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<!>Can't find the last upload file, need reupload",
                                                 dr["LocalFullFilePath"].ToString(), dr["FtpFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                                        }
                                    }
                                }
                            }
                        
                    }
                    #endregion
                }
                else
                {
                    lblStatus.Text = "Getting files list for upload, please wait...";
                    Thread.Sleep(300);
                    Application.DoEvents();

                    #region read files and folders
                    foreach (DataRow dr in dtChoosedList.Rows)
                    {
                        string localpath = dr["LocalPath"].ToString();
                        if (localpath.EndsWith("\\"))
                        {
                            localpath = localpath.Substring(0, localpath.Length - 1);
                        }
                        if (dr["IsFolder"].ToString() == "Y")
                        {
                            string ipath = localpath.Substring(0, localpath.LastIndexOf("\\"));
                            //try
                            //{
                            //    ftp.MakeFolder(LocalFileFolder, RemoteFileFolder, ipath);//如果源是文件夹，则连源文件夹一起上传
                            //}
                            //catch { }
                            DoWhileLocalIsFolder(ftppathwithoutip, localpath, initiallocalpath);
                        }
                        else
                        {
                                //只上传指定文件
                                //ftp.Upload(o, iRow, RemoteFileFolder, LocalFileFolder, truelocalfilefolder);

                                //是文件的，要把文件名从路径中去掉，显示在界面上
                                DataRow drFile = dtAllFiles.NewRow();
                                if (localpath.LastIndexOf("\\") < 0)
                                {
                                    drFile["LocalPath"] = "";
                                    drFile["File Name"] = localpath;
                                }
                                else
                                {
                                    drFile["LocalPath"] = localpath.Substring(0, localpath.LastIndexOf("\\"));
                                    drFile["File Name"] = localpath.Substring(localpath.LastIndexOf("\\") + 1, localpath.Length - localpath.LastIndexOf("\\") - 1);
                                }
                                drFile["Size"] = dr["Size"].ToString();
                                drFile["Type"] = dr["Type"].ToString();
                                drFile["Modify Date"] = dr["Modify Date"].ToString();
                                drFile["FileFullPath"] = dr["FileFullPath"].ToString();
                                drFile["IsFolder"] = "N";
                                drFile["FtpFullFilePath"] = GetFtpFullPath(new FileInfo(dr["FileFullPath"].ToString()), null, initiallocalpath, ftppathwithoutip);//文件夹第一层这个路径就是文件路径了，不用再加文件名
                                dtAllFiles.Rows.Add(drFile);
                        }
                    }
                    #endregion

                    foreach (DataRow drallfile in dtAllFiles.Rows)
                    {
                        DataGridViewCheckBoxColumn dgvcc = new DataGridViewCheckBoxColumn();
                        dgvcc.HeaderText = "";
                        dgvcc.Resizable = DataGridViewTriState.False;

                        dgvList.Rows.Add(res.wait, false, drallfile["LocalPath"].ToString(), drallfile["File Name"].ToString(), drallfile["Size"].ToString(), "",
                            drallfile["FileFullPath"].ToString(), drallfile["FtpFullFilePath"].ToString(), drallfile["Type"].ToString(), drallfile["Modify Date"].ToString());
                    }
                }

                dgvList.ClearSelection();
                chkAll.Checked = true;
                lblTotal.Text = dgvList.Rows.Count.ToString();
                lblStatus.Text = "Get files list finished";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Download", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpload.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void DoWhileLocalIsFolder(string FtpPathWithoutIP, string currentfolder, string initialpath)
        {

            DirectoryInfo di = new DirectoryInfo(currentfolder);
            DataRow drFile = dtAllFiles.NewRow();
            drFile["LocalPath"] = currentfolder;
            drFile["File Name"] = di.Name.ToString();
            drFile["Size"] = "0";
            drFile["Type"] = "File Folder";
            drFile["Modify Date"] = di.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
            drFile["FileFullPath"] = di.FullName.ToString();
            drFile["IsFolder"] = "Y";
            drFile["FtpFullFilePath"] = GetFtpFullPath(null, di, initiallocalpath, ftppathwithoutip);
            dtAllFiles.Rows.Add(drFile);

            foreach (FileInfo fi in di.GetFiles())
            {
                DataRow drFile2 = dtAllFiles.NewRow();
                drFile2["LocalPath"] = currentfolder;
                drFile2["File Name"] = fi.Name.ToString();
                drFile2["Size"] = fi.Length.ToString();
                drFile2["Type"] = fi.Extension.ToString();
                drFile2["Modify Date"] = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                drFile2["FileFullPath"] = fi.FullName.ToString();
                drFile2["IsFolder"] = "N";
                drFile2["FtpFullFilePath"] = GetFtpFullPath(fi, null, initiallocalpath, ftppathwithoutip);
                dtAllFiles.Rows.Add(drFile2);
            }

            DirectoryInfo[] dis = di.GetDirectories();

            foreach (DirectoryInfo dii in dis)
            {
                //try
                //{
                //    ftp.MakeFolder(dii.FullName, FtpPathWithoutIP, initialpath);
                //}
                //catch { }
                DoWhileLocalIsFolder(FtpPathWithoutIP, dii.FullName.ToString(), initialpath);
            }

        }

        private string GetFtpFullPath(FileInfo fileInf,DirectoryInfo diInf,string initialpathlocal,string FtpPathWithoutip)
        {
            if (FtpPathWithoutip.StartsWith("/") == false)
            {
                FtpPathWithoutip = "/" + FtpPathWithoutip;
            }
            string uri = "";
            string filename = "";
            if (fileInf != null)
            {
                filename = fileInf.FullName.Replace(initialpathlocal, "").Replace(@"\", "/");
            }
            if (diInf != null)
            {
                filename = diInf.FullName.Replace(initialpathlocal, "").Replace(@"\", "/");
            }
            if (filename.StartsWith("/"))
            {
                filename = filename.Substring(1, filename.Length - 1);
            }
            if (type == "FTP")
            {
                if (FtpPathWithoutip == "/" || FtpPathWithoutip == "")
                {
                    uri = "ftp://" + siteip + ":" + port + "/" + filename;
                }
                else
                {
                    uri = "ftp://" + siteip + ":" + port + FtpPathWithoutip + filename;
                }
            }
            else
            {
                if (FtpPathWithoutip == "/" || FtpPathWithoutip == "")
                {
                    uri = filename;
                }
                else
                {
                    uri = FtpPathWithoutip + filename;
                }
            }
            return uri;
        }

        private void ResetStructCurrentUpload()
        {
            currentuploadinfo.SiteName = "";
            currentuploadinfo.FtpPath = "";
            currentuploadinfo.FileNameWithoutPath = "";
            currentuploadinfo.LocalPath = "";
            currentuploadinfo.Type = "";
            currentuploadinfo.FileSize = 0;
            currentuploadinfo.FtpFullFileName = "";
            currentuploadinfo.CurrentUsedFtpFullFileName = "";
        }

        private void MakeFolder(string FtpFullFolderPath)
        {
            //if (FtpFullFolderPath.EndsWith("/") == false)
            //{
            //    FtpFullFolderPath = FtpFullFolderPath + "/";
            //}
            if (type == "FTP")
            {
                FtpFullFolderPath = FtpFullFolderPath.Replace("ftp://" + siteip + ":" + port + "/", "");
                string[] folders = FtpFullFolderPath.Split('/');
                string needfolder = "";
                foreach (string folder in folders)
                {
                    needfolder = needfolder + folder + "/";
                    //needfolder = needfolder.Substring(0, needfolder.Length - 1);
                    try
                    {
                        Ftp.MakeFolder(needfolder);
                    }
                    catch { }
                }
            }
            else
            {
                FtpFullFolderPath = FtpFullFolderPath.Replace("ftp://" + siteip + ":" + port + "/", "");
                string[] folders = FtpFullFolderPath.Split('/');
                string needfolder = "";
                foreach (string folder in folders)
                {
                    needfolder = needfolder + folder + "/";
                    //needfolder = needfolder.Substring(0, needfolder.Length - 1);
                    try
                    {
                        sFtp.MakeFolder(needfolder);
                    }
                    catch { }
                }
            }
        }

        private void SetSelectCellStatus(bool Isreadonly)
        {
            foreach (DataGridViewRow dgvr in dgvList.Rows)
            {
                dgvr.Cells["Selected"].ReadOnly = Isreadonly;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            btnUpload.Enabled = false;
            btnPause.Enabled = true;
            chkAll.Enabled = false;
            btnAbort.Enabled = false;
            try
            {
                //SetSelectCellStatus(true);
                dgvList.Enabled = false;
                if (btnUpload.Text.ToUpper().EndsWith("UPLOAD"))
                {
                    lblStatus.Text = "Saving the upload list, please wait...";
                    Application.DoEvents();
                    try
                    {
                        if (callbyPause == false)
                        {
                            DB.DeleteAllUnfinishedUploadItem(sitename);
                            foreach (DataGridViewRow dgvr in dgvList.Rows)
                            {
                                if ((bool)dgvr.Cells[1].Value == true)
                                {
                                    if (dgvr.Cells["Status"].Value.ToString() == "" || dgvr.Cells["Status"].Value.ToString().StartsWith("<!>"))
                                    {
                                        DB.SaveUploadlist(sitename, dgvr.Cells["LP"].Value.ToString(), dgvr.Cells["FileName"].Value.ToString(),
                                            lblFtpPath.Text, dgvr.Cells["Type"].Value.ToString(), dgvr.Cells["Size"].Value.ToString(),
                                            dgvr.Cells["FileFullName"].Value.ToString(), dgvr.Cells["FtpFullFileName"].Value.ToString(), dgvr.Cells["ModifyDate"].Value.ToString());
                                    }
                                }
                                //dgvr.Cells["Status"].Value = "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    lblStatus.Text = "Start uploading...";
                    if (callbyPause == false)
                    {
                        lblFinished.Text = "0";
                    }
                    else
                    {
                        callbyPause = false;
                    }

                    //向上滚动DGV
                    int idgvX = 0, idgvY = 0, iCellX = 0, iCellY = 0;
                    idgvX = dgvList.Location.X;
                    idgvY = dgvList.Location.Y;
                    bool b_Move = false;
                    int iRowCount = -1;
                    dgvList.FirstDisplayedScrollingRowIndex = 0;

                    for (int i = 0; i < dgvList.Rows.Count; i++)
                    {
                        if (btnPause.Text == "Start")
                        {
                            break;
                        }
                        #region //向上滚动
                        if (b_Move)
                        {
                            dgvList.FirstDisplayedScrollingRowIndex = dgvList.Rows[i].Index - iRowCount;
                        }
                        iCellX = dgvList.GetCellDisplayRectangle(0, dgvList.Rows[i].Index, false).X;
                        iCellY = dgvList.GetCellDisplayRectangle(0, dgvList.Rows[i].Index, false).Y;

                        //如果得到的单元格的Y坐标=0了，说明到底了，要开始往上移动
                        //如果得到的单元格的Y坐标加上单元格高度比控件高度高了，要开始往上移动
                        //一旦开始移动，则每个循环都要移动了
                        //2倍的单元格高度，因为到底后可能出现横向滚动条，多以提早开始移动
                        if (iCellY == 0 || idgvY + iCellY + dgvList.Rows[i].Cells[0].Size.Height + dgvList.Rows[i].Cells[0].Size.Height > idgvY + dgvList.Height)
                        {
                            dgvList.FirstDisplayedScrollingRowIndex = dgvList.Rows[i].Index - iRowCount;
                            iCellY = dgvList.GetCellDisplayRectangle(0, dgvList.Rows[i].Index, false).Y;
                            b_Move = true;
                        }
                        else
                        {
                            if (b_Move == false) { iRowCount++; }
                        }
                        #endregion

                        if ((bool)dgvList.Rows[i].Cells[1].Value == true
                            &&
                            (dgvList.Rows[i].Cells["Status"].Value.ToString().StartsWith("<OK>")==false && dgvList.Rows[i].Cells["Status"].Value.ToString().StartsWith("<ERROR>")==false))
                        {
                            try
                            {
                                ResetStructCurrentUpload();
                                currentuploadinfo.SiteName = sitename;
                                currentuploadinfo.LocalPath = dgvList.Rows[i].Cells["LP"].Value.ToString();
                                currentuploadinfo.FileNameWithoutPath = dgvList.Rows[i].Cells["FileName"].Value.ToString();
                                currentuploadinfo.FtpPath = lblFtpPath.Text;
                                currentuploadinfo.Type = dgvList.Rows[i].Cells["Type"].Value.ToString();
                                currentuploadinfo.FileSize = Convert.ToInt32(dgvList.Rows[i].Cells["Size"].Value);
                                currentuploadinfo.FtpFullFileName = dgvList.Rows[i].Cells["FtpFullFileName"].Value.ToString();

                                if (currentuploadinfo.Type == "File Folder")
                                {

                                    MakeFolder(currentuploadinfo.FtpFullFileName);
         
                                    DB.DeleteUploaditem(currentuploadinfo.SiteName, currentuploadinfo.LocalPath, currentuploadinfo.FileNameWithoutPath);
                                    dgvList.Rows[i].Cells[0].Value = res.ok;
                                    dgvList.Rows[i].Cells["Status"].Value = "<OK>Finished";
                                }
                                else
                                {
                                    if (renamefile == "Y")
                                    {
                                        currentuploadinfo.CurrentUsedFtpFullFileName = dgvList.Rows[i].Cells["FtpFullFileName"].Value.ToString() + ".tmp";
                                    }
                                    else
                                    {
                                        currentuploadinfo.CurrentUsedFtpFullFileName = dgvList.Rows[i].Cells["FtpFullFileName"].Value.ToString();
                                    }
                                    dgvList.Rows[i].Cells[0].Value = res.upload;

                                    int finishedlength = 0;
                                    if (dgvList.Rows[i].Cells["Status"].Value.ToString() != "")
                                    {
                                        if (type == "FTP")
                                        {
                                            using (DataTable dtOnefile = Ftp.GetFileList(currentuploadinfo.CurrentUsedFtpFullFileName))
                                            {
                                                if (dtOnefile != null && dtOnefile.Rows.Count > 0)
                                                {
                                                    finishedlength = Convert.ToInt32((dtOnefile.Rows[0]["Size"]).ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (DataTable dtOnefile = sFtp.GetFileList(currentuploadinfo.CurrentUsedFtpFullFileName))
                                            {
                                                if (dtOnefile != null && dtOnefile.Rows.Count > 0)
                                                {
                                                    finishedlength = Convert.ToInt32((dtOnefile.Rows[0]["Size"]).ToString());
                                                }
                                            }
                                        }
                                    }

                                    if (currentuploadinfo.FtpFullFileName.Length > currentuploadinfo.FileNameWithoutPath.Length)
                                    {
                                        string ftpfilepath = currentuploadinfo.FtpFullFileName.Substring(0, currentuploadinfo.FtpFullFileName.Length - currentuploadinfo.FileNameWithoutPath.Length - 1);

                                        MakeFolder(ftpfilepath);
                                    }
                                    if (type == "FTP")
                                    {
                                        Ftp.Upload(this, i, ftppathwithoutip,
                                            dgvList.Rows[i].Cells["FileFullName"].Value.ToString(),
                                            initiallocalpath, finishedlength);

                                    }
                                    else
                                    {
                                        sFtp.Upload(this, i, ftppathwithoutip,
                                            dgvList.Rows[i].Cells["FileFullName"].Value.ToString(),
                                            initiallocalpath, finishedlength);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                dgvList.Rows[i].Cells[0].Value = res.error;
                                dgvList.Rows[i].Cells["Status"].Value = "<ERROR>" + ex.Message;
                                DB.DeleteUploaditem(currentuploadinfo.SiteName, currentuploadinfo.LocalPath, currentuploadinfo.FileNameWithoutPath);
                            }
                            finally
                            {
                                lblFinished.Text = (Convert.ToInt32(lblFinished.Text) + 1).ToString();
                                Application.DoEvents();
                            }
                        }
                    }
                }
            }
            finally
            {
                if (btnPause.Text != "Start")
                {
                    //SetSelectCellStatus(false);
                    dgvList.Enabled = true;
                    btnPause.Enabled = false;
                    btnUpload.Enabled = true;
                    chkAll.Enabled = true;
                    btnAbort.Enabled = true;
                    MessageBox.Show("Upload finished", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                if (type == "FTP")
                {
                    Ftp.UploadDownloadPause = "Y";
                }
                else
                {
                    sFtp.UploadDownloadPause = "Y";
                }
                btnPause.Text = "Start";
            }
            else
            {
                if (type == "FTP")
                {
                    Ftp.UploadDownloadPause = "N";
                }
                else
                {
                    sFtp.UploadDownloadPause = "N";
                }
                btnPause.Text = "Pause";
                callbyPause = true;
                btnUpload_Click(null, null);
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {

            if (DialogResult.No == MessageBox.Show("Do you really want to abort selected upload items?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.AppStarting;
                for (int i = dgvList.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)dgvList.Rows[i].Cells[1].Value == true)
                    {
                        DB.DeleteUploaditem(lblSite.Text, dgvList.Rows[i].Cells["LP"].Value.ToString(), dgvList.Rows[i].Cells["FileName"].Value.ToString());
                        dgvList.Rows.RemoveAt(i);
                    }
                }

                lblSelected.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

    }
}
