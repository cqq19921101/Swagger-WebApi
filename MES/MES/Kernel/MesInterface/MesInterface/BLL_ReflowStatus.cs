using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_ReflowStatus : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_ReflowStatus dal = new DAL_ReflowStatus();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            //deveui, appskey, newkskey, devaddr

            string pdline = bllOther.GetParaValueReturnString(param, "pdline");
            string status = bllOther.GetParaValueReturnString(param, "status");

            string rel = "";
            try
            {
                rel = dal.SaveData(pdline.ToUpper(), status.ToUpper());
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
    }
}