using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FTPTool
{
    public partial class frmBackup : Form
    {
        DB DB = new DB();

        public frmBackup()
        {
            InitializeComponent();
        }

        #region Backup

        private void btnBkPath_Click(object sender, EventArgs e)
        {
            fbd.SelectedPath = Application.StartupPath;
            fbd.ShowDialog();
            try
            {
                txtBkPath.Text = fbd.SelectedPath;
            }
            catch { }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (chkAction.Checked == false && chkSch.Checked == false && chkSite.Checked == false)
            {
                return;
            }

            bool b_error = false;

            if (txtBkPath.Text == "")
            {
                MessageBox.Show("Please choose a path", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Directory.Exists(txtBkPath.Text) == false)
            {
                Directory.CreateDirectory(txtBkPath.Text);
            }
            if (chkAction.Checked)
            {
                try
                {
                    using (DataTable dt = DB.GetActionType())
                    {
                        dt.TableName = "ActionBackup";
                        string filename = txtBkPath.Text + "\\Backup_Action_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        dt.WriteXml(filename);
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Backup action failed\r\n" + ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (chkSch.Checked)
            {
                try
                {
                    using (DataSet ds = DB.GetScheduleToBackup())
                    {
                        string filename = txtBkPath.Text + "\\Backup_Schedule_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        ds.WriteXml(filename);
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Backup schedule failed\r\n" + ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (chkSite.Checked)
            {
                try
                {
                    using (DataTable dt = DB.GetSiteProfileToBackup())
                    {
                        dt.TableName = "SiteProfileBackup";
                        string filename = txtBkPath.Text + "\\Backup_SiteProfile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        dt.WriteXml(filename);
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Backup site profile failed\r\n" + ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (b_error == true)
            {
                MessageBox.Show("Backup finished, some errors occured", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Backup finished", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            try
            {
                txtSchedule.Text = ofd.FileName;
            }
            catch
            {
                txtSchedule.Text = "";
            }
        }

        private void btnSiteProfile_Click(object sender, EventArgs e)
        {
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            try
            {
                txtSiteProfile.Text = ofd.FileName;
            }
            catch
            {
                txtSiteProfile.Text = "";
            }
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            try
            {
                txtAction.Text = ofd.FileName;
            }
            catch
            {
                txtAction.Text = "";
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (txtAction.Text == "" && txtSchedule.Text == "" && txtSiteProfile.Text == "")
            {
                return;
            }

            bool b_error = false;

            if(txtAction.Text!="" && txtAction.Text.ToUpper().EndsWith(".XML"))
            {
                try
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.ReadXml(txtAction.Text);
                        if (CheckDataTableExistAndRight(ds, "ActionBackup", 1) == -1)
                        {
                            b_error = true;
                            MessageBox.Show("Action backup file format error, restore failed", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //Step1 Delete temp table
                            DB.DeleteActionRestoreTable();
                            //Step2 Upload to temp table
                            foreach (DataRow dr in ds.Tables["ActionBackup"].Rows)
                            {
                                DB.AddRestoreItemIntoAction(dr["Action"].ToString());
                            }
                            //Step3 Move to live table
                            DB.MoveDataToAction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Restore action failed\r\n" + ex.Message, "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (txtSchedule.Text != "" && txtSchedule.Text.ToUpper().EndsWith(".XML"))
            {
                try
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.ReadXml(txtSchedule.Text);
                        if (CheckDataTableExistAndRight(ds, "Schedule_RunStep", 7) == -1)
                        {
                            b_error = true;
                            MessageBox.Show("Schedule_RunStep backup file format error, restore failed", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (CheckDataTableExistAndRight(ds, "Schedule_BaseData", 11) == -1)
                            {
                                b_error = true;
                                MessageBox.Show("Schedule_BaseData backup file format error, restore failed", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                //Step1 Delete temp table
                                DB.DeleteScheduleRestoreTable();

                                //Step2 Upload to temp table
                                foreach (DataRow dr in ds.Tables["Schedule_RunStep"].Rows)
                                {
                                    DB.AddRestoreItemIntoScheduleRunStep(dr["ScheduleName"].ToString(),
                                                                            dr["Step"].ToString(),
                                                                            dr["Action"].ToString(),
                                                                            dr["RemoteFileFolder"].ToString(),
                                                                            dr["RemoteIsFolder"].ToString(),
                                                                            dr["LocalFileFolder"].ToString(),
                                                                            dr["LocalIsFolder"].ToString());
                                }
                                foreach (DataRow dr in ds.Tables["Schedule_BaseData"].Rows)
                                {
                                    DB.AddRestoreItemIntoScheduleBaseData(dr["ScheduleName"].ToString(),
                                                                            dr["SiteProfile"].ToString(),
                                                                            dr["StartDateTime"].ToString(),
                                                                            dr["LastRunDateTime"].ToString(),
                                                                            dr["Repeat"].ToString(),
                                                                            dr["Status"].ToString(),
                                                                            dr["SuccessfulMail"].ToString(),
                                                                            dr["Mail"].ToString(),
                                                                            dr["ReconnectTimes"].ToString(),
                                                                            dr["ReconnectInterval"].ToString(),
                                                                            dr["FileExistAction"].ToString());
                                }
                                //Step3 Move to live table
                                DB.MoveDataToSchedule();
                            }
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Restore schedule failed\r\n" + ex.Message, "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (txtSiteProfile.Text != "" && txtSiteProfile.Text.ToUpper().EndsWith(".XML"))
            {
                try
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.ReadXml(txtSiteProfile.Text);
                        if (CheckDataTableExistAndRight(ds, "SiteProfileBackup", 8) == -1)
                        {
                            b_error = true;
                            MessageBox.Show("SiteProfile backup file format error, restore failed", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //Step1 Delete temp table
                            DB.DeleteSiteProfileRestoreTable();
                            //Step2 Upload to temp table
                            foreach (DataRow dr in ds.Tables["SiteProfileBackup"].Rows)
                            {
                                DB.AddRestoreItemIntoSiteProfile(dr["Type"].ToString(),
                                                                    dr["SiteName"].ToString(),
                                                                    dr["SiteIP"].ToString(),
                                                                    dr["UserID"].ToString(),
                                                                    dr["Password"].ToString(),
                                                                    dr["RenameFile"].ToString(),
                                                                    dr["Description"].ToString(),
                                                                    dr["Port"].ToString());
                            }
                            //Step3 Move to live table
                            DB.MoveDataToSiteProfile();
                        }
                    }
                }
                catch (Exception ex)
                {
                    b_error = true;
                    MessageBox.Show("Restore action failed\r\n" + ex.Message, "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (b_error == true)
            {
                MessageBox.Show("Restore finished, some errors occured", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Restore finished", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            MessageBox.Show("Application must restart after restore!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        private int CheckDataTableExistAndRight(DataSet ds, string TableName, int iColQty)
        {
            int i = 0;
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.TableName == TableName)
                {
                    if (dt.Columns.Count == iColQty)
                    {
                        return i;
                    }
                    else
                    {
                        return -1;
                    }
                }
                i++;
            }
            return -1;
        }

        #endregion








    }
}
