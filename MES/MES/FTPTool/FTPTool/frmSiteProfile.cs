using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace FTPTool
{
    public partial class frmSiteProfile : Form
    {
        DB DB = new DB();

        public frmSiteProfile()
        {
            InitializeComponent();
        }

        private void frmSiteProfile_Load(object sender, EventArgs e)
        {
            LoadTreeView();
        }

        private void LoadTreeView()
        {
            tvFTPSite.Nodes.Clear();
            TreeNode tnSite = new TreeNode("FTP Sites", 1, 1);
            using (DataTable dt = DB.GetSiteProfile())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode tn = new TreeNode(dr["SiteName"].ToString(), 0, 0);
                    tnSite.Nodes.Add(tn);
                }
            }
            //foreach (DataRow dr in Para.dsSiteProfile.Tables["Connection"].Rows)
            //{
            //    TreeNode tn = new TreeNode(dr["Name"].ToString(), 0, 0);
            //    tnSite.Nodes.Add(tn);
            //}
            tvFTPSite.Nodes.Add(tnSite);
            tvFTPSite.ExpandAll();
        }

        private void tvFTPSite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                return;//根节点
            }
            string Name = e.Node.Text;
            using (DataTable dt = DB.GetSiteProfileDetail(Name))
            {
                cboType.Text = dt.Rows[0]["Type"].ToString();
                txtName.Text = Name;
                txtSite.Text = dt.Rows[0]["SiteIP"].ToString();
                txtUserID.Text = dt.Rows[0]["UserID"].ToString();
                txtPassword.Text = Security.Decrypt(dt.Rows[0]["Password"].ToString());
                txtPort.Text = dt.Rows[0]["Port"].ToString();
                if (dt.Rows[0]["RenameFile"].ToString() == "Y")
                {
                    chkRename.Checked = true;
                }
                else
                {
                    chkRename.Checked = false;
                }
                txtDesc.Text = dt.Rows[0]["Description"].ToString();
            }
            //DataRow[] drs = Para.dsSiteProfile.Tables["Connection"].Select("Name='" + Name + "'");
            //if (drs.Length > 0)
            //{
            //    txtName.Text = drs[0]["Name"].ToString();
            //    txtSite.Text = drs[0]["Site"].ToString();
            //    txtUserID.Text = drs[0]["UserID"].ToString();
            //    txtPassword.Text = Security.Decrypt(drs[0]["Password"].ToString());
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboType.Text == "")
            {
                MessageBox.Show("Please choose a type", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboType.Focus();
                return;
            }
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please key in the Name", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }
            if (txtSite.Text.Trim() == "")
            {
                MessageBox.Show("Please key in the Site", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSite.Focus();
                return;
            }
            if (txtUserID.Text.Trim() == "")
            {
                MessageBox.Show("Please key in the UserID", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserID.Focus();
                return;
            }
            if (txtPort.Text.Trim() == "")
            {
                txtPort.Text = "21";
            }
            try
            {
                string encryptpassword = Security.CreateEncryptString(txtPassword.Text.Trim());

                if (DB.IsSiteProfileExist(txtName.Text.Trim())==true)
                {
                    if (MessageBox.Show("This profile is already exist, do you want to replace it?",
                        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        encryptpassword = null;
                        return;
                    }
                    DB.UpdateSiteProfile(cboType.Text,txtName.Text.Trim(), txtSite.Text.Trim(), txtUserID.Text.Trim(),
                        encryptpassword, (chkRename.Checked) ? "Y" : "N", txtDesc.Text.Trim().ToString(),txtPort.Text.Trim());
                }
                else
                {
                    DB.AddNewSiteProfile(cboType.Text,txtName.Text.Trim(), txtSite.Text.Trim(), txtUserID.Text.Trim(),
                        encryptpassword, (chkRename.Checked) ? "Y" : "N", txtDesc.Text.Trim().ToString(),txtPort.Text.Trim());
                }

                #region read XML
                //if (Para.dsSiteProfile.Tables["Connection"].Select("Name='" + txtName.Text.Trim() + "'").Length > 0)
                //{
                //    if (MessageBox.Show("This profile is already exist, do you want to replace it?",
                //        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                //    {
                //        encryptpassword = null;
                //        return;
                //    }
                //    foreach (DataRow dr in Para.dsSiteProfile.Tables["Connection"].Rows)
                //    {
                //        if (dr["Name"].ToString() == txtName.Text.Trim())
                //        {
                //            dr["Site"] = txtSite.Text.Trim();
                //            dr["UserID"] = txtUserID.Text.Trim();
                //            dr["Password"] = encryptpassword;

                //            Para.dsSiteProfile.WriteXml(Application.StartupPath + "\\siteprofile.xml");
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    DataRow drNew = Para.dsSiteProfile.Tables["Connection"].NewRow();
                //    drNew["Name"] = txtName.Text.Trim();
                //    drNew["Site"] = txtSite.Text.Trim();
                //    drNew["UserID"] = txtUserID.Text.Trim();
                //    drNew["Password"] = encryptpassword;
                //    Para.dsSiteProfile.Tables["Connection"].Rows.Add(drNew);
                //    Para.dsSiteProfile.WriteXml(Application.StartupPath + "\\siteprofile.xml");
                //}
                #endregion
                encryptpassword = null;
                LoadTreeView();
                MessageBox.Show("Save the site profile finished.", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save the site profile error.\r\n"+ex.Message, "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdDel_DelSite_Click(object sender, EventArgs e)
        {
            if (tvFTPSite.SelectedNode == null)
            {
                return;
            }
            if (tvFTPSite.SelectedNode.Parent == null)
            {
                return;//根节点
            }
            try
            {
                if (MessageBox.Show("Do you want to delete it?",
                      "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                DB.DeleteSiteProfile(tvFTPSite.SelectedNode.Text.ToString());

                //int i = 0;
                //foreach (DataRow dr in Para.dsSiteProfile.Tables["Connection"].Rows)
                //{
                //    if (dr["Name"].ToString() == tvFTPSite.SelectedNode.Text.ToString())
                //    {
                //        Para.dsSiteProfile.Tables["Connection"].Rows[i].Delete();
                //        Para.dsSiteProfile.WriteXml(Application.StartupPath + "\\siteprofile.xml");
                //        break;
                //    }
                //    i++;
                //}
                LoadTreeView();
                MessageBox.Show("Delete the site profile finished.", "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete the site profile error.\r\n" + ex.Message, "FTP Site Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}

