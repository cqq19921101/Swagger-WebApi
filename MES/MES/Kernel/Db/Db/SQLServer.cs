using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Liteon.Mes.Db
{
    /// <summary>
    /// 處理SQL Server連接查詢的公用類
    /// </summary>
    public class SQLServer
    {
        
        private static string CONN_STRING = "";
        private SqlConnection conn = null;

        /// <summary>
        /// 獲取連接字符串
        /// </summary>
        public static string GET_CONNSTR
        {
            get { return CONN_STRING; }
        }

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

        private SqlParameter[] CreateSQLParam(string[,] paramArray)
        {
            SqlParameter[] paramList = new SqlParameter[paramArray.Length / 2];
            for (int i = 0; i < paramArray.Length / 2; i++)
            {
                paramList[i] = new SqlParameter();
                paramList[i].ParameterName = paramArray[i, 0].ToString();
                paramList[i].Value = paramArray[i, 1].ToString();
            }
            return paramList;
        }
        
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
                string serverIP = ds.Tables["Config"].Rows[0]["HostName"].ToString();
                string dbName = ds.Tables["Config"].Rows[0]["ServiceName"].ToString();
                string uid = DesDecrypt(ds.Tables["Config"].Rows[0]["Account"].ToString());
                string pwd = DesDecrypt(ds.Tables["Config"].Rows[0]["Password"].ToString());
                int timeOut;
                try
                {
                    timeOut = Convert.ToInt32(ds.Tables["Config"].Rows[0]["Timeout"].ToString());
                }
                catch
                {
                    timeOut = 0;
                }

                CreateConnectionString(serverIP, dbName, uid, pwd, timeOut);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 產生連接字符串
        /// </summary>
        /// <param name="serverIP">服務器IP</param>
        /// <param name="dbName">數據名</param>
        /// <param name="uid">用戶名</param>
        /// <param name="pwd">密碼</param>
        /// <param name="timeOut">連接超時時間，輸入0則使用默認時間</param>
        public static void CreateConnectionString(string serverIP, string dbName, string uid, string pwd, int timeOut)
        {
            string connString = "Database=" + dbName + ";Server=" + serverIP + ";Uid=" + uid + ";Pwd=" + pwd + ";Network Library=DBMSSOCN;MultipleActiveResultSets=true";

            if (timeOut >= 0)
            {
                connString += ";Connect Timeout=" + timeOut.ToString();
            }

            CONN_STRING = connString;
            //QMSSDK.Db.Connections.cn = new SqlConnection(connString);
            //QMSSDK.Db.Connections.cn.Open();
        }

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
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    conn = null;
                }
            }
        }

        /// <summary>
        /// 創建資料庫連接
        /// </summary>
        /// <returns>返回數據庫連接</returns>
        private SqlConnection CreateConnection()
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
                    conn = new SqlConnection(CONN_STRING);
                    conn.Open();
                    return conn;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                    //MessageBox.Show(ex.Message, "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return conn;
                }
            }
        }



        #region Execute SQL command

        /// <summary>
        /// 創建查詢command命令
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns></returns>
        private SqlCommand GetExecSQLCommand(string sql, string[,] paramArray)
        {
            conn = CreateConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            if (paramArray != null)
            {
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
            SqlCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable("ResultTable");
                //以防萬一連接被關閉
                if(conn!=null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(dt);
                da.Dispose();
                da = null;
            }
            catch(Exception ex)
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
        /// <param name="paramArray">SqlParameter[]</param>
        /// <returns></returns>
        public DataTable ExecSQLReturnDataTable2(string sql, SqlParameter[] paramArray)
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            try
            {
                conn = CreateConnection();
                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 600;
                if (paramArray != null)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable("ResultTable");
                //以防萬一連接被關閉
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
            SqlCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                //以防萬一連接被關閉
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
        /// <param name="paramArray">SqlParameter</param>
        /// <returns></returns>
        public DataSet ExecSQLReturnDataSet2(string sql, SqlParameter[] paramArray)
        {
            DataSet ds = null;
            SqlCommand cmd = null;
            try
            {
                conn = CreateConnection();
                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 600;
                if (paramArray != null)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                //以防萬一連接被關閉
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
            SqlCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                //以防萬一連接被關閉
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
        /// <param name="paramArray">SqlParameter</param>
        public void ExecSQLNonQuery2(string sql, SqlParameter[] paramArray)
        {
            SqlCommand cmd = null;
            try
            {
                conn = CreateConnection();
                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 600;
                if (paramArray != null)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                //以防萬一連接被關閉
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
