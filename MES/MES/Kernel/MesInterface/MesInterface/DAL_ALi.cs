using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_ALi : Liteon.Mes.Db.Oracle
    {
        public DataSet QueryData(string cmdCode, string sn)
        {
            string sql = "LITEON.USP_QUERY_AliData";
            OracleParameter[] ops = {
                 new OracleParameter("v_cmdCode",OracleDbType.Varchar2,0,cmdCode,ParameterDirection.Input),
                  new OracleParameter("v_sn",OracleDbType.Varchar2,0,sn.ToUpper(),ParameterDirection.Input),
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