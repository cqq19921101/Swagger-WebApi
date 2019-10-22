namespace FTPTool
{
    partial class frmFullUploadProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFullUploadProgress));
            this.lblFinished = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.t_prepare = new System.Windows.Forms.Timer(this.components);
            this.lblTotal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblFtpPath = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.lblSite = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FtpFullFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModifyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFinished
            // 
            this.lblFinished.AutoSize = true;
            this.lblFinished.Location = new System.Drawing.Point(673, 23);
            this.lblFinished.Name = "lblFinished";
            this.lblFinished.Size = new System.Drawing.Size(14, 13);
            this.lblFinished.TabIndex = 36;
            this.lblFinished.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(606, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Finished:";
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(535, 23);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(14, 13);
            this.lblSelected.TabIndex = 34;
            this.lblSelected.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(468, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Selected:";
            // 
            // t_prepare
            // 
            this.t_prepare.Enabled = true;
            this.t_prepare.Interval = 1000;
            this.t_prepare.Tick += new System.EventHandler(this.t_prepare_Tick);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(391, 23);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(14, 13);
            this.lblTotal.TabIndex = 32;
            this.lblTotal.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(345, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Total:";
            // 
            // lblFtpPath
            // 
            this.lblFtpPath.AutoSize = true;
            this.lblFtpPath.Location = new System.Drawing.Point(405, 9);
            this.lblFtpPath.Name = "lblFtpPath";
            this.lblFtpPath.Size = new System.Drawing.Size(49, 13);
            this.lblFtpPath.TabIndex = 30;
            this.lblFtpPath.Text = "FtpPath";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(345, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "FtpPath:";
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.Location = new System.Drawing.Point(752, 357);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(79, 23);
            this.btnUpload.TabIndex = 28;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Location = new System.Drawing.Point(690, 357);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(56, 23);
            this.btnPause.TabIndex = 27;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbort.Location = new System.Drawing.Point(97, 357);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(100, 23);
            this.btnAbort.TabIndex = 26;
            this.btnAbort.Text = "Abort Selected";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // chkAll
            // 
            this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(12, 361);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(79, 17);
            this.chkAll.TabIndex = 25;
            this.chkAll.Text = "Select All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(68, 9);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(77, 13);
            this.lblSite.TabIndex = 24;
            this.lblSite.Text = "Current Site";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(14, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Site:";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvList.BackgroundColor = System.Drawing.Color.White;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.Selected,
            this.LP,
            this.FileName,
            this.Size,
            this.Status,
            this.FileFullName,
            this.FtpFullFileName,
            this.Type,
            this.ModifyDate});
            this.dgvList.Location = new System.Drawing.Point(12, 49);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.RowTemplate.Height = 23;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvList.Size = new System.Drawing.Size(819, 302);
            this.dgvList.TabIndex = 22;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(823, 2);
            this.label2.TabIndex = 21;
            this.label2.Text = "label2";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(68, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(69, 13);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Status Msg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Status:";
            // 
            // Image
            // 
            this.Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Image.HeaderText = "";
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Width = 5;
            // 
            // Selected
            // 
            this.Selected.HeaderText = "";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Width = 20;
            // 
            // LP
            // 
            this.LP.HeaderText = "LocalPath";
            this.LP.Name = "LP";
            this.LP.ReadOnly = true;
            this.LP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LP.Width = 200;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "FileName";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileName.Width = 150;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            this.Size.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Size.Width = 80;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Status.Width = 49;
            // 
            // FileFullName
            // 
            this.FileFullName.HeaderText = "FileFullName";
            this.FileFullName.Name = "FileFullName";
            this.FileFullName.ReadOnly = true;
            this.FileFullName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileFullName.Visible = false;
            // 
            // FtpFullFileName
            // 
            this.FtpFullFileName.HeaderText = "FtpFullFileName";
            this.FtpFullFileName.Name = "FtpFullFileName";
            this.FtpFullFileName.ReadOnly = true;
            this.FtpFullFileName.Visible = false;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Visible = false;
            // 
            // ModifyDate
            // 
            this.ModifyDate.HeaderText = "ModifyDate";
            this.ModifyDate.Name = "ModifyDate";
            this.ModifyDate.ReadOnly = true;
            this.ModifyDate.Visible = false;
            // 
            // frmFullUploadProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(843, 392);
            this.Controls.Add(this.lblFinished);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblFtpPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.lblSite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFullUploadProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload";
            this.Load += new System.EventHandler(this.frmFullUploadProgress_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFinished;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer t_prepare;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblFtpPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn LP;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FtpFullFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModifyDate;
    }
}