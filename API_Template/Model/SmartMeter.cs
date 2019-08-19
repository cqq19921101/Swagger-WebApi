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

namespace API.Model
{

    #region KWH實例化

    /// <summary>
    /// 每天,每月用電量的條件
    /// </summary>
    public class SmartMeterKWH_Input
    {
        public string did { get; set; }//電錶ID 例:190164-OPS ALED車間
        //public string type { get; set; }//R-實時  D-昨天

        public string functiontype { get; set; }//方法類型
    }

    public class SmartMeterKWH_Output
    {
        public string Line { get; set; }//車間名
        public string did { get; set; }//電錶ID 例:190164-OPS ALED車間
        public string ActValue { get; set; }//實際值  累計的數據

        public string TargetValue { get; set; }//上限值

    }


    /// <summary>
    /// 抓取每天預警用電量的條件
    /// </summary>
    public class SmartMeterKWHAlert_Input
    {
        public string did { get; set; }//電錶ID 例:190164-OPS ALED車間
    }

    public class SmartMeterKWHAlert_Output
    {
        public string Line { get; set; }//車間名
        public string did { get; set; }//電錶ID 例:190164-OPS ALED車間
        public string type { get; set; }//
        public string dt { get; set; }//
        public string ActValue { get; set; }//

        public string TargetValue { get; set; }//上限值

    }

    #endregion

    #region UTS實例化
    public class SmartMeterUTS_Input
    {
        #region UTS
        public string PLANTNO { get; set; }//廠別 默認2301
        public string PRODUCTLINECODE { get; set; }//車間編號
        #endregion
    }

    public class SmartMeterUTS_Output
    {
        public string PLANTNO { get; set; }//廠別 默認2301
        public string PRODUCTLINECODE { get; set; }//車間編號 
        public string GR_DATE { get; set; }//時間
        public string QTY { get; set; }//UTS

    }
    #endregion


    /// <summary>
    /// 智能電錶 CRUD
    /// </summary>
    public class GetMeter_Helper
    {
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        static  SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// 智能電錶抓取當天實時用電量異常的數據和正常的數據（type=normal --- 正常數據 ; type = abnormal --- 超過上限值的數據）
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetMeterKWHAlert(SmartMeterKWHAlert_Input Parameter)
        {
            int Limit = GetLimitReal(Parameter.did);//根據did抓取對應的上限值
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select Top 1 T2.Line, T1.did, T1.type, convert(varchar(16), T1.dt, 121) as dt, Convert(Float,T1.total)  AS ActValue, Convert(Float,T3.VALUE1) AS TargetValue
                        from KWH T1, dbo.TB_Line_Param T2, dbo.TB_APPLICATION_PARAM T3
                        where T3.VALUE2 = @did and T1.did = @did and T2.LineCode = @did
                        and T1.type = 'R'
                        and DateDiff(dd, T1.dt, getdate()) = 0 and total > @total
                        order by dt desc");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
            opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
            DataTable dt =  sdb.GetDataTable(sb.ToString(), opc);
            return JsonConvert.SerializeObject(dt);

        }

        /// <summary>
        /// 智能電錶抓取總用電量 當天抓取昨日一天的總用電量
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetMeterKWH(SmartMeterKWH_Input Parameter)
        {
            SqlConnection cn = new SqlConnection(conn);
            cn.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" select T3.Line, T1.did, SUM(convert(int,T1.total)) as ActValue, Convert(Float,T2.VALUE1) as TargetValue 
                         from dbo.KWH T1, TB_APPLICATION_PARAM T2, dbo.TB_Line_Param T3
                         where  T1.type = 'R' 
                         and T2.PARAME_ITEM = 'upper'  AND T2.VALUE2 = @did and T1.did = @did and T3.LineCode = @did
                          ");
            opc.Clear();
            switch (Parameter.functiontype)
            {
                case "Day":
                    sb.Append(" and DateDiff(dd,dt,getdate())=0 and T2.VALUE5 = 'Day'");
                    break;
                case "Month":
                    sb.Append(" and DateDiff(MM,dt,getdate())=0 and T2.VALUE5 = 'Month'");
                    break;
                default:
                    sb.Append(" T1.did = ''");
                    break;
            }
            sb.Append(" group by did, VALUE1, PARAME_NAME, Line");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@did", Parameter.did);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);

            //opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
            //DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
            //return JsonConvert.SerializeObject(dt);

        }

        /// <summary>
        /// 智能電錶抓取昨日一天的UTS數據 上午9:20左右 MES會把數據拋到DB中
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public static string GetMeterUTS(SmartMeterUTS_Input Parameter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select  PLANTNO,PRODUCTLINECODE, convert(varchar(10),GR_DATE,121) as GR_DATE,QTY from UTS where PRODUCTLINECODE = @PRODUCTLINECODE and PLANTNO = @PLANTNO   order by GR_DATE desc");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@PRODUCTLINECODE", SqlDbType.NVarChar, Parameter.PRODUCTLINECODE));
            opc.Add(DataPara.CreateDataParameter("@PLANTNO", SqlDbType.NVarChar, Parameter.PLANTNO));
            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
            return JsonConvert.SerializeObject(dt);
        }


        #region 抓取實時/每天的用電量上限值 不同車間的數值不同
        /// <summary>
        /// 根據did抓取實時用電量的上限值 每個did對應的上限值不同
        /// </summary>
        /// <param name="did"></param>
        /// <returns>return int</returns>
        public static int GetLimitReal(string did)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_APPLICATION_PARAM where 1=1 and PARAME_ITEM = 'upper'");
            opc.Clear();
            switch (did)
            {
                case "190164"://ALED
                    sb.Append(" and FUNCTION_NAME  = 'Alert' and PARAME_NAME = 'RealTimeKWHLimit' ");
                    break;
                case "190171"://SSD
                    sb.Append(" and FUNCTION_NAME  = 'SSDAlert' and PARAME_NAME = 'SSDRealTimeKWHLimit' ");
                    break;
                case "190172"://SMD
                    sb.Append(" and FUNCTION_NAME  = 'SMDAlert' and PARAME_NAME = 'SMDRealTimeKWHLimit' ");
                    break;
                case "190124"://全廠
                    sb.Append(" and FUNCTION_NAME  = 'ALLAlert' and PARAME_NAME = 'ALLRealTimeKWHLimit' ");
                    break;
                case "190160"://OSD
                case "190161":
                case "190174":
                case "190175":
                    sb.Append(" and FUNCTION_NAME  = 'OSDAlert' and PARAME_NAME = 'OSDRealTimeKWHLimit' ");
                    break;
            }
            string Limit = sdb.GetRowString(sb.ToString(), opc, "Value1");
            return int.Parse(Limit);
        }

        /// <summary>
        /// 根據did抓取每天用電量的上限值 每個did對應的上限值不同
        /// </summary>
        /// <param name="did"></param>
        /// <returns>return int</returns>
        public static int GetLimitDay(string did)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_APPLICATION_PARAM where 1=1 and PARAME_ITEM = 'upper'");
            opc.Clear();
            switch (did)
            {
                case "190164"://ALED
                    sb.Append(" and FUNCTION_NAME  = 'Alert' and PARAME_NAME = 'DayKWHLimit' ");
                    break;
                case "190171"://SSD
                    sb.Append(" and FUNCTION_NAME  = 'SSDAlert' and PARAME_NAME = 'SSDDayKWHLimit' ");
                    break;
                case "190172"://SMD
                    sb.Append(" and FUNCTION_NAME  = 'SMDAlert' and PARAME_NAME = 'SMDDayKWHLimit' ");
                    break;
                case "190124"://全廠
                    sb.Append(" and FUNCTION_NAME  = 'ALLAlert' and PARAME_NAME = 'ALLDayKWHLimit' ");
                    break;
                case "190160"://OSD
                case "190161":
                case "190174":
                case "190175":
                    sb.Append(" and FUNCTION_NAME  = 'OSDAlert' and PARAME_NAME = 'OSDDayKWHLimit' ");
                    break;
            }
            string Limit = sdb.GetRowString(sb.ToString(), opc, "Value1");
            return int.Parse(Limit);
        }
        #endregion

    }
}