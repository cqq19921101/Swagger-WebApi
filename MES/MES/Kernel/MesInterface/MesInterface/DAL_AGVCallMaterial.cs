using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_AGVCallMaterial :DAL
    {
        public string SaveData(string factoryId, string line, string process, string calltype, string partno, string qty, string targetlocation)
        {
            string sql = "LITEON.USP_LGS_CALL_MATERIAL";
            OracleParameter[] ops = { 
                new OracleParameter("v_factoryId",OracleDbType.Int32,0,factoryId,ParameterDirection.Input),
                new OracleParameter("v_lineName",OracleDbType.Varchar2,0,line,ParameterDirection.Input),
                new OracleParameter("v_process",OracleDbType.Varchar2,0,process,ParameterDirection.Input),
                new OracleParameter("v_callType",OracleDbType.Varchar2,0,calltype,ParameterDirection.Input),
                new OracleParameter("v_partNo",OracleDbType.Varchar2,0,partno,ParameterDirection.Input),
                new OracleParameter("v_qty",OracleDbType.Int32,0,qty,ParameterDirection.Input),
                new OracleParameter("v_targetLocation",OracleDbType.Varchar2,0,targetlocation,ParameterDirection.Input),
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
    }
}