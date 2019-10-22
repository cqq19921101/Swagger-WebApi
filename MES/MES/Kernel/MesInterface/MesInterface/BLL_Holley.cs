using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_Holley : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_Holley dal = new DAL_Holley();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {

            string loraid = bllOther.GetParaValueReturnString(param, "loraid");
            string power = bllOther.GetParaValueReturnString(param, "power");
            string freq_error = bllOther.GetParaValueReturnString(param, "freq_error");
            string rssi = bllOther.GetParaValueReturnString(param, "rssi");
            string snr = bllOther.GetParaValueReturnString(param, "snr");
            string info_1 = bllOther.GetParaValueReturnString(param, "info_1");
            string info_2 = bllOther.GetParaValueReturnString(param, "info_2");
            string info_3 = bllOther.GetParaValueReturnString(param, "info_3");

            string rel = "";
            try
            {
                rel = dal.SaveData(loraid, power, freq_error, rssi, snr, info_1, info_2, info_3);
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
            string loraid = bllOther.GetParaValueReturnString(param, "loraid");
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

                    DataTable dtValue = dal.QueryData(loraid);

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