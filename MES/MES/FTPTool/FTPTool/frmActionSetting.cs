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
    public partial class frmActionSetting : Form
    {
        DB DB = new DB();

        private string _Action = "";
        private string _RemoteFileOrFolder = "";
        private string _RemoteIsFolder = "";
        private string _LocalFileOrFolder = "";
        private string _LocalIsFolder = "";
        private string _Cancel = "";


        private string inputAction = "";
        private string inputRemotefolder = "";
        private string inputRemoteisfolder = "";
        private string inputlocalfolder = "";
        private string inputlocalisfolder = "";

        public frmActionSetting()
        {
            InitializeComponent();
        }

        public frmActionSetting(string _inputAction,string _inputRemotefolder,string _inputRemoteisfolder,string _inputlocalfolder,string _inputlocalisfolder)
        {
            InitializeComponent();
            inputAction = _inputAction;
            inputRemotefolder = _inputRemotefolder;
            inputRemoteisfolder = _inputRemoteisfolder;
            inputlocalfolder = _inputlocalfolder;
            inputlocalisfolder = _inputlocalisfolder;
        }

        private void frmActionSetting_Load(object sender, EventArgs e)
        {
            try
            {
                using (DataTable dt = DB.GetActionType())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cboAction.Items.Add(dr["Action"].ToString());
                    }
                }

                if (inputAction != "")
                {
                    cboAction.Text = inputAction;
                    txtRemote.Text = inputRemotefolder;
                    if (inputRemoteisfolder == "Y")
                    {
                        chkRemote.Checked = true;
                    }
                    else
                    {
                        chkRemote.Checked = false;
                    }
                    txtLocal.Text = inputlocalfolder;
                    if (inputlocalisfolder == "Y")
                    {
                        chkLocal.Checked = true;
                    }
                    else
                    {
                        chkLocal.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel = "Y";
            this.Dispose();
        }

        public string Action
        {
            set 
            { 
                _Action = value; 
            }
            get
            {
                return _Action;
            }
        }

        public string RemoteFileOrFolder
        {
            set
            {
                _RemoteFileOrFolder = value;
            }
            get
            {
                return _RemoteFileOrFolder;
            }
        }

        public string RemoteIsFolder
        {
            set
            {
                _RemoteIsFolder = value;
            }
            get
            {
                return _RemoteIsFolder;
            }
        }

        public string LocalFileOrFolder
        {
            set
            {
                _LocalFileOrFolder = value;
            }
            get
            {
                return _LocalFileOrFolder;
            }
        }

        public string LocalIsFolder
        {
            set
            {
                _LocalIsFolder = value;
            }
            get
            {
                return _LocalIsFolder;
            }
        }

        public string Cancel
        {
            set { _Cancel = value; }
            get { return _Cancel; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboAction.Text == "")
            {
                MessageBox.Show("Please choose a action", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboAction.Focus();
                return;
            }

            if (chkRemote.Checked)
            {
                if (txtRemote.Text.Trim().EndsWith("/") == false)
                {
                    MessageBox.Show("Remote folder must end with /", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (txtRemote.Text.Trim().EndsWith("/"))
                {
                    MessageBox.Show("Remote file can't end with /", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (txtLocal.Text.Trim().Length > 0)
            {
                if (txtLocal.Text.Trim().EndsWith("\\"))
                {
                    txtLocal.Text = txtLocal.Text.Trim().Substring(0, txtLocal.Text.Trim().Length - 1);
                }
            }
            if (chkLocal.Checked)
            {
                if (Directory.Exists(txtLocal.Text.Trim()) == false)
                {
                    MessageBox.Show("Local folder does not exist, please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (cboAction.Text.StartsWith("MoveLocalFileToBackupFolder"))
                {
                    if (Directory.Exists(txtLocal.Text.Trim()) == false)
                    {
                        MessageBox.Show("Back up folder does not exist, please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    if (Directory.Exists(txtLocal.Text.Trim()) == true)
                    {
                        MessageBox.Show("Local file setting is a folder, please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            //if (chkRemote.Checked && chkLocal.Checked==false)
            //{
            //    MessageBox.Show("Remote is a folder, but local isn't a folder, please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (chkRemote.Checked == false && chkRemote.Visible==true && chkLocal.Checked)
            //{
            //    MessageBox.Show("Local is a folder, but remote isn't a folder, please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}


            Action = cboAction.Text;


            RemoteFileOrFolder = txtRemote.Text.Trim();
            RemoteIsFolder = chkRemote.Checked ? "Y" : "N";
            LocalFileOrFolder = txtLocal.Text.Trim();
            LocalIsFolder = chkLocal.Checked ? "Y" : "N";


            if (Action == "Upload" || Action == "Download")
            {
                if (RemoteFileOrFolder == "" || RemoteFileOrFolder=="*")
                {
                    MessageBox.Show("Remote file or folder is error, can't be null or / or *", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (LocalFileOrFolder == "" || RemoteFileOrFolder == "*")
                {
                    MessageBox.Show("Local file or folder is error, can't be null or / or *", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (Action == "Delete")
            {
                if (RemoteFileOrFolder == "/" || RemoteFileOrFolder == "*")
                {
                    MessageBox.Show("Remote file or folder is error, can't be / or *", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (Action == "DeleteRemoteExceptParentFolder")
            {
                if (chkRemote.Checked == false)
                {
                    MessageBox.Show("Remote must be a folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (Action.StartsWith("MoveLocalFileToBackupFolder"))
            {
                if (txtRemote.Text.Trim() == "")
                {
                    MessageBox.Show("Please set the local folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtLocal.Text.Trim() == "")
                {
                    MessageBox.Show("Please set the back up folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtLocal.Text.Trim().ToUpper().IndexOf(txtRemote.Text.Trim().ToUpper())>=0)
                {
                    MessageBox.Show("Local folder can not in backup folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtRemote.Text.Trim().ToUpper().IndexOf(txtLocal.Text.Trim().ToUpper()) >= 0)
                {
                    MessageBox.Show("Back up folder can not in local folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (chkLocal.Checked && Directory.Exists(txtRemote.Text.Trim()) == false)
                {
                    MessageBox.Show("Local folder does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            Cancel = "N";
            this.Dispose();
        }

        private void btnBfile_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            try
            {
                if (cboAction.Text == "Run External Program")
                {
                    txtRemote.Text = ofd.FileName.ToString();
                    chkRemote.Checked = false;
                    txtLocal.Text = txtRemote.Text.Substring(0, txtRemote.Text.LastIndexOf("\\"));
                    if (txtLocal.Text != "")
                    {
                        if (txtLocal.Text.IndexOf("\\")<0)
                        {
                            txtLocal.Text = txtLocal.Text + "\\";
                        }
                    }
                }
                else
                {
                    txtLocal.Text = ofd.FileName.ToString();
                    chkLocal.Checked = false;
                }
            }
            catch { }
        }

        private void btnBfolder_Click(object sender, EventArgs e)
        {
            fbd.ShowDialog();
            try
            {
                txtLocal.Text = fbd.SelectedPath.ToString();
                chkLocal.Checked = true;
            }
            catch { }
        }

        private void cboAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAction.Text == "Run External Program")
            {
                label2.Text = "Run this program or batch file";
                label4.Text = "";
                chkRemote.Checked = false;
                chkRemote.Visible = false;
                label3.Text = "Start in this folder";
                chkLocal.Checked = true;
                chkLocal.Visible = false;
                btnBfile.Location = new Point(252, 128);
                btnBfile.Text = "Browse File";
                btnBfile.Visible = true;
                btnLocalFolder.Visible = false;
            }
            else if (cboAction.Text.StartsWith("MoveLocalFileToBackupFolder"))
            {
                label2.Text = "";
                label4.Text = "Local folder or file";
                chkRemote.Visible = false;
                label3.Text = "Backup folder";
                chkLocal.Checked = true;
                chkLocal.Visible = true;
                btnBfile.Location = new Point(157, 203);
                btnBfile.Visible = false;
                btnLocalFolder.Visible = true;
                
            }
            else
            {
                label2.Text = "Remote file or folder";
                label4.Text = "FTP starts with /, SFTP starts with blank";
                chkRemote.Checked = false;
                chkRemote.Visible = true;
                label3.Text = "Local file or folder";
                chkLocal.Checked = false;
                chkLocal.Visible = true;
                btnBfile.Location = new Point(157, 203);
                btnBfile.Text = "Browse File";
                btnBfile.Visible = true;
                btnLocalFolder.Visible = false;
            }
        }

        private void chkRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemote.Checked)
            {
                if (txtRemote.Text.Trim() == "")
                {
                    txtRemote.Text = "/";
                }
                else
                {
                    if (txtRemote.Text.Trim().EndsWith("/") == false)
                    {
                        txtRemote.Text = txtRemote.Text.Trim() + "/";
                    }
                }
            }
            else
            {
                if (txtRemote.Text.Trim() == "")
                {
                    return;
                }
                else
                {
                    if (txtRemote.Text.Trim().EndsWith("/") == true)
                    {
                        txtRemote.Text = txtRemote.Text.Trim().Substring(0, txtRemote.Text.Trim().Length - 1);
                    }
                }
            }
        }

        private void lkTips_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string tips = "If the local path end with *.*, this tool will upload all files and folders(include sub folders) in current folder.\r\n" +
            "If the local path contain * (ex. abc*.txt), this tool will only upload right files in current folder.\r\n";

            MessageBox.Show(tips, "Path tips", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void chkLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLocal.Checked && cboAction.Text!= "Run External Program" && cboAction.Text.StartsWith("MoveLocalFileToBackupFolder")==false)
            {
                if (chkRemote.Checked==false) {
                    MessageBox.Show("Remote must be a folder while local need to set as a folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    chkLocal.Checked = false;
                }
            }
        }

        private void btnLocalFolder_Click(object sender, EventArgs e)
        {
            fbd.ShowDialog();
            try
            {
                txtRemote.Text = fbd.SelectedPath.ToString();
                chkLocal.Checked = true;
            }
            catch { }
        }
    }
}
