using LiteOn.EA.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPSMJ_GetRealTimeEvent
{
    class Program
    {
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        static void Main(string[] args)
        {
            GetData();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void GetData()
        {
            string url = "http://10.141.64.50:8088/api/transaction/monitor?timestamp=1494467276539&access_token=jerry";//正式区
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "GET";

            string result = "";
            try
            {
                using (var res = request.GetResponse() as HttpWebResponse)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                bool status = true;
                request.Abort();
            }
            catch (Exception ex)
            {
                bool status = false;
                request.Abort();
            }

        }
    }
}
