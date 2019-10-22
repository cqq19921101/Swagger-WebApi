namespace FTPTool
{
    partial class frmSiteProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiteProfile));
            this.tvFTPSite = new System.Windows.Forms.TreeView();
            this.cmdDel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdDel_DelSite = new System.Windows.Forms.ToolStripMenuItem();
            this.ilSites = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSite = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkRename = new System.Windows.Forms.CheckBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.cmdDel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvFTPSite
            // 
            this.tvFTPSite.ContextMenuStrip = this.cmdDel;
            this.tvFTPSite.ImageIndex = 1;
            this.tvFTPSite.ImageList = this.ilSites;
            this.tvFTPSite.Location = new System.Drawing.Point(13, 26);
            this.tvFTPSite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tvFTPSite.Name = "tvFTPSite";
            this.tvFTPSite.SelectedImageIndex = 0;
            this.tvFTPSite.Size = new System.Drawing.Size(188, 344);
            this.tvFTPSite.TabIndex = 0;
            this.tvFTPSite.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFTPSite_AfterSelect);
            // 
            // cmdDel
            // 
            this.cmdDel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdDel_DelSite});
            this.cmdDel.Name = "cmdDel";
            this.cmdDel.Size = new System.Drawing.Size(162, 26);
            // 
            // cmdDel_DelSite
            // 
            this.cmdDel_DelSite.Name = "cmdDel_DelSite";
            this.cmdDel_DelSite.Size = new System.Drawing.Size(161, 22);
            this.cmdDel_DelSite.Text = "Delete this site";
            this.cmdDel_DelSite.Click += new System.EventHandler(this.cmdDel_DelSite_Click);
            // 
            // ilSites
            // 
            this.ilSites.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSites.ImageStream")));
            this.ilSites.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSites.Images.SetKeyName(0, "Client");
            this.ilSites.Images.SetKeyName(1, "Site");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "FTP Site Profiles";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(211, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(211, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "FTP Site";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(211, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "UserID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(211, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 14);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(214, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(217, 22);
            this.txtName.TabIndex = 6;
            // 
            // txtSite
            // 
            this.txtSite.Location = new System.Drawing.Point(211, 122);
            this.txtSite.Name = "txtSite";
            this.txtSite.Size = new System.Drawing.Size(220, 22);
            this.txtSite.TabIndex = 7;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(211, 164);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(220, 22);
            this.txtUserID.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(211, 206);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.ShortcutsEnabled = false;
            this.txtPassword.Size = new System.Drawing.Size(220, 22);
            this.txtPassword.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(275, 370);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(356, 370);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkRename
            // 
            this.chkRename.AutoSize = true;
            this.chkRename.ForeColor = System.Drawing.Color.Blue;
            this.chkRename.Location = new System.Drawing.Point(214, 235);
            this.chkRename.Name = "chkRename";
            this.chkRename.Size = new System.Drawing.Size(162, 18);
            this.chkRename.TabIndex = 12;
            this.chkRename.Text = "Use temp name while";
            this.chkRename.UseVisualStyleBackColor = true;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(211, 334);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ShortcutsEnabled = false;
            this.txtDesc.Size = new System.Drawing.Size(220, 30);
            this.txtDesc.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(211, 319);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "Site Description";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(230, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(182, 14);
            this.label7.TabIndex = 10;
            this.label7.Text = "uploading and downloading";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(211, 276);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 14);
            this.label8.TabIndex = 16;
            this.label8.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(211, 293);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(220, 22);
            this.txtPort.TabIndex = 11;
            this.txtPort.Text = "21";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(211, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 14);
            this.label9.TabIndex = 17;
            this.label9.Text = "Type";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "FTP",
            "SFTP",
            "SSH"});
            this.cboType.Location = new System.Drawing.Point(211, 38);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(220, 22);
            this.cboType.TabIndex = 18;
            // 
            // frmSiteProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 410);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkRename);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtSite);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tvFTPSite);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSiteProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP Site Profile";
            this.Load += new System.EventHandler(this.frmSiteProfile_Load);
            this.cmdDel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvFTPSite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList ilSites;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSite;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip cmdDel;
        private System.Windows.Forms.ToolStripMenuItem cmdDel_DelSite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox chkRename;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboType;
    }
}