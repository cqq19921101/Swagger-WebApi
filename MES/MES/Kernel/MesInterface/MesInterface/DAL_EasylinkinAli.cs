using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_EasylinkinAli : DAL
    {
        public DataTable QueryData(string deveui)
        {
            string sql = "SELECT B.PART_NO FROM SAJET.G_SN_STATUS A,SAJET.SYS_PART B WHERE SERIAL_NUMBER=:sn AND A.MODEL_ID=B.PART_ID";
            string[,] p = { { "sn", deveui.ToUpper() } };
            string partNo = "";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql, p))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    partNo = null;
                    throw new Exception("沒有找到deveui對應的料號信息");
                }
                partNo = dt.Rows[0][0].ToString();
            }
            sql = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PARAM_NAME='CUSTOPTIONVAL_慧聯_Ali'";
            string paramString = "";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                if (dt.Rows.Count == 0)
                {
                    sql = null;
                    partNo = null;
                    paramString = null;
                    throw new Exception("沒有找到欄位配置信息");
                }
                paramString = dt.Rows[0][0].ToString();
            }
            sql = MakeOptionColumns(ref paramString, partNo, deveui);

            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                string[] param = paramString.Split('|');
                DataTable dtValue = new DataTable();
                dtValue.Columns.Add("Category");
                dtValue.Columns.Add("Value");

                foreach (string one in param)
                {
                    DataRow dr = dtValue.NewRow();
                    dr["Category"] = one;
                    dr["Value"] = "";
                    dtValue.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtValue.Rows)
                    {
                        dr["Value"] = dt.Rows[0][dr["Category"].ToString()].ToString();
                    }
                }
                else
                {
                    throw new Exception("沒有找到料號的配置信息");
                }

                return dtValue;
            }
        }

        private string MakeOptionColumns(ref string paramString,string pn,string sn)
        {
            string[] param = paramString.Split('|');
            string options = "SELECT KEYVALUE PARTNO";
            for (int i = 1; i <= param.Length; i++)
            {
                options += ",OPTION" + i.ToString() + " \"" + param[i - 1] + "\"";
            }
            options += " ,PIN_CODE,APPKEY,APPEUI JOINEUI FROM SAJET.CUSTOMER_OPTION_VALUE a,LITEON.EASYLINKIN_DEVEUI b WHERE CUSTOMER='慧聯_Ali' AND KEYVALUE='" + pn + "' AND b.deveui='"+sn+"'";
            paramString += "|PIN_CODE|APPKEY|JOINEUI";
            return options;
        }
    }
}