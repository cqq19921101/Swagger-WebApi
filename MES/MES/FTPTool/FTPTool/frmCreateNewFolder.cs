using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmCreateNewFolder : Form
    {
        private bool isCancal = false;
        private string folder = "";

        public frmCreateNewFolder()
        {
            InitializeComponent();
        }

        private void frmCreateNewFolder_Load(object sender, EventArgs e)
        {

        }

        public string FolderName
        {
            set { folder = value; }
            get { return folder; }
        }

        public bool IsCancel
        {
            set { isCancal = value; }
            get { return isCancal; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtfolder.Text.Trim() == "")
            {
                MessageBox.Show("Please key in a new folder name", "New folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FolderName = txtfolder.Text.Trim();
            isCancal = false;
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Dispose();
        }
    }
}
