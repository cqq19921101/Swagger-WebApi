using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_SMTSPI : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_SMTSPI dal = new DAL_SMTSPI();

       

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string machine_code = bllOther.GetParaValueReturnString(param, "machine");
            string status = bllOther.GetParaValueReturnString(param, "status");
            //string calltype = bllOther.GetParaValueReturnString(param, "calltype");
            //string pn = bllOther.GetParaValueReturnString(param, "pn");
            //string qty = bllOther.GetParaValueReturnString(param, "qty");
            //string targetLocation = bllOther.GetParaValueReturnString(param, "targetlocation");

            string rel = "";
            try
            {
                rel = dal.SaveData(factoryId,machine_code,status);
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