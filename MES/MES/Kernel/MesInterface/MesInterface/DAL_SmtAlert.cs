using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_SmtAlert:DAL
    {
        public string SaveData(string machineCode, string errorCode)
        {
            string sql = "LITEON.usp_event_message_for_didi";
            OracleParameter[] ops = { 
                new OracleParameter("v_machine_code",OracleDbType.Varchar2,0,machineCode,ParameterDirection.Input),
                new OracleParameter("v_error_code",OracleDbType.Varchar2,0,errorCode,ParameterDirection.Input),
                new OracleParameter("v_result",OracleDbType.Varchar2,200,"",ParameterDirection.Output)
            };
            using (DataTable dt = this.ExecProcReturnOutput(sql, ops))
            {
                if (dt.Rows.Count == 0)
                {
                    return "沒有返回結果";
                }
                else
                {
                    return dt.Rows[0]["v_result"].ToString();
                }
            }
        }
    }
}