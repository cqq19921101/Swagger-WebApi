using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LiteOn.EA.BLL;
using LiteOn.EA.DAL;
using System.Configuration;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

namespace API.Model
{

    #region 實例化
    public class SmartWaterReal_Input
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N

        public string functiontype { get; set; }//方法類型

    }

    public class SmartWaterDay_Input
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N
        public string functiontype { get; set; }//方法類型
    }



    public class SmartWater_Output
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N
        public string type { get; set; }//Real:R  Day:D
        public string dt { get; set; }//抓取的日期 如2018-10-11
        public string UPDATE_TIME { get; set; }//type:R -->顯示每小時 例:01:00:00 / type:D --顯示每天的日期 2018-10-11
        public string totalA { get; set; }//用水量 水錶至今累計的用水量
        public string total { get; set; }//用水量 時間段的用水量 按type區分
        public string BU { get; set; }//BU   例：OPS

    }
    #endregion


    /// <summary>
    /// 智能水錶 CRUD
    /// </summary>
    public class GetWater_Helper
    {
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        static  SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// 智能水錶 抓取每天實時的用水量 (每小時更新) normal:正常  abnormal:異常
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetWaterReal(SmartWaterReal_Input Parameter)
        {
            int Limit = GetLimitReal();//上限值
            StringBuilder sb = new StringBuilder();
            sb.Append("select  did,type,  convert(varchar(16),dt,121) as dt,UPDATE_TIME,totalA,total,BU from Water where did = @did and type = 'R'   and DateDiff(dd,dt,getdate())=0 ");
            opc.Clear();
            switch (Parameter.functiontype.ToLower())
            {
                case "abnormal":
                    sb.Append("and total > @total");
                    opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
                    break;
            }
            sb.Append(" order by UPDATE_TIME ");
            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
            DataTable dt =  sdb.GetDataTable(sb.ToString(), opc);
            return JsonConvert.SerializeObject(dt);

        }

        /// <summary>
        /// 智能水錶抓取每天的總用水量（當天抓取昨日整天的總用水量）
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetWaterDay(SmartWaterDay_Input Parameter)
        {
            int Limit = GetLimitDay();//上限值
            StringBuilder sb = new StringBuilder();
            sb.Append("select  did,type,  convert(varchar(16),dt,121) as dt,UPDATE_TIME,totalA,total,BU from Water where did = @did and type = 'D' and  DateDiff(dd,dt,getdate())=1    ");
            opc.Clear();
            switch (Parameter.functiontype.ToLower())
            {
                case "abnormal":
                    sb.Append("and total > @total");
                    opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
                    break;
            }

            sb.Append(" order by UPDATE_TIME");
            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
            return JsonConvert.SerializeObject(dt);

        }


        /// <summary>
        /// 抓取實時用水量的上限值 
        /// </summary>
        /// <param name="did"></param>
        /// <returns>return int</returns>
        public static  int GetLimitReal()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_APPLICATION_PARAM where 1=1 and PARAME_ITEM = 'upper' and FUNCTION_NAME  = 'OPSWater' and PARAME_NAME = 'WRealTimeKWHLimit'");
            opc.Clear();
            string Limit = sdb.GetRowString(sb.ToString(), opc,"Value1");
            return int.Parse(Limit);
        }

        /// <summary>
        /// 抓取每天用水量的上限值 
        /// </summary>
        /// <param name="did"></param>
        /// <returns>return int</returns>
        public static int GetLimitDay()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_APPLICATION_PARAM where 1=1 and PARAME_ITEM = 'upper' and FUNCTION_NAME  = 'OPSWater' and PARAME_NAME = 'WDayKWHLimit'");
            opc.Clear();
            string Limit = sdb.GetRowString(sb.ToString(), opc, "Value1");
            return int.Parse(Limit);
        }

    }
}