using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL_LORA : BLL
    {
        BLL_Other bllOther = new BLL_Other();
        DAL_LORA dal = new DAL_LORA();

        public override DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            //dev_addr,dev_eui,gateway_eui,rssi,lsnr,freq,datr,port,uplink_count,gateway_list,acc_x,acc_y,acc_z,mag_x,mag_y,mag_z,humidity,ambient_light,atmospheric_pressure,temperature,pm25,log_time

            string dev_addr = bllOther.GetParaValueReturnString(param, "dev_addr");
            string dev_eui = bllOther.GetParaValueReturnString(param, "dev_eui");
            string gateway_eui = bllOther.GetParaValueReturnString(param, "gateway_eui");
            string rssi = bllOther.GetParaValueReturnString(param, "rssi");
            string lsnr = bllOther.GetParaValueReturnString(param, "lsnr");
            string freq = bllOther.GetParaValueReturnString(param, "freq");
            string datr = bllOther.GetParaValueReturnString(param, "datr");
            string port = bllOther.GetParaValueReturnString(param, "port");
            string uplink_count = bllOther.GetParaValueReturnString(param, "uplink_count");
            string gateway_list = bllOther.GetParaValueReturnString(param, "gateway_list");
            string acc_x = bllOther.GetParaValueReturnString(param, "acc_x");
            string acc_y = bllOther.GetParaValueReturnString(param, "acc_y");
            string acc_z = bllOther.GetParaValueReturnString(param, "acc_z");
            string mag_x = bllOther.GetParaValueReturnString(param, "mag_x");
            string mag_y = bllOther.GetParaValueReturnString(param, "mag_y");
            string mag_z = bllOther.GetParaValueReturnString(param, "mag_z");
            string humidity = bllOther.GetParaValueReturnString(param, "humidity");
            string ambient_light = bllOther.GetParaValueReturnString(param, "ambient_light");
            string atmospheric_pressure = bllOther.GetParaValueReturnString(param, "atmospheric_pressure");
            string temperature = bllOther.GetParaValueReturnString(param, "temperature");
            string pm25 = bllOther.GetParaValueReturnString(param, "pm25");
            string log_time = bllOther.GetParaValueReturnString(param, "log_time");

            string rel = "";
            try
            {

                rel = dal.SaveData(dev_addr, dev_eui, gateway_eui, rssi, lsnr, freq, datr, port,
                    uplink_count, gateway_list, acc_x, acc_y, acc_z, mag_x, mag_y, mag_z, humidity, ambient_light,
                    atmospheric_pressure, temperature, pm25, log_time);
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

        public override DataSet DoQuery(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            string dev_addr = bllOther.GetParaValueReturnString(param, "dev_addr");
            try
            {

                return dal.QueryData(dev_addr);
            }
            catch (Exception ex)
            {
                return bllOther.GetReturnDataSet("FAIL", ex.Message);
            }
        }
    }
}