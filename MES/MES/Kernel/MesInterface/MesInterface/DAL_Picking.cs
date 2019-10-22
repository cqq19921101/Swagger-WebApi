using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_Packing : DAL
    {
       
        public string QueryRouting(string sn)
        {
            string sql = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PARAM_NAME='MES_INTERFACE_PICKING_STATION'";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    return "沒有找到MES_INTERFACE_PICKING_STATION的配置";
                }
                string processId = dt.Rows[0][0].ToString();
                sql = "SELECT PROCESS_ID FROM G_SN_STATUS WHERE SERIAL_NUMBER=:sn";
                string[,] p = { { "sn", sn.ToUpper() } };
                using (DataTable dtSN = this.ExecSQLReturnDataTable(sql, p))
                {
                    if (dtSN.Rows.Count == 0)
                    {
                        return "SN沒有找到";
                    }
                    else
                    {
                        string currProcId = dtSN.Rows[0][0].ToString();
                        if (processId == currProcId)
                        {
                            return "OK";
                        }
                        else
                        {
                            sql = "SELECT PROCESS_NAME FROM SYS_PROCESS WHERE PROCESS_ID=:currProcId";
                            string[,] p1 = { { "currProcId", currProcId } };
                            using (DataTable dtProc = this.ExecSQLReturnDataTable(sql, p1))
                            {
                                if (dtProc.Rows.Count == 0)
                                {
                                    return "站別錯誤，當前站別是 " + currProcId;
                                }
                                else
                                {
                                    return "站別錯誤，當前站別是 " + dtProc.Rows[0][0].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        public string QueryRoutingNew(string sn,string terminalId,string empno)
        {
            string sql = "sajet.SJ_CKRT_ROUTE";
            OracleParameter[] ops = {
                        new OracleParameter("TERMINALID",OracleDbType.Varchar2,0,terminalId,ParameterDirection.Input),
                        new OracleParameter("TSN",OracleDbType.Varchar2,0,sn,ParameterDirection.Input),
                        new OracleParameter("TRES",OracleDbType.Varchar2,2000,"",ParameterDirection.Output)
                    };
            using(DataTable dt = this.ExecProcReturnOutput(sql, ops))
            {
                string rel = dt.Rows[0]["TRES"].ToString();
                if (rel != "OK")
                {
                    return "站別錯誤，應該去 " + rel;
                }
                else
                {
                    //站別對的就過站
                    sql = "select to_char(sysdate,'yyyy-mm-dd hh24:mi:ss') tnow from dual";
                    using(DataTable dtNow = this.ExecSQLReturnDataTable(sql))
                    {
                        string tnow = dtNow.Rows[0][0].ToString();
                        sql = "sajet.sj_go";
                        OracleParameter[] ops2 = {
                            new OracleParameter("tterminalid",OracleDbType.Int32,0,terminalId,ParameterDirection.Input),
                            new OracleParameter("tsn",OracleDbType.Varchar2,0,sn,ParameterDirection.Input),
                            new OracleParameter("tnow",OracleDbType.Date,0,Convert.ToDateTime(tnow),ParameterDirection.Input),
                            new OracleParameter("temp",OracleDbType.Varchar2,0,empno,ParameterDirection.Input),
                            new OracleParameter("tres",OracleDbType.Varchar2,2000,"",ParameterDirection.Output)
                        };
                        using(DataTable dtGo = this.ExecProcReturnOutput(sql, ops2))
                        {
                            return dtGo.Rows[0]["tres"].ToString();
                        }
                    }
                }
            }
        }

        public DataTable QueryPartNo(string sn)
        {
            string sql = "SELECT 'PART_NO' Category,b.PART_NO VALUE FROM G_SN_STATUS a,SYS_PART b WHERE SERIAL_NUMBER=:sn AND a.MODEL_ID=b.PART_ID";
            string[,] p = { { "sn", sn.ToUpper() } };
            using (DataTable dt = this.ExecSQLReturnDataTable(sql, p))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    throw new Exception("沒有找到SN");
                }
                else
                {
                    return dt;
                }
            }
        }            
    }
}