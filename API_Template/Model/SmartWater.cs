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
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace API.Model
{

    #region 實例化
    public class SmartWater_Input
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N

        public string functiontype { get; set; }//方法類型

    }

    public class SmartWaterAlert_Input
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N
    }



    public class SmartWater_Output : ReturnMessage
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N
        public string ActValue { get; set; }//實際值  累計的數據

        public string TargetValue { get; set; }//上限值

    }

    public class SmartWaterAlert_Output : ReturnMessage
    {
        public string did { get; set; }//南水錶:A1S  北水錶:A1N
        public string type { get; set; }//Real:R  Day:D
        public string dt { get; set; }//抓取的日期 如2018-10-11
        public string UPDATE_TIME { get; set; }//
        public string ActValue { get; set; }//
        public string TargetValue { get; set; }//上限值

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
        public static async Task<SmartWater_Output> GetWater(SmartWater_Input Parameter)
        {
            SmartWater_Output rm = new SmartWater_Output();

            try
            {
                await Task.Run(() =>
                {
                    SqlConnection cn = new SqlConnection(conn);
                    cn.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" select T1.did,SUM(convert(int,T1.total)) as ActValue,Convert(int,T2.VALUE1) as TargetValue 
                                 from dbo.[Water] T1,TB_APPLICATION_PARAM T2
                                 where T1.type = 'R' 
                                 and T2.PARAME_ITEM = 'upper'  AND T2.VALUE2 = 'A1' and T1.did = @did
                                  ");
                    opc.Clear();
                    switch (Parameter.functiontype)
                    {
                        case "Day":
                            sb.Append("  and DateDiff(dd,T1.dt,getdate())=0 and T2.VALUE5 = 'Day'");
                            break;
                        case "Month":
                            sb.Append("  and DateDiff(MM,T1.dt,getdate())=0 and T2.VALUE5 = 'Month'");
                            break;
                        default:
                            sb.Append(" T1.did = ''");
                            break;
                    }
                    sb.Append(" group by VALUE1,did");
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                    {
                        cmd.Parameters.AddWithValue("@did", Parameter.did);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                    JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
                    rm.Success = true;
                    rm.Status = "success";
                    rm.Command = "GetWater";
                    rm.Array = jArray;
                });

            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Status = "Error";
                rm.Command = "GetWater";
            }
            return rm;
        }

        /// <summary>
        /// 智能水錶抓取每天的總用水量（當天抓取昨日整天的總用水量）
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static async Task<SmartWaterAlert_Output> GetWaterAlert(SmartWaterAlert_Input Parameter)
        {
            SmartWaterAlert_Output rm = new SmartWaterAlert_Output();

            try
            {
                await Task.Run(() =>
                {
                    int Limit = GetLimitDay();//上限值
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"select Top 1  T1.did, T1.type, convert(varchar(16), T1.dt, 121) as dt,T1.UPDATE_TIME, Convert(int,T1.total)  AS ActValue, 
			            Convert(Float,T3.VALUE1) AS TargetValue
                        from Water T1, dbo.TB_Line_Param T2, dbo.TB_APPLICATION_PARAM T3
                        where T3.VALUE2 = 'A1' and T1.did = @did and T2.LineCode = @did
                        and T1.type = 'R'
                        and DateDiff(dd, T1.dt, getdate()) = 0 
                        and total > @total
                        order by dt desc ");
                    opc.Clear();
                    opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
                    opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
                    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
                    JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
                    rm.Success = true;
                    rm.Status = "success";
                    rm.Command = "GetWaterAlert";
                    rm.Array = jArray;
                });

            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Status = "Error";
                rm.Command = "GetWaterAlert";
            }
            return rm;
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