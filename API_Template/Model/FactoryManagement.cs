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

namespace API.Model
{

    #region GetHC_QueryLvData & GetHC_QueryDLBuffer


    #region 參數實例化 --- GetHC
    public class GetHC_Input
    {
        public string BU { get; set; }//BU

    }
    public class GetHCDLBuffer_Output
    {
        public string ORG { get; set; }
        public string DEPT_ID { get; set; }
        public string SUB_DEPT { get; set; }
        public string DL_DEMAND { get; set; }
        public string DL_ACT { get; set; }
        public string IDL_ACT { get; set; }
        public string TTL_HC { get; set; }
        public string DL_CUM_NEW_HIRE { get; set; }
        public string DL_BUFFER { get; set; }
        public string G1_RATE { get; set; }
        public string G2_RATE { get; set; }
        public string G3_RATE { get; set; }
        public string G4_RATE { get; set; }
        public string G5_RATE { get; set; }
        public string rowNumber { get; set; }
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
        public static string GetHC_QueryDLBuffer(GetHC_Input Parameter)
        {
            string Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.BU));
            opc.Add(DataPara.CreateProcParameter("@P_DATE", SqlDbType.VarChar, 10, ParameterDirection.Input, Date));
            DataTable dt = sdb.RunProc2("P_DailyReprot_QueryDLBuffer", opc);
            return JsonConvert.SerializeObject(dt);

        }


    }
    #endregion

    #region GetNSB 

    #region 參數實例化 --- GetNSB
    public class GetNSB_Input
    {
        public string SPMON { get; set; }//日期
        public string PRODH { get; set; }//車間編號

    }
    public class GetNSB_Output
    {
        public string NETWR { get; set; }//NETWR
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
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT Sum(NETWR) as NETWR
                        FROM [SAPTEST].[dbo].[ZTVDSS913]
                        where substring(SPMON,1,4) = @SPMON and PRODH = @PRODH ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@SPMON", SqlDbType.NVarChar, Parameter.SPMON));
            opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
            string NETWR = sdb.GetRowString(sb.ToString(),opc, "NETWR");
            m.Add(NETWR);
            return jss.Serialize(m); 

        }
    }


    #endregion

    #region GetNUB
    #region 參數實例化 --- GetNUB
    public class GetNUB_Input
    {
        public string SPMON { get; set; }//日期
        public string PRODH { get; set; }//車間編號

    }
    public class GetNUB_Output
    {
        public string FKIMG { get; set; }//NUB 單位是台幣
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
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT Sum(FKIMG) as FKIMG
                        FROM [SAPTEST].[dbo].[ZTVDSS913]
                        where substring(SPMON,1,4) = @SPMON and PRODH = @PRODH ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@SPMON", SqlDbType.NVarChar, Parameter.SPMON));
            opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
            string FKIMG = sdb.GetRowString(sb.ToString(), opc, "FKIMG");
            m.Add(FKIMG);
            return jss.Serialize(FKIMG);

        }
    }
    #endregion

}