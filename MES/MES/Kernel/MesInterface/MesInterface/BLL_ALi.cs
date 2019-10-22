using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_ALi : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_ALi dal = new DAL_ALi();

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
            string sn = bllOther.GetParaValueReturnString(param, "sn");
            try
            {
                return dal.QueryData(cmdCode, sn);
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }
    }
}