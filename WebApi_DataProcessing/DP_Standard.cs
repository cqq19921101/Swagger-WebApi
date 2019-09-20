using LiteOn.EA.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_DataProcessing
{
    public abstract class DP_Standard
    {

        static string conn = ConfigurationManager.AppSettings["DBConnection"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        public DP_Standard()
        {

        }

        #region 抽象方法 區分 各Site或者各BU
        public abstract DataTable ReturnTECOByCZOPS(string PlantNo,string LineCode);//CZ OPS


        #endregion

        #region Public Function ------ GetTECO


        /// <summary>
        /// 計算TECO Yield
        /// </summary>
        /// <param name="plantno"></param>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static DataTable ComputeTecoYield(string plantno, string description, DateTime nowTime)
        {
            try
            {
                DataTable TecoYieldData = TecoYieldStructrue(); ;
                DataTable Temp = new DataTable();
                string LineCode = description + ".";
                Temp = ComputeSQL(plantno, LineCode, "*", "*", "*", nowTime);
                return Temp;

            }
            catch (Exception ex)
            {

            }
            return null;
        }


        /// <summary>
        /// Oracle SQL 拼接
        /// </summary>
        /// <param name="plantno"></param>
        /// <param name="description"></param>
        /// <param name="series1"></param>
        /// <param name="series2"></param>
        /// <param name="processtype"></param>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static DataTable ComputeSQL(string plantno, string description, string series1, string series2, string processtype, DateTime nowTime)
        {

            string year = nowTime.Year.ToString();
            string[] procType = processtype.Split(',');
            string month = "case when month='01' then '1' when month='02' then '2' when month='03' then '3' when month='04' then '4' " +
            "when month='05' then '5' when month='06' then '6' when month='07' then '7' when month='08' then '8' when month='09' then '9' " +
            "when month='10' then '10' when month='11' then '11' when month='12' then '12' else '#' end as month";
            StringBuilder head = new StringBuilder();
            head.Append("year,month,description");
            // head.Append("year,month,description,series1,series2,processtype");
            if (series1 != "*") head.Append(",series1");
            if (series2 != "*") head.Append(",series2");
            if (processtype != "*") head.Append(",processtype");
            StringBuilder sb = new StringBuilder();
            sb.Append("select " + head.ToString().Replace("month", month));
            sb.Append(",to_char(\"Normal PO Input\",'FM99,999,999,999,999') as \"Normal PO Input\" ,");
            sb.Append("to_char(\"Net Output\",'FM99,999,999,999,999') as \"Net Output\",");
            if (description.ToUpper() == "ALL.")
            {
                sb.Append("case when \"Net Output\" <= 0 or \"Normal PO Input\" <= 0 then '0' else to_char(round((\"Net Output\" / \"Normal PO Input\"), 4) * 10000)   end as \"T-Yield\" ");
            }
            else
            {
                sb.Append("case when \"Net Output\" <= 0 or \"Normal PO Input\" <= 0 then '99999' else to_char(round((\"Net Output\" / \"Normal PO Input\"), 4) * 10000)   end as \"T-Yield\" ");
            }
            sb.Append("from ( select " + head.ToString() + ", ");
            sb.Append("sum(case when ordertype='ZO01' then grqty_101 + grqty_531 + scrapqty else 0 end) as \"Normal PO Input\" ,");
            sb.Append("(sum(case when ordertype in ('ZO01','ZO06') then grqty_101+ grqty_531  else 0 end) -");
            sb.Append("sum(case when ordertype in ('ZO02','ZO03','ZO07') then scrapqty + 0 else 0 end))as \"Net Output\",");
            sb.Append("sum(case when ordertype in ('ZO01','ZO02','ZO03','ZO07') then scrapqty+0 else 0 end) as \"Total Scrap\",");
            sb.Append("sum(case when ordertype='ZO06' then grqty_101 + grqty_531  else 0 end) as Bonus,");
            sb.Append("sum(case when ordertype='ZO01' then grqty_101 + grqty_531 + scrapqty else 0 end) as \"NormalPOInput\" ,");
            sb.Append("sum(case when ordertype='ZO01' then grqty_101 + grqty_531 else 0 end) as \"NormalPOOutput\" ,");
            sb.Append("sum(case when ordertype='ZO02' then grqty_101 + grqty_531+scrapqty  else 0 end) as \"ReWorkPOInput\",");
            sb.Append("sum(case when ordertype='ZO02' then grqty_101 + grqty_531  else 0 end) as \"ReWorkPOOutput\",");

            sb.Append("sum(case when ordertype='ZO06' then grqty_101 + grqty_531+scrapqty  else 0 end) as \"BonusPOInput\",");
            sb.Append("sum(case when ordertype='ZO06' then grqty_101 + grqty_531  else 0 end) as \"BonusPOOutput\" ");
            sb.Append("from ledoaydatatecoyield o where o.year='" + year + "'  and  o.plantno='" + plantno + "' and o.tecodate < '" + nowTime.ToString("yyyyMMdd") + "' ");
            if (description.ToUpper() != "ALL.")
            {
                sb.Append("  and description='" + description + "' and o.ordertype not in('ZO04','ZO05') ");
            }
            //sb.Append("from ledoaydatatecoyield_eai2 o where o.year='" + year + "'  and  plantno='" + plantno + "' and description='" + description + "' and o.ordertype not in('ZO04','ZO05') ");
            sb.Append("and upper(nvl(series1,' ')) like  decode(upper('" + series1 + "'),'*','%',upper('" + series1 + "')) ");
            sb.Append("and upper(nvl(series2,' ')) like  decode(upper('" + series2 + "'),'*','%',upper('" + series2 + "')) ");
            // sb.Append("and upper(nvl(processtype,' ')) like  decode(upper('" + processtype + "'),'*','%',upper('" + processtype + "')) ");
            sb.Append("group  by ");
            sb.Append(head.ToString());
            sb.Append(" )order by month desc");
            string sqlBuilder = sb.ToString().Replace("'", "''");
            StringBuilder sql = new StringBuilder();
            if (description.ToUpper() == "ALL.")
            {
                sql.Append("select Top 1 T1.[YEAR],T1.[MONTH],'ALL' as 'DESCRIPTION' ,");
                sql.Append("  SUM(Convert(bigint, REPLACE(T1.[Net Output], ',', ''))) as 'NetOutput',");
                sql.Append("  SUM(Convert(bigint, REPLACE(T1.[Normal PO Input], ',', ''))) as 'NormalPOInput',");
                sql.Append("  Convert(int,cast(SUM(Convert(float, REPLACE(T1.[Net Output], ',', ''))) / SUM(Convert(float, REPLACE(T1.[Normal PO Input], ',', ''))) as decimal(18, 4)) * 10000)  as 'currentTECO',");
                sql.Append(" 10000 as targetTECO");
                sql.Append(" from openquery(MESDB12," + "'" + sqlBuilder + "') T1");
                sql.Append(" group by T1.[YEAR],T1.[MONTH]");
                sql.Append(" order by MONTH DESC");
            }
            else
            {
                sql.Append("select Top 1 T1.[YEAR],T1.[MONTH],T1.[DESCRIPTION],Convert(int,T1.[T-Yield]) as 'currentTECO', 10000 as targetTECO  from openquery(MESDB12," + "'" + sqlBuilder + "') T1 ");
            }

            //DataTable dt = DataBase.SqlToTable(sb.ToString());
            DataTable dt = sdb.GetDataTable(sql.ToString(), opc);
            return dt;
        }


        /// <summary>
        /// TECOYield構造
        /// </summary>
        /// <returns></returns>
        public static DataTable TecoYieldStructrue()
        {
            DataTable dt = new DataTable();
            DataColumn[] dcs = new DataColumn[14];
            dcs[0] = new DataColumn("Line");
            dcs[1] = new DataColumn(DateTime.Now.Year.ToString());
            dcs[2] = new DataColumn("Jan");
            dcs[3] = new DataColumn("Feb");
            dcs[4] = new DataColumn("Mar");
            dcs[5] = new DataColumn("Apr");
            dcs[6] = new DataColumn("May");
            dcs[7] = new DataColumn("Jun");
            dcs[8] = new DataColumn("Jul");
            dcs[9] = new DataColumn("Aug");
            dcs[10] = new DataColumn("Sep");
            dcs[11] = new DataColumn("Oct");
            dcs[12] = new DataColumn("Nov");
            dcs[13] = new DataColumn("Dec");
            dt.Columns.AddRange(dcs);
            return dt;
        }


        /// <summary>
        /// 行拼接
        /// </summary>
        /// <param name="rowname"></param>
        /// <param name="valuename"></param>
        /// <param name="TecoYieldData"></param>
        /// <returns></returns>
        public static DataTable AppendRow(string[] rowname, string[] valuename, DataTable TecoYieldData)
        {
            DataRow drDate = TecoYieldData.NewRow();
            for (int i = 0; i < rowname.Length; i++)
            {
                drDate[rowname[i]] = valuename[i];

            }
            TecoYieldData.Rows.Add(drDate);
            return TecoYieldData;
        }

        /// <summary>
        /// Value 填充
        /// </summary>
        /// <param name="row"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="TecoYieldData"></param>
        /// <returns></returns>
        public static DataTable PutValue(int[] row, string key, string[] value, DataTable TecoYieldData)
        {
            for (int i = 0; i < value.Length; i++)
            {
                TecoYieldData.Rows[row[i]][key] = value[i];
            }
            return TecoYieldData;
        }
        #endregion


    }
}
