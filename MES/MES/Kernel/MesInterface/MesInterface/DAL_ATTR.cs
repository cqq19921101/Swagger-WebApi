using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_ATTR : DAL
    {
        public string SaveData(string sn,string key,string value)
        {
            try
            {
                string sql = "SAJET.USP_MESINTERFACE_SAVEATTR";
                OracleParameter[] ops = {
                        new OracleParameter("v_sn",OracleDbType.Varchar2,0,sn.ToUpper(),ParameterDirection.Input),
                        new OracleParameter("v_key",OracleDbType.Varchar2,0,key.ToUpper(),ParameterDirection.Input),
                        new OracleParameter("v_value",OracleDbType.Varchar2,0,value,ParameterDirection.Input),
                        new OracleParameter("v_out",OracleDbType.Varchar2,2000,"",ParameterDirection.Output)
                    };
                using (DataTable dt = this.ExecProcReturnOutput(sql, ops))
                {
                    return dt.Rows[0]["v_out"].ToString();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string QueryData(string sn, string key)
        {
            try
            {
                string sql = "select value from sajet.sn_attr where sn=:sn and key=:qkey";
                string[,] p = { { "sn", sn.ToUpper() }, { "qkey", key.ToUpper() } };
                string value = "";
                using (DataTable dt = this.ExecSQLReturnDataTable(sql, p))
                {
                    if (dt.Rows.Count > 0)
                    {
                        value = dt.Rows[0][0].ToString();
                    }
                }
                return value;
            }
            catch
            {
                return "";
            }
        }
    }
}