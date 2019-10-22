using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace FTPTool
{
    class DB
    {
        string strSQL = "";

        #region Site Profile

        public DataTable GetSiteProfile()
        {
            strSQL = "SELECT SiteName FROM SiteProfile";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, null);
            return dt;
        }

        public DataTable GetSiteProfileDetail(string SiteName)
        {
            strSQL = "SELECT Type,SiteIP,UserID,Password,RenameFile,Description,Port FROM SiteProfile WHERE SiteName=@SiteName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
            return dt;
        }

        public bool IsSiteProfileExist(string SiteName)
        {
            strSQL = "SELECT SiteIP FROM SiteProfile WHERE SiteName=@SiteName";
            using (DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) }))
            {
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void UpdateSiteProfile(string type,string SiteName, string SiteIP, string UserID, string Password,string RenameFile,string Description,string Port)
        {
            strSQL = "UPDATE SiteProfile SET Type=@type,SiteIP=@SiteIP,UserID=@UserID,Password=@Password,RenameFile=@RenameFile,Description=@Description,Port=@Port WHERE SiteName=@SiteName";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@type", type),
                                        new SQLiteParameter("@SiteIP", SiteIP),
                                        new SQLiteParameter("@UserID", UserID),
                                        new SQLiteParameter("@Password", Password),
                                        new SQLiteParameter("@SiteName", SiteName),
                                        new SQLiteParameter("@RenameFile", RenameFile),
                                        new SQLiteParameter("@Description", Description),
                                        new SQLiteParameter("@Port", Port)});
        }

        public void AddNewSiteProfile(string type,string SiteName, string SiteIP, string UserID, string Password, string RenameFile, string Description,string Port)
        {
            strSQL = "INSERT INTO SiteProfile(Type,SiteName,SiteIP,UserID,Password,RenameFile,Description,Port) VALUES (@type,@SiteName,@SiteIP,@UserID,@Password,@RenameFile,@Description,@Port)";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@type", type),
                                        new SQLiteParameter("@SiteIP", SiteIP),
                                        new SQLiteParameter("@UserID", UserID),
                                        new SQLiteParameter("@Password", Password),
                                        new SQLiteParameter("@SiteName", SiteName),
                                        new SQLiteParameter("@RenameFile", RenameFile),
                                        new SQLiteParameter("@Description", Description),
                                        new SQLiteParameter("@Port", Port)});
        }

        public void DeleteSiteProfile(string SiteName)
        {
            strSQL = "DELETE FROM SiteProfile WHERE SiteName=@SiteName";
            Sqlite.ExecuteNonQuery(strSQL,
                 new SQLiteParameter[] {new SQLiteParameter("@SiteName", SiteName)});
        }
        #endregion

        #region Action

        public DataTable GetActionType()
        {
            strSQL = "SELECT Action FROM ActionType";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, null);
            return dt;
        }

        public bool CheckActionIsAlreadyExist(string Action)
        {
            strSQL = "SELECT Action FROM ActionType WHERE Action=@Action";
            using (DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@Action", Action) }))
            {
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void AddNewActionType(string Action)
        {
            strSQL = "INSERT INTO ActionType(Action) VALUES (@Action)";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@Action", Action) });
        }

        public void UpdateActionName(string OldAction, string NewAction)
        {
            strSQL = "UPDATE ActionType SET Action=@NewAction WHERE Action=@OldAction";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@OldAction", OldAction) ,
                                         new SQLiteParameter("@NewAction", NewAction)});
            strSQL = "UPDATE Schedule_RunStep SET Action=@NewAction WHERE Action=@OldAction";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@OldAction", OldAction) ,
                                         new SQLiteParameter("@NewAction", NewAction)});
        }

        public bool CheckActionIsAlreadyUsed(string Action)
        {
            strSQL = "SELECT Action FROM Schedule_RunStep WHERE Action=@Action Limit 1";
            using (DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@Action", Action) }))
            {
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void DeleteAction(string Action)
        {
            strSQL = "DELETE FROM ActionType WHERE Action=@Action";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@Action", Action) });
        }

        #endregion

        #region Add New Schedule

        public bool CheckScheduleNameIsAlreadyExist(string SchName)
        {
            strSQL = "SELECT ScheduleName FROM Schedule_BaseData WHERE ScheduleName=@SchName";
            using (DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) }))
            {
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void AddNewScheduleBaseData(string SchName, string SiteProfile, string StartDateTime,
            string LastRunDateTime, string Repeat, string Status, string SuccessfulMail, string Mail,
            string ReconnectTimes, string ReconnectInterval, string FileExistAction)
        {
            strSQL = "INSERT INTO Schedule_BaseData(ScheduleName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,SuccessfulMail,Mail,ReconnectTimes,ReconnectInterval,FileExistAction)" +
                " VALUES (@SchName,@SiteProfile,@StartDateTime,@LastRunDateTime,@Repeat,@Status,@SuccessfulMail,@Mail,@ReconnectTimes,@ReconnectInterval,@FileExistAction)";
            Sqlite.ExecuteNonQuery(strSQL,
               new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName),
                                        new SQLiteParameter("@SiteProfile", SiteProfile),
                                        new SQLiteParameter("@StartDateTime", StartDateTime),
                                        new SQLiteParameter("@LastRunDateTime", LastRunDateTime),
                                        new SQLiteParameter("@Repeat", Repeat),
                                        new SQLiteParameter("@Status", Status),
                                        new SQLiteParameter("@SuccessfulMail",SuccessfulMail),
                                        new SQLiteParameter("@Mail", Mail),
                                        new SQLiteParameter("@ReconnectTimes",ReconnectTimes),
                                        new SQLiteParameter("@ReconnectInterval", ReconnectInterval),
                                        new SQLiteParameter("@FileExistAction", FileExistAction)});
                
        }

        public void AddNewScheduleActionData(string SchName, string Step, string Action, string RemoteFileFolder,
            string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            strSQL = "INSERT INTO Schedule_RunStep(ScheduleName,Step,Action,RemoteFileFolder,RemoteIsFolder"+
                ",LocalFileFolder,LocalIsFolder)" +
                " VALUES (@SchName,@Step,@Action,@RemoteFileFolder,@RemoteIsFolder,@LocalFileFolder,@LocalIsFolder)";
            Sqlite.ExecuteNonQuery(strSQL,
               new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName),
                                        new SQLiteParameter("@Step", Step),
                                        new SQLiteParameter("@Action", Action),
                                        new SQLiteParameter("@RemoteFileFolder", RemoteFileFolder),
                                        new SQLiteParameter("@RemoteIsFolder", RemoteIsFolder),
                                        new SQLiteParameter("@LocalFileFolder", LocalFileFolder),
                                        new SQLiteParameter("@LocalIsFolder", LocalIsFolder)});
        }

        #endregion

        #region Update Old Schedule

        public DataTable GetOldScheduleBaseData(string SchName)
        {
            strSQL = "SELECT StartDateTime,Repeat,Status,SuccessfulMail,Mail,ReconnectTimes,ReconnectInterval,FileExistAction FROM Schedule_BaseData WHERE ScheduleName=@SchName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            return dt;
        }

        public DataTable GetOldScheduleActionData(string SchName)
        {
            strSQL = "SELECT Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder FROM Schedule_RunStep WHERE ScheduleName=@SchName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            return dt;
        }

        public void UpdateScheduleBaseData(string SchName, string SiteProfile,string StartDateTime,
          string LastRunDateTime, string Repeat, string Status, string SuccessfulMail, string Mail, 
            string ReconnectTimes, string ReconnectInterval,string FileExistAction)
        {
            strSQL = "UPDATE Schedule_BaseData SET SiteProfile=@SiteProfile,StartDateTime=@StartDateTime,LastRunDateTime=@LastRunDateTime,Repeat=@Repeat,Status=@Status,SuccessfulMail=@SuccessfulMail,Mail=@Mail,ReconnectTimes=@ReconnectTimes,ReconnectInterval=@ReconnectInterval,FileExistAction=@FileExistAction" +
                " WHERE ScheduleName=@SchName";
            Sqlite.ExecuteNonQuery(strSQL,
               new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName),
                                        new SQLiteParameter("@StartDateTime", StartDateTime),
                                        new SQLiteParameter("@SiteProfile", SiteProfile),
                                        new SQLiteParameter("@LastRunDateTime", LastRunDateTime),
                                        new SQLiteParameter("@Repeat", Repeat),
                                        new SQLiteParameter("@Status", Status),
                                        new SQLiteParameter("@SuccessfulMail", SuccessfulMail),
                                        new SQLiteParameter("@Mail", Mail),
                                        new SQLiteParameter("@ReconnectTimes",ReconnectTimes),
                                        new SQLiteParameter("@ReconnectInterval", ReconnectInterval),
                                        new SQLiteParameter("@FileExistAction", FileExistAction)});

        }

        public void DeleteScheduleActionData(string SchName)
        {
            strSQL = "DELETE FROM Schedule_RunStep WHERE ScheduleName=@SchName";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
        }

        #endregion

        #region Delete Schedule

        public bool DeleteSchedule(string SchName)
        {
            try
            {
                strSQL = "DELETE FROM Schedule_RunStep WHERE ScheduleName=@SchName";
                Sqlite.ExecuteNonQuery(strSQL,
                    new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            }
            catch
            {
                return false;
            }
            /////////////////////////////////////////////////////////////////////////////////
            try
            {

                strSQL = "DELETE FROM Schedule_BaseData WHERE ScheduleName=@SchName";
                Sqlite.ExecuteNonQuery(strSQL,
                    new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion 



        #region Copy Schedule

        public bool IsScheduleExist(string SchName)
        {
            try
            {
                strSQL = "SELECT * FROM Schedule_BaseData WHERE ScheduleName=@SchName";
                using (DataTable dt = Sqlite.ExecuteDataTable(strSQL,
                    new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) }))
                {
                    if (dt.Rows.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return true;
            }
        }

        public void CopySchedule(string sSchName, string dSchName)
        {
            strSQL = "INSERT INTO Schedule_BaseData(ScheduleName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,SuccessfulMail,Mail,ReconnectTimes,ReconnectInterval,FileExistAction)"
                 + "SELECT @dSchName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,SuccessfulMail,Mail,ReconnectTimes,ReconnectInterval,FileExistAction FROM Schedule_BaseData WHERE ScheduleName=@sSchName";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@sSchName", sSchName), new SQLiteParameter("@dSchName", dSchName) });
            strSQL = "INSERT INTO Schedule_RunStep(ScheduleName,Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder)"
                 + " SELECT @dSchName,Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder FROM Schedule_RunStep WHERE ScheduleName=@sSchName";
            Sqlite.ExecuteNonQuery(strSQL,
                new SQLiteParameter[] { new SQLiteParameter("@sSchName", sSchName), new SQLiteParameter("@dSchName", dSchName) });
        }

        #endregion

        #region MainForm

        public DataTable GetMainFormScheduleList()
        {
            strSQL = "SELECT ScheduleName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,Mail,SuccessfulMail,FileExistAction FROM Schedule_BaseData";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL,null);
            return dt;
        }

        public DataTable GetScheduleToAdd(string schName)
        {
            strSQL = "SELECT ScheduleName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,Mail,SuccessfulMail,FileExistAction FROM Schedule_BaseData WHERE ScheduleName=@schName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@schName", schName) });
            return dt;
        }

        public void UpdateLastRunTime(string SchName,string Time)
        {
            strSQL = "UPDATE Schedule_BaseData SET LastRunDateTime=@Time WHERE ScheduleName=@SchName";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName),new SQLiteParameter("@Time",Time) });
        }

        #endregion

        #region Update Schedule Status

        public void UpdateScheduleStatus(string SchName, string Status)
        {
            strSQL = "UPDATE Schedule_BaseData SET Status=@Status WHERE ScheduleName=@SchName";
            Sqlite.ExecuteNonQuery(strSQL,
                   new SQLiteParameter[] { new SQLiteParameter("@Status", Status),
                       new SQLiteParameter("@SchName", SchName) });
        }

        #endregion

        #region Backup

        public DataTable GetSiteProfileToBackup()
        {
            strSQL = "SELECT Type,SiteName,SiteIP,UserID,Password,RenameFile,Description,Port FROM SiteProfile";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, null);
            return dt;
        }

        public DataSet GetScheduleToBackup()
        {
            strSQL = "SELECT * FROM Schedule_BaseData";
            DataTable dt1 = Sqlite.ExecuteDataTable(strSQL, null);
            dt1.TableName="Schedule_BaseData";
            strSQL = "SELECT * FROM Schedule_RunStep";
            DataTable dt2 = Sqlite.ExecuteDataTable(strSQL, null);
            dt2.TableName="Schedule_RunStep";
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            return ds;
        }

        public void DeleteActionRestoreTable()
        {
            strSQL = "DELETE FROM ActionType_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL,null);
        }

        public void AddRestoreItemIntoAction(string Action)
        {
            strSQL = "INSERT INTO ActionType_ForRestore(Action) VALUES (@Action)";
            Sqlite.ExecuteNonQuery(strSQL,
                 new SQLiteParameter[] { new SQLiteParameter("@Action", Action) });
        }

        public void MoveDataToAction()
        {
            strSQL = "DELETE FROM ActionType";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "INSERT INTO ActionType SELECT * FROM ActionType_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
        }

        public void DeleteScheduleRestoreTable()
        {
            strSQL = "DELETE FROM Schedule_BaseData_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "DELETE FROM Schedule_RunStep_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
        }

        public void AddRestoreItemIntoScheduleBaseData(string ScheduleName, string SiteProfile,
            string StartDateTime, string LastRunDateTime, string Repeat, string Status, string SuccessfulMail, string Mail, string ReconnectTimes, string ReconnectInterval, string FileExistAction)
        {
            strSQL = "INSERT INTO Schedule_BaseData_ForRestore(ScheduleName,SiteProfile,StartDateTime,LastRunDateTime,Repeat,Status,SuccessfulMail,Mail,ReconnectTimes,ReconnectInterval,FileExistAction)" +
                " VALUES (@ScheduleName,@SiteProfile,@StartDateTime,@LastRunDateTime,@Repeat,@Status,@SuccessfulMail,@Mail,@ReconnectTimes,@ReconnectInterval,@FileExistAction)";
            Sqlite.ExecuteNonQuery(strSQL,
                 new SQLiteParameter[] { new SQLiteParameter("@ScheduleName", ScheduleName),
                                         new SQLiteParameter("@SiteProfile", SiteProfile),
                                         new SQLiteParameter("@StartDateTime", StartDateTime),
                                         new SQLiteParameter("@LastRunDateTime", LastRunDateTime),
                                         new SQLiteParameter("@Repeat", Repeat),
                                         new SQLiteParameter("@Status", Status),
                                         new SQLiteParameter("@SuccessfulMail", SuccessfulMail),
                                         new SQLiteParameter("@Mail", Mail),
                                         new SQLiteParameter("@ReconnectTimes", ReconnectTimes),
                                         new SQLiteParameter("@ReconnectInterval", ReconnectInterval),
                                         new SQLiteParameter("@FileExistAction", FileExistAction)});
        }

        public void AddRestoreItemIntoScheduleRunStep(string ScheduleName, string Step, string Action,
           string RemoteFileFolder, string RemoteIsFolder, string LocalFileFolder, string LocalIsFolder)
        {
            strSQL = "INSERT INTO Schedule_RunStep_ForRestore(ScheduleName,Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder)" +
               " VALUES (@ScheduleName,@Step,@Action,@RemoteFileFolder,@RemoteIsFolder,@LocalFileFolder,@LocalIsFolder)";
            Sqlite.ExecuteNonQuery(strSQL,
                 new SQLiteParameter[] { new SQLiteParameter("@ScheduleName", ScheduleName),
                                         new SQLiteParameter("@Step", Step),
                                         new SQLiteParameter("@Action", Action),
                                         new SQLiteParameter("@RemoteFileFolder", RemoteFileFolder),
                                         new SQLiteParameter("@RemoteIsFolder", RemoteIsFolder),
                                         new SQLiteParameter("@LocalFileFolder", LocalFileFolder),
                                         new SQLiteParameter("@LocalIsFolder", LocalIsFolder)});
        }

        public void MoveDataToSchedule()
        {
            strSQL = "DELETE FROM Schedule_RunStep";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "INSERT INTO Schedule_RunStep SELECT * FROM Schedule_RunStep_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);

            strSQL = "DELETE FROM Schedule_BaseData";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "INSERT INTO Schedule_BaseData SELECT * FROM Schedule_BaseData_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
        }

        public void DeleteSiteProfileRestoreTable()
        {
            strSQL = "DELETE FROM SiteProfile_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
        }

        public void AddRestoreItemIntoSiteProfile(string type,string SiteName, string SiteIP, string UserID, string Password, string RenameFile, string Description,string Port)
        {
            strSQL = "INSERT INTO SiteProfile_ForRestore(Type,SiteName,SiteIP,UserID,Password,RenameFile,Description,Port)" +
                " VALUES (@type,@SiteName,@SiteIP,@UserID,@Password,@RenameFile,@Description,@Port)";
            Sqlite.ExecuteNonQuery(strSQL,
                 new SQLiteParameter[] { new SQLiteParameter("@type", type),
                                         new SQLiteParameter("@SiteName", SiteName),
                                         new SQLiteParameter("@SiteIP", SiteIP),
                                         new SQLiteParameter("@UserID", UserID),
                                         new SQLiteParameter("@Password", Password),
                                         new SQLiteParameter("@RenameFile", RenameFile),
                                         new SQLiteParameter("@Description", Description),
                                         new SQLiteParameter("@Port", Port)});
        }

        public void MoveDataToSiteProfile()
        {
            strSQL = "DELETE FROM SiteProfile";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "INSERT INTO SiteProfile SELECT * FROM SiteProfile_ForRestore";
            Sqlite.ExecuteNonQuery(strSQL, null);
        }

        #endregion

        #region Mail

        public DataTable GetMailAccountSetting()
        {
            strSQL = "SELECT MailFrom,SMTP,Account,Password FROM Mail";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, null);
            return dt;
        }

        public void SaveMailSetting(string MailFrom, string SMTP, string Account, string Password)
        {
            strSQL = "DELETE FROM Mail";
            Sqlite.ExecuteNonQuery(strSQL, null);
            strSQL = "INSERT INTO Mail(MailFrom,SMTP,Account,Password) VALUES (@MailFrom,@SMTP,@Account,@Password)";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[]{new SQLiteParameter("@MailFrom",MailFrom),
                                                                new SQLiteParameter("@SMTP",SMTP),
                                                                new SQLiteParameter("@Account",MailFrom),
                                                                new SQLiteParameter("@Password",Password)});
        }

        #endregion

        #region ForBLL

        public DataTable GetBLLSchRunStep(string SchName)
        {
            //strSQL = "SELECT Step,Action,RemoteFileFolder,RemoteIsFolder,LocalFileFolder,LocalIsFolder FROM Schedule_RunStep WHERE ScheduleName=@SchName ORDER BY Step";
            strSQL = "SELECT a.Step,a.Action,a.RemoteFileFolder,a.RemoteIsFolder,a.LocalFileFolder,a.LocalIsFolder,b.FileExistAction "
                    + " FROM Schedule_RunStep a INNER JOIN Schedule_BaseData b ON a.ScheduleName = b.ScheduleName AND a.ScheduleName=@SchName ORDER BY a.Step";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            return dt;
        }

        public DataTable GetBLLSiteProfile(string SchName)
        {
            strSQL = "SELECT Type,SiteName,SiteIP,UserID,Password,RenameFile,Port FROM SiteProfile WHERE SiteName =(SELECT SiteProfile FROM Schedule_BaseData WHERE ScheduleName=@SchName)";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            return dt;
        }

        public DataTable GetReconnectTimesAndInterval(string SchName)
        {
            strSQL = "SELECT ReconnectTimes,ReconnectInterval FROM Schedule_BaseData WHERE ScheduleName=@SchName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SchName", SchName) });
            return dt;
        }

        public bool CheckDownloadIsFinished(string SiteName)
        {
            strSQL = "SELECT TOP 1 FileNameWithoutPath FROM DownloadBreakpoint WHERE SiteName=@SiteName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
            if (dt.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetDownloadBreakpointResumeFileList(string SiteName)
        {
            strSQL = "SELECT FtpPath,FileNameWithoutPath,LocalPath,Type,FileSize,FtpFullFilePath,LocalFullFilePath,ModifyDate FROM DownloadBreakpoint WHERE SiteName=@SiteName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
            return dt;
        }

        public void DeleteAllUnfinishedDownloadItem(string SiteName)
        {
            strSQL = "DELETE FROM DownloadBreakpoint WHERE SiteName=@SiteName";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
        }

        public void SaveDownloadlist(string SiteName, string Ftppath, string Filenamewithoutpath, string Localpath, string Type, string Filesize, string FtpFullFilePath, string LocalFullFilePath, string ModifyDate)
        {
            strSQL = "SELECT * FROM DownloadBreakpoint WHERE SiteName=@SiteName AND FtpPath=@Ftppath AND FileNameWithoutPath=@Filenamewithoutpath AND LocalPath=@Localpath AND FileSize=@Filesize AND FtpFullFilePath=@FtpFullFilePath";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                                   new SQLiteParameter("@Ftppath",Ftppath),
                                                                                   new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                                   new SQLiteParameter("@Localpath",Localpath),
                                                                                   new SQLiteParameter("@Filesize",Filesize),
                                                                                   new SQLiteParameter("@FtpFullFilePath",FtpFullFilePath)});
            if (dt.Rows.Count == 0)
            {
                Addnewdownloaditem(SiteName, Ftppath, Filenamewithoutpath, Localpath, Type, Filesize, FtpFullFilePath, LocalFullFilePath, ModifyDate);
            }
            else
            {
                strSQL = "DELETE FROM DownloadBreakpoint WHERE SiteName=@SiteName AND FtpPath=@Ftppath AND FileNameWithoutPath=@Filenamewithoutpath AND LocalPath=@Localpath";
                Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                       new SQLiteParameter("@Ftppath",Ftppath),
                                                                       new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                       new SQLiteParameter("@Localpath",Localpath)});
                Addnewdownloaditem(SiteName, Ftppath, Filenamewithoutpath, Localpath, Type, Filesize, FtpFullFilePath, LocalFullFilePath, ModifyDate);
            }
        }

        public void Addnewdownloaditem(string SiteName, string Ftppath, string Filenamewithoutpath, string Localpath, string Type, string Filesize, string FtpFullFilePath, string LocalFullFilePath, string ModifyDate)
        {
            strSQL = "INSERT INTO DownloadBreakpoint(SiteName,FtpPath,FileNameWithoutPath,LocalPath,Type,FileSize,FtpFullFilePath,LocalFullFilePath,ModifyDate) VALUES (@SiteName,@Ftppath,@Filenamewithoutpath,@Localpath,@Type,@Filesize,@FtpFullFilePath,@LocalFullFilePath,@ModifyDate)";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                       new SQLiteParameter("@Ftppath",Ftppath),
                                                                       new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                       new SQLiteParameter("@Localpath",Localpath),
                                                                       new SQLiteParameter("@Type",Type),
                                                                       new SQLiteParameter("@Filesize",Filesize),
                                                                       new SQLiteParameter("@FtpFullFilePath",FtpFullFilePath),
                                                                       new SQLiteParameter("@LocalFullFilePath",LocalFullFilePath),
                                                                       new SQLiteParameter("@ModifyDate",ModifyDate)});
        }

        public void Deletedownloaditem(string SiteName, string Ftppath, string Filenamewithoutpath)
        {
            strSQL = "DELETE FROM DownloadBreakpoint WHERE SiteName=@SiteName AND FtpPath=@Ftppath AND FileNameWithoutPath=@Filenamewithoutpath";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                                   new SQLiteParameter("@Ftppath",Ftppath),
                                                                                   new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath)});
        }




        public bool CheckUploadIsFinished(string SiteName)
        {
            strSQL = "SELECT TOP 1 FileNameWithoutPath FROM UploadBreakpoint WHERE SiteName=@SiteName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
            if (dt.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetUploadBreakpointResumeFileList(string SiteName)
        {
            strSQL = "SELECT LocalPath,FileNameWithoutPath,FtpPath,Type,FileSize,LocalFullFilePath,FtpFullFilePath,ModifyDate FROM UploadBreakpoint WHERE SiteName=@SiteName";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
            return dt;
        }

        public void DeleteAllUnfinishedUploadItem(string SiteName)
        {
            strSQL = "DELETE FROM UploadBreakpoint WHERE SiteName=@SiteName";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName", SiteName) });
        }

        public void SaveUploadlist(string SiteName, string Localpath, string Filenamewithoutpath, string Ftppath, string Type, string Filesize, string LocalFullFilePath, string FtpFullFilePath, string ModifyDate)
        {
            strSQL = "SELECT * FROM UploadBreakpoint WHERE SiteName=@SiteName AND LocalPath=@Localpath AND FileNameWithoutPath=@Filenamewithoutpath AND FtpPath=@Ftppath AND FileSize=@Filesize AND LocalFullFilePath=@LocalFullFilePath";
            DataTable dt = Sqlite.ExecuteDataTable(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                                   new SQLiteParameter("@Ftppath",Ftppath),
                                                                                   new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                                   new SQLiteParameter("@Localpath",Localpath),
                                                                                   new SQLiteParameter("@Filesize",Filesize),
                                                                                   new SQLiteParameter("@LocalFullFilePath",LocalFullFilePath)});
            if (dt.Rows.Count == 0)
            {
                Addnewuploaditem(SiteName, Ftppath, Filenamewithoutpath, Localpath, Type, Filesize, FtpFullFilePath, LocalFullFilePath, ModifyDate);
            }
            else
            {
                strSQL = "DELETE FROM UploadBreakpoint WHERE SiteName=@SiteName AND FtpPath=@Ftppath AND FileNameWithoutPath=@Filenamewithoutpath AND LocalPath=@Localpath";
                Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                       new SQLiteParameter("@Ftppath",Ftppath),
                                                                       new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                       new SQLiteParameter("@Localpath",Localpath)});
                Addnewuploaditem(SiteName, Ftppath, Filenamewithoutpath, Localpath, Type, Filesize, FtpFullFilePath, LocalFullFilePath, ModifyDate);
            }
        }

        public void Addnewuploaditem(string SiteName, string Ftppath, string Filenamewithoutpath, string Localpath, string Type, string Filesize, string FtpFullFilePath, string LocalFullFilePath, string ModifyDate)
        {
            strSQL = "INSERT INTO UploadBreakpoint(SiteName,FtpPath,FileNameWithoutPath,LocalPath,Type,FileSize,FtpFullFilePath,LocalFullFilePath,ModifyDate) VALUES (@SiteName,@Ftppath,@Filenamewithoutpath,@Localpath,@Type,@Filesize,@FtpFullFilePath,@LocalFullFilePath,@ModifyDate)";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                       new SQLiteParameter("@Ftppath",Ftppath),
                                                                       new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath),
                                                                       new SQLiteParameter("@Localpath",Localpath),
                                                                       new SQLiteParameter("@Type",Type),
                                                                       new SQLiteParameter("@Filesize",Filesize),
                                                                       new SQLiteParameter("@FtpFullFilePath",FtpFullFilePath),
                                                                       new SQLiteParameter("@LocalFullFilePath",LocalFullFilePath),
                                                                       new SQLiteParameter("@ModifyDate",ModifyDate)});
        }

        public void DeleteUploaditem(string SiteName, string Localpath, string Filenamewithoutpath)
        {
            strSQL = "DELETE FROM UploadBreakpoint WHERE SiteName=@SiteName AND LocalPath=@Localpath AND FileNameWithoutPath=@Filenamewithoutpath";
            Sqlite.ExecuteNonQuery(strSQL, new SQLiteParameter[] { new SQLiteParameter("@SiteName",SiteName),
                                                                                   new SQLiteParameter("@Localpath",Localpath),
                                                                                   new SQLiteParameter("@Filenamewithoutpath",Filenamewithoutpath)});
        }


        #endregion

    }
}
