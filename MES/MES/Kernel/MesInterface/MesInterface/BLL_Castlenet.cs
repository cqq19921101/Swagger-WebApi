using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_Castlenet : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_Castlenet dal = new DAL_Castlenet();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string sigfoxId = bllOther.GetParaValueReturnString(param, "sigfoxid");
            string readId = bllOther.GetParaValueReturnString(param, "readid");
            string readPac = bllOther.GetParaValueReturnString(param, "readpac");
            string key = bllOther.GetParaValueReturnString(param, "key");
            string fwVer = bllOther.GetParaValueReturnString(param, "fwver");
            string sigfoxLibVer = bllOther.GetParaValueReturnString(param, "sigfoxlibver");
            string f86813power = bllOther.GetParaValueReturnString(param, "f86813power");
            string f86813freqErr = bllOther.GetParaValueReturnString(param, "f86813freqerr");
            string f90220power = bllOther.GetParaValueReturnString(param, "f90220power");
            string f90220freqErr = bllOther.GetParaValueReturnString(param, "f90220freqerr");
            string f92320power = bllOther.GetParaValueReturnString(param, "f92320power");
            string f92320freqErr = bllOther.GetParaValueReturnString(param, "f92320freqerr");
            string f92080power = bllOther.GetParaValueReturnString(param, "f92080power");
            string f92080freqErr = bllOther.GetParaValueReturnString(param, "f92080freqerr");
            string f92325power = bllOther.GetParaValueReturnString(param, "f92325power");
            string f92325freqErr = bllOther.GetParaValueReturnString(param, "f92325freqerr");


            string rel = "";
            try
            {
                rel = dal.SaveData(sigfoxId,readId,readPac,key,fwVer,sigfoxLibVer,f86813power,f86813freqErr,
                    f90220power,f90220freqErr,f92320power,f92320freqErr,f92080power,f92080freqErr,f92325power,f92325freqErr);
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