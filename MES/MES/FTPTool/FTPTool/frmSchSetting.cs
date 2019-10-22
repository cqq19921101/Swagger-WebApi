using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmSchSetting : Form
    {
        DB DB = new DB();

        string title = "Schedule Setting";
        string SchName = "";
        string SiteProfile = "";
        string StartRunTime = "";

        private string _Cancel = "";
        private string _SchNameRtn = "";
        private string _SiteProfileRtn = "";
        private string _StartRunTimeRtn = "";
        private string _RepeatRtn = "";
        private string _StatusRtn = "";
        private string _Mail = "";
        private string _SuccessfulMail = "";

        public frmSchSetting(string _SchName, string _SiteProfile)
        {
            InitializeComponent();
            SchName = _SchName;
            SiteProfile = _SiteProfile;
        }

        private void frmSchSetting_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.AppStarting;
                if(DateTime.Now.Hour<=9)
                {
                txtHour.Text = "0"+DateTime.Now.Hour.ToString();
                }
                else
                {
                    txtHour.Text = DateTime.Now.Hour.ToString();
                }
                if (DateTime.Now.Minute <= 9)
                {
                    txtMinute.Text = "0" + DateTime.Now.Minute.ToString();
                }
                else
                {
                    txtMinute.Text = DateTime.Now.Minute.ToString();
                }
                using (DataTable dt = DB.GetSiteProfile())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cboSite.Items.Add(dr["SiteName"].ToString());
                    }
                }
                if (SchName != "")
                {
                    txtSchName.Text = SchName;
                    txtSchName.Enabled = false;
                    cboSite.Text = SiteProfile;
                    //cboSite.Enabled = false;
                    btnOK.Text = "Update";

                    //Update先带出原有设置
                    using (DataTable dt = DB.GetOldScheduleBaseData(SchName))
                    {
                        dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["StartDateTime"].ToString().Substring(0, 10));

                        string tmptime = dt.Rows[0]["StartDateTime"].ToString().Substring(11, 5);
                        //dtpTime.Value = Convert.ToDateTime(dt.Rows[0]["StartDateTime"].ToString().Substring(11, 5));
                        txtHour.Text = tmptime.Substring(0, 2);
                        txtMinute.Text = tmptime.Substring(3, 2);
                        if (dt.Rows[0]["Status"].ToString() == "Y")
                        {
                            chkEnabled.Checked = true;
                        }
                        else
                        {
                            chkEnabled.Checked = false;
                        }
                        if (dt.Rows[0]["SuccessfulMail"].ToString() == "Y")
                        {
                            chkSuccessfulMail.Checked = true;
                        }
                        else
                        {
                            chkSuccessfulMail.Checked = false;
                        }
                        int irepeat = Convert.ToInt32(dt.Rows[0]["Repeat"].ToString());
                        if (irepeat < 60)
                        {
                            cboRepeat.Text = irepeat.ToString();
                        }
                        if (irepeat % 60 == 0)
                        {
                            if ((irepeat / 60) % 24 == 0)
                            {
                                cboRepeat.Text = ((irepeat / 60) / 24).ToString() + "Day";
                            }
                            else
                            {
                                cboRepeat.Text = (irepeat / 60).ToString() + "Hour";
                            }
                        }
                        else
                        {
                            cboRepeat.Text = irepeat.ToString();
                        }

                        //cboRepeat.Text = dt.Rows[0]["Repeat"].ToString();
                        txtMail.Text = dt.Rows[0]["Mail"].ToString();
                        StartRunTime = dt.Rows[0]["StartDateTime"].ToString();
                        cboReConnectTimes.Text = dt.Rows[0]["ReconnectTimes"].ToString();
                        cboReconnectInterval.Text = dt.Rows[0]["ReconnectInterval"].ToString();

                        if (dt.Rows[0]["FileExistAction"].ToString() == "Override")
                        {
                            rbOverride.Checked = true;
                        }
                        else
                        {
                            rbRename.Checked = true;
                        }
                    }
                    using (DataTable dt = DB.GetOldScheduleActionData(SchName))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dgvStepDetail.Rows.Add(dr["Step"].ToString(), dr["Action"].ToString(),
                                dr["RemoteFileFolder"].ToString(),
                                dr["RemoteIsFolder"].ToString(),
                                dr["LocalFileFolder"].ToString(),
                                dr["LocalIsFolder"].ToString());
                        }
                    }
                }
                else
                {
                    txtSchName.Focus();
                    btnOK.Text = "Add";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cancel = "Y";
                this.Cursor = Cursors.Default;
            }
        }

        #region 返回结果

        public string Cancel
        {
            set { _Cancel = value; }
            get { return _Cancel; }
        }
        public string SchNameRtn
        {
            set { _SchNameRtn = value; }
            get { return _SchNameRtn; }
        }
        public string SiteProfileRtn
        {
            set { _SiteProfileRtn = value; }
            get { return _SiteProfileRtn; }
        }

        public string StartRunTimeRtn
        {
            set { _StartRunTimeRtn = value; }
            get { return _StartRunTimeRtn; }
        }
        public string RepeatRtn
        {
            set { _RepeatRtn = value; }
            get { return _RepeatRtn; }
        }
        public string StatusRtn
        {
            set { _StatusRtn = value; }
            get { return _StatusRtn; }
        }
        public string Mail
        {
            set { _Mail = value; }
            get { return _Mail; }
        }
        public string SuccessfulMail
        {
            set { _SuccessfulMail = value; }
            get { return _SuccessfulMail; }
        }

        #endregion

        private string GetRepaetString()
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cboRepeat.Text, @"^\d+Day$"))
            {
                if (cboRepeat.Text == "0Day")
                {
                    return "";
                }
                else
                {
                    return (Convert.ToInt32(cboRepeat.Text.Substring(0, cboRepeat.Text.Length - 3)) * 1440).ToString();
                }
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(cboRepeat.Text, @"^\d+Hour$"))
            {
                if (cboRepeat.Text == "0Hour")
                {
                    return "";
                }
                else
                {
                    return (Convert.ToInt32(cboRepeat.Text.Substring(0, cboRepeat.Text.Length - 4)) * 60).ToString();
                }
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(cboRepeat.Text, @"^0+$"))
            {
                return "";
            }
            else
            {
                for (int i = 0; i < cboRepeat.Text.Trim().Length; i++)
                {
                    string tmp = cboRepeat.Text.Trim().Substring(i, 1);
                    if (System.Text.RegularExpressions.Regex.IsMatch(tmp, @"^\D+$"))
                    {
                        return "";
                    }
                }
                return cboRepeat.Text.Trim().ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (btnOK.Text == "Add")
            {
                #region Add
                if (txtSchName.Text == "")
                {
                    MessageBox.Show("Please give a schedule name", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSchName.Focus();
                    return;
                }
                if (cboSite.Text == "")
                {
                    MessageBox.Show("Please choose a site profile", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboSite.Focus();
                    return;
                }
                if (cboRepeat.Text.Trim() == "")
                {
                    MessageBox.Show("Please set a repeat minute", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboRepeat.Focus();
                    return;
                }
                //if (System.Text.RegularExpressions.Regex.IsMatch(cboRepeat.Text.Trim(), "^\\d+$") == false)
                //{
                //    MessageBox.Show("The repeat minute must use int type", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cboRepeat.Focus();
                //    return;
                //} 
                if (GetRepaetString() == "")
                {
                    MessageBox.Show("The repeat setting is error, please check", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSchName.SelectAll();
                    return;
                }
                if (dgvStepDetail.Rows.Count == 0)
                {
                    MessageBox.Show("Please set the schedule action", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboReConnectTimes.Text.Trim() == "")
                {
                    cboReConnectTimes.Text = "0";
                }
                if (cboReconnectInterval.Text.Trim() == "")
                {
                    cboReconnectInterval.Text = "1";
                }

                //Add new schedule
                try
                {
                    if (DB.CheckScheduleNameIsAlreadyExist(txtSchName.Text.Trim()) == true)
                    {
                        MessageBox.Show("This schedule name is already exist, please use another", title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtSchName.SelectAll();
                        return;
                    }
                    string tmptime = txtHour.Text.Trim() + ":" + txtMinute.Text.Trim();
                   
                    StartRunTime = dtpDate.Value.ToString("yyyy-MM-dd") + " " + tmptime;// dtpTime.Value.ToString("HH:mm");
                    DB.AddNewScheduleBaseData(txtSchName.Text.Trim(), cboSite.Text, StartRunTime, StartRunTime, GetRepaetString(),
                        chkEnabled.Checked ? "Y" : "N", chkSuccessfulMail.Checked ? "Y" : "N", txtMail.Text.Trim().ToString(),
                        cboReConnectTimes.Text.Trim().ToString(), cboReconnectInterval.Text.Trim().ToString(), rbOverride.Checked ? "Override" : "Rename");

                    foreach (DataGridViewRow dgvr in dgvStepDetail.Rows)
                    {
                        DB.AddNewScheduleActionData(txtSchName.Text.Trim(),
                            dgvr.Cells["Step"].Value.ToString(),
                            dgvr.Cells["Action"].Value.ToString(),
                            dgvr.Cells["RemoteFileFolder"].Value.ToString(),
                            dgvr.Cells["RemoteIsFolder"].Value.ToString(),
                            dgvr.Cells["LocalFileFolder"].Value.ToString(),
                            dgvr.Cells["LocalIsFolder"].Value.ToString());
                    }
                    Cancel = "N";
                    MessageBox.Show("Add a new schedule finished", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion
            }
            else if (btnOK.Text == "Update")
            {
                #region Update
                if (cboRepeat.Text.Trim() == "")
                {
                    MessageBox.Show("Please set a repeat minute", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboRepeat.Focus();
                    return;
                }
                //if (System.Text.RegularExpressions.Regex.IsMatch(cboRepeat.Text.Trim(), "^\\d+$") == false)
                //{
                //    MessageBox.Show("The repeat minute must use int type", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cboRepeat.Focus();
                //    return;
                //}
                if (GetRepaetString() == "")
                {
                    MessageBox.Show("The repeat setting is error, please check", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSchName.SelectAll();
                    return;
                }
                if (dgvStepDetail.Rows.Count == 0)
                {
                    MessageBox.Show("Please set the schedule action", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboReConnectTimes.Text.Trim() == "")
                {
                    cboReConnectTimes.Text = "0";
                }
                if (cboReconnectInterval.Text.Trim() == "")
                {
                    cboReconnectInterval.Text = "1";
                }

                //Update schedule
                try
                {
                    string tmptime = txtHour.Text.Trim() + ":" + txtMinute.Text.Trim();
                    StartRunTime = dtpDate.Value.ToString("yyyy-MM-dd") + " " + tmptime;// dtpTime.Value.ToString("HH:mm");


                    DB.UpdateScheduleBaseData(SchName,cboSite.Text, StartRunTime, StartRunTime, GetRepaetString(),
                        chkEnabled.Checked ? "Y" : "N", chkSuccessfulMail.Checked ? "Y" : "N", txtMail.Text.Trim().ToString(),
                        cboReConnectTimes.Text.Trim().ToString(), cboReconnectInterval.Text.Trim().ToString(),rbOverride.Checked?"Override":"Rename");

                    DB.DeleteScheduleActionData(SchName);

                    foreach (DataGridViewRow dgvr in dgvStepDetail.Rows)
                    {
                        DB.AddNewScheduleActionData(SchName,
                            dgvr.Cells["Step"].Value.ToString(),
                            dgvr.Cells["Action"].Value.ToString(),
                            dgvr.Cells["RemoteFileFolder"].Value.ToString(),
                            dgvr.Cells["RemoteIsFolder"].Value.ToString(),
                            dgvr.Cells["LocalFileFolder"].Value.ToString(),
                            dgvr.Cells["LocalIsFolder"].Value.ToString());
                    }
                    Cancel = "N";
                    MessageBox.Show("Update a schedule finished", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion
            }
            SchNameRtn = txtSchName.Text.Trim();
            SiteProfileRtn = cboSite.Text;
            StartRunTimeRtn = StartRunTime;
            RepeatRtn = GetRepaetString();// cboRepeat.Text.Trim();
            StatusRtn = chkEnabled.Checked ? "Y" : "N";
            Mail = txtMail.Text.Trim();
            SuccessfulMail = chkSuccessfulMail.Checked ? "Y" : "N";
            this.Dispose();
        }

        #region Action Setting

        private void cms_NewAction_Click(object sender, EventArgs e)
        {
            try
            {
                frmActionSetting fas = new frmActionSetting();
                fas.ShowDialog();
                if (fas.Cancel == "Y")
                {
                    return;
                }
                string Action = fas.Action;
                string RemoteFileOrFolder = fas.RemoteFileOrFolder;
                string RemoteIsFolder = fas.RemoteIsFolder;
                string LocalFileOrFolder = fas.LocalFileOrFolder;
                string LocalIsFolder = fas.LocalIsFolder;

                dgvStepDetail.Rows.Add(new object[] { "0", Action, RemoteFileOrFolder, RemoteIsFolder, LocalFileOrFolder, LocalIsFolder });

                RefreshActionStep();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cms_NewAction_Click(null, null);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvStepDetail.Rows.Count == 0)
            {
                return;
            }
            if (dgvStepDetail.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                if (DialogResult.No == MessageBox.Show("Do you want to delete this step?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                dgvStepDetail.Rows.RemoveAt(dgvStepDetail.SelectedRows[0].Index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshActionStep()
        {
            for (int i = 0; i < dgvStepDetail.Rows.Count; i++)
            {
                dgvStepDetail.Rows[i].Cells["Step"].Value = (i + 1).ToString();
            }
            dgvStepDetail.ClearSelection();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dgvStepDetail.Rows.Count == 0)
            {
                return;
            }
            if (dgvStepDetail.SelectedRows.Count == 0)
            {
                return;
            }

            int idx = dgvStepDetail.SelectedRows[0].Index;
            if (idx == 0)
            {
                return;
            }
            dgvStepDetail.Rows.InsertCopy(idx, idx - 1);
            foreach (DataGridViewColumn dgvc in dgvStepDetail.Columns)
            {
                dgvStepDetail.Rows[idx - 1].Cells[dgvc.Index].Value = dgvStepDetail.Rows[idx + 1].Cells[dgvc.Index].Value;
            }
            dgvStepDetail.Rows.RemoveAt(idx + 1);
            RefreshActionStep();
            dgvStepDetail.Rows[idx - 1].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dgvStepDetail.Rows.Count == 0)
            {
                return;
            }
            if (dgvStepDetail.SelectedRows.Count == 0)
            {
                return;
            }

            int idx = dgvStepDetail.SelectedRows[0].Index;
            if (idx == dgvStepDetail.Rows.Count - 1)
            {
                return;
            }
            dgvStepDetail.Rows.InsertCopy(idx, idx + 2);
            foreach (DataGridViewColumn dgvc in dgvStepDetail.Columns)
            {
                dgvStepDetail.Rows[idx + 2].Cells[dgvc.Index].Value = dgvStepDetail.Rows[idx].Cells[dgvc.Index].Value;
            }
            dgvStepDetail.Rows.RemoveAt(idx);
            RefreshActionStep();
            dgvStepDetail.Rows[idx + 1].Selected = true;
        }

        #endregion

        private void dgvStepDetail_DoubleClick(object sender, EventArgs e)
        {
            if (dgvStepDetail.Rows.Count == 0)
            {
                return;
            }
            if (dgvStepDetail.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                int row = dgvStepDetail.SelectedRows[0].Index;

                frmActionSetting fas = new frmActionSetting(dgvStepDetail.SelectedRows[0].Cells["Action"].Value.ToString(),
                                                            dgvStepDetail.SelectedRows[0].Cells["RemoteFileFolder"].Value.ToString(),
                                                            dgvStepDetail.SelectedRows[0].Cells["RemoteIsFolder"].Value.ToString(),
                                                            dgvStepDetail.SelectedRows[0].Cells["LocalFileFolder"].Value.ToString(),
                                                            dgvStepDetail.SelectedRows[0].Cells["LocalIsFolder"].Value.ToString());
                fas.ShowDialog();

                if (fas.Cancel == "Y")
                {
                    return;
                }
                string Action = fas.Action;
                string RemoteFileOrFolder = fas.RemoteFileOrFolder;
                string RemoteIsFolder = fas.RemoteIsFolder;
                string LocalFileOrFolder = fas.LocalFileOrFolder;
                string LocalIsFolder = fas.LocalIsFolder;

                dgvStepDetail.Rows[row].Cells["Action"].Value = Action;
                dgvStepDetail.Rows[row].Cells["RemoteFileFolder"].Value = RemoteFileOrFolder;
                dgvStepDetail.Rows[row].Cells["RemoteIsFolder"].Value = RemoteIsFolder;
                dgvStepDetail.Rows[row].Cells["LocalFileFolder"].Value = LocalFileOrFolder;
                dgvStepDetail.Rows[row].Cells["LocalIsFolder"].Value = LocalIsFolder;

                RefreshActionStep();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtHour_Click(object sender, EventArgs e)
        {
            lbHour.Visible = true;
            lbHour.SelectedItem = txtHour.Text.Trim();
        }

        private void lbHour_Click(object sender, EventArgs e)
        {
            txtHour.Text = lbHour.SelectedItem.ToString();
            lbHour.Visible = false;
        }

        private void txtMinute_Click(object sender, EventArgs e)
        {
            lbTime.Visible = true;
            lbTime.SelectedItem = txtMinute.Text.Trim();
        }

        private void lbTime_Click(object sender, EventArgs e)
        {
            txtMinute.Text = lbTime.SelectedItem.ToString();
            lbTime.Visible = false;
        }

        private void lkTips_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string tips="Please use a integer number as a minute.\r\n"+
                        "If you want to use hour, please use X+hour as the format(ex. 1Hour, 2Hour).\r\n"+
                        "If you want to use day, please use X+Day as the format(ex. 1Day, 2Day).\r\n"+
                        "Please don't use decimals.";
            MessageBox.Show(tips, "Repeat time tips", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }

}
