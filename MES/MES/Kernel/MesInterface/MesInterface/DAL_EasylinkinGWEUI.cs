using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_EasylinkinGWEUI : DAL
    {
        public string SaveData(string mac, string gweui,string status)
        {
            try
            {
                string sql = "LITEON.USP_SAVE_EASYLINKINGWEUIDATA";
                OracleParameter[] ops = {
                    new OracleParameter("v_mac",OracleDbType.Varchar2,0,mac.ToUpper(),ParameterDirection.Input),
                    new OracleParameter("v_gweui",OracleDbType.Varchar2,0,gweui,ParameterDirection.Input),
                    new OracleParameter("v_status",OracleDbType.Varchar2,0,status,ParameterDirection.Input),
                    new OracleParameter("v_out",OracleDbType.Varchar2,200,"",ParameterDirection.Output)
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

        public DataSet QueryData()
        {
            string sql = "LITEON.USP_QUERY_EASYLINKIN_GWEUI";
            OracleParameter[] ops = {
                        new OracleParameter("v_result",OracleDbType.RefCursor,2000,"",ParameterDirection.Output),
                        new OracleParameter("v_value",OracleDbType.RefCursor,2000,"",ParameterDirection.Output)
                    };
            using (DataSet ds = this.ExecProcReturnDataSet(sql, ops))
            {
                return ds;
            }
        }
    }
}