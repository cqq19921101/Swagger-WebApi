namespace WindowFramework
{
    partial class frmMain
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
            this.mnu = new System.Windows.Forms.MenuStrip();
            this.ssp1 = new System.Windows.Forms.StatusStrip();
            this.ssp1_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.BackColor = System.Drawing.Color.LightBlue;
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Size = new System.Drawing.Size(731, 24);
            this.mnu.TabIndex = 1;
            this.mnu.Text = "menuStrip1";
            // 
            // ssp1
            // 
            this.ssp1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssp1_status});
            this.ssp1.Location = new System.Drawing.Point(0, 446);
            this.ssp1.Name = "ssp1";
            this.ssp1.Size = new System.Drawing.Size(731, 22);
            this.ssp1.TabIndex = 3;
            // 
            // ssp1_status
            // 
            this.ssp1_status.Name = "ssp1_status";
            this.ssp1_status.Size = new System.Drawing.Size(131, 17);
            this.ssp1_status.Text = "toolStripStatusLabel1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 468);
            this.Controls.Add(this.ssp1);
            this.Controls.Add(this.mnu);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnu;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ssp1.ResumeLayout(false);
            this.ssp1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnu;
        private System.Windows.Forms.StatusStrip ssp1;
        private System.Windows.Forms.ToolStripStatusLabel ssp1_status;
    }
}

