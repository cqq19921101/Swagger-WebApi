using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_SMTSPI : DAL
    {
        /// <summary>
        /// 查詢是否存在數據
        /// </summary>
        /// <param name="machine_code">設備名稱</param>
        /// <returns></returns>
        public DataTable QueryData(string machine_code)
        {
            string sql = string.Format("select * from liteon.smt_spi_status where machine_code ='{0}'", machine_code);

            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                return dt;
            }
        }
        /// <summary>
        /// 發送fail時新增一台記錄，pass刪除記錄
        /// </summary>
        /// <param name="factory_id">廠別</param>
        /// <param name="machine_code">設備名稱</param>
        /// <param name="status">狀態</param>
        /// <returns></returns>
        public string SaveData(string factory_id, string machine_code, string status)
        {
            //故障狀態
            if (status != "OK")
            {
                //DataTable dt = QueryData(machine_code);
                //if(dt.Rows.Count != 0)
                //{
                //    return "OK";
                //}

                string sql = string.Format("INSERT INTO liteon.smt_spi_status(factory_id,machine_code,status,update_time) values('{0}','{1}','{2}',sysdate)", factory_id, machine_code, status);
                try
                {
                    this.ExecSQLNonQuery(sql);
                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
            else
            {
                string sql = string.Format("delete from  liteon.smt_spi_status where machine_code = '{0}'", machine_code);
                try
                {
                    this.ExecSQLNonQuery(sql);
                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

        }
    }
}