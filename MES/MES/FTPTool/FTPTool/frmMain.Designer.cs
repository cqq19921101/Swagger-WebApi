namespace FTPTool
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mnu = new System.Windows.Forms.MenuStrip();
            this.mnu_SiteProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_SiteViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_ActionGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_BK = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Mail = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSch = new System.Windows.Forms.DataGridView();
            this.ScheduleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SiteProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastRunTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextRunTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Repeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuccessfulMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_Modify = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_SetStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_Run = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_AddNewSch = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_CopySch = new System.Windows.Forms.ToolStripMenuItem();
            this.t_monitor = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms_notfiy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_nofity_smw = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSch)).BeginInit();
            this.cms.SuspendLayout();
            this.cms_notfiy.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_SiteProfile,
            this.mnu_SiteViewer,
            this.mnu_ActionGroup,
            this.mnu_BK,
            this.mnu_Mail,
            this.mnu_Exit,
            this.testToolStripMenuItem});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mnu.Size = new System.Drawing.Size(768, 25);
            this.mnu.TabIndex = 0;
            this.mnu.Text = "menuStrip1";
            // 
            // mnu_SiteProfile
            // 
            this.mnu_SiteProfile.Name = "mnu_SiteProfile";
            this.mnu_SiteProfile.Size = new System.Drawing.Size(82, 21);
            this.mnu_SiteProfile.Text = "Site Profile";
            this.mnu_SiteProfile.Click += new System.EventHandler(this.mnu_SiteProfile_Click);
            // 
            // mnu_SiteViewer
            // 
            this.mnu_SiteViewer.Name = "mnu_SiteViewer";
            this.mnu_SiteViewer.Size = new System.Drawing.Size(84, 21);
            this.mnu_SiteViewer.Text = "Site Viewer";
            this.mnu_SiteViewer.Click += new System.EventHandler(this.mnu_SiteViewer_Click);
            // 
            // mnu_ActionGroup
            // 
            this.mnu_ActionGroup.Name = "mnu_ActionGroup";
            this.mnu_ActionGroup.Size = new System.Drawing.Size(97, 21);
            this.mnu_ActionGroup.Text = "Action Group";
            this.mnu_ActionGroup.Click += new System.EventHandler(this.mnu_ActionGroup_Click);
            // 
            // mnu_BK
            // 
            this.mnu_BK.Name = "mnu_BK";
            this.mnu_BK.Size = new System.Drawing.Size(157, 21);
            this.mnu_BK.Text = "Backup/Restore Setting";
            this.mnu_BK.Click += new System.EventHandler(this.mnu_BK_Click);
            // 
            // mnu_Mail
            // 
            this.mnu_Mail.Name = "mnu_Mail";
            this.mnu_Mail.Size = new System.Drawing.Size(135, 21);
            this.mnu_Mail.Text = "MailAccount Setting";
            this.mnu_Mail.Click += new System.EventHandler(this.mnu_Mail_Click);
            // 
            // mnu_Exit
            // 
            this.mnu_Exit.Name = "mnu_Exit";
            this.mnu_Exit.Size = new System.Drawing.Size(40, 21);
            this.mnu_Exit.Text = "Exit";
            this.mnu_Exit.Click += new System.EventHandler(this.mnu_Exit_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(41, 21);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Visible = false;
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(66, 137);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(150, 137);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cName,
            this.Progress});
            this.dgv.Location = new System.Drawing.Point(66, 166);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(566, 41);
            this.dgv.TabIndex = 3;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            // 
            // Progress
            // 
            this.Progress.HeaderText = "Progress";
            this.Progress.Name = "Progress";
            this.Progress.ReadOnly = true;
            // 
            // dgvSch
            // 
            this.dgvSch.AllowUserToAddRows = false;
            this.dgvSch.AllowUserToDeleteRows = false;
            this.dgvSch.AllowUserToResizeRows = false;
            this.dgvSch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScheduleName,
            this.SiteProfile,
            this.StartTime,
            this.LastRunTime,
            this.Cost,
            this.NextRunTime,
            this.Repeat,
            this.Status,
            this.ProgressValue,
            this.Mail,
            this.SuccessfulMail});
            this.dgvSch.ContextMenuStrip = this.cms;
            this.dgvSch.Location = new System.Drawing.Point(13, 28);
            this.dgvSch.Name = "dgvSch";
            this.dgvSch.ReadOnly = true;
            this.dgvSch.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Verdana", 8F);
            this.dgvSch.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSch.RowTemplate.Height = 23;
            this.dgvSch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSch.Size = new System.Drawing.Size(743, 417);
            this.dgvSch.TabIndex = 4;
            this.dgvSch.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvSch_CellPainting);
            this.dgvSch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvSch_MouseClick);
            // 
            // ScheduleName
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ScheduleName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ScheduleName.HeaderText = "Schedule Name";
            this.ScheduleName.Name = "ScheduleName";
            this.ScheduleName.ReadOnly = true;
            this.ScheduleName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ScheduleName.Width = 150;
            // 
            // SiteProfile
            // 
            this.SiteProfile.HeaderText = "SiteProfile";
            this.SiteProfile.Name = "SiteProfile";
            this.SiteProfile.ReadOnly = true;
            this.SiteProfile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StartTime
            // 
            this.StartTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StartTime.HeaderText = "StartTime";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartTime.Width = 73;
            // 
            // LastRunTime
            // 
            this.LastRunTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LastRunTime.HeaderText = "LastRunTime";
            this.LastRunTime.Name = "LastRunTime";
            this.LastRunTime.ReadOnly = true;
            this.LastRunTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LastRunTime.Width = 93;
            // 
            // Cost
            // 
            this.Cost.HeaderText = "Cost(Sec)";
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            this.Cost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost.Width = 75;
            // 
            // NextRunTime
            // 
            this.NextRunTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NextRunTime.HeaderText = "NextScheduleTime";
            this.NextRunTime.Name = "NextRunTime";
            this.NextRunTime.ReadOnly = true;
            this.NextRunTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NextRunTime.Width = 127;
            // 
            // Repeat
            // 
            this.Repeat.HeaderText = "Repeat(Min)";
            this.Repeat.Name = "Repeat";
            this.Repeat.ReadOnly = true;
            this.Repeat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Repeat.Width = 87;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProgressValue
            // 
            this.ProgressValue.HeaderText = "ProgressValue";
            this.ProgressValue.Name = "ProgressValue";
            this.ProgressValue.ReadOnly = true;
            this.ProgressValue.Visible = false;
            // 
            // Mail
            // 
            this.Mail.HeaderText = "Mail";
            this.Mail.Name = "Mail";
            this.Mail.ReadOnly = true;
            this.Mail.Visible = false;
            // 
            // SuccessfulMail
            // 
            this.SuccessfulMail.HeaderText = "SuccessfulMail";
            this.SuccessfulMail.Name = "SuccessfulMail";
            this.SuccessfulMail.ReadOnly = true;
            this.SuccessfulMail.Visible = false;
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_Modify,
            this.cms_SetStatus,
            this.cms_Delete,
            this.cms_Run,
            this.cms_AddNewSch,
            this.cms_CopySch});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(190, 136);
            this.cms.Opening += new System.ComponentModel.CancelEventHandler(this.cms_Opening);
            // 
            // cms_Modify
            // 
            this.cms_Modify.Name = "cms_Modify";
            this.cms_Modify.Size = new System.Drawing.Size(189, 22);
            this.cms_Modify.Text = "Modify";
            this.cms_Modify.Click += new System.EventHandler(this.cms_Modify_Click);
            // 
            // cms_SetStatus
            // 
            this.cms_SetStatus.Name = "cms_SetStatus";
            this.cms_SetStatus.Size = new System.Drawing.Size(189, 22);
            this.cms_SetStatus.Text = "Set Status";
            this.cms_SetStatus.Click += new System.EventHandler(this.cms_SetStatus_Click);
            // 
            // cms_Delete
            // 
            this.cms_Delete.Name = "cms_Delete";
            this.cms_Delete.Size = new System.Drawing.Size(189, 22);
            this.cms_Delete.Text = "Delete";
            this.cms_Delete.Click += new System.EventHandler(this.cms_Delete_Click);
            // 
            // cms_Run
            // 
            this.cms_Run.Name = "cms_Run";
            this.cms_Run.Size = new System.Drawing.Size(189, 22);
            this.cms_Run.Text = "Run Immediately";
            this.cms_Run.Click += new System.EventHandler(this.cms_Run_Click);
            // 
            // cms_AddNewSch
            // 
            this.cms_AddNewSch.Name = "cms_AddNewSch";
            this.cms_AddNewSch.Size = new System.Drawing.Size(189, 22);
            this.cms_AddNewSch.Text = "Add New Schedule";
            this.cms_AddNewSch.Click += new System.EventHandler(this.cms_AddNewSch_Click);
            // 
            // cms_CopySch
            // 
            this.cms_CopySch.Name = "cms_CopySch";
            this.cms_CopySch.Size = new System.Drawing.Size(189, 22);
            this.cms_CopySch.Text = "Copy This Schedule";
            this.cms_CopySch.Click += new System.EventHandler(this.cms_CopySch_Click);
            // 
            // t_monitor
            // 
            this.t_monitor.Enabled = true;
            this.t_monitor.Tick += new System.EventHandler(this.t_monitor_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.cms_notfiy;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FTP Tool";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // cms_notfiy
            // 
            this.cms_notfiy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_nofity_smw});
            this.cms_notfiy.Name = "cms_notfiy";
            this.cms_notfiy.Size = new System.Drawing.Size(188, 26);
            // 
            // cms_nofity_smw
            // 
            this.cms_nofity_smw.Name = "cms_nofity_smw";
            this.cms_nofity_smw.Size = new System.Drawing.Size(187, 22);
            this.cms_nofity_smw.Text = "Show main window";
            this.cms_nofity_smw.Click += new System.EventHandler(this.cms_nofity_smw_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 457);
            this.Controls.Add(this.dgvSch);
            this.Controls.Add(this.mnu);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgv);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnu;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSch)).EndInit();
            this.cms.ResumeLayout(false);
            this.cms_notfiy.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnu;
        private System.Windows.Forms.ToolStripMenuItem mnu_SiteProfile;
        private System.Windows.Forms.ToolStripMenuItem mnu_SiteViewer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Progress;
        private System.Windows.Forms.DataGridView dgvSch;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem cms_Modify;
        private System.Windows.Forms.ToolStripMenuItem cms_AddNewSch;
        private System.Windows.Forms.ToolStripMenuItem cms_Delete;
        private System.Windows.Forms.ToolStripMenuItem cms_SetStatus;
        private System.Windows.Forms.ToolStripMenuItem mnu_BK;
        private System.Windows.Forms.ToolStripMenuItem mnu_Exit;
        private System.Windows.Forms.ToolStripMenuItem mnu_ActionGroup;
        private System.Windows.Forms.ToolStripMenuItem cms_Run;
        private System.Windows.Forms.ToolStripMenuItem mnu_Mail;
        private System.Windows.Forms.Timer t_monitor;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip cms_notfiy;
        private System.Windows.Forms.ToolStripMenuItem cms_nofity_smw;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SiteProfile;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastRunTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextRunTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Repeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgressValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mail;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuccessfulMail;
        private System.Windows.Forms.ToolStripMenuItem cms_CopySch;
    }
}

