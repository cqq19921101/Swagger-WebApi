using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Threading;

namespace FTPTool
{


    public class Sqlite
    {
        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <returns></returns>
        /// 
        private static SQLiteConnection GetSQLiteConnection()
        {
            return new SQLiteConnection("Data Source="+Application.StartupPath+"\\MySetting.db3");
        }

        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, string cmdText, SQLiteParameter[] parameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (parameters != null)
            {
                //foreach (object parm in p)
                //{
                //    cmd.Parameters.AddWithValue(string.Empty, parm);
                //}
                cmd.Parameters.AddRange(parameters);
            }
        }

        public static DataTable ExecuteDataTable(string cmdText, SQLiteParameter[] parameters)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = new SQLiteCommand();
            try
            {
                using (SQLiteConnection connection = GetSQLiteConnection())
                {
                    PrepareCommand(command, connection, cmdText, parameters);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                    da.Fill(dt);

                    return dt;
                }
            }
            catch
            {
                Thread.Sleep(3000);
                try
                {
                    using (SQLiteConnection connection = GetSQLiteConnection())
                    {
                        PrepareCommand(command, connection, cmdText, parameters);
                        SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                        da.Fill(dt);

                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteDataTable error-->" + ex.Message);
                }
            }
        }

        public static DataSet ExecuteDataset(string cmdText, SQLiteParameter[] parameters)
        {
            DataSet ds = new DataSet();
            SQLiteCommand command = new SQLiteCommand();
            try
            {
                using (SQLiteConnection connection = GetSQLiteConnection())
                {
                    PrepareCommand(command, connection, cmdText, parameters);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                    da.Fill(ds);

                    return ds;
                }
            }
            catch
            {
                Thread.Sleep(3000);
                try
                {
                    using (SQLiteConnection connection = GetSQLiteConnection())
                    {
                        PrepareCommand(command, connection, cmdText, parameters);
                        SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                        da.Fill(ds);

                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteDataset error-->" + ex.Message);
                }
            }
        }

        public static DataRow ExecuteDataRow(string cmdText, SQLiteParameter[] parameters)
        {
            try
            {
                DataSet ds = ExecuteDataset(cmdText, parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                Thread.Sleep(3000);
                try
                {
                    DataSet ds = ExecuteDataset(cmdText, parameters);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteDataRow error-->" + ex.Message);
                }
            }
        }

        ///// <summary>
        ///// 返回受影响的行数
        ///// </summary>
        ///// <param name="cmdText">a</param>
        ///// <param name="commandParameters">传入的参数</param>
        ///// <returns></returns>
        //public static int ExecuteNonQuery(string cmdText, params object[] p)
        //{
        //    SQLiteCommand command = new SQLiteCommand();
        //    using (SQLiteConnection connection = GetSQLiteConnection())
        //    {
        //        PrepareCommand(command, connection, cmdText, p);
        //        return command.ExecuteNonQuery();
        //    }
        //}

        /// <summary>
        /// 执行无返回SQL
        /// </summary>
        /// <param name="cmdText">a</param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static void ExecuteNonQuery(string cmdText, SQLiteParameter[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand();
            try
            {
                using (SQLiteConnection connection = GetSQLiteConnection())
                {
                    PrepareCommand(command, connection, cmdText, parameters);
                    int i = command.ExecuteNonQuery();
                }
            }
            catch
            {
                Thread.Sleep(3000);
                try
                {
                    using (SQLiteConnection connection = GetSQLiteConnection())
                    {
                        PrepareCommand(command, connection, cmdText, parameters);
                        int i = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteNonQuery error-->" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteReader(string cmdText, SQLiteParameter[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand();
            SQLiteConnection connection = GetSQLiteConnection();
            try
            {
                PrepareCommand(command, connection, cmdText, parameters);
                SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                connection.Close();
                throw;
            }
        }
        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, SQLiteParameter[] parameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();

            using (SQLiteConnection connection = GetSQLiteConnection())
            {
                PrepareCommand(cmd, connection, cmdText, parameters);
                return cmd.ExecuteScalar();
            }

        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cmdText"></param>
        /// <param name="countText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataSet ExecutePager(ref int recordCount, int pageIndex, int pageSize, string cmdText, string countText, SQLiteParameter[] parameters)
        {
            if (recordCount < 0)
                recordCount = int.Parse(ExecuteScalar(countText, parameters).ToString());
            DataSet ds = new DataSet();
            SQLiteCommand command = new SQLiteCommand();
            using (SQLiteConnection connection = GetSQLiteConnection())
            {
                PrepareCommand(command, connection, cmdText, parameters);
                SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, "result");
            }
            return ds;
        }

    }
}
