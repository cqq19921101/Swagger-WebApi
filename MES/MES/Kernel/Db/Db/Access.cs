using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace Liteon.Mes.Db
{
    /// <summary>
    /// 處理Access連接查詢的公用類
    /// </summary>
    public class Access
    {
        private static string CONN_STRING = "";
        private OleDbConnection conn = null;

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

        private OleDbParameter[] CreateSQLParam(string[,] paramArray)
        {
            OleDbParameter[] paramList = new OleDbParameter[paramArray.Length / 2];
            for (int i = 0; i < paramArray.Length / 2; i++)
            {
                paramList[i] = new OleDbParameter();
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
        /// 產生連接字符串，XML以Config為一個Table，裏面需要有DBName,LocalPath（不含DB名，放空則默認程序路徑）,Password（加密后的）三個節點
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
                string dbName = ds.Tables["Config"].Rows[0]["DBName"].ToString();
                string localPath = ds.Tables["Config"].Rows[0]["LocalPath"].ToString();
                if (String.IsNullOrEmpty(localPath))
                {
                    localPath = Application.StartupPath;
                }
                string pwd = DesDecrypt(ds.Tables["Config"].Rows[0]["Password"].ToString());
                
                CreateConnectionString(dbName, localPath, pwd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 產生連接字符串
        /// </summary>
        /// <param name="dbName">數據庫名稱，全稱，含mdb後綴</param>
        /// <param name="localPath">本地路徑，不包含數據庫名稱，放空則默認為程序路徑</param>
        /// <param name="pwd">密碼</param>
        public static void CreateConnectionString(string dbName, string localPath, string pwd)
        {
            string connString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + localPath + "\\" + dbName + ";Jet OLEDB:Database Password=" + pwd;
            CONN_STRING = connString;
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
                    catch (Exception ex)
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
        private OleDbConnection CreateConnection()
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
                    conn = new OleDbConnection(CONN_STRING);
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



        #region Execute SQL command

        /// <summary>
        /// 獲取當前數據庫中所有的表名稱
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTableNamesInThisDataBase()
        {
            try
            {
                conn = CreateConnection();
                return conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 獲取當前數據庫中指定表的所有列名稱
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetAllColumnNamesInThisTable(string table)
        {
            try
            {
                conn = CreateConnection();
                return conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, table, null });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 創建查詢command命令
        /// </summary>
        /// <param name="sql">查詢語句</param>
        /// <param name="paramArray">查詢參數二維數組，string[,] paraArray={{"p1","v1"},{"p2","v2"}};</param>
        /// <returns></returns>
        private OleDbCommand GetExecSQLCommand(string sql, string[,] paramArray)
        {
            conn = CreateConnection();
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

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
            OleDbCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
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
            OleDbCommand cmd = null;
            try
            {
                cmd = GetExecSQLCommand(sql, paramArray);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
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
            OleDbCommand cmd = null;
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
        /// <returns></returns>
        public void ExecSQLNonQuery(string sql)
        {
            this.ExecSQLNonQuery(sql, null);
        }

        #endregion
    }
}
