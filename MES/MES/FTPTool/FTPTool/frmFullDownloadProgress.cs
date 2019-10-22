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
    public partial class frmFullDownloadProgress : Form
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
        private string localpath;
        private string type, siteip, userid, pwd, renamefile, port;
        private string isCalcel = "Y";
        private struct CurrentDownloadInfo
        {
            public string SiteName;
            public string FtpPath;
            public string FileNameWithoutPath;
            public string LocalPath;
            public string Type;
            public int FileSize;
            public string LocalFullFileName;
            public string CurrentUsedLocalFullFileName;
        }

        CurrentDownloadInfo currentdownloadinfo;

        //定义刷新任务Status状态值的委托
        public delegate void SetScheduleStatus(int i, string Content, int Downloadcount);

        public frmFullDownloadProgress(string _sitename,string _localpath,DataTable _dtChoosedList)
        {
            sitename = _sitename;
            localpath = _localpath;
            dtChoosedList = new DataTable();
            //如果dtChoosedList是null，则说明之前有下载列表没有完成，需要继续下载
            dtChoosedList = _dtChoosedList;
            InitializeComponent();
        }

        private void frmFullDownloadProgress_Load(object sender, EventArgs e)
        {
            lblStatus.Text="Waiting...";
            lblSite.Text = sitename;
            lblLocalPath.Text = localpath;
            dtAllFiles = GetFileListDateTable();
            btnDownload.Enabled = false;
        }

        public string IsCancel
        {
            set { isCalcel = value; }
            get { return isCalcel; }
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
                    //if (type == "SFTP")
                    //{
                    //    btnPause.Text = "Exit";
                    //}
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
                    #region Continue download
                    btnDownload.Text = "Continue download";
                    btnDownload.Width = 129;
                    btnDownload.Left = 702;
                    btnPause.Left = 640;
                    lblStatus.Text = "Getting breakpoint resume files list...";
                    Thread.Sleep(300);
                    Application.DoEvents();

                    dtAllResumeFiles = DB.GetDownloadBreakpointResumeFileList(sitename);
                    if(dtAllResumeFiles.Rows.Count>0)
                    {
                        lblLocalPath.Text=dtAllResumeFiles.Rows[0]["LocalPath"].ToString();
                    }
                    foreach (DataRow dr in dtAllResumeFiles.Rows)
                    {
                        //取得文件列表后，检索本地文件下载情况。
                        if (dr["Type"].ToString() == "File Folder")
                        {
                            if (Directory.Exists(dr["LocalFullFilePath"].ToString()) == false)
                            {
                                DB.SaveDownloadlist(sitename, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblLocalPath.Text, dr["Type"].ToString(), dr["FileSize"].ToString(), dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["ModifyDate"].ToString());
                                dgvList.Rows.Add(res.wait, false, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<!>Can't find the folder, need recreat",
                                    dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["Type"].ToString(), dr["Modify Date"].ToString());
                            }
                        }
                        else
                        {
                            string filename = null;
                            if (renamefile == "Y")
                            {
                                filename = dr["LocalFullFilePath"].ToString() + ".tmp";
                            }
                            else
                            {
                                filename = dr["LocalFullFilePath"].ToString();
                            }

                            //查询FTP上的文件大小和现在的上次下载的是否一致，一致则继续下载，不一致则重新下载           
                            using (DataTable dtCheckFtpFileStatus = (type=="FTP")?Ftp.GetFileList(dr["FtpFullFilePath"].ToString()):sFtp.GetFileList(dr["FtpFullFilePath"].ToString()))
                            {
                                if (dtCheckFtpFileStatus == null || dtCheckFtpFileStatus.Rows.Count == 0)
                                {
                                    dgvList.Rows.Add(res.error, false, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<ERROR>Can't find the source file, can't download",
                                        dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["Type"].ToString(),dr["Modify Date"].ToString());
                                }
                                else
                                {
                                    if (dtCheckFtpFileStatus.Rows[0]["Size"].ToString() != dr["FileSize"].ToString()
                                         || dtCheckFtpFileStatus.Rows[0]["Modify Date"].ToString() != dr["ModifyDate"].ToString())
                                    {
                                        DB.SaveDownloadlist(sitename, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblLocalPath.Text, dr["Type"].ToString(), dtCheckFtpFileStatus.Rows[0]["Size"].ToString(), dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dtCheckFtpFileStatus.Rows[0]["Modify Date"].ToString());
                                        dgvList.Rows.Add(res.wait, false, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dtCheckFtpFileStatus.Rows[0]["Size"].ToString(), "<!>The source file has been changed, need redownload",
                                            dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["Type"].ToString(), dtCheckFtpFileStatus.Rows[0]["Modify Date"].ToString());
                                    }
                                    else
                                    {
                                        if (File.Exists(filename))
                                        {
                                            FileInfo fi = new FileInfo(filename);
                                            //fi的长度就是offset
                                            dgvList.Rows.Add(res.wait, false, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(),
                                                "<!>Download finished " + Convert.ToInt32((Convert.ToDouble(fi.Length) / Convert.ToDouble(dr["FileSize"].ToString())) * 100).ToString() + "%",
                                                dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                                        }
                                        else
                                        {
                                            DB.SaveDownloadlist(sitename, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), lblLocalPath.Text, dr["Type"].ToString(), dr["FileSize"].ToString(), dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["ModifyDate"].ToString());
                                            dgvList.Rows.Add(res.wait, false, dr["FtpPath"].ToString(), dr["FileNameWithoutPath"].ToString(), dr["FileSize"].ToString(), "<!>Can't find the last download file, need redownload",
                                                dr["FtpFullFilePath"].ToString(), dr["LocalFullFilePath"].ToString(), dr["Type"].ToString(), dr["ModifyDate"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    lblStatus.Text = "Getting files list for download, please wait...";
                    Thread.Sleep(300);
                    Application.DoEvents();

                    #region read files and folders
                    foreach (DataRow dr in dtChoosedList.Rows)
                    {
                        string ftppath = dr["FtpPath"].ToString();
                        ftppath = ftppath.Substring(sitename.Length, ftppath.Length - sitename.Length);
                        while (ftppath.StartsWith("/"))
                        {
                            ftppath = ftppath.Substring(1, ftppath.Length - 1);
                        }
                        int ilast;
                        string tmpremotefolder;
                        if (ftppath.EndsWith("/"))
                        {
                            tmpremotefolder = ftppath.Substring(0, ftppath.Length - 1);
                        }
                        else
                        {
                            tmpremotefolder = ftppath;
                        }
                        //本地需要建立下载目录同名的目录，就是将选中的FTP文件夹在本地建立，而不只是下载FTP文件夹里面的内容
                        ilast = tmpremotefolder.LastIndexOf("/");
                        string ipath = ftppath.Substring(ilast + 1, tmpremotefolder.Length - ilast - 1);
                        string localfolder = lblLocalPath.Text + "\\" + ipath;

                        if (dr["IsFolder"].ToString() == "Y")
                        {
                            DoWhileRemoteIsFolder(ftppath, localfolder);
                        }
                        else
                        {

                            //是文件的，要把文件名从路径中去掉，显示在界面上
                            DataRow drFile = dtAllFiles.NewRow();
                            if (ftppath.LastIndexOf("/") < 0)
                            {
                                drFile["FtpPath"] = "/";
                                drFile["File Name"] = ftppath;
                            }
                            else
                            {
                                drFile["FtpPath"] = ftppath.Substring(0, ftppath.LastIndexOf("/"));
                                drFile["File Name"] = ftppath.Substring(ftppath.LastIndexOf("/") + 1, ftppath.Length - ftppath.LastIndexOf("/") - 1);
                            }
                            drFile["Size"] = dr["Size"].ToString();
                            drFile["Type"] = dr["Type"].ToString();
                            drFile["Modify Date"] = dr["Modify Date"].ToString();
                            drFile["FileFullPath"] = dr["FileFullPath"].ToString();
                            drFile["IsFolder"] = "N";
                            drFile["LocalFullFilePath"] = localfolder;//文件夹第一层这个路径就是文件路径了，不用再加文件名
                            dtAllFiles.Rows.Add(drFile);
                        }
                    }
                    #endregion

                    foreach (DataRow drallfile in dtAllFiles.Rows)
                    {
                        DataGridViewCheckBoxColumn dgvcc = new DataGridViewCheckBoxColumn();
                        dgvcc.HeaderText = "";
                        dgvcc.Resizable = DataGridViewTriState.False;

                        dgvList.Rows.Add(res.wait, false, drallfile["FtpPath"].ToString(), drallfile["File Name"].ToString(), drallfile["Size"].ToString(), "",
                            drallfile["FileFullPath"].ToString(), drallfile["LocalFullFilePath"].ToString(), drallfile["Type"].ToString(), drallfile["Modify Date"].ToString());
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
                btnDownload.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void DoWhileRemoteIsFolder(string currftppath,string localpath)
        {
            if (localpath.EndsWith("\\"))
            {
                localpath = localpath.Substring(0, localpath.Length - 1);
            }
            //获取文件列表 
            if (currftppath == "/")
            {
                currftppath = "";
            }
            if (currftppath.EndsWith("/"))
            {
                currftppath = currftppath.Substring(0, currftppath.Length - 1);
            }

            DataTable dtfilelist = (type == "FTP") ? Ftp.GetFileList(currftppath) : sFtp.GetFileList(currftppath);
            if (dtfilelist != null)
            {
                //对al分析，是文件就下载，不是文件就访问里层目录
                foreach (DataRow dr in dtfilelist.Rows)
                {

                    string filetype = (dr["IsFolder"].ToString() == "Y") ? "Folder" : "File";

                    if (filetype == "File")
                    {
                        DataRow drFile = dtAllFiles.NewRow();
                        drFile["FtpPath"] = currftppath;
                        drFile["File Name"] = dr["File Name"].ToString();
                        drFile["Size"] = dr["Size"].ToString();
                        drFile["Type"] = dr["Type"].ToString();
                        drFile["Modify Date"] = dr["Modify Date"].ToString();
                        drFile["FileFullPath"] = dr["FileFullPath"].ToString();
                        drFile["IsFolder"] = "N";
                        drFile["LocalFullFilePath"] = localpath + "\\" + dr["File Name"].ToString();
                        dtAllFiles.Rows.Add(drFile);
                    }
                    else
                    {
                        string templocalfolder = localpath + "\\" + dr["File Name"].ToString();

                        DoWhileRemoteIsFolder(currftppath + "/" + dr["File Name"].ToString(),templocalfolder);
                    }
                }
            }
            else
            {
                //如果是空文件夹，则将文件夹纪录下来，为了保持下载上传的层次结构，空文件夹也是需要下载上传的
                DataRow drFile = dtAllFiles.NewRow();
                drFile["FtpPath"] = currftppath;
                drFile["File Name"] = currftppath;
                drFile["Size"] = "0";
                drFile["Type"] = "File Folder";
                drFile["Modify Date"] = "";
                drFile["FileFullPath"] = "";
                drFile["IsFolder"] = "Y";
                drFile["LocalFullFilePath"] = localpath;
                dtAllFiles.Rows.Add(drFile);
            }
        }

        private DataTable GetFileListDateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FtpPath");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Size");
            dt.Columns.Add("Type");
            dt.Columns.Add("Modify Date");
            dt.Columns.Add("FileFullPath");
            dt.Columns.Add("IsFolder");
            dt.Columns.Add("LocalFullFilePath");

            return dt;
        }

        private void UpdateDownloadStatus(int iRow, string Content, int Downloadcount)
        {
            dgvList.Rows[iRow].Cells["Status"].Value = Content;
            //dgvList.Rows[iRow].Cells["FinishedSize"].Value = Downloadcount.ToString();
            if (Downloadcount == -1)//下完了
            {
                DB.Deletedownloaditem(currentdownloadinfo.SiteName, currentdownloadinfo.FtpPath, currentdownloadinfo.FileNameWithoutPath);
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
                    chkAll.Checked=false;
                    clickbychkall = true;
                }
                else
                {
                    dgvList.Rows[e.RowIndex].Cells["Image"].Value = res.wait;
                    dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    lblSelected.Text = ((Convert.ToInt32(lblSelected.Text)) + 1).ToString();
                    if((Convert.ToInt32(lblSelected.Text)==dgvList.Rows.Count))
                    {
                        chkAll.Checked=true;
                    }
                }
                lblFinished.Text = "0";
            }

        }

        private void ResetStructCurrentDownload()
        {
            currentdownloadinfo.SiteName = "";
            currentdownloadinfo.FtpPath = "";
            currentdownloadinfo.FileNameWithoutPath = "";
            currentdownloadinfo.LocalPath = "";
            currentdownloadinfo.Type = "";
            currentdownloadinfo.FileSize = 0;
            currentdownloadinfo.LocalFullFileName = "";
            currentdownloadinfo.CurrentUsedLocalFullFileName = "";
        }

        private void SetSelectCellStatus(bool Isreadonly)
        {
            foreach (DataGridViewRow dgvr in dgvList.Rows)
            {
                dgvr.Cells["Selected"].ReadOnly = Isreadonly;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            btnPause.Enabled = true;
            chkAll.Enabled = false;
            btnAbort.Enabled = false;

            try
            {
                //SetSelectCellStatus(true);
                dgvList.Enabled = false;
                if (btnDownload.Text.ToUpper().EndsWith("DOWNLOAD"))
                {
                    lblStatus.Text = "Saving the download list, please wait...";
                    Application.DoEvents();
                    try
                    {
                        if (callbyPause == false)
                        {
                            DB.DeleteAllUnfinishedDownloadItem(sitename);
                            foreach (DataGridViewRow dgvr in dgvList.Rows)
                            {
                                if ((bool)dgvr.Cells[1].Value == true)
                                {
                                    if (dgvr.Cells["Status"].Value.ToString() == "" || dgvr.Cells["Status"].Value.ToString().StartsWith("<!>"))
                                    {
                                        DB.SaveDownloadlist(sitename, dgvr.Cells["FtpPath"].Value.ToString(), dgvr.Cells["FileName"].Value.ToString(),
                                            lblLocalPath.Text, dgvr.Cells["Type"].Value.ToString(), dgvr.Cells["Size"].Value.ToString(),
                                            dgvr.Cells["FileFullName"].Value.ToString(), dgvr.Cells["LocalFullFileName"].Value.ToString(), dgvr.Cells["ModifyDate"].Value.ToString());
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

                    lblStatus.Text = "Start downloading...";
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
                            (dgvList.Rows[i].Cells["Status"].Value.ToString().StartsWith("<OK>") == false && dgvList.Rows[i].Cells["Status"].Value.ToString().StartsWith("<ERROR>") == false))//download
                        {
                            try
                            {
                                ResetStructCurrentDownload();
                                currentdownloadinfo.SiteName = sitename;
                                currentdownloadinfo.FtpPath = dgvList.Rows[i].Cells["FtpPath"].Value.ToString();
                                currentdownloadinfo.FileNameWithoutPath = dgvList.Rows[i].Cells["FileName"].Value.ToString();
                                currentdownloadinfo.LocalPath = lblLocalPath.Text;
                                currentdownloadinfo.Type = dgvList.Rows[i].Cells["Type"].Value.ToString();
                                currentdownloadinfo.FileSize = Convert.ToInt32(dgvList.Rows[i].Cells["Size"].Value);
                                currentdownloadinfo.LocalFullFileName = dgvList.Rows[i].Cells["LocalFullFileName"].Value.ToString();

                                if (currentdownloadinfo.Type == "File Folder")
                                {
                                    if (Directory.Exists(currentdownloadinfo.LocalFullFileName) == false)
                                    {
                                        Directory.CreateDirectory(currentdownloadinfo.LocalFullFileName);
                                    }
                                    DB.Deletedownloaditem(currentdownloadinfo.SiteName, currentdownloadinfo.FtpPath, currentdownloadinfo.FileNameWithoutPath);
                                    dgvList.Rows[i].Cells[0].Value = res.ok;
                                    dgvList.Rows[i].Cells["Status"].Value = "<OK>Finished";
                                }
                                else
                                {
                                    if (renamefile == "Y")
                                    {
                                        currentdownloadinfo.CurrentUsedLocalFullFileName = dgvList.Rows[i].Cells["LocalFullFileName"].Value.ToString() + ".tmp";
                                    }
                                    else
                                    {
                                        currentdownloadinfo.CurrentUsedLocalFullFileName = dgvList.Rows[i].Cells["LocalFullFileName"].Value.ToString();
                                    }
                                    dgvList.Rows[i].Cells[0].Value = res.download;

                                    int finishedlength = 0;
                                    if (dgvList.Rows[i].Cells["Status"].Value.ToString() != "")
                                    {
                                        if (File.Exists(currentdownloadinfo.CurrentUsedLocalFullFileName))
                                        {
                                            finishedlength = (int)(new FileInfo(currentdownloadinfo.CurrentUsedLocalFullFileName)).Length;
                                        }
                                    }
                                    string localfilepath = currentdownloadinfo.LocalFullFileName.Substring(0, currentdownloadinfo.LocalFullFileName.Length - currentdownloadinfo.FileNameWithoutPath.Length - 1);
                                    if (Directory.Exists(localfilepath) == false)
                                    {
                                        Directory.CreateDirectory(localfilepath);
                                    }
                                    if (type == "FTP")
                                    {
                                        Ftp.Download(this, i, localfilepath,
                                            dgvList.Rows[i].Cells["FileName"].Value.ToString(),
                                            dgvList.Rows[i].Cells["FileFullName"].Value.ToString(), finishedlength);
                                    }
                                    else
                                    {
                                        sFtp.Download(this, i, localfilepath,
                                            dgvList.Rows[i].Cells["FileName"].Value.ToString(),
                                            dgvList.Rows[i].Cells["FileFullName"].Value.ToString(), finishedlength);
                                        sFtp = null;
                                        sFtp = new SFTP.SFTP(sitename, siteip, userid, pwd, port, renamefile, "Override");
                                        sFtp.Connect();
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    dgvList.Rows[i].Cells[0].Value = res.error;
                                    dgvList.Rows[i].Cells["Status"].Value = "<ERROR>" + ex.Message;
                                    DB.Deletedownloaditem(currentdownloadinfo.SiteName, currentdownloadinfo.FtpPath, currentdownloadinfo.FileNameWithoutPath);
                                }
                                catch { }
                            }
                            finally
                            {
                                try
                                {
                                    lblFinished.Text = (Convert.ToInt32(lblFinished.Text) + 1).ToString();
                                }
                                catch { }
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
                    btnDownload.Enabled = true;
                    chkAll.Enabled = true;
                    btnAbort.Enabled = true;
                    MessageBox.Show("Download finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            //if (btnPause.Text == "Exit")
            //{
            //    sFtp.StopDownload();
            //    //sFtp.Disconnect();
            //    //sFtp = null;
            //    this.Dispose();
            //}
            //else 
            if (btnPause.Text == "Pause")
            {
                if (type == "FTP")
                {
                    Ftp.UploadDownloadPause = "Y";
                }
                else
                {
                    sFtp.StopDownload();
                    sFtp.UploadDownloadPause = "Y";
                }
                btnPause.Text = "Start";
                dgvList.ReadOnly = false;
            }
            else
            {
                if (type == "FTP")
                {
                    Ftp.UploadDownloadPause = "N";
                }
                else
                {
                    sFtp = null;
                    sFtp = new SFTP.SFTP(sitename, siteip, userid, pwd, port, renamefile, "Override");
                    sFtp.Connect();
                    sFtp.ResumeDownload();
                    sFtp.UploadDownloadPause = "N";
                }
                btnPause.Text = "Pause";
                callbyPause = true;
                btnDownload_Click(null, null);
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("Do you really want to abort selected download items?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
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
                        DB.Deletedownloaditem(lblSite.Text, dgvList.Rows[i].Cells["FtpPath"].Value.ToString(), dgvList.Rows[i].Cells["FileName"].Value.ToString());
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
