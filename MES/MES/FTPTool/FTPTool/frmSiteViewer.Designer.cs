namespace FTPTool
{
    partial class frmSiteViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiteViewer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSite = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.dfbServer = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileModifyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Iconx = new System.Windows.Forms.DataGridViewImageColumn();
            this.cms_site_file = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_site_file_download = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_file_download_br = new System.Windows.Forms.ToolStripMenuItem();
            this.adToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.cms_site_file_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_file_rename = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_file_create = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_folder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_site_folder_download = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_folder_download_br = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cms_site_folder_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_site_folder_create = new System.Windows.Forms.ToolStripMenuItem();
            this.tvSiteInfo = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sp1 = new System.Windows.Forms.SplitContainer();
            this.sp_Site = new System.Windows.Forms.SplitContainer();
            this.sp_Local = new System.Windows.Forms.SplitContainer();
            this.tvLocal = new System.Windows.Forms.TreeView();
            this.cms_local_folder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_local_folder_upload = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_local_folder_upload_br = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cms_local_folder_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_local_folder_create = new System.Windows.Forms.ToolStripMenuItem();
            this.dfbLocal = new System.Windows.Forms.DataGridView();
            this.FileNameLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileTypeLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModifyDateTimeLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileFullPathLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsFolderLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IconLocal = new System.Windows.Forms.DataGridViewImageColumn();
            this.cms_local_file = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_local_file_upload = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_local_file_upload_br = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cms_local_file_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_local_file_create = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFullPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dfbServer)).BeginInit();
            this.cms_site_file.SuspendLayout();
            this.cms_site_folder.SuspendLayout();
            this.sp1.Panel1.SuspendLayout();
            this.sp1.Panel2.SuspendLayout();
            this.sp1.SuspendLayout();
            this.sp_Site.Panel1.SuspendLayout();
            this.sp_Site.Panel2.SuspendLayout();
            this.sp_Site.SuspendLayout();
            this.sp_Local.Panel1.SuspendLayout();
            this.sp_Local.Panel2.SuspendLayout();
            this.sp_Local.SuspendLayout();
            this.cms_local_folder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dfbLocal)).BeginInit();
            this.cms_local_file.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP Site";
            // 
            // cboSite
            // 
            this.cboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSite.FormattingEnabled = true;
            this.cboSite.Location = new System.Drawing.Point(66, 11);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(217, 21);
            this.cboSite.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Lime;
            this.btnConnect.Location = new System.Drawing.Point(288, 11);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(66, 21);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.BackColor = System.Drawing.Color.Red;
            this.btnDisConnect.Location = new System.Drawing.Point(359, 11);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(89, 21);
            this.btnDisConnect.TabIndex = 3;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = false;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // dfbServer
            // 
            this.dfbServer.AllowUserToAddRows = false;
            this.dfbServer.AllowUserToDeleteRows = false;
            this.dfbServer.AllowUserToResizeRows = false;
            this.dfbServer.BackgroundColor = System.Drawing.Color.White;
            this.dfbServer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dfbServer.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dfbServer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dfbServer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.FileSize,
            this.FileType,
            this.FileModifyDate,
            this.FileFullPath,
            this.IsFolder,
            this.Iconx});
            this.dfbServer.ContextMenuStrip = this.cms_site_file;
            this.dfbServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dfbServer.GridColor = System.Drawing.Color.White;
            this.dfbServer.Location = new System.Drawing.Point(0, 0);
            this.dfbServer.Name = "dfbServer";
            this.dfbServer.ReadOnly = true;
            this.dfbServer.RowHeadersVisible = false;
            this.dfbServer.RowTemplate.Height = 23;
            this.dfbServer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dfbServer.Size = new System.Drawing.Size(500, 218);
            this.dfbServer.TabIndex = 5;
            this.dfbServer.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dfbServer_CellMouseDoubleClick);
            this.dfbServer.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dfbServer_CellPainting);
            // 
            // FileName
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.FileName.DefaultCellStyle = dataGridViewCellStyle3;
            this.FileName.HeaderText = "FileName";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 180;
            // 
            // FileSize
            // 
            this.FileSize.HeaderText = "Size";
            this.FileSize.Name = "FileSize";
            this.FileSize.ReadOnly = true;
            this.FileSize.Width = 70;
            // 
            // FileType
            // 
            this.FileType.HeaderText = "FileType";
            this.FileType.Name = "FileType";
            this.FileType.ReadOnly = true;
            this.FileType.Width = 130;
            // 
            // FileModifyDate
            // 
            this.FileModifyDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileModifyDate.HeaderText = "ModifyDateTime";
            this.FileModifyDate.Name = "FileModifyDate";
            this.FileModifyDate.ReadOnly = true;
            // 
            // FileFullPath
            // 
            this.FileFullPath.HeaderText = "FileFullPath";
            this.FileFullPath.Name = "FileFullPath";
            this.FileFullPath.ReadOnly = true;
            this.FileFullPath.Visible = false;
            // 
            // IsFolder
            // 
            this.IsFolder.HeaderText = "IsFolder";
            this.IsFolder.Name = "IsFolder";
            this.IsFolder.ReadOnly = true;
            this.IsFolder.Visible = false;
            // 
            // Iconx
            // 
            this.Iconx.HeaderText = "Icon";
            this.Iconx.Name = "Iconx";
            this.Iconx.ReadOnly = true;
            this.Iconx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Iconx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Iconx.Visible = false;
            // 
            // cms_site_file
            // 
            this.cms_site_file.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_site_file_download,
            this.cms_site_file_download_br,
            this.adToolStripMenuItem,
            this.cms_site_file_delete,
            this.cms_site_file_rename,
            this.cms_site_file_create});
            this.cms_site_file.Name = "cms_site_file";
            this.cms_site_file.Size = new System.Drawing.Size(259, 120);
            // 
            // cms_site_file_download
            // 
            this.cms_site_file_download.Name = "cms_site_file_download";
            this.cms_site_file_download.Size = new System.Drawing.Size(258, 22);
            this.cms_site_file_download.Text = "Download";
            this.cms_site_file_download.Click += new System.EventHandler(this.cms_site_file_download_Click);
            // 
            // cms_site_file_download_br
            // 
            this.cms_site_file_download_br.Name = "cms_site_file_download_br";
            this.cms_site_file_download_br.Size = new System.Drawing.Size(258, 22);
            this.cms_site_file_download_br.Text = "Download (breakpoint resume)";
            this.cms_site_file_download_br.Click += new System.EventHandler(this.cms_site_file_download_br_Click);
            // 
            // adToolStripMenuItem
            // 
            this.adToolStripMenuItem.Name = "adToolStripMenuItem";
            this.adToolStripMenuItem.Size = new System.Drawing.Size(255, 6);
            // 
            // cms_site_file_delete
            // 
            this.cms_site_file_delete.Name = "cms_site_file_delete";
            this.cms_site_file_delete.Size = new System.Drawing.Size(258, 22);
            this.cms_site_file_delete.Text = "Delete";
            this.cms_site_file_delete.Click += new System.EventHandler(this.cms_site_file_delete_Click);
            // 
            // cms_site_file_rename
            // 
            this.cms_site_file_rename.Name = "cms_site_file_rename";
            this.cms_site_file_rename.Size = new System.Drawing.Size(258, 22);
            this.cms_site_file_rename.Text = "Rename File";
            this.cms_site_file_rename.Click += new System.EventHandler(this.cms_site_file_rename_Click);
            // 
            // cms_site_file_create
            // 
            this.cms_site_file_create.Name = "cms_site_file_create";
            this.cms_site_file_create.Size = new System.Drawing.Size(258, 22);
            this.cms_site_file_create.Text = "Create Noew Folder";
            this.cms_site_file_create.Click += new System.EventHandler(this.cms_site_file_create_Click);
            // 
            // cms_site_folder
            // 
            this.cms_site_folder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_site_folder_download,
            this.cms_site_folder_download_br,
            this.toolStripMenuItem1,
            this.cms_site_folder_delete,
            this.cms_site_folder_create});
            this.cms_site_folder.Name = "cms_site_folder";
            this.cms_site_folder.Size = new System.Drawing.Size(259, 98);
            // 
            // cms_site_folder_download
            // 
            this.cms_site_folder_download.Name = "cms_site_folder_download";
            this.cms_site_folder_download.Size = new System.Drawing.Size(258, 22);
            this.cms_site_folder_download.Text = "Download";
            this.cms_site_folder_download.Click += new System.EventHandler(this.cms_site_folder_download_Click);
            // 
            // cms_site_folder_download_br
            // 
            this.cms_site_folder_download_br.Name = "cms_site_folder_download_br";
            this.cms_site_folder_download_br.Size = new System.Drawing.Size(258, 22);
            this.cms_site_folder_download_br.Text = "Download (breakpoint resume)";
            this.cms_site_folder_download_br.Click += new System.EventHandler(this.cms_site_folder_download_br_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
            // 
            // cms_site_folder_delete
            // 
            this.cms_site_folder_delete.Name = "cms_site_folder_delete";
            this.cms_site_folder_delete.Size = new System.Drawing.Size(258, 22);
            this.cms_site_folder_delete.Text = "Delete";
            this.cms_site_folder_delete.Click += new System.EventHandler(this.cms_site_folder_delete_Click);
            // 
            // cms_site_folder_create
            // 
            this.cms_site_folder_create.Name = "cms_site_folder_create";
            this.cms_site_folder_create.Size = new System.Drawing.Size(258, 22);
            this.cms_site_folder_create.Text = "Create New Folder";
            this.cms_site_folder_create.Click += new System.EventHandler(this.cms_site_folder_create_Click);
            // 
            // tvSiteInfo
            // 
            this.tvSiteInfo.ContextMenuStrip = this.cms_site_folder;
            this.tvSiteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSiteInfo.Font = new System.Drawing.Font("Verdana", 8F);
            this.tvSiteInfo.HideSelection = false;
            this.tvSiteInfo.ImageIndex = 0;
            this.tvSiteInfo.ImageList = this.imageList1;
            this.tvSiteInfo.Location = new System.Drawing.Point(0, 0);
            this.tvSiteInfo.Name = "tvSiteInfo";
            this.tvSiteInfo.PathSeparator = "/";
            this.tvSiteInfo.SelectedImageIndex = 0;
            this.tvSiteInfo.Size = new System.Drawing.Size(251, 218);
            this.tvSiteInfo.TabIndex = 6;
            this.tvSiteInfo.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvSiteInfo_AfterCollapse);
            this.tvSiteInfo.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSiteInfo_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Site");
            this.imageList1.Images.SetKeyName(1, "folder");
            this.imageList1.Images.SetKeyName(2, "folderopen");
            this.imageList1.Images.SetKeyName(3, "desktop");
            this.imageList1.Images.SetKeyName(4, "mycomputer");
            this.imageList1.Images.SetKeyName(5, "disk");
            this.imageList1.Images.SetKeyName(6, "rom");
            this.imageList1.Images.SetKeyName(7, "usb");
            this.imageList1.Images.SetKeyName(8, "unknowdisk");
            this.imageList1.Images.SetKeyName(9, "network");
            // 
            // sp1
            // 
            this.sp1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sp1.Location = new System.Drawing.Point(4, 38);
            this.sp1.Name = "sp1";
            this.sp1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sp1.Panel1
            // 
            this.sp1.Panel1.Controls.Add(this.sp_Site);
            // 
            // sp1.Panel2
            // 
            this.sp1.Panel2.Controls.Add(this.sp_Local);
            this.sp1.Size = new System.Drawing.Size(755, 437);
            this.sp1.SplitterDistance = 218;
            this.sp1.TabIndex = 8;
            // 
            // sp_Site
            // 
            this.sp_Site.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sp_Site.Location = new System.Drawing.Point(0, 0);
            this.sp_Site.Name = "sp_Site";
            // 
            // sp_Site.Panel1
            // 
            this.sp_Site.Panel1.Controls.Add(this.tvSiteInfo);
            // 
            // sp_Site.Panel2
            // 
            this.sp_Site.Panel2.Controls.Add(this.dfbServer);
            this.sp_Site.Size = new System.Drawing.Size(755, 218);
            this.sp_Site.SplitterDistance = 251;
            this.sp_Site.TabIndex = 0;
            // 
            // sp_Local
            // 
            this.sp_Local.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sp_Local.Location = new System.Drawing.Point(0, 0);
            this.sp_Local.Name = "sp_Local";
            // 
            // sp_Local.Panel1
            // 
            this.sp_Local.Panel1.Controls.Add(this.tvLocal);
            // 
            // sp_Local.Panel2
            // 
            this.sp_Local.Panel2.Controls.Add(this.dfbLocal);
            this.sp_Local.Size = new System.Drawing.Size(755, 215);
            this.sp_Local.SplitterDistance = 251;
            this.sp_Local.TabIndex = 0;
            // 
            // tvLocal
            // 
            this.tvLocal.ContextMenuStrip = this.cms_local_folder;
            this.tvLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLocal.Font = new System.Drawing.Font("Verdana", 8F);
            this.tvLocal.HideSelection = false;
            this.tvLocal.ImageIndex = 0;
            this.tvLocal.ImageList = this.imageList1;
            this.tvLocal.Location = new System.Drawing.Point(0, 0);
            this.tvLocal.Name = "tvLocal";
            this.tvLocal.SelectedImageIndex = 0;
            this.tvLocal.Size = new System.Drawing.Size(251, 215);
            this.tvLocal.TabIndex = 7;
            this.tvLocal.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvLocal_AfterCollapse);
            this.tvLocal.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvLocal_AfterSelect);
            // 
            // cms_local_folder
            // 
            this.cms_local_folder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_local_folder_upload,
            this.cms_local_folder_upload_br,
            this.toolStripMenuItem2,
            this.cms_local_folder_delete,
            this.cms_local_folder_create});
            this.cms_local_folder.Name = "cms_local_folder";
            this.cms_local_folder.Size = new System.Drawing.Size(243, 98);
            // 
            // cms_local_folder_upload
            // 
            this.cms_local_folder_upload.Name = "cms_local_folder_upload";
            this.cms_local_folder_upload.Size = new System.Drawing.Size(242, 22);
            this.cms_local_folder_upload.Text = "Upload";
            this.cms_local_folder_upload.Click += new System.EventHandler(this.cms_local_folder_upload_Click);
            // 
            // cms_local_folder_upload_br
            // 
            this.cms_local_folder_upload_br.Name = "cms_local_folder_upload_br";
            this.cms_local_folder_upload_br.Size = new System.Drawing.Size(242, 22);
            this.cms_local_folder_upload_br.Text = "Upload (breakpoint resume)";
            this.cms_local_folder_upload_br.Click += new System.EventHandler(this.cms_local_folder_upload_br_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(239, 6);
            // 
            // cms_local_folder_delete
            // 
            this.cms_local_folder_delete.Name = "cms_local_folder_delete";
            this.cms_local_folder_delete.Size = new System.Drawing.Size(242, 22);
            this.cms_local_folder_delete.Text = "Delete";
            this.cms_local_folder_delete.Click += new System.EventHandler(this.cms_local_folder_delete_Click);
            // 
            // cms_local_folder_create
            // 
            this.cms_local_folder_create.Name = "cms_local_folder_create";
            this.cms_local_folder_create.Size = new System.Drawing.Size(242, 22);
            this.cms_local_folder_create.Text = "Create New Folder";
            this.cms_local_folder_create.Click += new System.EventHandler(this.cms_local_folder_create_Click);
            // 
            // dfbLocal
            // 
            this.dfbLocal.AllowUserToAddRows = false;
            this.dfbLocal.AllowUserToDeleteRows = false;
            this.dfbLocal.AllowUserToResizeRows = false;
            this.dfbLocal.BackgroundColor = System.Drawing.Color.White;
            this.dfbLocal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dfbLocal.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dfbLocal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dfbLocal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileNameLocal,
            this.SizeLocal,
            this.FileTypeLocal,
            this.ModifyDateTimeLocal,
            this.FileFullPathLocal,
            this.IsFolderLocal,
            this.IconLocal});
            this.dfbLocal.ContextMenuStrip = this.cms_local_file;
            this.dfbLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dfbLocal.GridColor = System.Drawing.Color.White;
            this.dfbLocal.Location = new System.Drawing.Point(0, 0);
            this.dfbLocal.Name = "dfbLocal";
            this.dfbLocal.ReadOnly = true;
            this.dfbLocal.RowHeadersVisible = false;
            this.dfbLocal.RowTemplate.Height = 23;
            this.dfbLocal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dfbLocal.Size = new System.Drawing.Size(500, 215);
            this.dfbLocal.TabIndex = 6;
            this.dfbLocal.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dfbLocal_CellMouseDoubleClick);
            this.dfbLocal.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dfbLocal_CellPainting);
            // 
            // FileNameLocal
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.FileNameLocal.DefaultCellStyle = dataGridViewCellStyle4;
            this.FileNameLocal.HeaderText = "FileName";
            this.FileNameLocal.Name = "FileNameLocal";
            this.FileNameLocal.ReadOnly = true;
            this.FileNameLocal.Width = 180;
            // 
            // SizeLocal
            // 
            this.SizeLocal.HeaderText = "Size";
            this.SizeLocal.Name = "SizeLocal";
            this.SizeLocal.ReadOnly = true;
            this.SizeLocal.Width = 70;
            // 
            // FileTypeLocal
            // 
            this.FileTypeLocal.HeaderText = "FileType";
            this.FileTypeLocal.Name = "FileTypeLocal";
            this.FileTypeLocal.ReadOnly = true;
            this.FileTypeLocal.Width = 130;
            // 
            // ModifyDateTimeLocal
            // 
            this.ModifyDateTimeLocal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModifyDateTimeLocal.HeaderText = "ModifyDateTime";
            this.ModifyDateTimeLocal.Name = "ModifyDateTimeLocal";
            this.ModifyDateTimeLocal.ReadOnly = true;
            // 
            // FileFullPathLocal
            // 
            this.FileFullPathLocal.HeaderText = "FileFullPath";
            this.FileFullPathLocal.Name = "FileFullPathLocal";
            this.FileFullPathLocal.ReadOnly = true;
            this.FileFullPathLocal.Visible = false;
            // 
            // IsFolderLocal
            // 
            this.IsFolderLocal.HeaderText = "IsFolder";
            this.IsFolderLocal.Name = "IsFolderLocal";
            this.IsFolderLocal.ReadOnly = true;
            this.IsFolderLocal.Visible = false;
            // 
            // IconLocal
            // 
            this.IconLocal.HeaderText = "Icon";
            this.IconLocal.Name = "IconLocal";
            this.IconLocal.ReadOnly = true;
            this.IconLocal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IconLocal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IconLocal.Visible = false;
            // 
            // cms_local_file
            // 
            this.cms_local_file.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_local_file_upload,
            this.cms_local_file_upload_br,
            this.toolStripMenuItem3,
            this.cms_local_file_delete,
            this.cms_local_file_create});
            this.cms_local_file.Name = "cms_local_file";
            this.cms_local_file.Size = new System.Drawing.Size(243, 98);
            // 
            // cms_local_file_upload
            // 
            this.cms_local_file_upload.Name = "cms_local_file_upload";
            this.cms_local_file_upload.Size = new System.Drawing.Size(242, 22);
            this.cms_local_file_upload.Text = "Upload";
            this.cms_local_file_upload.Click += new System.EventHandler(this.cms_local_file_upload_Click);
            // 
            // cms_local_file_upload_br
            // 
            this.cms_local_file_upload_br.Name = "cms_local_file_upload_br";
            this.cms_local_file_upload_br.Size = new System.Drawing.Size(242, 22);
            this.cms_local_file_upload_br.Text = "Upload (breakpoint resume)";
            this.cms_local_file_upload_br.Click += new System.EventHandler(this.cms_local_file_upload_br_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(239, 6);
            // 
            // cms_local_file_delete
            // 
            this.cms_local_file_delete.Name = "cms_local_file_delete";
            this.cms_local_file_delete.Size = new System.Drawing.Size(242, 22);
            this.cms_local_file_delete.Text = "Delete";
            this.cms_local_file_delete.Click += new System.EventHandler(this.cms_local_file_delete_Click);
            // 
            // cms_local_file_create
            // 
            this.cms_local_file_create.Name = "cms_local_file_create";
            this.cms_local_file_create.Size = new System.Drawing.Size(242, 22);
            this.cms_local_file_create.Text = "Create New Folder";
            this.cms_local_file_create.Click += new System.EventHandler(this.cms_local_file_create_Click);
            // 
            // txtFullPath
            // 
            this.txtFullPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFullPath.Location = new System.Drawing.Point(454, 11);
            this.txtFullPath.Name = "txtFullPath";
            this.txtFullPath.ReadOnly = true;
            this.txtFullPath.Size = new System.Drawing.Size(305, 20);
            this.txtFullPath.TabIndex = 9;
            // 
            // frmSiteViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 478);
            this.Controls.Add(this.txtFullPath);
            this.Controls.Add(this.sp1);
            this.Controls.Add(this.btnDisConnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cboSite);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmSiteViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP Site Viewer";
            this.Load += new System.EventHandler(this.frmSiteViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dfbServer)).EndInit();
            this.cms_site_file.ResumeLayout(false);
            this.cms_site_folder.ResumeLayout(false);
            this.sp1.Panel1.ResumeLayout(false);
            this.sp1.Panel2.ResumeLayout(false);
            this.sp1.ResumeLayout(false);
            this.sp_Site.Panel1.ResumeLayout(false);
            this.sp_Site.Panel2.ResumeLayout(false);
            this.sp_Site.ResumeLayout(false);
            this.sp_Local.Panel1.ResumeLayout(false);
            this.sp_Local.Panel2.ResumeLayout(false);
            this.sp_Local.ResumeLayout(false);
            this.cms_local_folder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dfbLocal)).EndInit();
            this.cms_local_file.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSite;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.DataGridView dfbServer;
        private System.Windows.Forms.DataGridViewImageColumn Icons;
        private System.Windows.Forms.TreeView tvSiteInfo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileModifyDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileFullPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsFolder;
        private System.Windows.Forms.DataGridViewImageColumn Iconx;
        private System.Windows.Forms.ContextMenuStrip cms_site_folder;
        private System.Windows.Forms.SplitContainer sp1;
        private System.Windows.Forms.SplitContainer sp_Site;
        private System.Windows.Forms.SplitContainer sp_Local;
        private System.Windows.Forms.DataGridView dfbLocal;
        private System.Windows.Forms.TreeView tvLocal;
        private System.Windows.Forms.ContextMenuStrip cms_site_file;
        private System.Windows.Forms.ContextMenuStrip cms_local_folder;
        private System.Windows.Forms.ContextMenuStrip cms_local_file;
        private System.Windows.Forms.ToolStripMenuItem cms_site_folder_download;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cms_site_folder_delete;
        private System.Windows.Forms.ToolStripMenuItem cms_site_file_download;
        private System.Windows.Forms.ToolStripSeparator adToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cms_site_file_delete;
        private System.Windows.Forms.ToolStripMenuItem cms_local_folder_upload;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cms_local_folder_delete;
        private System.Windows.Forms.ToolStripMenuItem cms_local_folder_create;
        private System.Windows.Forms.ToolStripMenuItem cms_local_file_upload;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem cms_local_file_delete;
        private System.Windows.Forms.ToolStripMenuItem cms_local_file_create;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileTypeLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModifyDateTimeLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileFullPathLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsFolderLocal;
        private System.Windows.Forms.DataGridViewImageColumn IconLocal;
        private System.Windows.Forms.ToolStripMenuItem cms_site_folder_create;
        private System.Windows.Forms.ToolStripMenuItem cms_site_file_create;
        private System.Windows.Forms.ToolStripMenuItem cms_site_file_rename;
        private System.Windows.Forms.ToolStripMenuItem cms_site_file_download_br;
        private System.Windows.Forms.ToolStripMenuItem cms_site_folder_download_br;
        private System.Windows.Forms.ToolStripMenuItem cms_local_folder_upload_br;
        private System.Windows.Forms.ToolStripMenuItem cms_local_file_upload_br;
        private System.Windows.Forms.TextBox txtFullPath;
    }
}