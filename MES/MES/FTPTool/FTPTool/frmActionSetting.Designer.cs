namespace FTPTool
{
    partial class frmActionSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionSetting));
            this.label1 = new System.Windows.Forms.Label();
            this.cboAction = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemote = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRemote = new System.Windows.Forms.CheckBox();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.chkLocal = new System.Windows.Forms.CheckBox();
            this.btnBfile = new System.Windows.Forms.Button();
            this.btnBfolder = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.lkTips = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLocalFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Action";
            // 
            // cboAction
            // 
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.FormattingEnabled = true;
            this.cboAction.Location = new System.Drawing.Point(12, 27);
            this.cboAction.Name = "cboAction";
            this.cboAction.Size = new System.Drawing.Size(251, 22);
            this.cboAction.TabIndex = 1;
            this.cboAction.SelectedIndexChanged += new System.EventHandler(this.cboAction_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Remote file or folder";
            // 
            // txtRemote
            // 
            this.txtRemote.Location = new System.Drawing.Point(12, 103);
            this.txtRemote.Name = "txtRemote";
            this.txtRemote.Size = new System.Drawing.Size(348, 22);
            this.txtRemote.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Local file or folder";
            // 
            // chkRemote
            // 
            this.chkRemote.AutoSize = true;
            this.chkRemote.Location = new System.Drawing.Point(12, 131);
            this.chkRemote.Name = "chkRemote";
            this.chkRemote.Size = new System.Drawing.Size(140, 18);
            this.chkRemote.TabIndex = 5;
            this.chkRemote.Text = "Remote is a folder";
            this.chkRemote.UseVisualStyleBackColor = true;
            this.chkRemote.CheckedChanged += new System.EventHandler(this.chkRemote_CheckedChanged);
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(12, 175);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(348, 22);
            this.txtLocal.TabIndex = 6;
            // 
            // chkLocal
            // 
            this.chkLocal.AutoSize = true;
            this.chkLocal.Location = new System.Drawing.Point(12, 203);
            this.chkLocal.Name = "chkLocal";
            this.chkLocal.Size = new System.Drawing.Size(124, 18);
            this.chkLocal.TabIndex = 7;
            this.chkLocal.Text = "Local is a folder";
            this.chkLocal.UseVisualStyleBackColor = true;
            this.chkLocal.CheckedChanged += new System.EventHandler(this.chkLocal_CheckedChanged);
            // 
            // btnBfile
            // 
            this.btnBfile.Location = new System.Drawing.Point(157, 203);
            this.btnBfile.Name = "btnBfile";
            this.btnBfile.Size = new System.Drawing.Size(89, 23);
            this.btnBfile.TabIndex = 8;
            this.btnBfile.Text = "Browse File";
            this.btnBfile.UseVisualStyleBackColor = true;
            this.btnBfile.Click += new System.EventHandler(this.btnBfile_Click);
            // 
            // btnBfolder
            // 
            this.btnBfolder.Location = new System.Drawing.Point(252, 203);
            this.btnBfolder.Name = "btnBfolder";
            this.btnBfolder.Size = new System.Drawing.Size(108, 23);
            this.btnBfolder.TabIndex = 9;
            this.btnBfolder.Text = "Browse Folder";
            this.btnBfolder.UseVisualStyleBackColor = true;
            this.btnBfolder.Click += new System.EventHandler(this.btnBfolder_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(285, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(285, 44);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fbd
            // 
            this.fbd.ShowNewFolderButton = false;
            // 
            // lkTips
            // 
            this.lkTips.AutoSize = true;
            this.lkTips.Location = new System.Drawing.Point(231, 9);
            this.lkTips.Name = "lkTips";
            this.lkTips.Size = new System.Drawing.Size(32, 14);
            this.lkTips.TabIndex = 12;
            this.lkTips.TabStop = true;
            this.lkTips.Text = "Tips";
            this.lkTips.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkTips_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 14);
            this.label4.TabIndex = 13;
            this.label4.Text = "FTP starts with /, SFTP starts with blank";
            // 
            // btnLocalFolder
            // 
            this.btnLocalFolder.Location = new System.Drawing.Point(252, 131);
            this.btnLocalFolder.Name = "btnLocalFolder";
            this.btnLocalFolder.Size = new System.Drawing.Size(108, 23);
            this.btnLocalFolder.TabIndex = 14;
            this.btnLocalFolder.Text = "Browse Folder";
            this.btnLocalFolder.UseVisualStyleBackColor = true;
            this.btnLocalFolder.Click += new System.EventHandler(this.btnLocalFolder_Click);
            // 
            // frmActionSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 245);
            this.ControlBox = false;
            this.Controls.Add(this.btnLocalFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lkTips);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBfolder);
            this.Controls.Add(this.btnBfile);
            this.Controls.Add(this.chkLocal);
            this.Controls.Add(this.txtLocal);
            this.Controls.Add(this.chkRemote);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRemote);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboAction);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmActionSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action Setting";
            this.Load += new System.EventHandler(this.frmActionSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboAction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRemote;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRemote;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.CheckBox chkLocal;
        private System.Windows.Forms.Button btnBfile;
        private System.Windows.Forms.Button btnBfolder;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.LinkLabel lkTips;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLocalFolder;
    }
}