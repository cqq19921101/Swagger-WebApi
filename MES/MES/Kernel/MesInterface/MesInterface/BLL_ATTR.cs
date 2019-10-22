using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_ATTR : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_ATTR dal = new DAL_ATTR();

        public override DataSet DoQuery(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string sn = bllOther.GetParaValueReturnString(param, "sn");
            string key = bllOther.GetParaValueReturnString(param, "key");          

            try
            {
                string value = dal.QueryData(sn, key);
                DataSet ds = bllOther.GetReturnDataSet("OK", "OK");
                DataTable dtValue = new DataTable();
                dtValue.Columns.Add("Category");
                dtValue.Columns.Add("Value");
                DataRow dr = dtValue.NewRow();
                dr["Category"] = key.ToUpper();
                dr["Value"] = value;
                dtValue.Rows.Add(dr);
                ds.Tables.Add(dtValue);
                return ds;
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string sn = bllOther.GetParaValueReturnString(param, "sn");
            string key = bllOther.GetParaValueReturnString(param, "key");
            string value = bllOther.GetParaValueReturnString(param, "value");
            string rel = "";
            try
            {
                rel = dal.SaveData(sn, key, value);
                if (rel == "OK")
                {
                    return bllOther.GetReturnDataTable("PASS", "");
                }
                else
                {
                    return bllOther.GetReturnDataTable("FAIL", rel);
                }
            }
            catch (Exception ex)
            {
                rel = ex.Message;
                return bllOther.GetReturnDataTable("FAIL", rel);
            }
        }
    }
}