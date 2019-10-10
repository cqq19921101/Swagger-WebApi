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
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using WebApi_DataProcessing;
using Newtonsoft.Json.Linq;
using WebApi_WriteTraceLog;
using WebApi_CacheHelper;

namespace API.Model
{

    #region GetHC_QueryLvData & GetHC_QueryDLBuffer


    #region 參數實例化 --- GetHC

    public class GetHCDLBuffer_Input
    {
        public string BU { get; set; }//BU
        public string DEPT_ID { get; set; }//DEPT_ID
        public string OPTIONAL { get; set; }
    }

    public class GetHC_Input
    {
        public string BU { get; set; }//BU

    }
    public class GetHCDLBuffer_Output : ReturnMessage
    {
        public string DEPT_ID { get; set; }
        public string DL_DEMAND { get; set; }
        public string DL_ACT { get; set; }
    }



    public class GetHCLvData_Output
    {
        public string DEPT_ID { get; set; }
        public string G1LastMonth_Incumbency { get; set; }
        public string G1HC { get; set; }
        public string G1Cum { get; set; }
        public string G1CumRate { get; set; }
        public string G1VolTO { get; set; }
        public string G1VolRate { get; set; }
        public string G1DailyTO { get; set; }

        public string G2LastMonth_Incumbency { get; set; }
        public string G2HC { get; set; }
        public string G2Cum { get; set; }
        public string G2CumRate { get; set; }
        public string G2VolTO { get; set; }
        public string G2VolRate { get; set; }
        public string G2DailyTO { get; set; }

        public string G3LastMonth_Incumbency { get; set; }
        public string G3HC { get; set; }
        public string G3Cum { get; set; }
        public string G3CumRate { get; set; }
        public string G3VolTO { get; set; }
        public string G3VolRate { get; set; }
        public string G3DailyTO { get; set; }

        public string G4LastMonth_Incumbency { get; set; }
        public string G4HC { get; set; }
        public string G4Cum { get; set; }
        public string G4CumRate { get; set; }
        public string G4VolTO { get; set; }
        public string G4VolRate { get; set; }
        public string G4DailyTO { get; set; }

        public string G5LastMonth_Incumbency { get; set; }
        public string G5HC { get; set; }
        public string G5Cum { get; set; }
        public string G5CumRate { get; set; }
        public string G5VolTO { get; set; }
        public string G5VolRate { get; set; }
        public string G5DailyTO { get; set; }

    }
    #endregion

    /// <summary>
    /// Get HC CRUD
    /// </summary>
    public class GetHC_Helper
    {
        static string conn = ConfigurationManager.AppSettings["HRReportDBConnection"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// QueryLvData
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetHC_QueryLvData(GetHC_Input Parameter)
        {
            opc.Clear();
            string Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.BU));
            opc.Add(DataPara.CreateProcParameter("@P_ID_DATE", SqlDbType.VarChar, 10, ParameterDirection.Input, Date));
            DataTable dt = sdb.RunProc2("P_DailyReprot_QueryLvData", opc);

            return JsonConvert.SerializeObject(dt);

        }

        /// <summary>
        /// QueryDLBuffer
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetHC_QueryDLBuffer(GetHCDLBuffer_Input Parameter)
        {
            string Time = Cache_Helper.CheckIsExistByCache("GetHC_DLBuffer", Parameter.BU, Parameter.DEPT_ID);
            if (!string.IsNullOrEmpty(Time) && DateTime.Compare(DateTime.Now, DateTime.Parse(Time)) < 0) //存在缓存则抓取缓存中的数据
            {
                DataTable dt = Cache_Helper.GetAPICache("GetHC_DLBuffer", Parameter.BU, Parameter.DEPT_ID);
                DataRow dr = dt.Rows[0];
                //string Update_Time = dr["Update_Time"].ToString();//缓存中的时间
                string JsonResult = dr["Value3"].ToString();

                return JsonResult;
            }
            else
            {
                opc.Clear();
                string Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.BU));
                opc.Add(DataPara.CreateProcParameter("@P_DATE", SqlDbType.VarChar, 10, ParameterDirection.Input, Date));
                opc.Add(DataPara.CreateProcParameter("@P_DEPTID", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.DEPT_ID));
                DataTable dt = sdb.RunProc2("P_DailyReprot_QueryDLBuffer_API", opc);
                Cache_Helper.InsertCacheData("GetHC_DLBuffer", Parameter.BU, Parameter.DEPT_ID, JsonConvert.SerializeObject(dt), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 16:00:00");
                return JsonConvert.SerializeObject(dt);

            }
        }


    }
    #endregion

    #region GetNSB 

    #region 參數實例化 --- GetNSB
    public class GetNSB_Input
    {
        public string WERKS { get; set; }//廠別
        public string PRODH { get; set; }//車間編號
        public string OPTIONAL { get; set; }

    }
    public class GetNSB_Output : ReturnMessage
    {
        public string currentNSB { get; set; }//NETWR
        public string targetNSB { get; set; }//NETWR
    }

    #endregion


    /// <summary>
    /// GetNSB CRUD
    /// </summary>
    public class GetNSB_Helper
    {
        public static List<string> m = new List<string>();
        public static JavaScriptSerializer jss = new JavaScriptSerializer();

        static string conn = ConfigurationManager.AppSettings["SAPDBConnection"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// GetNSB
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetNSB(GetNSB_Input Parameter)
        {
            string Time = Cache_Helper.CheckIsExistByCache("GetNSB", Parameter.WERKS, Parameter.PRODH);
            if (!string.IsNullOrEmpty(Time) && DateTime.Compare(DateTime.Now,DateTime.Parse(Time)) < 0 ) //存在缓存则抓取缓存中的数据
            {
                DataTable dt = Cache_Helper.GetAPICache("GetNSB", Parameter.WERKS, Parameter.PRODH);
                DataRow dr = dt.Rows[0];
                //string Update_Time = dr["Update_Time"].ToString();//缓存中的时间
                string JsonResult = dr["Value3"].ToString();

                return JsonResult;
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                switch (Parameter.PRODH.ToUpper())
                {
                    case "ALL":
                        sb.Append(@"
                                SELECT T1.SPMON,'ALL' AS PRODH, Sum(convert(bigint,T1.NETWR)) as currentNSB, 
                                Convert(bigint,T2.LineCode) as targetNSB
                                FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
                                where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
                                and T2.Line = 'NSB' AND  T2.Value1 = 'ALL'
                                group by T2.LineCode,T1.SPMON");
                        opc.Clear();
                        break;
                    default:
                        sb.Append(@"SELECT T1.SPMON, T1.PRODH,Sum(convert(bigint,T1.NETWR)) as currentNSB, Convert(bigint,T2.LineCode) as targetNSB
                        FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
                        where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
                        and T2.Line = 'NSB'  and T2.Value1 = @PRODH
                        ");
                        opc.Clear();
                        sb.Append(" and T1.PRODH = @PRODH");
                        sb.Append(" Group by T2.LineCode,T1.SPMON,T1.PRODH");
                        opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
                        break;
                }
                opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, Parameter.WERKS));
                DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
                Cache_Helper.InsertCacheData("GetNSB", Parameter.WERKS, Parameter.PRODH, JsonConvert.SerializeObject(dt), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 02:30:00");
                return JsonConvert.SerializeObject(dt);
            }

        }

        //public static async Task<GetNSB_Output> GetNSB(GetNSB_Input Parameter)
        //{
        //    GetNSB_Output rm = new GetNSB_Output();

        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            switch (Parameter.PRODH.ToUpper())
        //            {
        //                case "ALL":
        //                    sb.Append(@"
        //                                SELECT T1.SPMON,'ALL' AS PRODH, Sum(convert(bigint,T1.NETWR)) as currentNSB, 
        //                                Convert(bigint,T2.LineCode) as targetNSB
        //                                FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
        //                                where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
        //                                and T2.Line = 'NSB' AND  T2.Value1 = 'ALL'
        //                                group by T2.LineCode,T1.SPMON");
        //                    opc.Clear();
        //                    break;
        //                default:
        //                    sb.Append(@"SELECT T1.SPMON, T1.PRODH,Sum(convert(bigint,T1.NETWR)) as currentNSB, Convert(bigint,T2.LineCode) as targetNSB
        //                        FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
        //                        where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
        //                        and T2.Line = 'NSB'  and T2.Value1 = @PRODH
        //                        ");
        //                    opc.Clear();
        //                    sb.Append(" and T1.PRODH = @PRODH");
        //                    sb.Append(" Group by T2.LineCode,T1.SPMON,T1.PRODH");
        //                    opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
        //                    break;
        //            }
        //            opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, Parameter.WERKS));
        //            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        //            JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
        //            rm.Success = true;
        //            rm.Status = "success";
        //            rm.Command = "GetNSB";
        //            rm.Array = jArray;
        //        });

        //    }
        //    catch (Exception ex)
        //    {

        //        //WriteTraceLog.Info("GetNSB 無資料！");
        //        rm.Success = false;
        //        rm.Status = "Error";
        //        rm.Command = "GetNSB";
        //    }
        //    return rm;
        //}



    }


    #endregion

    #region GetNUB
    #region 參數實例化 --- GetNUB
    public class GetNUB_Input
    {
        public string WERKS { get; set; }//廠別
        public string PRODH { get; set; }//車間編號
        public string OPTIONAL { get; set; }

    }
    public class GetNUB_Output : ReturnMessage
    {
        public string currentNUB { get; set; }//NUB 單位是台幣
        public string targetNUB { get; set; }//NUB 單位是台幣
        //public JArray JA { get; set; }
    }

    #endregion

    public class GetNUB_Helper
    {
        public static List<string> m = new List<string>();
        public static JavaScriptSerializer jss = new JavaScriptSerializer();

        static string conn = ConfigurationManager.AppSettings["SAPDBConnection"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// GetNUB
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>string 轉 Json</returns>
        public static string GetNUB(GetNUB_Input Parameter)
        {
            string Time = Cache_Helper.CheckIsExistByCache("GetNUB", Parameter.WERKS, Parameter.PRODH);
            if (!string.IsNullOrEmpty(Time) && DateTime.Compare(DateTime.Now, DateTime.Parse(Time)) < 0) //存在缓存则抓取缓存中的数据
            {
                DataTable dt = Cache_Helper.GetAPICache("GetNUB", Parameter.WERKS, Parameter.PRODH);
                DataRow dr = dt.Rows[0];
                //string Update_Time = dr["Update_Time"].ToString();//缓存中的时间
                string JsonResult = dr["Value3"].ToString();

                return JsonResult;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                switch (Parameter.PRODH.ToUpper())
                {
                    case "ALL":
                        sb.Append(@"
                                SELECT T1.SPMON,'ALL' AS PRODH, Sum(convert(bigint,T1.FKIMG)) as currentNUB, 
                                Convert(bigint,T2.LineCode) as targetNUB
                                FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
                                where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
                                and T2.Line = 'NUB'  and T2.Value1 = 'ALL'
                                group by T2.LineCode,T1.SPMON");
                        opc.Clear();
                        break;
                    default:
                        sb.Append(@"SELECT T1.SPMON, T1.PRODH, Sum(convert(bigint,T1.FKIMG)) as currentNUB, Convert(bigint,T2.LineCode) as targetNUB
                        FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
                        where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
                        and T2.Line = 'NUB'  and T2.Value1 = @PRODH
                        ");
                        sb.Append(" and T1.PRODH = @PRODH");
                        sb.Append(" Group by T2.LineCode,T1.SPMON,T1.PRODH");
                        opc.Clear();
                        opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
                        break;
                }
                opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, Parameter.WERKS));
                DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
                Cache_Helper.InsertCacheData("GetNUB", Parameter.WERKS, Parameter.PRODH, JsonConvert.SerializeObject(dt), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 02:30:00");
                return JsonConvert.SerializeObject(dt);
            }


        }


        /// <summary>
        /// 異步處理 NUB資料
        /// </summary>
        /// <param name="Para"></param>
        /// <returns></returns>
        //public static async Task<GetNUB_Output> GetNUB(GetNUB_Input Para)
        //{
        //    GetNUB_Output rm = new GetNUB_Output();

        //    try
        //    {
        //            await Task.Run(() =>
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            switch (Para.PRODH.ToUpper())
        //            {
        //                case "ALL":
        //                    sb.Append(@"
        //                                SELECT T1.SPMON,'ALL' AS PRODH, Sum(convert(bigint,T1.FKIMG)) as currentNUB, 
        //                                Convert(bigint,T2.LineCode) as targetNUB
        //                                FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
        //                                where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
        //                                and T2.Line = 'NUB' AND  T2.Value1 = 'ALL'
        //                                group by T2.LineCode,T1.SPMON");
        //                    opc.Clear();
        //                    break;
        //                default:
        //                    sb.Append(@"SELECT T1.SPMON, T1.PRODH, Sum(convert(bigint,T1.FKIMG)) as currentNUB, Convert(bigint,T2.LineCode) as targetNUB
        //                        FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
        //                        where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
        //                        and T2.Line = 'NUB' and T2.Value1 = @PRODH
        //                        ");
        //                    sb.Append(" and T1.PRODH = @PRODH");
        //                    sb.Append("     group by T2.LineCode,T1.SPMON,T1.PRODH");
        //                    opc.Clear();
        //                    opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Para.PRODH));
        //                    break;
        //            }
        //            opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, Para.WERKS));
        //            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        //            JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
        //            rm.Success = true;
        //            rm.Status = "success";
        //            rm.Command = "GetNUB";
        //            rm.Array = jArray;

        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteTraceLog.Info("GetNUB 無資料！");
        //        rm.Success = false;
        //        rm.Status = "Error";
        //        rm.Command = "GetNUB";

        //    }

        //    return rm;
        //}

    }


    #endregion

    #region GetTECO
    public class TECO_Input
    {
        public string PlantNo { get; set; }
        public string Line { get; set; }
    }

    public class TECO_Output : ReturnMessage
    {
        public string PlantNo { get; set; }
        public string Line { get; set; }
        public string currentYield {get;set;}
        public string targetTield { get; set; }
    }

    public static class TECO_Helper
    {
        /// <summary>
        /// GetTECO
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public static string GetTECO(TECO_Input Parameter)
        {
            string Time = Cache_Helper.CheckIsExistByCache("GetTECO", Parameter.PlantNo, Parameter.Line);
            if (!string.IsNullOrEmpty(Time) && DateTime.Compare(DateTime.Now, DateTime.Parse(Time)) < 0) //存在缓存则抓取缓存中的数据
            {
                DataTable dt = Cache_Helper.GetAPICache("GetTECO", Parameter.PlantNo, Parameter.Line);
                DataRow dr = dt.Rows[0];
                //string Update_Time = dr["Update_Time"].ToString();//缓存中的时间
                string JsonResult = dr["Value3"].ToString();
                
                return JsonResult;
            }
            else
            {
                var FuncTeco = new DP_TECO();
                DataTable dt = FuncTeco.ReturnTECOByCZOPS(Parameter.PlantNo, Parameter.Line);
                Cache_Helper.InsertCacheData("GetTECO", Parameter.PlantNo, Parameter.Line, JsonConvert.SerializeObject(dt),DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 07:00:00");
                return JsonConvert.SerializeObject(dt);

            }
        }


    }


    #endregion

}