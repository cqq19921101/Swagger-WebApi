using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Collections;
using System.IO;


namespace FTPTool
{
    public partial class frmSiteViewer : Form
    {
        DB DB = new DB();

        string type = "";
        string Site = "";
        string UserID = "";
        string Password = "";
        string Port = "";

        bool b_refreshlocalfolderbrowser = false;

        public frmSiteViewer()
        {
            InitializeComponent();
        }

        private void frmSiteViewer_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(System.Text.Encoding.Default.WebName.ToString());
            //return;
            try
            {
                using (DataTable dt = DB.GetSiteProfile())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cboSite.Items.Add(dr["SiteName"].ToString());
                    }
                }

                LoadLocalFolder();
                LoadLocalFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
                tvLocal.SelectedNode = tvLocal.Nodes[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ////////////////local computer
        #region local computer
        private void LoadLocalFolder()
        {
            try
            {
                string[] dirs = Directory.GetDirectories(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
                tvLocal.Nodes.Clear();
                TreeNode tnLocal = new TreeNode("Desktop", 3, 3);
                TreeNode tnComputer = new TreeNode("My Computer", 4, 4);
                //string[] disks = System.Environment.GetLogicalDrives();
                DriveInfo[] disks = System.IO.DriveInfo.GetDrives();
                foreach (DriveInfo disk in disks)
                {
                    int icontype = 5;
                    switch (disk.DriveType)
                    {
                        case DriveType.Fixed:
                            icontype = 5;
                            break;
                        case DriveType.CDRom:
                            icontype = 6;
                            break;
                        case DriveType.Removable:
                            icontype = 7;
                            break;
                        case DriveType.Unknown:
                            icontype = 8;
                            break;
                        case DriveType.Network:
                            icontype = 9;
                            break;
                        default:
                            icontype = 5;
                            break;
                    }

                    TreeNode tndisk = new TreeNode(disk.Name, icontype, icontype);

                    tnComputer.Nodes.Add(tndisk);
                }
                tnLocal.Nodes.Add(tnComputer);
                foreach (string dir in dirs)
                {
                    TreeNode tn = new TreeNode(Path.GetFileName(dir), 1, 2);
                    tnLocal.Nodes.Add(tn);
                }

                tvLocal.Nodes.Add(tnLocal);
                tvLocal.ExpandAll();
                //tvLocal.SelectedNode = tvSiteInfo.Nodes[0];


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLocalFile(string path)
        {
            using (DataTable dt = GetLocalFileList(path))
            {
                if (dt == null)
                {
                    dfbLocal.Rows.Clear();
                }
                else
                {
                    RefreshDfbLocal(dt);
                }
            }
        }

        private DataTable GetLocalFileList(string path)
        {
            DataTable dt = Tool.GetFileListDateTable();
            if (path.EndsWith("\\") == false)
            {
                path = path + "\\";
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                if (di.Attributes.ToString() == "Hidden, System, Directory")
                {
                    continue;
                }
                DataRow dr = dt.NewRow();
                dr["File Name"] = di.Name;
                dr["FileFullPath"] = di.FullName;
                dr["Size"] = "0";
                dr["Modify Date"] = di.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dr["Type"] = "File Folder";
                dr["IsFolder"] = "Y";
                dt.Rows.Add(dr);

            }
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                DataRow dr = dt.NewRow();
                dr["File Name"] = fi.Name;
                dr["FileFullPath"] = fi.FullName;
                dr["Size"] = fi.Length.ToString();
                dr["Modify Date"] = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dr["Type"] = Tool.GetFileType(fi.Extension);
                dr["IsFolder"] = "N";
                dt.Rows.Add(dr);

            }

            return dt;
        }

        private void RefreshDfbLocal(DataTable dtFileList)
        {
            dfbLocal.Rows.Clear();
            if (dtFileList != null)
            {
                foreach (DataRow dr in dtFileList.Rows)
                {
                    dfbLocal.Rows.Add(dr["File Name"], dr["Size"], dr["Type"], dr["Modify Date"], dr["FileFullPath"], dr["IsFolder"], dr["Icon"]);
                }
                dfbLocal.ClearSelection();
            }
            dfbLocal.AlternatingRowsDefaultCellStyle.BackColor = Color.Cornsilk;
        }

        private string Getusefullocalfolder(string path)
        {
            if (path.StartsWith(@"Desktop\My Computer"))
            {
                path = path.Replace(@"Desktop\My Computer\", "").Replace("\\\\","\\");
                if (path.EndsWith("\\"))
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }
            else if (path.StartsWith(@"Desktop\"))
            {
                path = path.Replace(@"Desktop\", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\");
            }
            else if (path == "Desktop")
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            }

            return path;
        }

        private void tvLocal_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                if (b_refreshlocalfolderbrowser == false)
                {
                    b_refreshlocalfolderbrowser = true;
                    return;
                }

                string localfolder = "";
                TreeNode tn = e.Node;
                localfolder = e.Node.FullPath;
                if (localfolder == @"Desktop\My Computer")
                {
                    RefreshDfbLocal(null);
                    return;
                }
                localfolder = Getusefullocalfolder(localfolder);
                do
                {
                    if (tn.Parent == null)
                    {
                        localfolder = "/";
                    }
                    else
                    {
                        if (tn.ImageIndex < 3 || tn.ImageIndex > 9)
                        {
                            if (tn.IsExpanded)
                            {
                                tn.ImageIndex = 2;
                            }
                            else
                            {
                                tn.ImageIndex = 1;
                            }
                        }
                        //ftpfolder = ftpfolder + "/" + tn.Parent.Text + "/" + tn.Text;
                        tn = tn.Parent;
                    }
                } while (tn.Parent != null);

                if (localfolder == "/")
                {
                    LoadLocalFolder();
                    LoadLocalFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
                    b_refreshlocalfolderbrowser = false;
                    tvLocal.SelectedNode = tvLocal.Nodes[0];
                    return;
                }
                using (DataTable dt = GetLocalFileList(localfolder))
                {
                    e.Node.Nodes.Clear();
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["IsFolder"].ToString() == "Y")
                            {
                                TreeNode tnChild = new TreeNode(dr["File Name"].ToString(), 1, 2);
                                e.Node.Nodes.Add(tnChild);
                            }
                        }
                    }
                    RefreshDfbLocal(dt);
                }
            }
            catch (Exception ex)
            {
                RefreshDfbLocal(null);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvLocal_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                return;
            }
            if (e.Node.ImageIndex >= 3 && e.Node.ImageIndex <= 9)
            {
                return;
            }
            e.Node.ImageIndex = 1;
        }

        private void dfbLocal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    if (this.dfbLocal.Rows[e.RowIndex].Cells["FileNameLocal"].Value == DBNull.Value)
                        return;

                    string sValue = Convert.ToString(this.dfbLocal.Rows[e.RowIndex].Cells["FileNameLocal"].Value);

                    string sFilePath = Convert.ToString(this.dfbLocal.Rows[e.RowIndex].Cells["FileFullPathLocal"].Value);
                    string sIsFolder = Convert.ToString(this.dfbLocal.Rows[e.RowIndex].Cells["IsFolderLocal"].Value);

                    Icon icon;
                    if (sIsFolder == "Y")
                    {
                        icon = Tool.GetDirectoryIcon(true);
                    }
                    else
                    {
                        string ext = sFilePath.Substring(sFilePath.LastIndexOf("."), sFilePath.Length - sFilePath.LastIndexOf("."));
                        icon = Tool.GetFileIcon(ext, true);
                    }


                    int i = (e.CellBounds.Height - 1) > 16 ? 16 : e.CellBounds.Height - 1;
                    Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, i, i);

                    using (Brush gridBrush = new SolidBrush(this.dfbServer.GridColor),
                            backColorBrush = new SolidBrush(e.CellStyle.BackColor),
                            alternatebackColorBrush = new SolidBrush(Color.Cornsilk),
                            selectbackcoroBrush=new SolidBrush(Color.FromArgb(51,153,255)))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush, 2))
                        {
                            
                            // Erase the cell.
                            if (e.RowIndex % 2 != 0)
                            {
                                if (dfbLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected == true)
                                {
                                    e.Graphics.FillRectangle(selectbackcoroBrush, e.CellBounds);
                                }
                                else
                                {
                                    e.Graphics.FillRectangle(alternatebackColorBrush, e.CellBounds);
                                }
                            }
                            else
                            {
                                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                            }

                            //划线
                            Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top);
                            Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top + e.CellBounds.Height);
                            Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height);
                            Point[] ps = new Point[] { p1, p2, p3 };
                            e.Graphics.DrawLines(gridLinePen, ps);

                            //画图标
                            //e.Graphics.DrawImage(image, newRect);
                            e.Graphics.DrawIcon(icon, newRect);
                            //画字符串
                            if (dfbLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected == true)
                            {
                                dfbLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromKnownColor(KnownColor.MenuHighlight);
                                e.Graphics.DrawString(sValue, e.CellStyle.Font, Brushes.White,
                                    e.CellBounds.Left + 20, e.CellBounds.Top + 5, StringFormat.GenericDefault);
                            }
                            else
                            {
                                dfbLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromKnownColor(KnownColor.White);
                                e.Graphics.DrawString(sValue, e.CellStyle.Font, Brushes.Black,
                                    e.CellBounds.Left + 20, e.CellBounds.Top + 5, StringFormat.GenericDefault);
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            catch { }
        }

        private void dfbLocal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    if (dfbLocal.Rows[e.RowIndex].Cells["IsFolderLocal"].Value.ToString() == "Y")
                    {
                        foreach (TreeNode tn in tvLocal.SelectedNode.Nodes)
                        {
                            if (tn.Text == dfbLocal.Rows[e.RowIndex].Cells["FileNameLocal"].Value.ToString())
                            {
                                tvLocal.SelectedNode = tn;
                                tn.ExpandAll();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        //////////////ftp server
        #region ftp server
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cboSite.Text == "")
            {
                return;
            }
            txtFullPath.Text = "";
            try
            {
                this.Cursor = Cursors.AppStarting;
                //string logname = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                //Tool.SaveLog(logname, "Start connect");
                using (DataTable dt = GetSiteInfoList(cboSite.Text,""))
                {
                    if (dt == null)
                    {
                        MessageBox.Show("Can't get site info, please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; 
                    }
                    LoadTreeView(cboSite.Text, dt);
                    //刷新DataGridView
                    RefreshDfbServer(dt);
                    cboSite.Enabled = false;
                }
                //Tool.SaveLog(logname, "End connect");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private DataTable GetSiteInfoList(string SiteName,string ftpfolder)
        {
            //string logname = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            //Tool.SaveLog(logname, "Read profile");
            using (DataTable dtSite = DB.GetSiteProfileDetail(SiteName))
            {
                //Tool.SaveLog(logname, "Read profile finish");
                type = dtSite.Rows[0]["Type"].ToString();
                Site = dtSite.Rows[0]["SiteIP"].ToString();
                UserID = dtSite.Rows[0]["UserID"].ToString();
                Password = Security.Decrypt(dtSite.Rows[0]["Password"].ToString());
                Port = dtSite.Rows[0]["Port"].ToString();

                DataTable dt=null;
                if (type == "FTP")
                {
                    FTP.FTP ftp = null;
                    try
                    {
                        //Tool.SaveLog(logname, "Create ftp instance start");
                        ftp = new FTP.FTP(SiteName, Site, UserID, Password, Port);
                        //Tool.SaveLog(logname, "Create ftp instance finish");
                        //Tool.SaveLog(logname, "Get file list start");
                        dt = ftp.GetFileList(ftpfolder);
                        //Tool.SaveLog(logname, "Get file list finish");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        ftp = null;
                    }
                }
                else if (type == "SSH")
                {
                    SFTP.SFTP sftp = null;
                    try
                    {
                        sftp = new SFTP.SFTP(SiteName, Site, UserID, Port);
                        sftp.Connect();
                        dt = sftp.GetFileList(ftpfolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sftp = null;
                    }
                }
                else
                {
                    SFTP.SFTP sftp = null;
                    try
                    {
                        sftp = new SFTP.SFTP(SiteName, Site, UserID, Password, Port);
                        sftp.Connect();
                        dt = sftp.GetFileList(ftpfolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sftp = null;
                    }
                }
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Type"] = Tool.GetFileType(dr["Type"].ToString());
                    }
                }
                return dt;
            }
        }

        private void LoadTreeView(string SiteName, DataTable dtSiteInfo)
        {
            tvSiteInfo.Nodes.Clear();
            TreeNode tnSite = new TreeNode(SiteName, 0, 0);

            foreach (DataRow dr in dtSiteInfo.Rows)
            {
                if (dr["IsFolder"].ToString() == "Y")
                {
                    TreeNode tn = new TreeNode(dr["File Name"].ToString(), 1,2);
                    tnSite.Nodes.Add(tn);
                }
            }

            tvSiteInfo.Nodes.Add(tnSite);
            tvSiteInfo.ExpandAll();
            tvSiteInfo.SelectedNode = tvSiteInfo.Nodes[0];
        }

        private void tvSiteInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string ftpfolder = "";
                TreeNode tn = e.Node;
                ftpfolder = e.Node.FullPath;
                do
                {
                    if (tn.Parent == null)
                    {
                        ftpfolder = "/";
                    }
                    else
                    {
                        if (tn.IsExpanded)
                        {
                            tn.ImageIndex = 2;
                        }
                        else
                        {
                            tn.ImageIndex = 1;
                        }
                        //ftpfolder = ftpfolder + "/" + tn.Parent.Text + "/" + tn.Text;
                        tn = tn.Parent;
                    }
                } while (tn.Parent != null);
                if (ftpfolder != "/")
                {
                    ftpfolder = ftpfolder.Substring(cboSite.Text.Length, ftpfolder.Length - cboSite.Text.Length);
                }
                txtFullPath.Text = ftpfolder;
                if (ftpfolder.StartsWith("/"))
                {
                    ftpfolder = ftpfolder.Substring(1, ftpfolder.Length - 1);
                }

                using (DataTable dtSiteInfo = GetSiteInfoList(cboSite.Text, ftpfolder))
                {
                    e.Node.Nodes.Clear();
                    if (dtSiteInfo != null)
                    {
                        foreach (DataRow dr in dtSiteInfo.Rows)
                        {
                            if (dr["IsFolder"].ToString() == "Y")
                            {
                                TreeNode tnChild = new TreeNode(dr["File Name"].ToString(), 1, 2);
                                e.Node.Nodes.Add(tnChild);
                            }
                        }
                    }
                    RefreshDfbServer(dtSiteInfo);
                }
            }
            catch (Exception ex)
            {
                RefreshDfbServer(null);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RefreshDfbServer(DataTable dtFileList)
        {
            dfbServer.Rows.Clear();
            if (dtFileList != null)
            {
                foreach (DataRow dr in dtFileList.Rows)
                {
                    dfbServer.Rows.Add(dr["File Name"], dr["Size"], dr["Type"], dr["Modify Date"], dr["FileFullPath"], dr["IsFolder"], dr["Icon"]);
                }
                dfbServer.ClearSelection();
            }
            dfbServer.AlternatingRowsDefaultCellStyle.BackColor = Color.Cornsilk;
        }

        private void dfbServer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    if (this.dfbServer.Rows[e.RowIndex].Cells["FileName"].Value == DBNull.Value)
                        return;

                    string sValue = Convert.ToString(this.dfbServer.Rows[e.RowIndex].Cells["FileName"].Value);

                    string sFilePath = Convert.ToString(this.dfbServer.Rows[e.RowIndex].Cells["FileName"].Value);
                    string sIsFolder = Convert.ToString(this.dfbServer.Rows[e.RowIndex].Cells["IsFolder"].Value);

                    Icon icon;
                    if (sIsFolder == "Y")
                    {
                        icon = Tool.GetDirectoryIcon(true);
                    }
                    else
                    {
                        string ext = sFilePath.Substring(sFilePath.LastIndexOf("."), sFilePath.Length - sFilePath.LastIndexOf("."));
                        icon = Tool.GetFileIcon(ext, true);
                    }


                    int i = (e.CellBounds.Height - 1) > 16 ? 16 : e.CellBounds.Height - 1;
                    Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, i, i);

                    using (Brush gridBrush = new SolidBrush(this.dfbServer.GridColor), 
                            backColorBrush = new SolidBrush(e.CellStyle.BackColor),
                            alternatebackColorBrush = new SolidBrush(Color.Cornsilk),
                            selectbackcoroBrush=new SolidBrush(Color.FromArgb(51,153,255)))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush, 2))
                        {
                            // Erase the cell.
                            if (e.RowIndex % 2 != 0)
                            {
                                if (dfbServer.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected == true)
                                {
                                    e.Graphics.FillRectangle(selectbackcoroBrush, e.CellBounds);
                                }
                                else
                                {
                                    e.Graphics.FillRectangle(alternatebackColorBrush, e.CellBounds);
                                }
                            }
                            else
                            {
                                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                            }

                            //划线
                            Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top);
                            Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top + e.CellBounds.Height);
                            Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height);
                            Point[] ps = new Point[] { p1, p2, p3 };
                            e.Graphics.DrawLines(gridLinePen, ps);

                            //画图标
                            //e.Graphics.DrawImage(image, newRect);
                            e.Graphics.DrawIcon(icon, newRect);
                            //画字符串
                            if (dfbServer.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected == true)
                            {
                                dfbServer.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromKnownColor(KnownColor.MenuHighlight);
                                e.Graphics.DrawString(sValue, e.CellStyle.Font, Brushes.White,
                                    e.CellBounds.Left + 20, e.CellBounds.Top + 5, StringFormat.GenericDefault);
                            }
                            else
                            {
                                dfbServer.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromKnownColor(KnownColor.White);
                                e.Graphics.DrawString(sValue, e.CellStyle.Font, Brushes.Black,
                                    e.CellBounds.Left + 20, e.CellBounds.Top + 5, StringFormat.GenericDefault);
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            catch { }
        }

        private void dfbServer_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    if (dfbServer.Rows[e.RowIndex].Cells["IsFolder"].Value.ToString() == "Y")
                    {
                        this.Cursor = Cursors.WaitCursor;
                        txtFullPath.Text = dfbServer.Rows[e.RowIndex].Cells["FileFullPath"].Value.ToString();
                        foreach (TreeNode tn in tvSiteInfo.SelectedNode.Nodes)
                        {
                            if (tn.Text == dfbServer.Rows[e.RowIndex].Cells["FileName"].Value.ToString())
                            {
                                tvSiteInfo.SelectedNode = tn;
                                tn.ExpandAll();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            txtFullPath.Text = "";
            tvSiteInfo.Nodes.Clear();
            dfbServer.Rows.Clear();
            cboSite.Enabled = true;
        }

        private void tvSiteInfo_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                return;
            }
            e.Node.ImageIndex = 1;
        }
        #endregion


        #region operate menu

        #region local folder

        private void cms_local_folder_create_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            try
            {
                frmCreateNewFolder fcnf = new frmCreateNewFolder();
                fcnf.ShowDialog();

                if (fcnf.IsCancel == false)
                {
                    string folder = fcnf.FolderName;
                    string currpath = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);
                    if (Directory.Exists(currpath + "\\" + folder))
                    {
                        MessageBox.Show("This folder is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Directory.CreateDirectory(currpath + "\\" + folder);
                        TreeNode tn = new TreeNode(folder, 1, 2);
                        tvLocal.SelectedNode.Nodes.Add(tn);
                        LoadLocalFile(currpath);
                        MessageBox.Show("Create a new folder finished", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_folder_delete_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (tvLocal.SelectedNode.ImageIndex != 1 && tvLocal.SelectedNode.ImageIndex != 2)
            {
                MessageBox.Show("Please choose a folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DialogResult.No == MessageBox.Show("Do you really want to delete this folder?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                TreeNode tn = tvLocal.SelectedNode.Parent;
                string path = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);
                Directory.Delete(path,true);
                tvLocal.SelectedNode = tn;
                MessageBox.Show("Delete a folder finihsed", "Delete folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_folder_upload_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                TreeNode tn = tvSiteInfo.SelectedNode;
                string ftppath = tn.FullPath;
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);

                frmUploadStatusWindow fusw = new frmUploadStatusWindow(cboSite.Text, localpath, ftppath);
                fusw.ShowDialog();
                System.Windows.Forms.Application.DoEvents();
                tvSiteInfo.SelectedNode = tn;
                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                MessageBox.Show("Upload finished", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_folder_upload_br_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                TreeNode tnsite = tvSiteInfo.SelectedNode;
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tnsite.FullPath;//ftppath要去除site信息
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                ftppath = ftppath.Substring(cboSite.Text.Length, ftppath.Length - cboSite.Text.Length);
                string localpath = Getusefullocalfolder(tnlocal.FullPath);
                //上传时要新建被选中的文件夹，所以要在路径上退一级
                localpath = localpath.Substring(0, localpath.LastIndexOf("\\"));

                using (DataTable dtexist = DB.GetUploadBreakpointResumeFileList(cboSite.Text))
                {
                    if (dtexist.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Exist unfinished upload task, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            frmFullUploadProgress ffup = new frmFullUploadProgress(cboSite.Text, ftppath, null, localpath);
                            ffup.ShowDialog();
                            return;
                        }
                        else
                        {
                            DB.DeleteAllUnfinishedUploadItem(cboSite.Text);
                        }
                    }
                }

                DataTable dtChoosedList = new DataTable();
                dtChoosedList.Columns.Add("LocalPath");
                dtChoosedList.Columns.Add("Size");
                dtChoosedList.Columns.Add("Modify Date");
                dtChoosedList.Columns.Add("Type");
                dtChoosedList.Columns.Add("FileFullPath");
                dtChoosedList.Columns.Add("IsFolder");

                foreach (DataGridViewRow dgvr in dfbLocal.Rows)
                {
                    string filename = dgvr.Cells["FileNameLocal"].Value.ToString();
                    string isfolder = dgvr.Cells["IsFolderLocal"].Value.ToString();

                    DataRow dr = dtChoosedList.NewRow();
                    dr["LocalPath"] = localpath + "\\" + filename;
                    dr["Size"] = dgvr.Cells["SizeLocal"].Value.ToString();
                    dr["Type"] = dgvr.Cells["FileTypeLocal"].Value.ToString();
                    dr["Modify Date"] = dgvr.Cells["ModifyDateTimeLocal"].Value.ToString();
                    dr["FileFullPath"] = dgvr.Cells["FileFullPathLocal"].Value.ToString();
                    dr["IsFolder"] = isfolder;
                    dtChoosedList.Rows.Add(dr);
                }
                frmFullUploadProgress ffupnew = new frmFullUploadProgress(cboSite.Text, ftppath, dtChoosedList, localpath);
                ffupnew.ShowDialog();

                tvSiteInfo.SelectedNode = tnsite;
                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnsite));

                if (ffupnew.IsCancel == "N")
                {
                    DB.DeleteAllUnfinishedUploadItem(cboSite.Text);
                    MessageBox.Show("Upload finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region local file


        private void cms_local_file_create_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            try
            {
                frmCreateNewFolder fcnf = new frmCreateNewFolder();
                fcnf.ShowDialog();

                if (fcnf.IsCancel == false)
                {
                    string folder = fcnf.FolderName;
                    string currpath = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);
                    if (Directory.Exists(currpath + "\\" + folder))
                    {
                        MessageBox.Show("This folder is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Directory.CreateDirectory(currpath + "\\" + folder);
                        TreeNode tn = new TreeNode(folder, 1, 2);
                        tvLocal.SelectedNode.Nodes.Add(tn);
                        LoadLocalFile(currpath);
                        MessageBox.Show("Create a new folder finished", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_file_delete_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (dfbLocal.SelectedCells.Count==0)
            {
                MessageBox.Show("Please choose some rows to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DialogResult.No == MessageBox.Show("Do you really want to delete these folders and files?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbLocal.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }
                TreeNode tn = tvLocal.SelectedNode;
                string path = Getusefullocalfolder(tn.FullPath);
                foreach (int iRow in alrow)
                {
                    string filename = dfbLocal.Rows[iRow].Cells["FileNameLocal"].Value.ToString();
                    string isfolder = dfbLocal.Rows[iRow].Cells["IsFolderLocal"].Value.ToString();
                    if (isfolder == "N")
                    {
                        File.SetAttributes(path + "\\" + filename, FileAttributes.Normal);
                        File.Delete(path + "\\" + filename);
                    }
                    else
                    {
                        Directory.Delete(path + "\\" + filename, true);
                    }
                }
                tvLocal.SelectedNode = tn;
                tvLocal_AfterSelect(tvLocal, new TreeViewEventArgs(tn));
                MessageBox.Show("Delete selected folders and files finihsed", "Delete items", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_file_upload_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (dfbLocal.SelectedCells.Count == 0)
                {
                    MessageBox.Show("Please select some folders or files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                TreeNode tnsite = tvSiteInfo.SelectedNode;
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tnsite.FullPath;
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tnlocal.FullPath);

                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbLocal.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }

                foreach (int iRow in alrow)
                {
                    string filename = dfbLocal.Rows[iRow].Cells["FileNameLocal"].Value.ToString();

                    frmUploadStatusWindow fusw = new frmUploadStatusWindow(cboSite.Text, localpath + "\\" + filename, ftppath);
                    fusw.ShowDialog();
                    //System.Windows.Forms.Application.DoEvents();
                }
                tvSiteInfo.SelectedNode = tnsite;
                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnsite));
                MessageBox.Show("Upload finished", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_local_file_upload_br_Click(object sender, EventArgs e)
        {
            if (tvLocal.SelectedNode == null)
            {
                return;
            }
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                TreeNode tnsite = tvSiteInfo.SelectedNode;
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tnsite.FullPath;//ftppath要去除site信息
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                ftppath = ftppath.Substring(cboSite.Text.Length, ftppath.Length - cboSite.Text.Length);
                string localpath = Getusefullocalfolder(tnlocal.FullPath);


                using (DataTable dtexist = DB.GetUploadBreakpointResumeFileList(cboSite.Text))
                {
                    if (dtexist.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Exist unfinished upload task, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            frmFullUploadProgress ffup = new frmFullUploadProgress(cboSite.Text, ftppath, null, localpath);
                            ffup.ShowDialog();
                            return;
                        }
                        else
                        {
                            DB.DeleteAllUnfinishedUploadItem(cboSite.Text);
                        }
                    }
                }
                if (dfbLocal.SelectedCells.Count == 0)
                {
                    MessageBox.Show("Please select some folders or files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbLocal.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }

                DataTable dtChoosedList = new DataTable();
                dtChoosedList.Columns.Add("LocalPath");
                dtChoosedList.Columns.Add("Size");
                dtChoosedList.Columns.Add("Modify Date");
                dtChoosedList.Columns.Add("Type");
                dtChoosedList.Columns.Add("FileFullPath");
                dtChoosedList.Columns.Add("IsFolder");

                foreach (int iRow in alrow)
                {
                    string filename = dfbLocal.Rows[iRow].Cells["FileNameLocal"].Value.ToString();
                    string isfolder = dfbLocal.Rows[iRow].Cells["IsFolderLocal"].Value.ToString();

                    DataRow dr = dtChoosedList.NewRow();
                    dr["LocalPath"] = localpath+"\\" + filename;
                    dr["Size"] = dfbLocal.Rows[iRow].Cells["SizeLocal"].Value.ToString();
                    dr["Type"] = dfbLocal.Rows[iRow].Cells["FileTypeLocal"].Value.ToString();
                    dr["Modify Date"] = dfbLocal.Rows[iRow].Cells["ModifyDateTimeLocal"].Value.ToString();
                    dr["FileFullPath"] = dfbLocal.Rows[iRow].Cells["FileFullPathLocal"].Value.ToString();
                    dr["IsFolder"] = isfolder;
                    dtChoosedList.Rows.Add(dr);
                }
                frmFullUploadProgress ffupnew = new frmFullUploadProgress(cboSite.Text, ftppath, dtChoosedList, localpath);
                ffupnew.ShowDialog();

                tvSiteInfo.SelectedNode = tnsite;
                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnsite));

                if (ffupnew.IsCancel == "N")
                {
                    DB.DeleteAllUnfinishedUploadItem(cboSite.Text);
                    MessageBox.Show("Upload finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region ftp folder

        private void cms_site_folder_create_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }

            try
            {
                TreeNode tn = tvSiteInfo.SelectedNode;
                string ftppath = tn.FullPath;
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                ftppath = ftppath.Substring(cboSite.Text.Length + 1, ftppath.Length - cboSite.Text.Length - 1);
                frmCreateNewFolder fcnf = new frmCreateNewFolder();
                fcnf.ShowDialog();

                if (fcnf.IsCancel == false)
                {
                    string folder = fcnf.FolderName;
                    string errmsg = "";
                    if (type == "FTP")
                    {
                        FTP.FTP ftp = getFtpClass(cboSite.Text, out errmsg);
                        if (ftp == null)
                        {
                            MessageBox.Show("Get Ftp class failed\r\n" + errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errmsg = null;
                            return;
                        }
                        errmsg = null;

                        try
                        {
                            ftp.MakeFolder(ftppath + folder);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Create a new folder failed, maybe it's already exist. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        SFTP.SFTP sftp = getSFtpClass(cboSite.Text, out errmsg);
                        if (sftp == null)
                        {
                            MessageBox.Show("Get SFtp class failed\r\n" + errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errmsg = null;
                            return;
                        }
                        errmsg = null;

                        try
                        {
                            sftp.MakeFolder(ftppath + folder);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Create a new folder failed, maybe it's already exist. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                    MessageBox.Show("Create a new folder finished", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_folder_delete_Click(object sender, EventArgs e)
        {
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }
            if (tvSiteInfo.SelectedNode.ImageIndex != 1 && tvSiteInfo.SelectedNode.ImageIndex != 2)
            {
                MessageBox.Show("Please choose a folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DialogResult.No == MessageBox.Show("Do you really want to delete this folder?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                TreeNode tn = tvSiteInfo.SelectedNode.Parent;
                string ftppath = tvSiteInfo.SelectedNode.FullPath;
                ftppath = ftppath.Substring(cboSite.Text.Length + 1, ftppath.Length - cboSite.Text.Length - 1);

                try
                {
                    using (DataTable dt = DB.GetSiteProfileDetail(cboSite.Text))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Can't find ftp setting, please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string type = dt.Rows[0]["Type"].ToString();
                        string siteip = dt.Rows[0]["SiteIP"].ToString();
                        string userid = dt.Rows[0]["UserID"].ToString();
                        string password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                        string port = dt.Rows[0]["Port"].ToString();
                        ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType("Delete");
                        OnStepAction.RunAction(this, 1,type, cboSite.Text, siteip, userid, password,port, "Delete", ftppath, "Y", "", "N");    

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete a folder failed. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                MessageBox.Show("Delete a folder finihsed", "Delete folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_folder_download_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                MessageBox.Show("Please choose a ftp folder", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (tvLocal.SelectedNode == null)
            {
                MessageBox.Show("Please choose a local folder", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tvSiteInfo.SelectedNode.FullPath;
                
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);
               

                frmDownloadStatusWindow fdsw = new frmDownloadStatusWindow(cboSite.Text, localpath, ftppath, "Y");
                fdsw.ShowDialog();
                System.Windows.Forms.Application.DoEvents();
                tvLocal_AfterSelect(tvLocal, new TreeViewEventArgs(tnlocal));
                MessageBox.Show("Download finished", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_folder_download_br_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                MessageBox.Show("Please choose a ftp folder", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (tvLocal.SelectedNode == null)
            {
                MessageBox.Show("Please choose a local folder", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (DataTable dtexist = DB.GetDownloadBreakpointResumeFileList(cboSite.Text))
                {
                    if (dtexist.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Exist unfinished download task, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            frmFullDownloadProgress ffdp = new frmFullDownloadProgress(cboSite.Text, "", null);
                            ffdp.ShowDialog();
                            return;
                        }
                        else
                        {
                            DB.DeleteAllUnfinishedDownloadItem(cboSite.Text);
                        }
                    }
                }

                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tvSiteInfo.SelectedNode.FullPath;

                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tvLocal.SelectedNode.FullPath);
                localpath = localpath + "\\" + tvSiteInfo.SelectedNode.Text;

                DataTable dtChoosedList = new DataTable();
                dtChoosedList.Columns.Add("FtpPath");
                dtChoosedList.Columns.Add("Size");
                dtChoosedList.Columns.Add("Modify Date");
                dtChoosedList.Columns.Add("Type");
                dtChoosedList.Columns.Add("FileFullPath");
                dtChoosedList.Columns.Add("IsFolder");

                foreach (DataGridViewRow dgvr in dfbServer.Rows)
                {
                    string filename = dgvr.Cells["FileName"].Value.ToString();
                    string isfolder = dgvr.Cells["IsFolder"].Value.ToString();

                    DataRow dr = dtChoosedList.NewRow();
                    dr["FtpPath"] = ftppath + filename;
                    dr["Size"] = dgvr.Cells["FileSize"].Value.ToString();
                    dr["Type"] = dgvr.Cells["FileType"].Value.ToString();
                    dr["Modify Date"] = dgvr.Cells["FileModifyDate"].Value.ToString();
                    dr["FileFullPath"] = dgvr.Cells["FileFullPath"].Value.ToString();
                    dr["IsFolder"] = isfolder;
                    dtChoosedList.Rows.Add(dr);
                }

                frmFullDownloadProgress ffdpnew = new frmFullDownloadProgress(cboSite.Text, localpath, dtChoosedList);
                ffdpnew.ShowDialog();


                tvLocal_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnlocal));

                if (ffdpnew.IsCancel == "N")
                {
                    DB.DeleteAllUnfinishedDownloadItem(cboSite.Text);
                    MessageBox.Show("Download finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ftp file

        private void cms_site_file_create_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }

            try
            {
                TreeNode tn = tvSiteInfo.SelectedNode;
                string ftppath = tn.FullPath;
                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                ftppath = ftppath.Substring(cboSite.Text.Length + 1, ftppath.Length - cboSite.Text.Length - 1);
                frmCreateNewFolder fcnf = new frmCreateNewFolder();
                fcnf.ShowDialog();

                if (fcnf.IsCancel == false)
                {
                    string folder = fcnf.FolderName;
                    string errmsg = "";

                    if (type == "FTP")
                    {
                        FTP.FTP ftp = getFtpClass(cboSite.Text, out errmsg);
                        if (ftp == null)
                        {
                            MessageBox.Show("Get Ftp class failed\r\n" + errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errmsg = null;
                            return;
                        }
                        errmsg = null;

                        try
                        {
                            ftp.MakeFolder(ftppath + folder);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Create a new folder failed, maybe it's already exist. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        SFTP.SFTP sftp = getSFtpClass(cboSite.Text, out errmsg);
                        if (sftp == null)
                        {
                            MessageBox.Show("Get SFtp class failed\r\n" + errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errmsg = null;
                            return;
                        }
                        errmsg = null;

                        try
                        {
                            sftp.MakeFolder(ftppath + folder);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Create a new folder failed, maybe it's already exist. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                    MessageBox.Show("Create a new folder finished", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_file_delete_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }

            if (dfbServer.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please choose some rows to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DialogResult.No == MessageBox.Show("Do you really want to delete these folders and files?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbServer.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }
                TreeNode tn = tvSiteInfo.SelectedNode;
                string path = tn.FullPath;
                if (path.EndsWith("/") == false)
                {
                    path = path + "/";
                }
                path = path.Substring(cboSite.Text.Length + 1, path.Length - cboSite.Text.Length - 1);
                try
                {
                    using (DataTable dt = DB.GetSiteProfileDetail(cboSite.Text))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Can't find ftp setting, please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string type = dt.Rows[0]["Type"].ToString();
                        string siteip = dt.Rows[0]["SiteIP"].ToString();
                        string userid = dt.Rows[0]["UserID"].ToString();
                        string password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                        string port= dt.Rows[0]["Port"].ToString();
                        ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType("Delete");

                        foreach (int iRow in alrow)
                        {
                            string filename = dfbServer.Rows[iRow].Cells["FileName"].Value.ToString();
                            string isfolder = dfbServer.Rows[iRow].Cells["IsFolder"].Value.ToString();
                            if (isfolder == "N")
                            {
                                OnStepAction.RunAction(this, 1, type,cboSite.Text, siteip, userid, password,port, "Delete", path+"/"+filename, "N", "", "N");
                            }
                            else
                            {
                                OnStepAction.RunAction(this, 1, type,cboSite.Text, siteip, userid, password,port, "Delete", path + "/" + filename, "Y", "", "N");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete a folder failed. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                MessageBox.Show("Delete selected folders and files finihsed", "Delete items", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_file_download_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }
            if (tvLocal.SelectedNode == null)
            {
                MessageBox.Show("Please choose a local folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
     
            try
            {
                if (dfbServer.SelectedCells.Count == 0)
                {
                    MessageBox.Show("Please select some folders or files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                TreeNode tnsite = tvSiteInfo.SelectedNode;
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tvSiteInfo.SelectedNode.FullPath;

                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tnlocal.FullPath);

                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbServer.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }

                foreach (int iRow in alrow)
                {
                    string filename = dfbServer.Rows[iRow].Cells["FileName"].Value.ToString();
                    string isfolder=dfbServer.Rows[iRow].Cells["IsFolder"].Value.ToString();

                    frmDownloadStatusWindow fdsw = new frmDownloadStatusWindow(cboSite.Text, localpath, ftppath + filename, isfolder);
                    fdsw.ShowDialog();
                }
                tvLocal_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnlocal));
                MessageBox.Show("Download finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_file_download_br_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }
            if (tvLocal.SelectedNode == null)
            {
                MessageBox.Show("Please choose a local folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (DataTable dtexist = DB.GetDownloadBreakpointResumeFileList(cboSite.Text))
                {
                    if (dtexist.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Exist unfinished download task, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            frmFullDownloadProgress ffdp = new frmFullDownloadProgress(cboSite.Text, "", null);
                            ffdp.ShowDialog();
                            return;
                        }
                        else
                        {
                            DB.DeleteAllUnfinishedDownloadItem(cboSite.Text);
                        }
                    }
                }
                if (dfbServer.SelectedCells.Count == 0)
                {
                    MessageBox.Show("Please select some folders or files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                TreeNode tnsite = tvSiteInfo.SelectedNode;
                TreeNode tnlocal = tvLocal.SelectedNode;
                string ftppath = tvSiteInfo.SelectedNode.FullPath;

                if (ftppath.EndsWith("/") == false)
                {
                    ftppath = ftppath + "/";
                }
                string localpath = Getusefullocalfolder(tnlocal.FullPath);

                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbServer.SelectedCells)
                {
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }

                DataTable dtChoosedList = new DataTable();
                dtChoosedList.Columns.Add("FtpPath");
                dtChoosedList.Columns.Add("Size");
                dtChoosedList.Columns.Add("Modify Date");
                dtChoosedList.Columns.Add("Type");
                dtChoosedList.Columns.Add("FileFullPath");
                dtChoosedList.Columns.Add("IsFolder");

                foreach (int iRow in alrow)
                {
                    string filename = dfbServer.Rows[iRow].Cells["FileName"].Value.ToString();
                    string isfolder = dfbServer.Rows[iRow].Cells["IsFolder"].Value.ToString();

                    DataRow dr = dtChoosedList.NewRow();
                    dr["FtpPath"] = ftppath + filename;
                    dr["Size"] = dfbServer.Rows[iRow].Cells["FileSize"].Value.ToString();
                    dr["Type"] = dfbServer.Rows[iRow].Cells["FileType"].Value.ToString();
                    dr["Modify Date"] = dfbServer.Rows[iRow].Cells["FileModifyDate"].Value.ToString();
                    dr["FileFullPath"] = dfbServer.Rows[iRow].Cells["FileFullPath"].Value.ToString();
                    dr["IsFolder"] = isfolder;
                    dtChoosedList.Rows.Add(dr);
                }
                frmFullDownloadProgress ffdpnew = new frmFullDownloadProgress(cboSite.Text, localpath, dtChoosedList);
                ffdpnew.ShowDialog();

                tvLocal_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tnlocal));

                if (ffdpnew.IsCancel == "N")
                {
                    DB.DeleteAllUnfinishedDownloadItem(cboSite.Text);
                    MessageBox.Show("Download finished", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_site_file_rename_Click(object sender, EventArgs e)
        {
            if (cboSite.Enabled == true)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.Nodes.Count == 0)
            {
                MessageBox.Show("Please connect to a ftp site at first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tvSiteInfo.SelectedNode == null)
            {
                return;
            }

            if (dfbServer.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please choose some rows to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //获取选中的行
                ArrayList alrow = new ArrayList();
                foreach (DataGridViewCell dgvc in dfbServer.SelectedCells)
                {
                    if (dfbServer.Rows[dgvc.RowIndex].Cells["IsFolder"].Value.ToString() == "Y")
                    {
                        continue;
                    }
                    if (alrow.Contains(dgvc.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        alrow.Add(dgvc.RowIndex);
                    }
                }
                if (alrow.Count != 1)
                {
                    MessageBox.Show("Please choose only one file, don't choose several files or folders", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DialogResult.No == MessageBox.Show("Do you really want to rename this file?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }

                frmRenamefile fnf=new frmRenamefile();
                fnf.ShowDialog();
                string newfilename=fnf.Filename;

                TreeNode tn = tvSiteInfo.SelectedNode;
                string path = tn.FullPath;
                if (path.EndsWith("/") == false)
                {
                    path = path + "/";
                }
                path = path.Substring(cboSite.Text.Length + 1, path.Length - cboSite.Text.Length - 1);
                try
                {
                    using (DataTable dt = DB.GetSiteProfileDetail(cboSite.Text))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Can't find ftp setting, please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string type = dt.Rows[0]["Type"].ToString();
                        string siteip = dt.Rows[0]["SiteIP"].ToString();
                        string userid = dt.Rows[0]["UserID"].ToString();
                        string password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                        string port = dt.Rows[0]["Port"].ToString();
                        ActionBLL.Action OnStepAction =ActionBLL.GetAction.GetActionType("Rename");

                        foreach (int iRow in alrow)
                        {
                            string filename =  path + dfbServer.Rows[iRow].Cells["FileName"].Value.ToString();
                            if (type == "FTP")
                            {
                                newfilename = newfilename;//FTP改名，源文件名要带路径，新文件名只要名字即可
                            }
                            else
                            {
                                newfilename = path + newfilename;
                            }
                            string isfolder = dfbServer.Rows[iRow].Cells["IsFolder"].Value.ToString();
                            OnStepAction.RunAction(this, iRow,type, cboSite.Text, siteip, userid, password,port, "Rename", filename, "N", newfilename, "N");
                            //if (isfolder == "N")
                            //{
                            //    OnStepAction.RunAction(this, 1, cboSite.Text, siteip, userid, password, "Delete", path + "/" + filename, "N", "", "N");
                            //}
                            //else
                            //{
                            //    OnStepAction.RunAction(this, 1, cboSite.Text, siteip, userid, password, "Delete", path + "/" + filename, "Y", "", "N");
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Rename a file failed. The error message as below\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                tvSiteInfo_AfterSelect(tvSiteInfo, new TreeViewEventArgs(tn));
                MessageBox.Show("Rename selected file finihsed", "Delete items", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Site Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion


        private FTP.FTP getFtpClass(string SiteName,out string errmsg)
        {
            try
            {
                using (DataTable dt = DB.GetSiteProfileDetail(cboSite.Text))
                {
                    if (dt.Rows.Count == 0)
                    {
                        errmsg = "Can't find ftp setting, please check";
                        return null;
                    }
                    string siteip = dt.Rows[0]["SiteIP"].ToString();
                    string userid = dt.Rows[0]["UserID"].ToString();
                    string password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                    string port = dt.Rows[0]["Port"].ToString();

                    FTP.FTP ftp = new FTP.FTP(cboSite.Text, siteip, userid, password,port);
                    errmsg = "";
                    return ftp;
                }
            }
            catch(Exception ex)
            {
                errmsg = ex.Message;
                return null;
            }
        }

        private SFTP.SFTP getSFtpClass(string SiteName, out string errmsg)
        {
            try
            {
                using (DataTable dt = DB.GetSiteProfileDetail(cboSite.Text))
                {
                    if (dt.Rows.Count == 0)
                    {
                        errmsg = "Can't find sftp setting, please check";
                        return null;
                    }
                    string siteip = dt.Rows[0]["SiteIP"].ToString();
                    string userid = dt.Rows[0]["UserID"].ToString();
                    string password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                    string port = dt.Rows[0]["Port"].ToString();

                    SFTP.SFTP sftp = new SFTP.SFTP(cboSite.Text, siteip, userid, password, port);
                    errmsg = "";
                    return sftp;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return null;
            }
        }






















    }
}

