using LiteOn.EA.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_CacheHelper
{
    public class Cache_Helper
    {
        static string connTW = ConfigurationManager.AppSettings["DBConnectionTW"]; //TW 緩存表
        static SqlDB sdbTW = new SqlDB(connTW);
        static ArrayList opc = new ArrayList();


        #region Public Function  -------- TB_API_Cache 緩存
        /// <summary>
        /// 檢查緩存表是否存在此API方法的數據
        /// </summary>
        /// <param name="Command">接口方法</param>
        /// <param name="Value1">BU</param>
        /// <param name="Value2">車間(字母簡寫、編號)</param>
        /// <param name="Update_Time">日期 yyyy-MM-dd</param>
        /// <returns>Bool 类型</returns>
        public static bool CheckIsExistByCache(string Command, string Value1, string Value2,string Update_Time)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from  [TB_API_Cache] where Command = @Command and Value1 = @Value1 AND Value2 = @Value2 and Update_Time  = @Update_Time");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Command", SqlDbType.NVarChar, Command));
            opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.NVarChar, Value1));
            opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.NVarChar, Value2));
            opc.Add(DataPara.CreateDataParameter("@Update_Time", SqlDbType.NVarChar, Update_Time));
            DataTable dt = sdbTW.GetDataTable(sb.ToString(), opc);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据条件得到到期时间 (字符串型)
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <returns></returns>
        public static string CheckIsExistByCache(string Command, string Value1, string Value2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select Top 1 * from  [TB_API_Cache] where Command = @Command   ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Command", SqlDbType.NVarChar, Command));
            if (!string.IsNullOrEmpty(Value1))
            {
                sb.Append(" and Value1 = @Value1");
                opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.NVarChar, Value1));
            }
            if (!string.IsNullOrEmpty(Value2))
            {
                sb.Append(" AND Value2 = @Value2");
                opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.NVarChar, Value2));
            }
            sb.Append("  order by SID DESC");
            return sdbTW.GetRowString(sb.ToString(),opc, "Update_Time");
        }

        /// <summary>
        /// 抓取緩存表中的Json字符串
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <returns></returns>
        public static DataTable GetAPICache(string Command, string Value1, string Value2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select Top 1 * from  [TB_API_Cache] where Command = @Command  ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Command", SqlDbType.NVarChar, Command));
            if (!string.IsNullOrEmpty(Value1))
            {
                sb.Append(" and Value1 = @Value1");
                opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.NVarChar, Value1));
            }
            if (!string.IsNullOrEmpty(Value2))
            {
                sb.Append(" AND Value2 = @Value2");
                opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.NVarChar, Value2));
            }
            sb.Append(" order by SID DESC");
            return sdbTW.GetDataTable(sb.ToString(),opc);
        }




        /// <summary>
        /// 將第一次獲取到的資料 Insert到緩存表中
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <param name="Value3"></param>
        /// <param name="Update_Time"></param>
        public static void InsertCacheData(string Command, string Value1, string Value2, string Value3, string Update_Time)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" INSERT INTO [dbo].[TB_API_Cache]
                       ([Command]
                       ,[Value1]
                       ,[Value2]
                       ,[Value3]
                       ,[Update_Time])
                 VALUES
                       (@Command,
                        @Value1,
                        @Value2,
                        @Value3,
                        @Update_Time)");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Command", SqlDbType.NVarChar, Command));
            opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.NVarChar, Value1));
            opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.NVarChar, Value2));
            opc.Add(DataPara.CreateDataParameter("@Value3", SqlDbType.NVarChar, Value3));
            opc.Add(DataPara.CreateDataParameter("@Update_Time", SqlDbType.NVarChar, Update_Time));
            sdbTW.ExecuteNonQuery(sb.ToString(), opc);

        }


        public static double CalcTimeDifference(DateTime t1,DateTime t2)
        {
            TimeSpan ts1 = new TimeSpan(t1.Ticks);
            TimeSpan ts2 = new TimeSpan(t2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return  ts.TotalMinutes;
        }
        #endregion

    }
}
