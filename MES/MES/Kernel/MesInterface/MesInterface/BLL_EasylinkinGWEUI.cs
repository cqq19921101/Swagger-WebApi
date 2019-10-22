using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_EasylinkGWEUI : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_EasylinkinGWEUI dal = new DAL_EasylinkinGWEUI();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string mac = bllOther.GetParaValueReturnString(param, "mac");
            string gweui = bllOther.GetParaValueReturnString(param, "gw_eui");
            string status = bllOther.GetParaValueReturnString(param, "status");
            string rel = "";
            try
            {
                rel = dal.SaveData(mac,gweui,status);
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
            try
            {

                return dal.QueryData();
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }
    }
}