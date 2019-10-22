using LiteOn.EA.DAL;
using Newtonsoft.Json.Linq;
using OpsMJ_New;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationByMeterAndWater
{
    class Program
    {
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        static string MeterDid = ConfigurationManager.AppSettings["METERDID"];
        static SqlDB sdb = new SqlDB(conn);
        static ArrayList opc = new ArrayList();

        static void Main(string[] args)
        {
            string[] MeterArray = MeterDid.Split(',');
            foreach (string MeterID in MeterArray)
            {
                try
                {
                    PushNotification(MeterID);

                }
                catch (Exception ex)
                {

                }                
                //GetToken();
                //CallPushAPI();
            }
        }

        /// <summary>
        /// 推送方法
        /// </summary>
        /// <param name="MeterID"></param>
        public static void PushNotification(string MeterID)
        {
            string Token = GetToken();//獲取token
            DataTable dt = GetMeteAlert(MeterID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                CallPushAPI(Token, MeterID, dr);//Push 異常資料給指定人員
                UpdateIsSend(dr);//異常資料Push后 更新 SendFlag
            }
        }

        /// <summary>
        /// 呼叫推送API方法
        /// </summary>
        public static void CallPushAPI(string Token, string MeterID, DataRow dr)
        {
            string title = string.Empty;
            string type = string.Empty;
            string Message = string.Empty;

            if (dr["did"].ToString().Contains("190"))
            {
                title = "用電異常";
                type = "2";
                Message = "產線:" + dr["did"].ToString() + "  用電上限值為:" + dr["VALUE1"].ToString() + "  超出上限值:" + dr["Count"].ToString() + "  時間:" + dr["dt"].ToString();
            }
            else
            {
                title = "用水異常";
                type = "1";
                Message = "水錶:" + dr["did"].ToString() + "  用水上限值為:" + dr["VALUE1"].ToString() + "  超出上限值:" + dr["Count"].ToString() + "  時間:" + dr["dt"].ToString() + dr["UPDATE_TIME"].ToString();
            }

            string url = "https://factory.iccapp.info/api/PushNotification";//正式区
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Timeout = 30 * 1000;//设置30s的超时
            var headers = request.Headers;
            headers["Authorization"] = "Bearer " + Token;
            request.Headers = headers;

            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "POST";


            WebProxy proxy = new WebProxy();                                      //定義一個網關對象
            proxy.Address = new Uri("**");              //網關服務器:端口
            proxy.Credentials = new NetworkCredential("**", "**");      //用戶名,密碼
            request.UseDefaultCredentials = true;                                      //啟用網關認証
            request.Proxy = proxy;                                                      //設置網關


            var Parameter = new
            {
                command = "PushNotification",
                employeeID = "21300076",
                message = Message,
                title = title,
                type = type,
                line = dr["did"].ToString()
            };

            var postData = JsonHelper.ObjectToString(Parameter);
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }
            postStream.Close();
            request.Abort();

        }

        /// <summary>
        /// 通過API獲取token
        /// </summary>
        public static string GetToken()
        {
            string url = "**";//正式区
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "POST";

            WebProxy proxy = new WebProxy();                                      //定義一個網關對象
            proxy.Address = new Uri("**");              //網關服務器:端口
            proxy.Credentials = new NetworkCredential("**", "**");//用戶名,密碼
            request.UseDefaultCredentials = true;                                      //啟用網關認証
            request.Proxy = proxy;                                                      //設置網關

            var Parameter = new
            {
                command = "Authenticate",
                account = "it@**.info",
                password = "**"
            };

            var postData = JsonHelper.ObjectToString(Parameter);
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }
            postStream.Close();
            request.Abort();
            JArray ja = JArray.Parse("[" + result + "]");
            JObject jo = (JObject)ja[0];
            return jo.GetValue("token").ToString();
        }


        /// <summary>
        /// 根據上限值 抓取異常的電量
        /// </summary>
        /// <returns></returns>
        private static DataTable GetMeteAlert(string did)
        {
            int Limit = GetLimitReal(did);//根據did抓取對應的上限值
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select TOP 1 T1.*,T2.VALUE1,CONVERT(INT,T1.total) - CONVERT(int,T2.VALUE1) AS Count
                                from KWH T1, dbo.TB_APPLICATION_PARAM T2
                                where T2.VALUE2 = @did and T1.did = @did
                                and T1.type = 'R'
                                and DateDiff(dd, T1.dt, getdate()) = 0 
								and total > @total
								and T1.ISSEND <> 'Y'
                                order by dt desc");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, did));
            opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
            return sdb.GetDataTable(sb.ToString(), opc);
        }

        /// <summary>
        /// 更新異常數據的推送Flag欄位
        /// </summary>
        /// <param name="did"></param>
        /// <param name="dt"></param>
        private static void UpdateIsSend(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Update  KWH set ISSEND = 'Y'   where did  = @did and dt = @dt");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, dr["did"].ToString()));
            if (dr["did"].ToString().Contains("190"))
            {
                opc.Add(DataPara.CreateDataParameter("@dt", SqlDbType.NVarChar, dr["dt"].ToString()));
            }
            else
            {
                sb.Append(" UPDATE_TIME = @UPDATE_TIME");
                opc.Add(DataPara.CreateDataParameter("@dt", SqlDbType.NVarChar, dr["dt"].ToString()));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", SqlDbType.NVarChar, dr["UPDATE_TIME"].ToString()));
            }
            sdb.ExecuteNonQuery(sb.ToString(), opc);
        }


        /// <summary>
        /// 根據did抓取實時用電量的上限值 每個did對應的上限值不同
        /// </summary>
        /// <param name="did"></param>
        /// <returns>return int</returns>
        public static int GetLimitReal(string did)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_APPLICATION_PARAM where 1=1 and PARAME_ITEM = 'upper'");
            opc.Clear();
            switch (did)
            {
                case "190164"://ALED
                    sb.Append(" and FUNCTION_NAME  = 'Alert' and PARAME_NAME = 'RealTimeKWHLimit' ");
                    break;
                case "190171"://SSD
                    sb.Append(" and FUNCTION_NAME  = 'SSDAlert' and PARAME_NAME = 'SSDRealTimeKWHLimit' ");
                    break;
                case "190172"://SMD
                    sb.Append(" and FUNCTION_NAME  = 'SMDAlert' and PARAME_NAME = 'SMDRealTimeKWHLimit' ");
                    break;
                case "190124"://全廠
                    sb.Append(" and FUNCTION_NAME  = 'ALLAlert' and PARAME_NAME = 'ALLRealTimeKWHLimit' ");
                    break;
                case "190160"://OSD
                case "190161":
                case "190174":
                case "190175":
                    sb.Append(" and FUNCTION_NAME  = 'OSDAlert' and PARAME_NAME = 'OSDRealTimeKWHLimit' ");
                    break;
            }
            string Limit = sdb.GetRowString(sb.ToString(), opc, "Value1");
            return int.Parse(Limit);
        }

    }
}
