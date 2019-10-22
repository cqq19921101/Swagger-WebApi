using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_EasylinkinAli : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_EasylinkinAli dal = new DAL_EasylinkinAli();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {

            string rel = "";
            try
            {
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
            
            try
            {
                string sn = bllOther.GetParaValueReturnString(param, "deveui");
                using (DataSet ds = new DataSet())
                {
                    DataTable dtRel = bllOther.GetReturnDataSet("OK", "OK").Tables[0];
                    DataTable dtRel2 = dtRel.Clone();
                    foreach (DataRow dr in dtRel.Rows)
                    {
                        DataRow drNew = dtRel2.NewRow();
                        drNew.ItemArray = dr.ItemArray;
                        dtRel2.Rows.Add(drNew);
                    }

                    DataTable dtValue = dal.QueryData(sn);

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