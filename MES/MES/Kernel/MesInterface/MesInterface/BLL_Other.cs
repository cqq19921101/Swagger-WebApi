using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_Other
    {
        public object GetParaValue(Dictionary<string, object> dict, string filter)
        {
            try
            {
                object value = "";
                if (dict.TryGetValue(filter, out value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public string GetParaValueReturnString(Dictionary<string, object> dict, string filter)
        {
            try
            {
                object value = "";
                if (dict.TryGetValue(filter, out value))
                {
                    return (string)value;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public DataTable GetReturnDataTable(string result,string msg)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("iResult");
            dtResult.Columns.Add("iMessage");
            DataRow dr = dtResult.NewRow();
            dr["iResult"] = result;
            dr["iMessage"] = msg;
            dtResult.Rows.Add(dr);
            return dtResult;
        }

        public DataSet GetReturnDataSet(string result, string msg)
        {
            DataSet ds = new DataSet();
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("iResult");
            dtResult.Columns.Add("iMessage");
            DataRow dr = dtResult.NewRow();
            dr["iResult"] = result;
            dr["iMessage"] = msg;
            dtResult.Rows.Add(dr);
            ds.Tables.Add(dtResult);
            return ds;
        }

        public void SaveErrorLog(string factoryId, string commandType, string funcType, string sn,
           string checkCmdResult, string urlContent, string ip)
        {
            DAL_Other dal = new MesInterface.DAL_Other();
            dal.SaveErrorLog(factoryId, commandType, funcType, sn, checkCmdResult, urlContent, ip);
        }

        public void SaveLog(string factoryId,string commandType,string funcType,string sn,
            string requestTime,string responseTime,string urlContent,string ip)
        {
            DAL_Other dal = new MesInterface.DAL_Other();
            dal.SaveLog(factoryId, commandType, funcType, sn, requestTime, responseTime, urlContent, ip);
        }
    }
}