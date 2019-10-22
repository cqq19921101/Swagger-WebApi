using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_Solder : DAL
    {       
        public string SaveWarmData(string type, string sn,string fixtureno, string empno)
        {
            string sql = "SAJET.USP_SOLDER_WARM";
            OracleParameter[] ops = {
                        new OracleParameter("v_type",OracleDbType.Varchar2,0,type,ParameterDirection.Input),
                        new OracleParameter("v_solderSN",OracleDbType.Varchar2,0,sn.ToUpper(),ParameterDirection.Input),
                        new OracleParameter("v_fixtureNo",OracleDbType.Varchar2,0,fixtureno,ParameterDirection.Input),
                        new OracleParameter("v_userId",OracleDbType.Varchar2,0,empno,ParameterDirection.Input),
                        new OracleParameter("v_out",OracleDbType.Varchar2,2000,"",ParameterDirection.Output)
            };

            using (DataTable dt = this.ExecProcReturnOutput(sql,ops))
            {
                sql = null;
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