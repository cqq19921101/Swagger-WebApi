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
    public partial class frmDownloadStatusWindow : Form
    {
        DB DB = new DB();

        private string localpath;
        private string ftppath;
        private string sitename;
        private string isfolder;

        public delegate void setPb(decimal iProgress);
        public delegate void setCX(string Content);
        public delegate void SetProgressStatus(int i, string Content, decimal iProgress);

        public frmDownloadStatusWindow(string _site,string _localpath,string _ftppath,string _isfolder)
        {
            InitializeComponent();
            localpath = _localpath;
            ftppath = _ftppath;
            sitename = _site;
            isfolder = _isfolder;
        }

        private void frmDownloadStatusWindow_Load(object sender, EventArgs e)
        {
            lblSource.Text = ftppath;
            lblTarget.Text = localpath;
            lblCurrent.Text = "Waiting...";
            //btnStart_Click(null, null);
            timer1.Enabled = true;
        }

        private void UpdateProgressStatus(int iRow, string Content, decimal iProgress)
        {
            try
            {
                lock (pb)
                {
                    pb.Value = Convert.ToInt32(iProgress * 100);
                }
                lock (lblCurrent)
                {
                    lblCurrent.Text = Content;
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            { }
        }

        private void SetPB(decimal progress)
        {
            pb.Value = Convert.ToInt32(progress * 100);
        }

        private void SetContent(string str)
        {
            lblCurrent.Text = str;
        }

        public void SetMessage(int iRow, string Content, decimal iProgress)
        {
            //SetProgressStatus sps = new SetProgressStatus(this.UpdateProgressStatus);
            //sps.Invoke(iRow, Content, iProgress);
            setPb pb1=new setPb(SetPB);
            pb.BeginInvoke(pb1, new object[] { iProgress });
            setCX cx=new setCX(SetContent);
            lblCurrent.BeginInvoke(cx, new object[] { Content });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled=false;
            Download();
        }

        private void Download()
        {
            try
            {
                string type, siteip, userid, password, renamefile,port;
                //连接FTP
                using (DataTable dt = DB.GetSiteProfileDetail(sitename))
                {
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Can't find ftp setting, please check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    type = dt.Rows[0]["Type"].ToString();
                    siteip = dt.Rows[0]["SiteIP"].ToString();
                    userid = dt.Rows[0]["UserID"].ToString();
                    password = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                    renamefile = dt.Rows[0]["RenameFile"].ToString();
                    ftppath = ftppath.Substring(sitename.Length, ftppath.Length - sitename.Length);
                    port = dt.Rows[0]["Port"].ToString();
                }

                ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType("Download");
                if (isfolder=="N")
                {
                    //文档
                    OnStepAction.RunAction(this, 1,type, sitename, siteip, userid, password,port, renamefile, "Override", "Download", ftppath, "N", localpath, "N");
                    //OnStepAction.RunAction(this, 1, sitename, siteip, userid, password, "Download", ftppath, "N", localpath, "N");
                }
                else
                {
                    //文件夹
                    OnStepAction.RunAction(this, 1, type,sitename, siteip, userid, password,port, renamefile, "Override", "Download", ftppath, "Y", localpath, "Y");
                    //OnStepAction.RunAction(this, 1, sitename, siteip, userid, password, "Download", ftppath, "Y", localpath, "Y");
                }
            }
            catch (Exception ex)
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
