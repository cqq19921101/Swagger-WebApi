using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmInput : Form
    {
        public frmInput()
        {
            InitializeComponent();
        }

        private string result = "";
        private bool isCancel = false;

        private void btnOK_Click(object sender, EventArgs e)
        {
            result=txtInput.Text.Trim().ToString();
            isCancel = false;
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = "";
            isCancel = true;
            this.Dispose();
        }

        public string Result
        {
            get { return result; }
        }

        public bool IsCancel
        {
            get { return isCancel; }
        }


    }
}
