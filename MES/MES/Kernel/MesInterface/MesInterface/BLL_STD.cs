using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MesInterface
{
    public class BLL_STD : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_STD dal = new DAL_STD();

        public override DataSet DoQuery(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string sn = bllOther.GetParaValueReturnString(param, "sn");
            string line = bllOther.GetParaValueReturnString(param, "line");
            string process = bllOther.GetParaValueReturnString(param, "process");
            string terminal = bllOther.GetParaValueReturnString(param, "terminal");

            try
            {

                return dal.QueryData(sn,line,process,terminal);
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string cmd = bllOther.GetParaValueReturnString(param, "cmd");
            string cmdString = bllOther.GetParaValueReturnString(param, "cmdstring");

            string rel = "";
            try
            {

                //rel = dal.CallSp(cmd, cmdString);
                //if (rel == "沒有返回結果")
                //{
                //    return bllOther.GetReturnDataTable("FAIL", rel);
                //}
                return bllOther.GetReturnDataTable("PASS", rel);
            }
            catch (Exception ex)
            {
                rel = ex.Message;
                return bllOther.GetReturnDataTable("FAIL", rel);
            }
        }
    }
}