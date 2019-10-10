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


    #region GetNSB 

    #region 參數實例化 --- GetNSB
    public class Test_Input
    {
        public string WERKS { get; set; }//廠別
        public string PRODH { get; set; }//車間編號
        public string OPTIONAL { get; set; }

    }
    public class Test_Output : ReturnMessage
    {
        public string currentNSB { get; set; }//NETWR
        public string targetNSB { get; set; }//NETWR
    }

    #endregion


    /// <summary>
    /// GetNSB CRUD
    /// </summary>
    public class Test_Helper
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


}