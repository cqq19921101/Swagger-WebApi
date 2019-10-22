using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_SmtAlert:BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_SmtAlert dal = new DAL_SmtAlert();

        public override DataSet DoQuery(string cmdCode,string factoryId, Dictionary<string,object> param,  string urlString)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("iResult");
            dt1.Columns.Add("iMessage");
            dt1.TableName = "Table1";
            DataRow dr = dt1.NewRow();
            dr["iResult"] = "OK";
            dr["iMessage"] = "";
            dt1.Rows.Add(dr);
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Category");
            dt2.Columns.Add("Value");
            dt2.TableName = "Table2";
            DataRow dr1 = dt2.NewRow();
            dr1["Category"] = "Cate1";
            dr1["Value"] = "Value1";
            dt2.Rows.Add(dr1);
            DataRow dr2 = dt2.NewRow();
            dr2["Category"] = "Cate2";
            dr2["Value"] = "Value2";
            dt2.Rows.Add(dr2);
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            return ds;
        }

        public override DataTable DoAdd(string cmdCode,string factoryId, Dictionary<string, object> param, string urlString)
        {
            string data = bllOther.GetParaValueReturnString(param, "data");
            string[] list = data.Split(';');
            string rel = "";
            try
            {
                if (list.Length != 3)
                {
                    return bllOther.GetReturnDataTable("FAIL","字符串長度不對. Data: " + data);
                }
                rel = dal.SaveData(list[1], list[2]);
                if (rel != "OK")
                {
                    rel = rel + ". Data: " + data;
                    return bllOther.GetReturnDataTable("FAIL", rel);
                }
                return bllOther.GetReturnDataTable("PASS", "");
            }
            catch (Exception ex)
            {
                rel = ex.Message + ". Data: " + data;
                return bllOther.GetReturnDataTable("FAIL", rel);
            }
        }
    }
}