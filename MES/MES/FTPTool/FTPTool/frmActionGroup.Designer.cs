namespace FTPTool
{
    partial class frmActionGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionGroup));
            this.lbAction = new System.Windows.Forms.ListBox();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.rbAdd = new System.Windows.Forms.RadioButton();
            this.rbModify = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAction
            // 
            this.lbAction.ContextMenuStrip = this.cms;
            this.lbAction.FormattingEnabled = true;
            this.lbAction.ItemHeight = 14;
            this.lbAction.Location = new System.Drawing.Point(12, 12);
            this.lbAction.Name = "lbAction";
            this.lbAction.Size = new System.Drawing.Size(200, 228);
            this.lbAction.TabIndex = 0;
            this.lbAction.SelectedIndexChanged += new System.EventHandler(this.lbAction_SelectedIndexChanged);
            this.lbAction.Click += new System.EventHandler(this.lbAction_Click);
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_Delete});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(114, 26);
            // 
            // cms_Delete
            // 
            this.cms_Delete.Name = "cms_Delete";
            this.cms_Delete.Size = new System.Drawing.Size(113, 22);
            this.cms_Delete.Text = "Delete";
            this.cms_Delete.Click += new System.EventHandler(this.cms_Delete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Action";
            // 
            // rbAdd
            // 
            this.rbAdd.AutoSize = true;
            this.rbAdd.Checked = true;
            this.rbAdd.Location = new System.Drawing.Point(258, 27);
            this.rbAdd.Name = "rbAdd";
            this.rbAdd.Size = new System.Drawing.Size(49, 18);
            this.rbAdd.TabIndex = 2;
            this.rbAdd.TabStop = true;
            this.rbAdd.Text = "Add";
            this.rbAdd.UseVisualStyleBackColor = true;
            this.rbAdd.CheckedChanged += new System.EventHandler(this.rbAdd_CheckedChanged);
            // 
            // rbModify
            // 
            this.rbModify.AutoSize = true;
            this.rbModify.Location = new System.Drawing.Point(325, 27);
            this.rbModify.Name = "rbModify";
            this.rbModify.Size = new System.Drawing.Size(65, 18);
            this.rbModify.TabIndex = 3;
            this.rbModify.Text = "Modify";
            this.rbModify.UseVisualStyleBackColor = true;
            this.rbModify.CheckedChanged += new System.EventHandler(this.rbModify_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "New";
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(283, 62);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(138, 22);
            this.txtAction.TabIndex = 4;
            // 
            // txt2
            // 
            this.txt2.Location = new System.Drawing.Point(283, 90);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(138, 22);
            this.txt2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(224, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 2);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(337, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "Add";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(337, 181);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(222, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Right click to delete";
            // 
            // frmActionGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 249);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.txtAction);
            this.Controls.Add(this.rbModify);
            this.Controls.Add(this.rbAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAction);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmActionGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action Group";
            this.Load += new System.EventHandler(this.frmActionGroup_Load);
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAdd;
        private System.Windows.Forms.RadioButton rbModify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem cms_Delete;
    }
}