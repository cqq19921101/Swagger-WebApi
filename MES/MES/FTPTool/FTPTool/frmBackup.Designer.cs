namespace FTPTool
{
    partial class frmBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackup));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnBkPath = new System.Windows.Forms.Button();
            this.txtBkPath = new System.Windows.Forms.TextBox();
            this.chkSite = new System.Windows.Forms.CheckBox();
            this.chkSch = new System.Windows.Forms.CheckBox();
            this.chkAction = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSiteProfile = new System.Windows.Forms.Button();
            this.txtSiteProfile = new System.Windows.Forms.TextBox();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.txtSchedule = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnAction = new System.Windows.Forms.Button();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(415, 199);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnBackup);
            this.tabPage1.Controls.Add(this.btnBkPath);
            this.tabPage1.Controls.Add(this.txtBkPath);
            this.tabPage1.Controls.Add(this.chkSite);
            this.tabPage1.Controls.Add(this.chkSch);
            this.tabPage1.Controls.Add(this.chkAction);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(407, 172);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Backup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(282, 131);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 5;
            this.btnBackup.Text = "Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnBkPath
            // 
            this.btnBkPath.Location = new System.Drawing.Point(363, 93);
            this.btnBkPath.Name = "btnBkPath";
            this.btnBkPath.Size = new System.Drawing.Size(29, 23);
            this.btnBkPath.TabIndex = 4;
            this.btnBkPath.Text = "...";
            this.btnBkPath.UseVisualStyleBackColor = true;
            this.btnBkPath.Click += new System.EventHandler(this.btnBkPath_Click);
            // 
            // txtBkPath
            // 
            this.txtBkPath.BackColor = System.Drawing.Color.White;
            this.txtBkPath.Location = new System.Drawing.Point(31, 94);
            this.txtBkPath.Name = "txtBkPath";
            this.txtBkPath.ReadOnly = true;
            this.txtBkPath.Size = new System.Drawing.Size(326, 22);
            this.txtBkPath.TabIndex = 3;
            // 
            // chkSite
            // 
            this.chkSite.AutoSize = true;
            this.chkSite.Checked = true;
            this.chkSite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSite.Location = new System.Drawing.Point(31, 46);
            this.chkSite.Name = "chkSite";
            this.chkSite.Size = new System.Drawing.Size(93, 18);
            this.chkSite.TabIndex = 2;
            this.chkSite.Text = "Site Profile";
            this.chkSite.UseVisualStyleBackColor = true;
            // 
            // chkSch
            // 
            this.chkSch.AutoSize = true;
            this.chkSch.Checked = true;
            this.chkSch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSch.Location = new System.Drawing.Point(31, 21);
            this.chkSch.Name = "chkSch";
            this.chkSch.Size = new System.Drawing.Size(83, 18);
            this.chkSch.TabIndex = 1;
            this.chkSch.Text = "Schedule";
            this.chkSch.UseVisualStyleBackColor = true;
            // 
            // chkAction
            // 
            this.chkAction.AutoSize = true;
            this.chkAction.Location = new System.Drawing.Point(282, 21);
            this.chkAction.Name = "chkAction";
            this.chkAction.Size = new System.Drawing.Size(64, 18);
            this.chkAction.TabIndex = 0;
            this.chkAction.Text = "Action";
            this.chkAction.UseVisualStyleBackColor = true;
            this.chkAction.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSiteProfile);
            this.tabPage2.Controls.Add(this.txtSiteProfile);
            this.tabPage2.Controls.Add(this.btnSchedule);
            this.tabPage2.Controls.Add(this.txtSchedule);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btnRestore);
            this.tabPage2.Controls.Add(this.btnAction);
            this.tabPage2.Controls.Add(this.txtAction);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(407, 172);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Restore";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSiteProfile
            // 
            this.btnSiteProfile.Location = new System.Drawing.Point(352, 51);
            this.btnSiteProfile.Name = "btnSiteProfile";
            this.btnSiteProfile.Size = new System.Drawing.Size(29, 23);
            this.btnSiteProfile.TabIndex = 18;
            this.btnSiteProfile.Text = "...";
            this.btnSiteProfile.UseVisualStyleBackColor = true;
            this.btnSiteProfile.Click += new System.EventHandler(this.btnSiteProfile_Click);
            // 
            // txtSiteProfile
            // 
            this.txtSiteProfile.Location = new System.Drawing.Point(96, 52);
            this.txtSiteProfile.Name = "txtSiteProfile";
            this.txtSiteProfile.Size = new System.Drawing.Size(250, 22);
            this.txtSiteProfile.TabIndex = 17;
            // 
            // btnSchedule
            // 
            this.btnSchedule.Location = new System.Drawing.Point(352, 26);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(29, 23);
            this.btnSchedule.TabIndex = 16;
            this.btnSchedule.Text = "...";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // txtSchedule
            // 
            this.txtSchedule.Location = new System.Drawing.Point(96, 27);
            this.txtSchedule.Name = "txtSchedule";
            this.txtSchedule.Size = new System.Drawing.Size(250, 22);
            this.txtSchedule.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 14);
            this.label3.TabIndex = 14;
            this.label3.Text = "Site Profile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "Schedule";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "Action";
            this.label1.Visible = false;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(282, 131);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 11;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(352, 79);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(29, 23);
            this.btnAction.TabIndex = 10;
            this.btnAction.Text = "...";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Visible = false;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(96, 80);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(250, 22);
            this.txtAction.TabIndex = 9;
            this.txtAction.Visible = false;
            // 
            // ofd
            // 
            this.ofd.Filter = "Xml file|*.xml";
            // 
            // frmBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 197);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup and restore";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtBkPath;
        private System.Windows.Forms.CheckBox chkSite;
        private System.Windows.Forms.CheckBox chkAction;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnBkPath;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.CheckBox chkSch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSiteProfile;
        private System.Windows.Forms.TextBox txtSiteProfile;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.TextBox txtSchedule;

    }
}