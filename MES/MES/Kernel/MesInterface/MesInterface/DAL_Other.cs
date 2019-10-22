using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_Other : DAL
    {
        public void SaveLog(string factoryId, string commandType, string funcType, string sn,
           string requestTime, string responseTime, string urlContent, string ip)
        {
            try
            {
                string sql = "liteon.USP_MES_INTERFACE_LOG";
                OracleParameter[] ops = {
                        new OracleParameter("v_factoryId",OracleDbType.Varchar2,20,factoryId,ParameterDirection.Input),
                        new OracleParameter("v_cmdType",OracleDbType.Varchar2,20,commandType,ParameterDirection.Input),
                        new OracleParameter("v_func",OracleDbType.Varchar2,30,funcType,ParameterDirection.Input),
                        new OracleParameter("v_sn",OracleDbType.Varchar2,100,sn,ParameterDirection.Input),
                        new OracleParameter("v_url",OracleDbType.Varchar2,2000,urlContent,ParameterDirection.Input),
                        new OracleParameter("v_request",OracleDbType.Varchar2,20,requestTime,ParameterDirection.Input),
                        new OracleParameter("v_response",OracleDbType.Varchar2,20,responseTime,ParameterDirection.Input),
                        new OracleParameter("v_ip",OracleDbType.Varchar2,50,ip,ParameterDirection.Input)

                };
                this.ExecProcNonQuery(sql, ops);
            }
            catch(Exception ex)
            {

            }
        }

        public void SaveErrorLog(string factoryId, string commandType, string funcType, string sn,
           string checkCmdResult, string urlContent, string ip)
        {
            try
            {
                string sql = "liteon.USP_MES_INTERFACE_ERRLOG";
                OracleParameter[] ops = {
                        new OracleParameter("v_factoryId",OracleDbType.Varchar2,20,factoryId,ParameterDirection.Input),
                        new OracleParameter("v_cmdType",OracleDbType.Varchar2,20,commandType,ParameterDirection.Input),
                        new OracleParameter("v_func",OracleDbType.Varchar2,30,funcType,ParameterDirection.Input),
                        new OracleParameter("v_sn",OracleDbType.Varchar2,100,sn,ParameterDirection.Input),
                        new OracleParameter("v_url",OracleDbType.Varchar2,2000,urlContent,ParameterDirection.Input),
                        new OracleParameter("v_chkCmdResult",OracleDbType.Varchar2,300,checkCmdResult,ParameterDirection.Input),
                        new OracleParameter("v_ip",OracleDbType.Varchar2,50,ip,ParameterDirection.Input)

                };
                this.ExecProcNonQuery(sql, ops);
            }
            catch (Exception ex)
            {

            }
        }
    }
}