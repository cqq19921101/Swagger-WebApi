using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_Easylinkin : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_Easylinkin dal = new DAL_Easylinkin();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            //deveui, appskey, newkskey, devaddr

            string deveui = bllOther.GetParaValueReturnString(param, "deveui");
            string appkey = bllOther.GetParaValueReturnString(param, "appkey");
            string appskey = bllOther.GetParaValueReturnString(param, "appskey");
            string nwkskey = bllOther.GetParaValueReturnString(param, "nwkskey");
            string devaddr = bllOther.GetParaValueReturnString(param, "devaddr");
           
            string rel = "";
            try
            {
                rel = dal.SaveData(deveui, appkey, appskey, nwkskey, devaddr);
                if (rel != "OK")
                {
                    return bllOther.GetReturnDataTable("FAIL", rel);
                }
                return bllOther.GetReturnDataTable("PASS", "");
            }
            catch (Exception ex)
            {
                rel = ex.Message;
                return bllOther.GetReturnDataTable("FAIL", rel);
            }
        }

        public override DataSet DoQuery(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string dev_addr = bllOther.GetParaValueReturnString(param, "deveui");
            try
            {
                using (DataSet ds = new DataSet())
                {
                    DataTable dtRel = bllOther.GetReturnDataSet("OK", "OK").Tables[0];
                    DataTable dtRel2 = dtRel.Clone();
                    foreach(DataRow dr in dtRel.Rows)
                    {
                        DataRow drNew = dtRel2.NewRow();
                        drNew.ItemArray = dr.ItemArray;
                        dtRel2.Rows.Add(drNew);
                    }

                    DataTable dtValue = dal.QueryData(dev_addr);

                    ds.Tables.Add(dtRel2);
                    ds.Tables.Add(dtValue);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }
    }
}