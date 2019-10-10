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


    #region 參數實例化 --- GetOAY
    public class GetOAY_Input
    {
        public string factory { get; set; }//廠別
        public string line { get; set; }//線別
        public string series { get; set; } //產品系列
        public string stage { get; set; }
    }
    public class GetOAY_Output
    {
        public string line { get; set; } //線別
        public string currentOAY { get; set; } //OAY 實際值
        public string targetOAY { get; set; } //OAY 目標值
    }
    #endregion

    /// <summary>
    /// GetOAY
    /// </summary>
    public class GETOAY_Helper
    {
        //public static List<string> m = new List<string>();
        //public static JavaScriptSerializer jss = new JavaScriptSerializer();

        static string conn = ConfigurationManager.AppSettings["DataPlateFormA1"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        /// <summary>
        /// GetOAY
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns>Datatable 轉 Json</returns>
        public static string GetOAY(GetOAY_Input Parameter)
        {
            StringBuilder sb = new StringBuilder();
            string workdate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

            sb.Append("  select isnull( cast( round( (case sum(QTY_INPUT) when 0 then 1 else sum(QTY_OUTPUT) / sum(QTY_INPUT) end)*10000, 0 )  as int), 99999)   currentOAY, 99999 targetOAY ");
            sb.Append("    from RPT_OUTPUT ");
            sb.Append("   where PLANTNO = @PLANTNO ");
            sb.Append("     and WORK_DATE = @WORK_DATE ");
            if (Parameter.line != "ALL")
                sb.Append("     and PDLINECODE = '" + Parameter.line + "' ");
            if (Parameter.stage != "ALL")
                sb.Append("     and STAGECODE = '" + Parameter.stage + "' ");
            if (Parameter.series != "ALL")
                sb.Append("     and SERIES = '" + Parameter.series + "' ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@PLANTNO", SqlDbType.NVarChar, Parameter.factory));
            opc.Add(DataPara.CreateDataParameter("@WORK_DATE", SqlDbType.NVarChar, workdate));
            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
            dt.Columns.Add("line");
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["line"] = Parameter.line;
            }

            if (Parameter.stage == "ALL")
            {
                sb.Clear();
                sb.Append(" select cast( (case GOAL_OAY when 0 then 99999 else GOAL_OAY*100 end) as int) GOAL_OAY ");
                sb.Append("   from [dbo].[RPT_KPI] ");
                sb.Append("  where PLANTNO = @PLANTNO and WORK_DATE = @WORK_DATE ");
                sb.Append("    and PDLINECODE = '" + Parameter.line + "' ");
                sb.Append("    and SERIES = '" + Parameter.series + "' ");
                DataTable dt_g = sdb.GetDataTable(sb.ToString(), opc);
                if (dt_g.Rows.Count > 0)
                {
                    dt.Rows[0]["targetOAY"] = dt_g.Rows[0]["GOAL_OAY"];
                }
            }

            return JsonConvert.SerializeObject(dt);
        }

        
    }



    #region 參數實例化 --- GetQuanlity
    public class GetQuanlity_Input
    {
        public string factory { get; set; }//廠別
        public string line { get; set; }//線別
    }
    public class GetQuanlity_Output
    {
        public string currentQuanlity { get; set; } //Quanlity 實際值
        public string targetQuanlity { get; set; } //Quanlity 目標值
    }
    #endregion

    /// <summary>
    /// GetOAY
    /// </summary>
    public class GetQuanlity_Helper
    {
        static string conn = ConfigurationManager.AppSettings["DataPlateFormA1"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        public static string GetQuanlity(GetQuanlity_Input Parameter)
        {
            StringBuilder sb = new StringBuilder();
            string workdate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string workmonth = workdate.Substring(0, 6) + "%";

            sb.Append("  select count(1) currentQuality, 99999 targetQuality ");
            sb.Append("    from rpt_CCR ");
            sb.Append("   where plant = @PLANTNO ");
            sb.Append("     and ccr_received_date like @WORK_MONTH ");
            if (Parameter.line != "ALL")
                sb.Append("     and pdline = '" + Parameter.line + "' ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@PLANTNO", SqlDbType.NVarChar, Parameter.factory));
            opc.Add(DataPara.CreateDataParameter("@WORK_MONTH", SqlDbType.NVarChar, workmonth));
            DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
            
            return JsonConvert.SerializeObject(dt);
        }

    }


    }
