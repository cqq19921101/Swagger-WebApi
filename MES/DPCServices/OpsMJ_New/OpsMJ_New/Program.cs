using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Data.SqlClient;
using Liteon.ICM.DataCore;
using LiteOn.EA.BLL;
using System.Configuration;


namespace OpsMJ_New
{
    class Program
    {
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = null;
        static void Main(string[] args)
        {
            try
            {
                CheckUser(null);
                    WriteLog(true, "程式運行成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                WriteLog(true, ex.Message);
                SendMails("flag");
            }
        }

        private static void PostAddUser_A3()
        {
            sdb = new SqlDB(DataPara.GetDbConnectionString("ICDB"));
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@P_EmpId", DbType.String, "Employeeid", ParameterDirection.Input));
            DataTable dtuser = sdb.ExecuteProcTable("P_GetCardUSERInfo", opc);
            if (dtuser.Rows.Count > 0)
            {
                foreach (DataRow druser in dtuser.Rows)
                {
                    bool Status = CheckUser(druser);
                    if (Status == false)
                    {
                        InsertUser(druser);
                    }
                }
                SendMails("success");

            }
            else
            {
                SendMails("None");
            }
        }


        private static void InsertUser(DataRow druser)
        {
            string url = "http://10.141.64.50:8088/api/person/add?access_token=jerry";//正式区
            //string url = "http://10.141.1.119:8088/api/person/add?access_token=jerry";//测试区
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "POST";

            var user = new
            {
                pin = druser["employee_id"].ToString(),
                deptCode = druser["dept_id"].ToString(),
                name = druser["name"].ToString(),
                lastName = druser["logonid"].ToString(),
                gender = "M",
                cardNo = druser["fPhysicalNo"].ToString(),
                accLevelIds = "1,21",
                accStartTime = "2018-07-24 12:45:00",
                accEndTime = "2099-08-24 12:45:00"
            };

            var postData = JsonHelper.ObjectToString(user);
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

        private static bool CheckUser(DataRow druser)
        {
            //string url = "http://10.141.64.50:8088/api/person/get/" + druser["employee_id"].ToString() + "?access_token=jerry";//正式区
            string url = "http://10.141.64.50:8088/api/accLevel/list?pageNo=1&pageSize=1000&access_token=jerry";//正式区
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
                return status;
            }
            catch (Exception ex)
            {
                bool status = false;
                return status;
                request.Abort();
            }


        }

        static private void WriteLog(bool errFlag, string msg)
        {

            if (errFlag)
            {
                msg = "[ERROR] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
            }
            else
            {
                msg = "[     ] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
            }

            string errLogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";

            string logFile = errLogPath + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            //路徑不存在則建立
            if (!Directory.Exists(errLogPath))
            {
                Directory.CreateDirectory(errLogPath);
            }

            //檢查文件存在
            if (!File.Exists(logFile))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFile);
                try
                {
                    sw.WriteLine(msg);
                }
                catch (Exception)
                {
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                //文件存在則複寫
                StreamWriter sw = File.AppendText(logFile);
                try
                {
                    sw.WriteLine(msg);
                }
                catch (Exception)
                {
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }

        }

        public static void exitProgram(object source, System.Timers.ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        protected static void SendMails(string Result)
        {
            string subject = string.Empty;
            string mailBody = string.Empty;

            subject = "門禁新進人員新增提醒";
            if (Result == "success")
            {
                mailBody = "Dear " + "All" + ": \r\n    系統新增資料成功,請知悉.\r\n";
            }
            else if (Result == "None")
            {
                mailBody = "Dear " + "All" + ": \r\n    昨日無新進人員,請知悉.\r\n";
            }
            else
            {
                mailBody = "Dear " + "All" + ": \r\n    系統新增資料異常,請聯繫IT處理.\r\n";
            }

            SendMails( subject, mailBody);

        }

        private static void SendMails(string subject, string mailBody)
        {
            SendMail sendMail = new SendMail();
            ArrayList to = new ArrayList();
            ArrayList cc = new ArrayList();
            string TestMode = ConfigurationSettings.AppSettings["TestMode"].ToString();
            string testMailReciver = ConfigurationSettings.AppSettings["TestMailReceiver"].ToString();
            string EmailToGroup = ConfigurationSettings.AppSettings["EmailToGroup"].ToString();
            string EmailCCGroup = ConfigurationSettings.AppSettings["EmailCCGroup"].ToString();
            if (TestMode == "Y")
            {
                to.Add(testMailReciver);
            }
            else
            {
                to.Add(EmailToGroup);
                cc.Add(EmailCCGroup);
            }
            sendMail.SendMail_Normal(to, cc, subject, mailBody, false);
        }

    }
}
