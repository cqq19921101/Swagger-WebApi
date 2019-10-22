using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmRenamefile : Form
    {
        private bool isCancal = false;
        private string filename = "";

        public frmRenamefile()
        {
            InitializeComponent();
        }

        public string Filename
        {
            set { filename = value; }
            get { return filename; }
        }

        public bool IsCancel
        {
            set { isCancal = value; }
            get { return isCancal; }
        }

        private void frmRenamefile_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim() == "")
            {
                MessageBox.Show("Please key in a new file name", "Rename file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Filename = txtname.Text.Trim();
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
