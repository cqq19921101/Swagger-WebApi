using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_Holley : DAL
    {
        public string SaveData(string loraid, string power, string freq_error, 
            string rssi, string snr,string info1,string info2,string info3)
        {
            try
            {                
                string sql = "LITEON.USP_SAVE_HOLLEYDATA";
                OracleParameter[] ops = {
                    new OracleParameter("v_loraid",OracleDbType.Varchar2,0,loraid.ToUpper(),ParameterDirection.Input),
                    new OracleParameter("v_power",OracleDbType.Varchar2,0,power,ParameterDirection.Input),
                    new OracleParameter("v_freq_error",OracleDbType.Varchar2,0,freq_error,ParameterDirection.Input),
                    new OracleParameter("v_rssi",OracleDbType.Varchar2,0,rssi,ParameterDirection.Input),
                    new OracleParameter("v_snr",OracleDbType.Varchar2,0,snr,ParameterDirection.Input),
                    new OracleParameter("v_info1",OracleDbType.Varchar2,0,info1,ParameterDirection.Input),
                    new OracleParameter("v_info2",OracleDbType.Varchar2,0,info2,ParameterDirection.Input),
                    new OracleParameter("v_info3",OracleDbType.Varchar2,0,info3,ParameterDirection.Input),
                    new OracleParameter("V_OUT",OracleDbType.Varchar2,200,"",ParameterDirection.Output)
                    };
                using (DataTable dt = this.ExecProcReturnOutput(sql, ops))
                {
                    if (dt.Rows.Count == 0)
                    {
                        return "沒有返回結果";
                    }
                    else
                    {
                        return dt.Rows[0]["v_out"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataTable QueryData(string loraid)
        {
            string sql = "SELECT B.PART_NO FROM SAJET.G_SN_STATUS A,SAJET.SYS_PART B WHERE SERIAL_NUMBER=:sn AND A.MODEL_ID=B.PART_ID";
            string[,] p = { { "sn", loraid.ToUpper() } };
            string partNo = "";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql, p))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    partNo = null;
                    throw new Exception("沒有找到loraid對應的料號信息");
                }
                partNo = dt.Rows[0][0].ToString();
            }
            sql = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PARAM_NAME='CUSTOPTIONVAL_HOLLEY'";
            string paramString = "";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    partNo = null;
                    paramString = null;
                    throw new Exception("沒有找到欄位配置信息");
                }
                paramString = dt.Rows[0][0].ToString();
            }
            sql = MakeOptionColumns(paramString, partNo);

            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                string[] param = paramString.Split('|');
                DataTable dtValue = new DataTable();
                dtValue.Columns.Add("Category");
                dtValue.Columns.Add("Value");

                foreach (string one in param)
                {
                    DataRow dr = dtValue.NewRow();
                    dr["Category"] = one;
                    dr["Value"] = "";
                    dtValue.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtValue.Rows)
                    {
                        dr["Value"] = dt.Rows[0][dr["Category"].ToString()].ToString();
                    }
                }
                else
                {
                    throw new Exception("沒有找到料號的配置信息");
                }

                return dtValue;
            }
        }

        private string MakeOptionColumns(string paramString,string pn)
        {
            string[] param = paramString.Split('|');
            string options = "SELECT KEYVALUE PARTNO";
            for (int i = 1; i <= param.Length; i++)
            {
                options += ",OPTION" + i.ToString() + " \"" + param[i - 1] + "\"";
            }
            options += " FROM SAJET.CUSTOMER_OPTION_VALUE WHERE CUSTOMER='HOLLEY' AND KEYVALUE='" + pn + "'";
            return options;
        }
    }
}