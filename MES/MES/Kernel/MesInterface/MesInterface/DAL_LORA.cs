using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace MesInterface
{
    public class DAL_LORA : DAL
    {
        public string SaveData(string dev_addr, string dev_eui, string gateway_eui, string rssi,
            string lsnr, string freq, string datr, string port, string uplink_count, string gateway_list,
            string acc_x, string acc_y, string acc_z, string mag_x, string mag_y, string mag_z, string humidity,
            string ambient_light, string atmospheric_pressure, string temperature, string pm25, string log_time)
        {
            string sql = "INSERT INTO LITEON.LORA_LOG(DEV_ADDR,DEV_EUI,GATEWAY_EUI,RSSI,LSNR,"
                + "FREQ,DATR,PORT,UPLINK_COUNT,GATEWAY_LIST,ACC_X,ACC_Y,ACC_Z,MAG_X,MAG_Y,MAG_Z,"
                + "HUMIDITY,AMBIENT_LIGHT,ATMOSPHERIC_PRESSURE,TEMPERATURE,PM25,LOG_TIME) VALUES ("
                + ":dev_addr,:dev_eui,:gateway_eui,:rssi,:lsnr,:freq,:datr,:port,:uplink_count,:gateway_list,"
                + ":acc_x,:acc_y,:acc_z,:mag_x,:mag_y,:mag_z,:humidity,:ambient_light,:atmospheric_pressure,"
                + ":temperature,:pm25,to_date(:log_time,'yyyy-mm-dd hh24:mi:ss'))";
            string[,] p = { { "dev_addr", dev_addr }, { "dev_eui", dev_eui }, { "gateway_eui", gateway_eui },
                {"rssi",rssi }, {"lsnr",lsnr }, {"freq",freq }, {"datr",datr }, {"port",port },
                {"uplink_count",uplink_count }, {"gateway_list",gateway_list }, {"acc_x",acc_x }, {"acc_y",acc_y },
                {"acc_z",acc_z }, {"mag_x",mag_x }, {"mag_y",mag_y }, {"mag_z",mag_z }, {"humidity",humidity },
                {"ambient_light",ambient_light }, {"atmospheric_pressure",atmospheric_pressure },
                {"temperature",temperature }, {"pm25",pm25 }, {"log_time",log_time }};
            try
            {
                this.ExecSQLNonQuery(sql, p);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataSet QueryData(string devAddr)
        {
            string sql = "LITEON.USP_QUERY_LORADATA";
            OracleParameter[] ops = {
                        new OracleParameter("v_devAddr",OracleDbType.Varchar2,0,devAddr,ParameterDirection.Input),
                        new OracleParameter("v_result",OracleDbType.RefCursor,2000,"",ParameterDirection.Output),
                        new OracleParameter("v_value",OracleDbType.RefCursor,2000,"",ParameterDirection.Output)
                    };
           using(DataSet ds = this.ExecProcReturnDataSet(sql, ops))
            {
                return ds;
            }
        }
    }
}