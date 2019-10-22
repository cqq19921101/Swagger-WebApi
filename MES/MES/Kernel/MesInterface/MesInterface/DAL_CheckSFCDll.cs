using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_CheckSFCDll : DAL
    {
        public string CallSp(string cmd, string rev)
        {
            string sql = "liteon.call_dll_return_string";
            OracleParameter[] ops = { 
                new OracleParameter("tcmd",OracleDbType.Varchar2,0,cmd,ParameterDirection.Input),
                new OracleParameter("trev",OracleDbType.Varchar2,0,rev,ParameterDirection.Input),
                new OracleParameter("tres",OracleDbType.Varchar2,2000,"",ParameterDirection.Output)
            };
            using (DataTable dt = this.ExecProcReturnOutput(sql, ops))
            {
                if (dt.Rows.Count == 0)
                {
                    return "沒有返回結果";
                }
                else
                {
                    return dt.Rows[0]["tres"].ToString();
                }
            }
        }
    }
}