using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using Oracle.ManagedDataAccess.Client;


namespace Liteon.Mes.Db
{
    ///// <summary>
    ///// 傳參結構體
    ///// </summary>
    //public struct ProcParam
    //{
    //    public string param;
    //    public string value;
    //    public DirectionType directionType;
    //    public ParamType paramType;
    //    public int paramLength;
    //}
    ///// <summary>
    ///// Oracle Procedure參數是IN還是OUT
    ///// </summary>
    //public enum DirectionType : int
    //{
    //    IN = 1,
    //    OUT = 2,
    //}
    ///// <summary>
    ///// Oracle數據類型枚舉
    ///// </summary>
    //public enum ParamType : int
    //{
    //    VARCHAR = 1,
    //    VARCHAR2 = 2,
    //    NUMBER = 3,
    //    NVARCHAR = 4,
    //    CURSOR = 5,
    //    DATETIME = 6,
    //    CHAR = 7,
    //    TIMESTAMP = 8,
    //    DOUBLE = 9,
    //    FLOAT = 10,
    //    INT16 = 11,
    //    INT32 = 12,
    //    CLOB = 13,
    //    LONGRAW = 16,
    //    NCHAR = 18,
    //    NCLOB = 19,
    //    RAW = 20,
    //    ROWID = 21
    //}

    /// <summary>
    /// 處理Oracle連接查詢的公用類
    /// </summary>
    public class Oracle
    {
        /// <summary>
        /// 連接字符串
        /// </summary>
        private static string CONN_STRING = "";
        private OracleConnection conn = null;
        //private OracleTransaction tran = null;

        ///// <summary>
        ///// 輸入規定值，產生一個參數的結構體
        ///// </summary>
        ///// <param name="param">參數名</param>
        ///// <param name="value">參數值，OUTPUT的請填""</param>
        ///// <param name="directionType">參數是IN還是OUT</param>
        ///// <param name="paramType">參數類型</param>
        ///// <param name="paramLength">參數長度，未知請填0</param>
        ///// <returns>返回一個結構體，後面存放到結構體數組中傳給執行過程</returns>
        //public static ProcParam CreateOneProcParam(string param, string value, DirectionType directionType, ParamType paramType, int paramLength)
        //{
        //    ProcParam p = new ProcParam();
        //    p.param = param.Trim();
        //    p.value = value.Trim();
        //    p.directionType = directionType;
        //    p.paramType = paramType;
        //    p.paramLength = paramLength;
        //    return p;
        //}
        /// <summary>
        /// 獲取連接字符串
        /// </summary>
        public static string GET_CONNSTR
        {
            get { return CONN_STRING; }
        }
        #region Prepare SQL parameters

        private static string DesDecrypt(string DecryptString)
        {
            try
            {
                byte[] inputByteArray = Convert.FromBase64String(DecryptString);
                System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();
                des.Key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                des.IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                System.Security.Cryptography.CryptoStream cStream = 
                    new System.Security.Cryptography.CryptoStream(mStream, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return System.Text.Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 創建查詢command命令
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="type">cmd類型是SP還是TEXT</param>
        /// <param name="ops">OracleParament数组;</param>
        /// <returns>返回查詢命令</returns>
        private OracleCommand GetExecCommand(string sql, string type, OracleParameter[] ops)
        {
            conn = CreateConnection();
            OracleCommand cmd = new OracleCommand(sql, conn);
            if (type == "SP")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }

            if (ops != null)
            {
                cmd.BindByName = true;
                cmd.Parameters.AddRange(ops);
            }
            return cmd;
        }

        /// <summary>
        /// 通過二維數組產生查詢參數，適用於直接SQL查詢，不適用於存儲過程
        /// </summary>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns>返回查詢參數數組</returns>
        private OracleParameter[] CreateSQLParam(string[,] paramArray)
        {
            try
            {
                OracleParameter[] paramList = new OracleParameter[paramArray.Length / 2];
                for (int i = 0; i < paramArray.Length / 2; i++)
                {
                    paramList[i] = new OracleParameter();
                    paramList[i].OracleDbType = OracleDbType.Varchar2;
                    paramList[i].ParameterName = paramArray[i, 0].ToString();
                    paramList[i].Value = paramArray[i, 1].ToString();
                }
                return paramList;
            }
            catch
            {
                return null;
            }
        }

        ///// <summary>
        ///// 通過結構體數組產生查詢參數數組
        ///// </summary>
        ///// <param name="paramArray">查詢參數結構體數組</param>
        ///// <returns>返回查詢參數數組</returns>
        //private OracleParameter[] CreateProcParamStruct(ProcParam[] paramArray)
        //{
        //    try
        //    {
        //        OracleParameter[] paramList = new OracleParameter[paramArray.Length];
        //        for (int i = 0; i < paramArray.Length; i++)
        //        {
        //            #region set OracleType
        //            OracleDbType o_type;
        //            switch (paramArray[i].paramType)
        //            {
        //                case ParamType.VARCHAR:
        //                case ParamType.VARCHAR2:
        //                    o_type = OracleDbType.Varchar2;
        //                    break;
        //                case ParamType.NUMBER:
        //                    o_type = OracleDbType.Long;
        //                    break;
        //                case ParamType.NVARCHAR:
        //                    o_type = OracleDbType.NVarchar2;
        //                    break;
        //                case ParamType.CURSOR:
        //                    o_type = OracleDbType.RefCursor;
        //                    break;
        //                case ParamType.DATETIME:
        //                    o_type = OracleDbType.Date;
        //                    break;
        //                case ParamType.CHAR:
        //                    o_type = OracleDbType.Char;
        //                    break;
        //                case ParamType.TIMESTAMP:
        //                    o_type = OracleDbType.TimeStamp;
        //                    break;
        //                case ParamType.DOUBLE:
        //                    o_type = OracleDbType.Double;
        //                    break;
        //                case ParamType.FLOAT:
        //                    o_type = OracleDbType.Double;
        //                    break;
        //                case ParamType.INT16:
        //                    o_type = OracleDbType.Int16;
        //                    break;
        //                case ParamType.INT32:
        //                    o_type = OracleDbType.Int32;
        //                    break;
        //                case ParamType.CLOB:
        //                    o_type = OracleDbType.Clob;
        //                    break;
        //                case ParamType.LONGRAW:
        //                    o_type = OracleDbType.LongRaw;
        //                    break;
        //                case ParamType.NCHAR:
        //                    o_type = OracleDbType.NChar;
        //                    break;
        //                case ParamType.NCLOB:
        //                    o_type = OracleDbType.NClob;
        //                    break;
        //                case ParamType.RAW:
        //                    o_type = OracleDbType.Raw;
        //                    break;
        //                default:
        //                    o_type = OracleDbType.Varchar2;
        //                    break;
        //            }
        //            #endregion
        //            if (paramArray[i].paramLength > 0)
        //            {
        //                paramList[i] = new OracleParameter(paramArray[i].param, o_type, paramArray[i].paramLength);
        //            }
        //            else
        //            {
        //                paramList[i] = new OracleParameter(paramArray[i].param, o_type);
        //            }
        //            if (DirectionType.IN == paramArray[i].directionType)
        //            {
        //                paramList[i].Direction = ParameterDirection.Input;
        //                paramList[i].Value = paramArray[i].value;
        //            }
        //            else
        //            {
        //                paramList[i].Direction = ParameterDirection.Output;
        //            }
        //        }
        //        return paramList;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region Prepare SQL connection

        /// <summary>
        /// 產生連接字符串
        /// </summary>
        /// <param name="connectionString">完整的連接字符串</param>
        public static void CreateConnectionString(string connectionString)
        {
            CONN_STRING = connectionString;
        }

        /// <summary>
        /// 產生連接字符串，默認協議TCP，端口1521
        /// </summary>
        /// <param name="xmlFileName">連接配置XML文件，放在程序根目錄下</param>
        public static void CreateConnectionStringByXMLFile(string xmlFileName)
        {
            string file = System.Windows.Forms.Application.StartupPath + "\\" + xmlFileName;
            if (System.IO.File.Exists(file) == false)
            {
                throw new Exception("沒有找到連接配置文件：\r\n" + file);
            }

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(file);
                string hostName = ds.Tables["Config"].Rows[0]["HostName"].ToString();
                string serviceName = ds.Tables["Config"].Rows[0]["ServiceName"].ToString();
                string account = DesDecrypt(ds.Tables["Config"].Rows[0]["Account"].ToString());
                string password = DesDecrypt(ds.Tables["Config"].Rows[0]["Password"].ToString());

                CreateConnectionString(hostName, serviceName, account, password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 產生連接字符串
        /// </summary>
        /// <param name="host">主機名稱</param>
        /// <param name="serviceName">服務名稱</param>
        /// <param name="protocol">協議</param>
        /// <param name="port">端口</param>
        /// <param name="uid">帳號</param>
        /// <param name="pwd">密碼</param>
        public static void CreateConnectionString(string host, string serviceName, string protocol, string port, string uid, string pwd)
        {
            string tmp = "(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=" + protocol.Trim() + ")(HOST=" + host.Trim() + ")(PORT=" + port.Trim() + ")))(CONNECT_DATA=(SERVICE_NAME= " + serviceName.Trim() + ")))";
            CONN_STRING = string.Format("data source={0};user id={1};password={2}", tmp, uid.Trim(), pwd.Trim());
        }


        /// <summary>
        /// 產生連接字符串，默認協議TCP，端口1521
        /// </summary>
        /// <param name="host">主機名稱</param>
        /// <param name="serviceName">服務名稱</param>
        /// <param name="uid">帳號</param>
        /// <param name="pwd">密碼</param>
        public static void CreateConnectionString(string host, string serviceName, string uid, string pwd)
        {
            CreateConnectionString(host, serviceName, "TCP", "1521", uid, pwd);
        }

        //public void Commit()
        //{
        //    tran.Commit();
        //    Close();
        //}

        //public void Rollback()
        //{
        //    tran.Rollback();
        //    Close();
        //}

        /// <summary>
        /// 關閉資料庫連接
        /// </summary>
        private void Close()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    conn = null;
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// 創建資料庫連接
        /// </summary>
        /// <returns>返回數據庫連接</returns>
        private OracleConnection CreateConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                return conn;
            }
            else
            {
                try
                {
                    conn = null;
                    if (String.IsNullOrEmpty(CONN_STRING))
                    {
                        throw new Exception("Connection string is null or empty");
                    }
                    conn = new OracleConnection(CONN_STRING);
                    conn.Open();
                    return conn;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                    //MessageBox.Show(ex.Message, "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return conn;
                }
            }
        }

        #endregion

        #region Execute stored procedure command

        ///// <summary>
        ///// 創建查詢command命令
        ///// </summary>
        ///// <param name="sql">查詢語句</param>
        ///// <param name="paramArray">查詢參數結構體數組;</param>
        ///// <returns>返回查詢命令</returns>
        //private OracleCommand GetExecProcCommandStruct(string sql, ProcParam[] paramArray)
        //{
        //    conn = CreateConnection();
        //    OracleCommand cmd = new OracleCommand(sql, conn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    if (paramArray != null)
        //    {
        //        cmd.Parameters.AddRange(CreateProcParamStruct(paramArray));
        //    }
        //    return cmd;
        //}



        ///// <summary>
        ///// 執行存儲過程，返回DataSet，必須有CURSOR類型的OUT參數，必須執行Package.Procedure
        ///// </summary>
        ///// <param name="sql">Package.Procedure的名字</param>
        ///// <param name="paramArray">輸入參數，結構體數組</param>
        ///// <returns>返回DataSet，有幾個CURSOR的OUT參數就有幾個DataSet</returns>
        //private DataSet ExecProcReturnDataSetByStruct(string sql, ProcParam[] paramArray)
        //{
        //    bool hasCursorOutType = false;
        //    for (int i = 0; i < paramArray.Length; i++)
        //    {
        //        if (paramArray[i].directionType == DirectionType.OUT && paramArray[i].paramType == ParamType.CURSOR)
        //        {
        //            hasCursorOutType = true;
        //            break;
        //        }
        //    }
        //    //沒有輸出類型的CURSOR參數，直接返回空
        //    if (!hasCursorOutType)
        //    {
        //        return null;
        //    }
        //    DataSet ds = null;
        //    OracleCommand cmd = null;
        //    try
        //    {
        //        cmd = GetExecProcCommandStruct(sql, paramArray);
        //        OracleDataAdapter da = new OracleDataAdapter(cmd);
        //        ds = new DataSet();
        //        da.Fill(ds);
        //        da.Dispose();
        //        da = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (cmd != null)
        //        {
        //            cmd.Dispose();
        //            cmd = null;
        //        }
        //        Close();
        //    }
        //    return ds;
        //}

        /// <summary>
        /// 執行存儲過程，返回DataSet，必須有CURSOR類型的OUT參數，必須執行Package.Procedure
        /// </summary>
        /// <param name="sql">Package.Procedure的名字</param>
        /// <param name="ops">輸入參數,OracleParameter數組</param>
        /// <returns>返回DataSet，有幾個CURSOR的OUT參數就有幾個DataSet</returns>
        public DataSet ExecProcReturnDataSet(string sql, OracleParameter[] ops)
        {
            bool hasCursorOutType = false;

            //沒有輸出類型的CURSOR參數，直接返回空
            if (!hasCursorOutType)
            {
                if (ops != null)
                {
                    foreach (OracleParameter p in ops)
                    {
                        if (p.OracleDbType == OracleDbType.RefCursor)
                        {
                            hasCursorOutType = true;
                            break;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            DataSet ds = null;
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql, "SP", ops);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                da = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return ds;
        }

        ///// <summary>
        ///// 執行存儲過程，返回DataTable，只有一行數據，每一列就是一個OUT參數的返回值
        ///// </summary>
        ///// <param name="sql">存儲過程的名字</param>
        ///// <param name="paramArray">輸入參數，結構體數組</param>
        ///// <returns>返回一個DataTable，只有一行，每一列就是一個OUT參數的返回值</returns>
        //private DataTable ExecProcReturnOutputByStruct(string sql, ProcParam[] paramArray)
        //{
        //    //先產生一個DataTable，裏面包含了OUTPUT的所有參數，每個參數一列
        //    DataTable dt = new DataTable();
        //    bool hasCursorOutType = false;
        //    ArrayList outParamList = new ArrayList();
        //    for (int i = 0; i < paramArray.Length; i++)
        //    {
        //        if (paramArray[i].directionType == DirectionType.OUT)
        //        {
        //            hasCursorOutType = true;
        //            //把列加進去
        //            outParamList.Add(paramArray[i].param);
        //            dt.Columns.Add(paramArray[i].param);
        //        }
        //    }
        //    //沒有輸出類型的CURSOR參數，直接返回空
        //    if (!hasCursorOutType)
        //    {
        //        return null;
        //    }

        //    OracleCommand cmd = null;
        //    try
        //    {
        //        cmd = GetExecProcCommandStruct(sql, paramArray);
        //        cmd.ExecuteNonQuery();
        //        DataRow dr = dt.NewRow();
        //        foreach (string outParam in outParamList)
        //        {
        //            try
        //            { dr[outParam] = cmd.Parameters[outParam].Value.ToString(); }
        //            catch
        //            {
        //                dr[outParam] = "";
        //            }
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (cmd != null)
        //        {
        //            cmd.Dispose();
        //            cmd = null;
        //        }
        //        Close();
        //    }
        //    return dt;
        //}

        /// <summary>
        /// 執行存儲過程，返回DataTable，只有一行數據，每一列就是一個OUT參數的返回值
        /// </summary>
        /// <param name="sql">存儲過程的名字</param>
        /// <param name="ops">輸入參數,OracleParameter數組</param>
        /// <returns>返回一個DataTable，只有一行，每一列就是一個OUT參數的返回值</returns>
        public DataTable ExecProcReturnOutput(string sql, OracleParameter[] ops)
        {
            //先產生一個DataTable，裏面包含了OUTPUT的所有參數，每個參數一列
            DataTable dt = new DataTable();
            bool hasCursorOutType = false;
            ArrayList outParamList = new ArrayList();
            for (int i = 0; i < ops.Length; i++)
            {
                if (ops[i].Direction == ParameterDirection.Output)
                {
                    hasCursorOutType = true;
                    //把列加進去
                    outParamList.Add(ops[i].ParameterName);
                    dt.Columns.Add(ops[i].ParameterName);
                }
            }
            //沒有輸出類型的CURSOR參數，直接返回空
            if (!hasCursorOutType)
            {
                return null;
            }

            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql,"SP", ops);
                cmd.ExecuteNonQuery();
                DataRow dr = dt.NewRow();
                foreach (string outParam in outParamList)
                {
                    try
                    { dr[outParam] = cmd.Parameters[outParam].Value.ToString(); }
                    catch
                    {
                        dr[outParam] = "";
                    }
                }
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return dt;
        }

        ///// <summary>
        ///// 執行存儲過程沒有返回值
        ///// </summary>
        ///// <param name="sql">存儲過程的名字</param>
        ///// <param name="paramArray">查詢參數結構體數組</param>
        ///// <returns>沒有返回值</returns>
        //private void ExecProcNonQueryByStruct(string sql, ProcParam[] paramArray)
        //{
        //    OracleCommand cmd = null;
        //    try
        //    {
        //        cmd = GetExecProcCommandStruct(sql, paramArray);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (cmd != null)
        //        {
        //            cmd.Dispose();
        //            cmd = null;
        //        }
        //        Close();
        //    }
        //}

        /// <summary>
        /// 執行存儲過程沒有返回值
        /// </summary>
        /// <param name="sql">存儲過程的名字</param>
        /// <param name="ops">查詢參數OracleParameter数组</param>
        /// <returns>沒有返回值</returns>
        public void ExecProcNonQuery(string sql, OracleParameter[] ops)
        {
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql,"SP", ops);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
        }

        /// <summary>
        /// 執行存儲過程沒有返回值
        /// </summary>
        /// <param name="sql">存儲過程的名字</param>
        /// <returns>沒有返回值</returns>
        public void ExecProcNonQuery(string sql)
        {
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql,"SP", null);
                //cmd = GetExecProcCommandStruct(sql, null);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
        }

        #endregion

        #region Execute SQL command

        /// <summary>
        /// 創建查詢command命令
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns></returns>
        private OracleCommand GetExecSQLCommand(string sql, string[,] paramArray)
        {
            conn = CreateConnection();
            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            
            if (paramArray != null)
            {
                cmd.BindByName = true;
                cmd.Parameters.AddRange(CreateSQLParam(paramArray));
            }
            return cmd;
        }

        /// <summary>
        /// 執行SQL返回DataTable
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns></returns>
        public DataTable ExecSQLReturnDataTable(string sql, string[,] paramArray)
        {
            DataTable dt = null;
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                dt = new DataTable("ResultTable");
                da.Fill(dt);
                da.Dispose();
                da = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return dt;
        }

        /// <summary>
        /// 執行SQL返回DataTable
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="ops">查詢參數OracleParameter数组</param>
        /// <returns></returns>
        public DataTable ExecSQLReturnDataTable2(string sql, OracleParameter[] ops)
        {
            DataTable dt = null;
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql, "TEXT", ops);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                dt = new DataTable("ResultTable");
                da.Fill(dt);
                da.Dispose();
                da = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return dt;
        }

        /// <summary>
        /// 執行SQL返回DataTable
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <returns></returns>
        public DataTable ExecSQLReturnDataTable(string sql)
        {
            return this.ExecSQLReturnDataTable(sql, null);
        }

        /// <summary>
        /// 執行SQL返回DataSet
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns></returns>
        public DataSet ExecSQLReturnDataSet(string sql, string[,] paramArray)
        {
            DataSet ds = null;
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                da = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return ds;
        }

        /// <summary>
        /// 執行SQL返回DataSet
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="ops">查詢參數OracleParameter数组</param>
        /// <returns></returns>
        public DataSet ExecSQLReturnDataSet2(string sql, OracleParameter[] ops)
        {
            DataSet ds = null;
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql, "TEXT", ops);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                da = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
            return ds;
        }

        /// <summary>
        /// 執行SQL返回DataSet
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <returns></returns>
        public DataSet ExecSQLReturnDataSet(string sql)
        {
            return this.ExecSQLReturnDataSet(sql, null);
        }

        /// <summary>
        /// 執行不帶返回值的SQL
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        public void ExecSQLNonQuery(string sql, string[,] paramArray)
        {
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
        }

        /// <summary>
        /// 執行不帶返回值的SQL
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="ops">查詢參數OracleParameter数组</param>
        public void ExecSQLNonQuery2(string sql, OracleParameter[] ops)
        {
            OracleCommand cmd = null;
            try
            {
                cmd = GetExecCommand(sql,"TEXT", ops);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                Close();
            }
        }

        /// <summary>
        /// 執行不帶返回值的SQL
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <returns></returns>
        public void ExecSQLNonQuery(string sql)
        {
            this.ExecSQLNonQuery(sql, null);
        }

        #endregion
    }
}

#region old
//private OracleType MappingOracleType(string typeString)
//{
//    switch(typeString.ToUpper().Trim())
//    {

//    }ParamType.CHAR
//}

///// <summary>
///// 通過二維數組產生查詢參數
///// </summary>
///// <param name="paramArray">查詢參數四維數組，string[,] paraArray={{{{"p1","v1",DirectionType,ParamType}}}};</param>
///// <returns></returns>
//private OracleParameter[] CreateProcParam(string[,,,] paramArray)
//{
//               try
//    {
//        OracleParameter[] paramList = new OracleParameter[paramArray.Length / 4];
//        for (int i = 0; i < paramArray.Length / 4; i++)
//        {
//            paramList[i] = new OracleParameter();
//            paramList[i].ParameterName = paramArray[i,0,0,0].ToString();
//            #region set OracleType
//            switch (paramArray[i, 0, 0, 3])
//            {
//                case "VARCHAR":
//                case "VARCHAR2":
//                    paramList[i].OracleType = OracleType.VarChar;
//                    break;
//                case "NUMBER":
//                    paramList[i].OracleType = OracleType.Number;
//                    break;
//                case "NVARCHAR":
//                    paramList[i].OracleType = OracleType.NVarChar;
//                    break;
//                case "CURSOR":
//                    paramList[i].OracleType = OracleType.Cursor;
//                    break;
//                case "DATETIME":
//                    paramList[i].OracleType = OracleType.DateTime;
//                    break;
//                case "CHAR":
//                    paramList[i].OracleType = OracleType.Char;
//                    break;
//                case "TIMESTAMP":
//                    paramList[i].OracleType = OracleType.Timestamp;
//                    break;
//                case "DOUBLE":
//                    paramList[i].OracleType = OracleType.Double;
//                    break;
//                case "FLOAT":
//                    paramList[i].OracleType = OracleType.Float;
//                    break;
//                case "INT16":
//                    paramList[i].OracleType = OracleType.Int16;
//                    break;
//                case "INT32":
//                    paramList[i].OracleType = OracleType.Int32;
//                    break;
//                case "CLOB":
//                    paramList[i].OracleType = OracleType.Clob;
//                    break;
//                case "INTERVAL_DAY_TO_SECOND":
//                    paramList[i].OracleType = OracleType.IntervalDayToSecond;
//                    break;
//                case "INTERVAL_YEAR_TO_MONTH":
//                    paramList[i].OracleType = OracleType.IntervalYearToMonth;
//                    break;
//                case "LONGRAW":
//                    paramList[i].OracleType = OracleType.LongRaw;
//                    break;
//                case "LONGVARCHAR":
//                    paramList[i].OracleType = OracleType.LongVarChar;
//                    break;
//                case "NCHAR":
//                    paramList[i].OracleType = OracleType.NChar;
//                    break;
//                case "NCLOB":
//                    paramList[i].OracleType = OracleType.NClob;
//                    break;
//                case "RAW":
//                    paramList[i].OracleType = OracleType.Raw;
//                    break;
//                case "ROWID":
//                    paramList[i].OracleType = OracleType.RowId;
//                    break;
//                case "SBYTE":
//                    paramList[i].OracleType = OracleType.SByte;
//                    break;
//                case "TIMESTAMPLOCAL":
//                    paramList[i].OracleType = OracleType.TimestampLocal;
//                    break;
//                case "UINT16":
//                    paramList[i].OracleType = OracleType.UInt16;
//                    break;
//                case "UINT32":
//                    paramList[i].OracleType = OracleType.UInt32;
//                    break;
//                default:
//                    paramList[i].OracleType = OracleType.VarChar;
//                    break;
//            }
//            #endregion
//            if ("IN" == paramArray[i, 0, 0, 2])
//            {
//                paramList[i].Direction = ParameterDirection.Input;
//                paramList[i].Value = paramArray[i, 0, 0, 1].ToString();
//            }
//            else
//            {
//                paramList[i].Direction = ParameterDirection.Output;
//            }
//        }
//        return paramList;
//    }
//    catch
//    {
//        return null;
//    }
//}
///// <summary>
///// 創建查詢command命令
///// </summary>
///// <param name="sql">查詢語句</param>
///// <param name="paramArray">查詢參數四維數組，string[,] paraArray={{{{"p1","v1",DirectionType,ParamType}}}};</param>
///// <returns></returns>
//private OracleCommand GetExecProcCommand(string sql, string[,,,] paramArray)
//{
//    conn = CreateConnection();
//    OracleCommand cmd = new OracleCommand(sql, conn);
//    cmd.CommandType = CommandType.StoredProcedure;

//    if (paramArray != null)
//    {
//        cmd.Parameters.AddRange(CreateProcParam(paramArray));
//    }
//    return cmd;
//}
///// <summary>
///// 執行存儲過程沒有返回值
///// </summary>
///// <param name="sql">存儲過程的名字</param>
///// <param name="paramArray">查詢參數四維數組，string[,] paraArray={{{{"p1","v1",DirectionType,ParamType}}}};</param>
///// <returns></returns>
//public void ExecProcNonQuery_A(string sql, string[, , ,] paramArray)
//{
//    OracleCommand cmd = null;
//    try
//    {
//        cmd = GetExecProcCommand(sql, paramArray);
//        cmd.ExecuteNonQuery();
//    }
//    catch
//    {
//        throw;
//    }
//    finally
//    {
//        if (cmd != null)
//        {
//            cmd.Dispose();
//            cmd = null;
//        }
//        Close();
//    }
//}

///// <summary>
//   /// 執行存儲過程，返回DataTable，只有一行數據，每一列就是一個OUT參數的返回值
//   /// </summary>
//   /// <param name="sql">存儲過程的名字</param>
//   /// <param name="paramArray">輸入參數，四維數組</param>
//   /// <returns></returns>
//   public DataTable ExecProcReturnOutput_A(string sql, string[, , ,] paramArray)
//   {
//       //先產生一個DataTable，裏面包含了OUTPUT的所有參數，每個參數一列
//       DataTable dt = new DataTable();
//       bool hasCursorOutType = false;
//       ArrayList outParamList = new ArrayList();
//       for (int i = 0; i < paramArray.Length / 4; i++)
//       {
//           if (paramArray[i, 0, 0, 2] == DirectionType.OUT.ToString())
//           {
//               hasCursorOutType = true;
//               //把列加進去
//               outParamList.Add(paramArray[i, 0, 0, 0]);
//               dt.Columns.Add(paramArray[i, 0, 0, 0]);
//           }
//       }
//       //沒有輸出類型的CURSOR參數，直接返回空
//       if (!hasCursorOutType)
//       {
//           return null;
//       }

//       OracleCommand cmd = null;
//       try
//       {
//           cmd = GetExecProcCommand(sql, paramArray);
//           cmd.ExecuteNonQuery();
//           DataRow dr = dt.NewRow();
//           foreach (string outParam in outParamList)
//           {
//               try
//               { dr[outParam] = cmd.Parameters[outParam].Value.ToString(); }
//               catch
//               {
//                   dr[outParam] = "";
//               }
//           }
//           dt.Rows.Add(dr);
//       }
//       catch
//       {
//           throw;
//       }
//       finally
//       {
//           if (cmd != null)
//           {
//               cmd.Dispose();
//               cmd = null;
//           }
//           Close();
//       }
//       return dt;
//   }

///// <summary>
// /// 執行存儲過程，返回DataSet，必須有CURSOR類型的OUT參數，必須執行Package.Procedure
// /// </summary>
// /// <param name="sql">Package.Procedure的名字</param>
// /// <param name="paramArray">輸入參數，四位數組</param>
// /// <returns>返回DataSet，有幾個CURSOR的OUT參數就有幾個DataSet</returns>
// public DataSet ExecProcReturnDataSet_A(string sql, string[,,,] paramArray)
// {
//     bool hasCursorOutType = false;
//     for (int i = 0; i < paramArray.Length / 4; i++)
//     {
//         if (paramArray[i, 0, 0, 2] == DirectionType.OUT.ToString() && paramArray[i, 0, 0, 3] == ParamType.CURSOR.ToString())
//         {
//             hasCursorOutType=true;
//             break;
//         }
//     }
//     //沒有輸出類型的CURSOR參數，直接返回空
//     if (!hasCursorOutType)
//     {
//         return null;
//     }
//     DataSet ds = null;
//     OracleCommand cmd = null;
//     try
//     {
//         cmd = GetExecProcCommand(sql, paramArray);
//         OracleDataAdapter da = new OracleDataAdapter(cmd);
//         ds = new DataSet();
//         da.Fill(ds);
//         da.Dispose();
//         da = null;
//     }
//     catch
//     {
//         throw;
//     }
//     finally
//     {
//         if (cmd != null)
//         {
//             cmd.Dispose();
//             cmd = null;
//         }
//         Close();
//     }
//     return ds;
// }
#endregion