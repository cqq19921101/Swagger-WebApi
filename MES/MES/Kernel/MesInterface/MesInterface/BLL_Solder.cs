using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_Solder : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_Solder dal = new DAL_Solder();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {            
            string rel = "";
            try
            {
                string sn = bllOther.GetParaValueReturnString(param, "sn");
                string fixtureno = bllOther.GetParaValueReturnString(param, "fixtureno");
                string empno = bllOther.GetParaValueReturnString(param, "empno");
                rel = dal.SaveWarmData(cmdCode, sn, fixtureno, empno);
                if (rel == "OK")
                {
                    return bllOther.GetReturnDataTable("PASS", "");
                }
                else
                {
                    throw new Exception(rel);
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