using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmActionGroup : Form
    {
        DB DB = new DB();

        public frmActionGroup()
        {
            InitializeComponent();
        }

        private void frmActionGroup_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshAction()
        {
            lbAction.Items.Clear();
            using (DataTable dt = DB.GetActionType())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lbAction.Items.Add(dr["Action"].ToString());
                }
            }
        }

        private void rbAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAdd.Checked)
            {
                txtAction.Enabled = true;
                txt2.Visible = false;
                label2.Visible = false;
                btnOK.Text = "Add";
            }
        }

        private void rbModify_CheckedChanged(object sender, EventArgs e)
        {
            if (rbModify.Checked)
            {
                txtAction.Enabled = false;
                txt2.Visible = true;
                label2.Visible = true;
                btnOK.Text = "Modify";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (btnOK.Text == "Add")
            {
                if (txtAction.Text.Trim() == "")
                {
                    MessageBox.Show("Please key in a action", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAction.Focus();
                    return;
                }
                if (DB.CheckActionIsAlreadyExist(txtAction.Text.Trim()) == true)
                {
                    MessageBox.Show("The action is already exist", "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAction.Text = "";
                    txtAction.Focus();
                    return;
                }
                try
                {
                    DB.AddNewActionType(txtAction.Text.Trim().ToString());
                    RefreshAction();
                    MessageBox.Show("Add a new action finished", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAction.Text = "";
                    txtAction.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (txtAction.Text.Trim() == "")
                {
                    MessageBox.Show("Please choose a action", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txt2.Text.Trim() == "")
                {
                    MessageBox.Show("Please key in a new action name", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt2.Focus();
                    return;
                }
                if (txtAction.Text.Trim() == txt2.Text.Trim())
                {
                    MessageBox.Show("Old and new action is the same", "Action", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt2.Focus();
                    return;
                }
                if (DialogResult.No == MessageBox.Show("Do you really want to modify it?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                try
                {
                    DB.UpdateActionName(txtAction.Text.Trim(), txt2.Text.Trim());
                    RefreshAction();
                    MessageBox.Show("Modify a action finished", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAction.Text = "";
                    txt2.Text = "";
                    txt2.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbAction_Click(null, null);
        }

        private void lbAction_Click(object sender, EventArgs e)
        {
            if (rbAdd.Checked)
            {
                return;
            }
            if (lbAction.Items.Count == 0)
            {
                return;
            }
            if (lbAction.SelectedItems.Count == 0)
            {
                return;
            }
            txtAction.Text = lbAction.SelectedItems[0].ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cms_Delete_Click(object sender, EventArgs e)
        {
            if (lbAction.Items.Count == 0)
            {
                return;
            }
            if (lbAction.SelectedItems.Count == 0)
            {
                return;
            }
            if (DB.CheckActionIsAlreadyUsed(lbAction.SelectedItems[0].ToString()) == true)
            {
                MessageBox.Show("The action is in use, can't delete", "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DialogResult.No == MessageBox.Show("Do you rally want to delete this action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                DB.DeleteAction(lbAction.SelectedItems[0].ToString());
                RefreshAction();
                MessageBox.Show("Delete a action finished", "Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
