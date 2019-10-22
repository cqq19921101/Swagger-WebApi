using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_Picking : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_Packing dal = new DAL_Packing();

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
            string terminalId = bllOther.GetParaValueReturnString(param, "terminalid");
            string empno= bllOther.GetParaValueReturnString(param, "empno");
            try
            {
                string rel = "";
                if (cmdCode == "1")
                {
                    //判斷站別
                    rel = dal.QueryRouting(sn);
                    if (rel == "OK")
                    {
                        return bllOther.GetReturnDataSet("OK", "OK");
                    }
                    else
                    {
                        throw new Exception(rel);
                    }
                }
                else if (cmdCode == "2")
                {
                    //查料號，換手臂程序
                    using (DataTable dt = dal.QueryPartNo(sn))
                    {
                        DataSet ds = new DataSet();
                        DataTable dtRel = bllOther.GetReturnDataSet("OK", "OK").Tables[0];
                        DataTable dtRel2 = dtRel.Clone();
                        foreach (DataRow dr in dtRel.Rows)
                        {
                            DataRow drNew = dtRel2.NewRow();
                            drNew.ItemArray = dr.ItemArray;
                            dtRel2.Rows.Add(drNew);
                        }


                        ds.Tables.Add(dtRel2);
                        ds.Tables.Add(dt);
                        return ds;
                    }
                }
                else if (cmdCode == "3")
                {
                    //判斷站別，正確的就過站
                    rel = dal.QueryRoutingNew(sn,terminalId,empno);
                    if (rel == "OK")
                    {
                        return bllOther.GetReturnDataSet("OK", "OK");
                    }
                    else
                    {
                        throw new Exception(rel);
                    }
                }
                else
                {
                    throw new Exception("cmdCode未識別");
                }                
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }
    }
}