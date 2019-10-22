using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_ReflowStatus : DAL
    {
        public string SaveData(string pdline, string status)
        {
            try
            {
                string sql = "LITEON.USP_REFLOW_UPDATESTATUS";
                OracleParameter[] ops = {
                    new OracleParameter("v_pdline",OracleDbType.Varchar2,0,pdline,ParameterDirection.Input),
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
    }
}