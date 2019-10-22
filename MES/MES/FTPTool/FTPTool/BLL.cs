using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;

namespace FTPTool
{
    class BLL
    { 
        #region 反射
        //    public void TestReflection()
    //    {
    //        System.Reflection.Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\ActionBLL.dll");
    //        //加载DLL
    //        System.Type t = ass.GetType("ActionBLL.RunExternalProgram");//获得类型
    //        object o = System.Activator.CreateInstance(t);//创建实例

    //        System.Reflection.MethodInfo mi = t.GetMethod("ShowMsg");//获得方法

    //        mi.Invoke(o, null);
        //    }
        #endregion

        DB DB = new DB();
        private string SchName = "";

        public BLL(string _SchName)
        {
            SchName = _SchName;
        }

        public void RunStepByStep(frmMain fm, int iRow)
        {
            string logname = "Log_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";

            string type,sitename,siteip,userid,pwd,renamefile,port;
            string connectresult = "";
            string Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder,FileExistAction;
            try
            {
                Tool.SaveLog(logname, "ScheduleName: " + SchName + "\t\tstart");

                using (DataTable dt = DB.GetBLLSchRunStep(SchName))
                {
                    DataTable dtSiteProfile = DB.GetBLLSiteProfile(SchName);
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("Can't find a site profile for this schedule");
                    }
                    type = dtSiteProfile.Rows[0]["Type"].ToString();
                    sitename = dtSiteProfile.Rows[0]["SiteName"].ToString();
                    siteip = dtSiteProfile.Rows[0]["SiteIP"].ToString();
                    userid = dtSiteProfile.Rows[0]["UserID"].ToString();
                    pwd = Security.Decrypt(dtSiteProfile.Rows[0]["Password"].ToString());
                    renamefile = dtSiteProfile.Rows[0]["RenameFile"].ToString();
                    port = dtSiteProfile.Rows[0]["Port"].ToString();

                    //先测试一下连接是否成功，如果不成功看一下是否有设置重连次数，有的话按时间间隔重连
                    #region 重连
                    //string logfilename = "Log_View_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                    
                    int i = 1;
                    ActionBLL.Action testconnect = null;
                    while (i <= Para.RECONNECTTIMES)
                    {
                        testconnect = ActionBLL.GetAction.GetActionType("TestFtpConnection");
                        connectresult = testconnect.RunAction(fm, type, sitename, siteip, userid, pwd, port);
                        if (connectresult != "OK")
                        {
                            //Tool.SaveLog(logfilename, "Test connect reconnect-->" + i.ToString());
                            i++;
                            continue;
                        }
                        else
                        {
                            break;
                        }

                    }
                    testconnect = null;
                    //MessageBox.Show(connectresult);
                    //return;
                    if (connectresult != "OK")
                    {
                        fm.SetMessage(iRow, "Connect Error:" + connectresult, 0);
                        Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", "ScheduleName: " + SchName + "\t" + connectresult);
                        using (DataTable dtReconnect = DB.GetReconnectTimesAndInterval(SchName))
                        {
                            if (dtReconnect.Rows.Count > 0)
                            {
                                int retimes = Convert.ToInt32(dtReconnect.Rows[0]["ReconnectTimes"].ToString());
                                if (retimes > 0)
                                {
                                    int reinterval = Convert.ToInt32(dtReconnect.Rows[0]["ReconnectInterval"].ToString());
                                    int ireconnecttimes = 1;
                                    while (ireconnecttimes <= retimes)
                                    {
                                        string errormessage = "Connect Error: It'll try connect to " + type + " at "
                                            + DateTime.Now.AddMinutes(reinterval).ToString("HH:mm:ss")
                                            + " for the " + ireconnecttimes.ToString() + "/" + retimes.ToString() + " times";
                                        fm.SetMessage(iRow, errormessage, 0);                                        
                                        Tool.SaveErrorLog("ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt", "ScheduleName: " + SchName + "\t" + errormessage);
                                        errormessage = null;
                                        Application.DoEvents();
                                        System.Threading.Thread.Sleep(60000 * reinterval);
                                        i = 1;
                                        while (i <= Para.RECONNECTTIMES)
                                        {
                                            testconnect = ActionBLL.GetAction.GetActionType("TestFtpConnection");
                                            connectresult = testconnect.RunAction(fm, type, sitename, siteip, userid, pwd, port);
                                            if (connectresult != "OK")
                                            {
                                                //Tool.SaveLog(logfilename, "Test connect reconnect-->" + i.ToString());
                                                i++;
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }

                                        }

                                        //testconnect = ActionBLL.GetAction.GetActionType("TestFtpConnection");
                                        //connectresult = testconnect.RunAction(fm, type, sitename, siteip, userid, pwd, port);
                                        ireconnecttimes++;
                                        if (connectresult == "OK")
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (connectresult != "OK")
                    {
                        throw new Exception("get connection fail-->" + connectresult);
                    }
                    #endregion

                    foreach (DataRow dr in dt.Rows)
                    {

                        Step = dr["Step"].ToString();
                        Action = dr["Action"].ToString();
                        RemoteFileFolder = dr["RemoteFileFolder"].ToString();
                        RemoteIsFolder = dr["RemoteIsFolder"].ToString();
                        LocalFileFolder = dr["LocalFileFolder"].ToString();
                        LocalIsFolder = dr["LocalIsFolder"].ToString();
                        FileExistAction = dr["FileExistAction"].ToString();
                        Tool.SaveLog(logname, "ScheduleName: " + SchName + "\tAction: " + Action
                                + "\tstart\tStep:" + Step + "\tRemoteFileFolder:" + RemoteFileFolder
                                + "\tRemoteIsFolder:" + RemoteIsFolder
                                + "\tLocalFileFolder:" + LocalFileFolder
                                + "\tLocalIsFolder:" + LocalIsFolder
                                + "\tFileExistAction:" + FileExistAction);
                        ActionBLL.Action OnStepAction = ActionBLL.GetAction.GetActionType(Action);
                        try
                        {
                            if (Action == "Upload" || Action == "Download")
                            {
                                OnStepAction.RunAction(fm, iRow, type, sitename, siteip, userid, pwd, port, renamefile, FileExistAction, Action, RemoteFileFolder, RemoteIsFolder, LocalFileFolder, LocalIsFolder);
                            }
                            else if (Action == "MoveLocalFileToBackupFolder")
                            {
                                OnStepAction.RunAction(fm, iRow, RemoteFileFolder, LocalIsFolder, LocalFileFolder, "Y");
                            }
                            else
                            {
                                OnStepAction.RunAction(fm, iRow, type, sitename, siteip, userid, pwd, port, Action, RemoteFileFolder, RemoteIsFolder, LocalFileFolder, LocalIsFolder);
                            }
                            Tool.SaveLog(logname, "ScheduleName: " + SchName + "\tAction: " + Action + "\tfinish");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Step " + Step + " error: " + ex.Message);
                        }
                        finally
                        {
                            OnStepAction = null;
                        }
                    }
                }

                Tool.SaveLog(logname, "ScheduleName: " + SchName + "\t\tfinish");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                logname = null;
                type = null;sitename = null; siteip = null; userid = null; pwd = null; renamefile = null; port = null;
                connectresult = null;
                Step = null; Action = null; RemoteFileFolder = null; RemoteIsFolder = null; LocalFileFolder = null;
                LocalIsFolder = null; FileExistAction = null;
                GC.Collect();
            }
        }
    }
}
