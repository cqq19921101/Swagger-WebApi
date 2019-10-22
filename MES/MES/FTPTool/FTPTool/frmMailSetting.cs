using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmMailSetting : Form
    {
        DB DB = new DB();

        public frmMailSetting()
        {
            InitializeComponent();
        }

        private void frmMailSetting_Load(object sender, EventArgs e)
        {
            try
            {
                using (DataTable dt = DB.GetMailAccountSetting())
                {
                    if (dt.Rows.Count > 0)
                    {
                        txtFrom.Text = dt.Rows[0]["MailFrom"].ToString();
                        txtSMTP.Text = dt.Rows[0]["SMTP"].ToString();
                        txtAccount.Text = dt.Rows[0]["Account"].ToString();
                        txtPassword.Text = dt.Rows[0]["Password"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DB.SaveMailSetting(txtFrom.Text.Trim(), txtSMTP.Text.Trim(), txtAccount.Text.Trim(), txtPassword.Text.Trim());
                Para.MAIL_FROM = txtFrom.Text.Trim();
                Para.MAIL_SMTP = txtSMTP.Text.Trim();
                Para.MAIL_ACCOUNT = txtAccount.Text.Trim();
                Para.MAIL_PWD = txtPassword.Text.Trim();
                MessageBox.Show("Save the mail setting finished", "Mail", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtReceiver.Text.Trim() == "")
            {
                MessageBox.Show("Please key in the receiver", "Mail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtReceiver.Focus();
                return;
            }
            try
            {
                Tool.SendMail(txtReceiver.Text.Trim(), "FTP Tool Mail Test", "This is a test from FTP tool", "");
                MessageBox.Show("OK", "Mail", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
