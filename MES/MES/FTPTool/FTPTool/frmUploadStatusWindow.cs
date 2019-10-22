using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FTPTool
{
    public partial class frmUploadStatusWindow : Form
    {
        DB DB = new DB();

        private string localpath;
        private string ftppath;
        private string sitename;

        public delegate void SetProgressStatus(int i, string Content, decimal iProgress);

        public frmUploadStatusWindow(string _site,string _localpath,string _ftppath)
        {
            InitializeComponent();
            localpath = _localpath;
            ftppath = _ftppath;
            sitename = _site;
        }

        private void frmUploadStatusWindow_Load(object sender, EventArgs e)
        {
            lblSource.Text = localpath;
            lblTarget.Text = ftppath;
            lblCurrent.Text = "Waiting...";
            //btnStart_Click(null, null);
            timer1.Enabled = true;
        }

        private void UpdateProgressStatus(int iRow, string Content, decimal iProgress)
        {
            lblCurrent.Text = Content;
            pb.Value = Convert.ToInt32(iProgress * 100);
            Application.DoEvents();
        }

        public void SetMessage(int iRow, string Content, decimal iProgress)
        {
            SetProgressStatus sps = new SetProgressStatus(this.UpdateProgressStatus);
            sps.Invoke(iRow, Content, iProgress);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Upload();
        }

        private void Upload()
        {
            try
            {
                string type,siteip, userid, password, port;
                //连接FTP
                using(DataTable dt=DB.GetSiteProfileDetail(sitename))
                {
                    if(dt.Rows.Count==0)
                    {
                        MessageBox.Show("Can't find ftp setting, please check","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                    type = dt.Rows[0]["Type"].ToString();
                    siteip=dt.Rows[0]["SiteIP"].ToString();
                    userid=dt.Rows[0]["UserID"].ToString();
                    password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                    port = dt.Rows[0]["Port"].ToString();
                    ftppath = ftppath.Substring(sitename.Length, ftppath.Length - sitename.Length);
                    
                }

                ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType("Upload");
                if (Directory.Exists(localpath) == false)
                {
                    //文档
                    OnStepAction.RunAction(this, 1, type,sitename, siteip, userid, password,port, "Upload", ftppath, "Y", localpath, "N");
                }
                else
                {
                    //文件夹
                    OnStepAction.RunAction(this, 1, type,sitename, siteip, userid, password,port, "Upload", ftppath, "Y", localpath, "Y");
                }
            }
            catch(Exception ex ) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }




    }
}
