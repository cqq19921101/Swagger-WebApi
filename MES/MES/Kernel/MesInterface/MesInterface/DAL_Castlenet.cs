using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_Castlenet : DAL
    {
        public string SaveData(string sigfoxId, string readId,string readPac,string key,string fwVer,string sigfoxLibVer,
                string f86813power,string f86813freqErr,string f90220power,string f90220freqErr,
                string f92320power,string f92320freqErr,string f92080power,string f92080freqErr,
                string f92325power,string f92325freqErr)
        {
            try
            {
                string sql = "LITEON.USP_SAVE_CASTLENET_DATA";
                OracleParameter[] ops = {
                    new OracleParameter("v_sigfoxId",OracleDbType.Varchar2,0,sigfoxId,ParameterDirection.Input),
                    new OracleParameter("v_readId",OracleDbType.Varchar2,0,readId,ParameterDirection.Input),
                    new OracleParameter("v_readPac",OracleDbType.Varchar2,0,readPac,ParameterDirection.Input),
                    new OracleParameter("v_key",OracleDbType.Varchar2,0,key,ParameterDirection.Input),
                    new OracleParameter("v_fwVer",OracleDbType.Varchar2,0,fwVer,ParameterDirection.Input),
                    new OracleParameter("v_sigfoxLibVer",OracleDbType.Varchar2,0,sigfoxLibVer,ParameterDirection.Input),
                    new OracleParameter("v_f86813power",OracleDbType.Varchar2,0,f86813power,ParameterDirection.Input),
                    new OracleParameter("v_f86813freqErr",OracleDbType.Varchar2,0,f86813freqErr,ParameterDirection.Input),
                    new OracleParameter("v_f90220power",OracleDbType.Varchar2,0,f90220power,ParameterDirection.Input),
                    new OracleParameter("v_f90220freqErr",OracleDbType.Varchar2,0,f90220freqErr,ParameterDirection.Input),
                    new OracleParameter("v_f92320power",OracleDbType.Varchar2,0,f92320power,ParameterDirection.Input),
                    new OracleParameter("v_f92320freqErr",OracleDbType.Varchar2,0,f92320freqErr,ParameterDirection.Input),
                    new OracleParameter("v_f92080power",OracleDbType.Varchar2,0,f92080power,ParameterDirection.Input),
                    new OracleParameter("v_f92080freqErr",OracleDbType.Varchar2,0,f92080freqErr,ParameterDirection.Input),
                    new OracleParameter("v_f92325power",OracleDbType.Varchar2,0,f92325power,ParameterDirection.Input),
                    new OracleParameter("v_f92325freqErr",OracleDbType.Varchar2,0,f92325freqErr,ParameterDirection.Input),
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