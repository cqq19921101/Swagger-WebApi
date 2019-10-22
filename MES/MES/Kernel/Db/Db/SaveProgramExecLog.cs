using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Net;
using System.Management;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Threading;

namespace Liteon.Mes.Db
{
    /// <summary>
    /// Db type
    /// </summary>
    public enum DbType : int
    {
        Oracle = 1,
        SqlServer = 2,
    }

    /// <summary>
    /// 保存執行的LOG
    /// </summary>
    public class SaveProgramExecLog
    {
        private class ParaClass
        {
            public ParaClass(DbType _dbType, string _logType, string _program, string _function,
                string _error_message, string _exception_trace, string _param_value, bool _saveScreenShot, string _startTime)
            {
                this.dbType = _dbType;
                this.logType = _logType;
                this.program = _program;
                this.function = _function;
                this.error_message = _error_message;
                this.exception_trace = _exception_trace;
                this.param_value = _param_value;
                this.saveScreenShot = _saveScreenShot;
                this.startTime = _startTime;
            }

            public DbType dbType { get; set; }
            public string logType { get; set; }
            public string program { get; set; }
            public string function { get; set; }
            public string error_message { get; set; }
            public string exception_trace { get; set; }
            public string param_value { get; set; }
            public bool saveScreenShot { get; set; }
            public string startTime { get; set; }
        }

        /// <summary>
        /// 保存正常執行的LOG
        /// </summary>
        /// <param name="t">數據庫類型，枚舉類型</param>
        /// <param name="program">執行程序</param>
        /// <param name="startTime">程序執行時間，手動填寫，依據情況填寫日期格式（按記錄時間的具體需求寫）</param>
        /// <returns></returns>
        public static void DoSaveNormalLog(DbType t, string program, string startTime)
        {
            DoSaveLog(t, "0", program.Trim(), startTime, "", "", "", "", false);
        }

        /// <summary>
        /// 保存正常執行的LOG
        /// </summary>
        /// <param name="t">數據庫類型，枚舉類型</param>
        /// <param name="program">執行程序</param>
        /// <returns></returns>
        public static void DoSaveNormalLog(DbType t, string program)
        {
            DoSaveLog(t, "0", program.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "", "", "", false);
        }

        /// <summary>
        /// 保存報錯的LOG
        /// </summary>
        /// <param name="t">數據庫類型，枚舉類型</param>
        /// <param name="program">執行程序</param>
        /// <param name="startTime">程序執行時間，手動填寫，依據情況填寫日期格式（按記錄時間的具體需求寫）</param>
        /// <param name="function">語句塊所屬function</param>
        /// <param name="error_message">報錯信息，ex.Message</param>
        /// <param name="exception_trace">報錯詳細信息，ex.ToString()</param>
        /// <param name="param_value">當前執行的參數值</param>
        /// <param name="saveScreenShot">是否保存截屏</param>
        /// <returns></returns>
        public static void DoSaveErrorLog(DbType t, string program, string startTime, string function, string error_message,
            string exception_trace, string param_value, bool saveScreenShot)
        {
            DoSaveLog(t, "1", program.Trim(), startTime, function.Trim(), error_message.Trim(),
                exception_trace.Trim(), param_value.Trim(), saveScreenShot);
        }

        /// <summary>
        /// 保存報錯的LOG
        /// </summary>
        /// <param name="t">數據庫類型，枚舉類型</param>
        /// <param name="program">執行程序</param>
        /// <param name="function">語句塊所屬function</param>
        /// <param name="error_message">報錯信息，ex.Message</param>
        /// <param name="exception_trace">報錯詳細信息，ex.ToString()</param>
        /// <param name="param_value">當前執行的參數值</param>
        /// <param name="saveScreenShot">是否保存截屏</param>
        /// <returns></returns>
        public static void DoSaveErrorLog(DbType t, string program, string function, string error_message,
            string exception_trace, string param_value, bool saveScreenShot)
        {
            DoSaveLog(t, "1", program.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), function.Trim(), error_message.Trim(),
                exception_trace.Trim(), param_value.Trim(), saveScreenShot);
        }

        private static void DoSaveLog(DbType t, string logType, string program,string startTime, string function, string error_message,
            string exception_trace, string param_value, bool saveScreenShot)
        {
            ParaClass pC = new ParaClass(t, logType, TruncateString(program, 50), TruncateString(function, 50),
                TruncateString(error_message, 2000), TruncateString(exception_trace, 2000), TruncateString(param_value, 2000),
                saveScreenShot, startTime);
            Thread th = new Thread(new ParameterizedThreadStart(SaveToDb));
            th.Start(pC);
            //return SaveToDb(t, logType, program, function, error_message, exception_trace, param_value, saveScreenShot, clientName);
        }

        private static string TruncateString(string input, int length)
        {
            return input.Length > length ? input.Substring(0, length) : input;
        }

        private static Oracle GetOracleDbInstance()
        {
            return new Oracle();
        }

        private static SQLServer GetSqlServerDbInstance()
        {
            return new SQLServer();
        }

        private static string GetDbName(DbType t)
        {
            try
            {
                string sql = "";
                DataTable dt = new DataTable();
                if (t == DbType.Oracle)
                {
                    Oracle oracle = GetOracleDbInstance();
                    sql = "SELECT INSTANCE_NAME FROM v$instance";
                    dt = oracle.ExecSQLReturnDataTable(sql);
                }
                else
                {
                    SQLServer sqlserver = GetSqlServerDbInstance();
                    sql = "SELECT DB_NAME() AS DBName";
                    dt = sqlserver.ExecSQLReturnDataTable(sql);
                }
                if (dt.Rows.Count == 0)
                {
                    return "";
                }
                else
                {
                    return dt.Rows[0][0].ToString();
                }
            }
            catch
            {
                return "";
            }
        }

        private static string GetClientName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return "";
            }
        }

        private static string GetLocalIP()
        {
            try
            {
                IPHostEntry IPHE = new IPHostEntry();
                IPHE = Dns.GetHostEntry(Dns.GetHostName());
                string ips = "";
                for (int i = 0; i < IPHE.AddressList.Length; i++)
                {
                    if (IPHE.AddressList[i].ToString().Length < 7)
                    {
                        continue;
                    }
                    if (IPHE.AddressList[i].ToString().IndexOf(":") > 0)
                    {
                        continue;
                    }
                    ips = ips + IPHE.AddressList[i].ToString() + ";";
                    return ips.Substring(0, ips.Length - 1);
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        //private static string SaveToDb(DbType t, string recordType, string program, 
        //    string function, string error_message, string exception_trace, string param_value, bool saveScreenShot,string client)
        private static void SaveToDb(object p)    
        {
            ParaClass pC = (ParaClass)p;

            string dbName = TruncateString(GetDbName(pC.dbType),50);
            string clientName = TruncateString(GetClientName(),50);
            string ip = TruncateString(GetLocalIP(), 50);
            try
            {
                string sql = "";
                byte[] b = null;
                if (pC.saveScreenShot)
                {
                    b = GetScreenSnapShot();
                }

                if (pC.dbType == DbType.Oracle)
                {
                    Oracle oracle = GetOracleDbInstance();
                    //sql = "INSERT INTO LITEON.DNS_PROGRAM_EXEC_LOG(DB_NAME,RECORD_TYPE,PROGRAM_NAME,FUNCTION_NAME,ERROR_MESSAGE,"
                    //    + "EXCEPTION_TRACE,PARAM_VALUE,IP,CLIENT_NAME,SCREEN_SHOT,LOG_DATETIME) VALUES ("
                    //    + ":thisDbName,:thisRecordType,:thisProgName,:thisFuncName,:thisErrMsg,"
                    //    + ":thisExcept,:thisPara,:thisIP,:thisClient,:thisScreen,SYSDATE)";
                    sql = "LITEON.USP_SAVE_PROGRAM_LOG";
                    OracleParameter[] ops = { 
                        new OracleParameter("v_type",OracleDbType.Varchar2,0,pC.logType,ParameterDirection.Input),
                        new OracleParameter("v_progName",OracleDbType.Varchar2,0,pC.program,ParameterDirection.Input),
                        new OracleParameter("v_funcName",OracleDbType.Varchar2,0,pC.function,ParameterDirection.Input),
                        new OracleParameter("v_errMsg",OracleDbType.Varchar2,0,pC.error_message,ParameterDirection.Input),
                        new OracleParameter("v_exMsg",OracleDbType.Varchar2,0,pC.exception_trace,ParameterDirection.Input),
                        new OracleParameter("v_param",OracleDbType.Varchar2,0,pC.param_value,ParameterDirection.Input),
                        new OracleParameter("v_IP",OracleDbType.Varchar2,0,ip,ParameterDirection.Input),
                        new OracleParameter("v_client",OracleDbType.Varchar2,0,clientName,ParameterDirection.Input),
                        new OracleParameter("v_img",OracleDbType.Blob,0,b,ParameterDirection.Input),
                        new OracleParameter("v_startTime",OracleDbType.Varchar2,0,pC.startTime,ParameterDirection.Input)
                    };
                    oracle.ExecProcNonQuery(sql, ops);
                    //oracle.ExecSQLNonQuery2(sql, ops);
                }
                else if (pC.dbType == DbType.SqlServer)
                {
                    SQLServer sqlserver = GetSqlServerDbInstance();
                    //sql = "INSERT INTO DNS_PROGRAM_EXEC_LOG(DB_NAME,RECORD_TYPE,PROGRAM_NAME,FUNCTION_NAME,ERROR_MESSAGE,"
                    //    + "EXCEPTION_TRACE,PARAM_VALUE,IP,CLIENT_NAME,SCREEN_SHOT,LOG_DATETIME) VALUES ("
                    //    + "@dbName,@recordType,@program,@funcName,@errmsg,"
                    //    + "@except,@params,@ip,@client,@SnapShot,GETDATE())";
                    sql = "EXEC USP_SAVE_PROGRAM_LOG @type,@progName,@funcName,@errMsg,@exMsg,@param,@IP,@client,@img,@startTime";
                    SqlParameter[] paras = new SqlParameter[10];

                    paras[0] = new SqlParameter("@type", SqlDbType.VarChar, 1);
                    paras[0].Value = pC.logType;
                    paras[1] = new SqlParameter("@progName", SqlDbType.NVarChar, 50);
                    paras[1].Value = pC.program;
                    paras[2] = new SqlParameter("@funcName", SqlDbType.NVarChar, 50);
                    paras[2].Value = pC.function;
                    paras[3] = new SqlParameter("@errMsg", SqlDbType.NVarChar, 2000);
                    paras[3].Value = pC.error_message;
                    paras[4] = new SqlParameter("@exMsg", SqlDbType.NVarChar, 2000);
                    paras[4].Value = pC.exception_trace;
                    paras[5] = new SqlParameter("@param", SqlDbType.NVarChar, 2000);
                    paras[5].Value = pC.param_value;
                    paras[6] = new SqlParameter("@IP", SqlDbType.NVarChar, 50);
                    paras[6].Value = ip;
                    paras[7] = new SqlParameter("@client", SqlDbType.NVarChar, 50);
                    paras[7].Value = clientName;
                    paras[8] = new SqlParameter("@img", SqlDbType.Image);
                    paras[9] = new SqlParameter("@startTime", SqlDbType.VarChar, 20);
                    paras[9].Value = pC.startTime;
                    if (b == null)
                    {
                        paras[8].Value = DBNull.Value;
                    }
                    else
                    {
                        paras[8].Value = b;
                    }
                    sqlserver.ExecSQLNonQuery2(sql, paras);
                }
                else
                {

                }
                //return "OK";
            }
            catch//(Exception ex)
            {
                //return ex.Message;
            }
            finally
            {
                GC.Collect();
            }
        }

        private static byte[] GetScreenSnapShot()
        {
            try
            {
                byte[] b = null;

                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                System.IO.Stream ms = new System.IO.MemoryStream();
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0,
                        bitmap.Size, CopyPixelOperation.SourceCopy);
                    g.Save();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                b = new byte[ms.Length];
                ms.Read(b, 0, b.Length);
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
