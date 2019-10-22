using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Data;

namespace FTPTool
{
    public partial class frmMain : Form
    {
        frmSiteProfile frmSiteProfile = null;
        frmSiteViewer frmSiteViewer = null;
        bool callbymenu = false;
        DB DB = new DB();
        //FTPx Ftp = new FTPx();

        //测试用，定义刷新住窗体进度条的委托
        public delegate void SetProgressStatus(int i);

        //定义刷新任务Status状态值的委托
        public delegate void SetScheduleStatus(int i, string Content, decimal iProgress);

        public frmMain()
        {
            InitializeComponent();
        }

        #region Menu

        private void mnu_SiteProfile_Click(object sender, EventArgs e)
        {
            if (frmSiteProfile == null || frmSiteProfile.IsDisposed)
            {
                frmSiteProfile = new frmSiteProfile();
            }
            frmSiteProfile.Show();
            frmSiteProfile.Activate();
        }

        private void mnu_SiteViewer_Click(object sender, EventArgs e)
        {
            if (frmSiteViewer == null || frmSiteViewer.IsDisposed)
            {
                frmSiteViewer = new frmSiteViewer();
            }
            frmSiteViewer.Show();
            frmSiteViewer.Activate();
        }

        private void mnu_ActionGroup_Click(object sender, EventArgs e)
        {
            frmActionGroup fag = new frmActionGroup();
            fag.ShowDialog();
        }

        private void mnu_BK_Click(object sender, EventArgs e)
        {
            frmBackup fb = new frmBackup();
            fb.ShowDialog();
        }

        private void mnu_Mail_Click(object sender, EventArgs e)
        {
            frmMailSetting fms = new frmMailSetting();
            fms.ShowDialog();
        }

        private void mnu_Exit_Click(object sender, EventArgs e)
        {
            if (exit())
            {
                callbymenu = true;
                Environment.Exit(0);
                //Application.Exit();
            }
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (callbymenu == false)
            {
                if (exit() == false)
                {
                    e.Cancel = true;
                }
            }
        }

        private bool exit()
        {
            t_monitor.Enabled = false;
            bool b_Run = false;
            foreach (DataGridViewRow dgvr in dgvSch.Rows)
            {
                if (dgvr.Cells["Status"].Value.ToString() != "Wating" && dgvr.Cells["Status"].Value.ToString() != "Disabled")
                {
                    b_Run = true;
                    if (DialogResult.No == MessageBox.Show("Do you really want to exit? Some scheduld is running now!", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        t_monitor.Enabled = true;
                        //e.Cancel = true;
                        return false;
                    }
                    else
                    {
                        SendSysMailForClose();
                        return true;
                        //Application.Exit();
                    }
                }
            }
            if (b_Run == false)
            {
                if (DialogResult.No == MessageBox.Show("Do you really want to exit? All schedule will be stopped", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    t_monitor.Enabled = true;
                    //e.Cancel = true;
                    return false;
                }
                else
                {
                    SendSysMailForClose();
                    return true;
                    //Application.Exit();
                }
            }
            return true;
        }

        private void SendSysMailForClose()
        {
            if (Para.dsSysMailLoop != null)
            {
                try
                {
                    Tool.SendMail(Para.dsSysMailLoop.Tables["MailLoop"].Rows[0]["Mail"].ToString(), "Ftp tool has been closed",
                        "If this is not in schedule, please check the tool and restart it!" +
                        "</br>======================================================================" +
                        "</br>IP: " + Tool.GetLocalIP() + "; Server: " + Tool.GetComputerName() + "; Tool path: " + Tool.GetApplicationPath(), "");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Send close alert mail fail:" + ex.Message, "Close", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            dgv.Rows.Add();
            this.WindowState = FormWindowState.Maximized;
            this.Text += " [Version: " + Tool.GetFileVersion().ToString() + "]";
            try
            {
                //Get Mail Setting
                using (DataTable dt = DB.GetMailAccountSetting())
                {
                    if (dt.Rows.Count > 0)
                    {
                        Para.MAIL_FROM = dt.Rows[0]["MailFrom"].ToString();
                        Para.MAIL_SMTP = dt.Rows[0]["SMTP"].ToString();
                        Para.MAIL_ACCOUNT = dt.Rows[0]["Account"].ToString();
                        Para.MAIL_PWD = dt.Rows[0]["Password"].ToString();
                    }
                }

                using (DataTable dt = DB.GetMainFormScheduleList())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dgvSch.Rows.Add(dr["ScheduleName"].ToString(),
                                        dr["SiteProfile"].ToString(),
                                        dr["StartDateTime"].ToString(),
                                        dr["LastRunDateTime"].ToString(),
                                        "0",
                                        (Convert.ToDateTime(dr["StartDateTime"].ToString()).AddMinutes(Convert.ToInt32(dr["Repeat"].ToString()))).ToString("yyyy-MM-dd HH:mm"),
                                        dr["Repeat"].ToString(),
                                        (dr["Status"].ToString() == "Y") ? "Waiting" : "Disabled",
                                        0,
                                        dr["Mail"].ToString(),
                                        dr["SuccessfulMail"].ToString()
                                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvSch.ClearSelection();
            }
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100, "FTP Tool", "I'm here,:) ", ToolTipIcon.Info);
            }
        }

        private void cms_nofity_smw_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            cms_nofity_smw_Click(null, null);
        }

        #region 单击Dgv空白处清空选择项

        private int GetRowIndexAt(int mouseLocation_Y)
        {
            if (dgvSch.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;
            }
            if (dgvSch.ColumnHeadersVisible == true && mouseLocation_Y <= dgvSch.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = dgvSch.FirstDisplayedScrollingRowIndex;
            int displayedCount = dgvSch.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount; )
            {
                if (dgvSch.Rows[index].Visible == true)
                {
                    Rectangle rect = dgvSch.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    k++;
                }
                index++;
            }
            return -1;
        }

        private void dgvSch_MouseClick(object sender, MouseEventArgs e)
        {
            if (GetRowIndexAt(e.Y) == -1)
            {
                dgvSch.ClearSelection();
            }
        }

        #endregion

        #region CMS

        private void cms_Modify_Click(object sender, EventArgs e)
        {
            if (dgvSch.Rows.Count == 0)
            {
                return;
            }
            if (dgvSch.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                int iRowidx = dgvSch.SelectedCells[0].RowIndex;
                string Status = dgvSch.Rows[iRowidx].Cells["Status"].Value.ToString();
                if (Status != "Disabled" && Status != "Waiting" && Status.StartsWith("Error:")==false)
                {
                    MessageBox.Show("This schedule is running now, please wait until it's finish", "Modify Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string name = dgvSch.Rows[iRowidx].Cells["ScheduleName"].Value.ToString();
                string profile = dgvSch.Rows[iRowidx].Cells["SiteProfile"].Value.ToString();
                frmSchSetting fss = new frmSchSetting(name, profile);
                fss.ShowDialog();

                //更新返回结果
                if (fss.Cancel == "N")
                {
                    foreach (DataGridViewRow dgvr in dgvSch.Rows)
                    {
                        if (dgvr.Cells["ScheduleName"].Value.ToString() == name)
                        {
                            dgvr.Cells["StartTime"].Value = fss.StartRunTimeRtn;
                            dgvr.Cells["LastRunTime"].Value = fss.StartRunTimeRtn;
                            dgvr.Cells["Repeat"].Value = fss.RepeatRtn;
                            dgvr.Cells["NextRunTime"].Value = (Convert.ToDateTime(fss.StartRunTimeRtn).AddMinutes(Convert.ToInt32(fss.RepeatRtn))).ToString("yyyy-MM-dd HH:mm");
                            if (fss.StatusRtn == "Y")
                            {
                                dgvr.Cells["Status"].Value = "Waiting";
                            }
                            else
                            {
                                dgvr.Cells["Status"].Value = "Disabled";
                            }
                            dgvr.Cells["Mail"].Value = fss.Mail;
                            dgvr.Cells["SuccessfulMail"].Value = fss.SuccessfulMail;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void cms_AddNewSch_Click(object sender, EventArgs e)
        {
            try
            {
                frmSchSetting fss = new frmSchSetting("", "");
                fss.ShowDialog();

                //更新返回结果
                if (fss.Cancel == "N")
                {
                    dgvSch.Rows.Add(fss.SchNameRtn, fss.SiteProfileRtn, fss.StartRunTimeRtn, fss.StartRunTimeRtn, "0",
                        (Convert.ToDateTime(fss.StartRunTimeRtn).AddMinutes(Convert.ToInt32(fss.RepeatRtn))).ToString("yyyy-MM-dd HH:mm"),
                        fss.RepeatRtn, (fss.StatusRtn == "Y") ? "Waiting" : "Disabled");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_Delete_Click(object sender, EventArgs e)
        {
            if (dgvSch.Rows.Count == 0)
            {
                return;
            }
            if (dgvSch.SelectedRows.Count == 0)
            {
                return;
            }
            string Status = dgvSch.Rows[dgvSch.SelectedCells[0].RowIndex].Cells["Status"].Value.ToString();
            if (Status != "Disabled" && Status != "Waiting")
            {
                MessageBox.Show("This schedule is running now, please wait until it's finish", "Delete Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (DialogResult.No == MessageBox.Show("Do you really want to delete this schedule?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            try
            {
                if (DB.DeleteSchedule(dgvSch.SelectedRows[0].Cells["ScheduleName"].Value.ToString()) == true)
                {
                    dgvSch.Rows.RemoveAt(dgvSch.SelectedRows[0].Index);
                    MessageBox.Show("Delete a schedule finished", "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Delete a schedule failed", "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_SetStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (cms_SetStatus.Text == "Enabled")
                {
                    DB.UpdateScheduleStatus(dgvSch.SelectedRows[0].Cells["ScheduleName"].Value.ToString(), "Y");
                    dgvSch.SelectedRows[0].Cells["Status"].Value = "Waiting";
                }
                else
                {
                    if (dgvSch.SelectedRows[0].Cells["Status"].Value.ToString() != "Waiting")
                    {
                        MessageBox.Show("This schedule is running now, please wait until it's finish", "Change Stauts", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        DB.UpdateScheduleStatus(dgvSch.SelectedRows[0].Cells["ScheduleName"].Value.ToString(), "N");
                        dgvSch.SelectedRows[0].Cells["Status"].Value = "Disabled";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cms_Run_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(StartRunASchecule));
            t.Start(dgvSch.SelectedRows[0].Index);
        }

        private void cms_CopySch_Click(object sender, EventArgs e)
        {
            if (dgvSch.Rows.Count == 0)
            {
                return;
            }
            if (dgvSch.SelectedRows.Count == 0)
            {
                return;
            }
            frmInput fI = new frmInput();
            fI.ShowDialog();
            fI.Dispose();
            if (fI.IsCancel)
            {
                return;
            }
            string newSchName = fI.Result;
            if (newSchName == "")
            {
                MessageBox.Show("The new schedule name can't be empty", "Copy Schedule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (DB.IsScheduleExist(newSchName))
                {
                    MessageBox.Show("The new schedule name is already exists", "Copy Schedule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DB.CopySchedule(dgvSch.SelectedRows[0].Cells["ScheduleName"].Value.ToString(), newSchName);
                using (DataTable dt = DB.GetScheduleToAdd(newSchName))
                {
                    if (dt.Rows.Count > 0)
                    {
                        dgvSch.Rows.Add(dt.Rows[0]["ScheduleName"].ToString(),
                            dt.Rows[0]["SiteProfile"].ToString(),
                            dt.Rows[0]["StartDateTime"].ToString(),
                            dt.Rows[0]["LastRunDateTime"].ToString(),
                            "0",
                            (Convert.ToDateTime(dt.Rows[0]["StartDateTime"].ToString()).AddMinutes(Convert.ToInt32(dt.Rows[0]["Repeat"].ToString()))).ToString("yyyy-MM-dd HH:mm"),
                            dt.Rows[0]["Repeat"].ToString(),
                            (dt.Rows[0]["Status"].ToString() == "Y") ? "Waiting" : "Disabled",
                            0,
                            dt.Rows[0]["Mail"].ToString(),
                            dt.Rows[0]["SuccessfulMail"].ToString()
                            );
                    }
                }
                MessageBox.Show("Copy finished", "Copy Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Copy Schedule", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dgvSch.Rows.Count == 0)
            {
                cms_SetStatus.Visible = false;
                cms_Delete.Visible = false;
                cms_Modify.Visible = false;
                cms_Run.Visible = false;
                cms_AddNewSch.Visible = true;
                cms_CopySch.Visible = false;
            }
            if (dgvSch.SelectedRows.Count == 0)
            {
                cms_SetStatus.Visible = false;
                cms_Delete.Visible = false;
                cms_Modify.Visible = false;
                cms_Run.Visible = false;
                cms_AddNewSch.Visible = true;
                cms_CopySch.Visible = false;
            }
            else
            {
                if (dgvSch.SelectedRows[0].Cells["Status"].Value.ToString() == "Disabled")
                {
                    cms_SetStatus.Text = "Enabled";
                }
                else
                {
                    cms_SetStatus.Text = "Disabled";
                }
                cms_SetStatus.Visible = true;
                cms_Delete.Visible = true;
                cms_Modify.Visible = true;
                cms_Run.Visible = true;
                cms_AddNewSch.Visible = true;
                cms_CopySch.Visible = true;
            }
        }

        #endregion


        private void UpdateScheduleStatus(int iRow,string Content,decimal iProgress)
        {
            dgvSch.Rows[iRow].Cells["Status"].Value = Content;
            dgvSch.Rows[iRow].Cells["ProgressValue"].Value = iProgress;
          
        }

        public void SetMessage(int iRow, string Content, decimal iProgress)
        {
            SetScheduleStatus sss = new SetScheduleStatus(this.UpdateScheduleStatus);
            sss.Invoke(iRow, Content, iProgress);
        }

        private void dgvSch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            lock (dgvSch.Rows[e.RowIndex])
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 7)
                {
                    if (dgvSch.Rows[e.RowIndex].Cells[7].Value.ToString() != "Waiting" && dgvSch.Rows[e.RowIndex].Cells[7].Value.ToString() != "Disabled")
                    {
                        Rectangle rg, fillrg;
                        using (Brush gridBrush = new SolidBrush(Color.Black), WhiteBrush = new SolidBrush(Color.White), backColorBrush = new SolidBrush(Color.FromArgb(204, 255, 0)))
                        {
                            e.PaintBackground(e.CellBounds, true);
                            using (Pen gridLinePen = new Pen(gridBrush))
                            {
                                // 画矩形框
                                rg = e.CellBounds;
                                rg.Inflate(-1, -2);
                                e.Graphics.FillRectangle(WhiteBrush, rg);
                                //e.Graphics.DrawRectangle(gridLinePen, rg);
                                // 填充矩形进度条
                                decimal width = Convert.ToDecimal(rg.Width - 3);
                                width = width * Convert.ToDecimal(dgvSch.Rows[e.RowIndex].Cells[8].Value);
                                fillrg = new Rectangle(rg.X + 1, rg.Y, Convert.ToInt32(width), rg.Height - 1);
                                e.Graphics.FillRectangle(backColorBrush, fillrg);
                            }
                            //e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, gridBrush, rg.X + rg.Width / 2 - 9, rg.Y);
                            e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, gridBrush, rg.X, rg.Y);
                            e.Handled = true;
                        }
                    }

                }
                Application.DoEvents();
            }
        }


        public void aaa(object iRow)
        {
            int retimes=3;
            if (retimes > 0)
            {

                int ireconnecttimes = 1;
                while (ireconnecttimes <= retimes)
                {

                    System.Threading.Thread.Sleep(30000 * 1);
                    Application.DoEvents();
                    ireconnecttimes++;
                }
            }
        }

        public void StartRunASchecule(object iRow)
        {
            string status = "";
            string SchName = "";
            BLL onebll = null;
            try
            {
                int i = (int)iRow;
                DateTime dtstart = DateTime.Now;
                status = dgvSch.Rows[i].Cells["Status"].Value.ToString();
                SchName = dgvSch.Rows[i].Cells["ScheduleName"].Value.ToString();
                onebll= new BLL(SchName);
                onebll.RunStepByStep(this, i);
                dgvSch.Rows[(int)iRow].Cells["Status"].Value = "Waiting";
                DateTime dtend = DateTime.Now;
                TimeSpan ts = dtend - dtstart;
                dgvSch.Rows[i].Cells["Cost"].Value = ts.TotalSeconds;
                if (status == "Disabled")
                {
                    dgvSch.Rows[i].Cells["Status"].Value = status;
                }
                if (dgvSch.Rows[i].Cells["SuccessfulMail"].Value.ToString() == "Y")
                {
                    try
                    {
                        Tool.SendMail(dgvSch.Rows[(int)iRow].Cells["Mail"].Value.ToString(), 
                                        "Ftp schedule successful -- "+ dgvSch.Rows[(int)iRow].Cells["ScheduleName"].Value.ToString(), 
                                        "Scheduld name: " + dgvSch.Rows[(int)iRow].Cells["ScheduleName"].Value.ToString() +
                                         "</br>Run time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                         "</br>======================================================================" +
                                         "</br>IP: " + Tool.GetLocalIP() + "; Server: " + Tool.GetComputerName() + "; Tool path: " + Tool.GetApplicationPath(), "");
                    }
                    catch(Exception ex)
                    {
                        Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", ex.ToString());
                    }
                                     
                }
            }
            catch (Exception ex)
            {
                Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", "ScheduleName: " + dgvSch.Rows[(int)iRow].Cells["ScheduleName"].Value.ToString() + "\t" + ex.Message);
                dgvSch.Rows[(int)iRow].Cells["Status"].Value = "Error:" + ex.Message;
                dgvSch.Rows[(int)iRow].Cells["Cost"].Value = 0;
                try
                {
                    Tool.SendMail(dgvSch.Rows[(int)iRow].Cells["Mail"].Value.ToString(),
                                         "Ftp schedule failed -- "+ dgvSch.Rows[(int)iRow].Cells["ScheduleName"].Value.ToString(), 
                                         "Scheduld name: " + dgvSch.Rows[(int)iRow].Cells["ScheduleName"].Value.ToString() +
                                         "</br>Failed time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                         "</br> Error Message: </br>" + ex.Message +
                                         "</br>======================================================================" +
                                         "</br>IP: " + Tool.GetLocalIP() + "; Server: " + Tool.GetComputerName() + "; Tool path: " + Tool.GetApplicationPath()
                                         , Para.ERRORLOGFILE);
                }
                catch(Exception ex1)
                {
                    Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", ex1.ToString());
                }

            }
            finally
            {
                status = null;
                SchName = null;
                onebll = null;
            }
        }






        private void t_monitor_Tick(object sender, EventArgs e)
        {
            //int i = 0;
            t_monitor.Interval = 10000;
            //Tool.SaveLog("testlog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (DataGridViewRow dgvr in dgvSch.Rows)
            {
                try
                {
                    if (dgvr.Cells["Status"].Value.ToString() == "Disabled")
                    {
                        //dgvr.Cells["Status"].Value = "Disabled";
                    }
                    else
                    {
                        DateTime nowtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                        if (dgvr.Cells["LastRunTime"].Value.ToString() == "")
                        {
                            dgvr.Cells["LastRunTime"].Value = dgvr.Cells["StartTime"].Value;
                        }

                        DateTime checknextstarttime;
                        if (dgvr.Cells["NextRunTime"].Value.ToString() == "")
                        {
                            checknextstarttime = Convert.ToDateTime(dgvr.Cells["StartTime"].Value.ToString());
                        }
                        else
                        {
                            checknextstarttime = Convert.ToDateTime(dgvr.Cells["NextRunTime"].Value.ToString());
                        }
                        int interval = Convert.ToInt32(dgvr.Cells["Repeat"].Value.ToString());
                        //计算下一次运行时间

                        while (checknextstarttime < nowtime)
                        {
                            checknextstarttime = checknextstarttime.AddMinutes(interval);
                        }
                        dgvr.Cells["NextRunTime"].Value = checknextstarttime.ToString("yyyy-MM-dd HH:mm");

                        if (checknextstarttime > nowtime && dgvr.Cells["Status"].Value.ToString() == "Waiting")
                        {
                            dgvr.Cells["Status"].Value = "Waiting";
                        }
                        else
                        {
                            if (dgvr.Cells["Status"].Value.ToString() == "Waiting" || dgvr.Cells["Status"].Value.ToString().StartsWith("Error:"))
                            {
                                if (nowtime.ToString("yyyy-MM-dd HH:mm") == checknextstarttime.ToString("yyyy-MM-dd HH:mm"))
                                {
                                    dgvr.Cells["LastRunTime"].Value = checknextstarttime.ToString("yyyy-MM-dd HH:mm");
                                    DB.UpdateLastRunTime(dgvr.Cells["ScheduleName"].Value.ToString(), dgvr.Cells["LastRunTime"].Value.ToString());
                                    dgvr.Cells["NextRunTime"].Value = checknextstarttime.AddMinutes(interval).ToString("yyyy-MM-dd HH:mm");

                                    Thread t = new Thread(new ParameterizedThreadStart(StartRunASchecule));
                                    t.Start(dgvr.Cells["Status"].RowIndex);


                                }
                            }
                        }
                        GC.Collect();
                    }
                    
                    //i++;
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", "Scheduld name: " + dgvr.Cells["ScheduleName"].Value.ToString() + "\t" + ex.Message);
                    try
                    {
                        Tool.SendMail(dgvr.Cells["Mail"].Value.ToString(),
                                        "Ftp scheduld failed -- "+ dgvr.Cells["ScheduleName"].Value.ToString(),
                                        "Scheduld name: " + dgvr.Cells["ScheduleName"].Value.ToString() +
                                        "</br>Failed time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                        "</br> Error Message: </br>" + ex.Message +
                                        "</br>======================================================================" +
                                        "</br>IP: " + Tool.GetLocalIP() + "; Server: " + Tool.GetComputerName() + "; Tool path: " + Tool.GetApplicationPath()
                                        , Para.ERRORLOGFILE);
                    }
                    catch(Exception ex1)
                    {
                        Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", ex1.ToString());
                    }
                }
            }
            Thread.Sleep(1000);
        }


        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType("Upload");


                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
























        #region 测试用代码

        //public void setProgressStatus(int i)
        //{
        //    progressBar1.Value = i;
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    bgw.RunWorkerAsync();
        //    //Thread ftpupload = new Thread(new ParameterizedThreadStart(Ftp.Upload));
        //    //ftpupload.Start(this);

        //}

        //private void SetDgvValue()
        //{
        //    dgv.Rows[0].Cells[0].Value = "Test Progress Bar...";
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        dgv.Rows[0].Cells[1].Value = i.ToString();
        //        Thread.Sleep(200);
        //    }
        //}

        //protected override void OnLoad(EventArgs e)
        //{

        //    base.OnLoad(e);
        //}

        //private void dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    lock (dgv)
        //    {
        //        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //        {
        //            if (dgv.Columns["Progress"].Index == e.ColumnIndex)
        //            {
        //                Rectangle rg, fillrg;
        //                using (Brush gridBrush = new SolidBrush(Color.Black), WhiteBrush = new SolidBrush(Color.White), backColorBrush = new SolidBrush(Color.FromArgb(204, 255, 0)))
        //                {
        //                    e.PaintBackground(e.CellBounds, true);
        //                    using (Pen gridLinePen = new Pen(gridBrush))
        //                    {
        //                        // 画矩形框
        //                        rg = e.CellBounds;
        //                        rg.Inflate(-6, -5);
        //                        e.Graphics.FillRectangle(WhiteBrush, rg);
        //                        e.Graphics.DrawRectangle(gridLinePen, rg);
        //                        // 填充矩形进度条
        //                        decimal width = Convert.ToDecimal(rg.Width - 1);
        //                        width = width * Convert.ToDecimal(e.Value);
        //                        fillrg = new Rectangle(rg.X + 1, rg.Y + 1, Convert.ToInt32(width), rg.Height - 1);
        //                        e.Graphics.FillRectangle(backColorBrush, fillrg);
        //                    }
        //                    e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, gridBrush, rg.X + rg.Width / 2 - 9, rg.Y);
        //                    e.Handled = true;
        //                }
        //            }
        //        }
        //        Application.DoEvents();
        //    }
        //}

        //private void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    Ftp.Upload(this);
        //    SetDgvValue();
        //}

        //private void bgw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{
        //    progressBar1.Value = e.ProgressPercentage;
        //}

        #endregion


    }
}


