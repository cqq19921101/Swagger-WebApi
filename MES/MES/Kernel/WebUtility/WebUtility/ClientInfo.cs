using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using System.Net;
using System.Configuration;

namespace Liteon.Mes.WebUtility
{
    public class ClientInfo
    {
        public static string GetIP()
        {
            try
            {
                string ip = string.Empty;
                if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                    ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                if (String.IsNullOrEmpty(ip))
                    ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return ip;
            }
            catch
            {
                return "";
            }
        }

        public static string GetClientName()
        {
            try
            {
                string hostIp = GetIP();
                System.Net.IPAddress address = System.Net.IPAddress.Parse(hostIp);
                System.Net.IPHostEntry ipInfor = System.Net.Dns.GetHostEntry(address);
                string clientName = ipInfor.HostName;
                return clientName;
            }
            catch
            {
                return "";
            }
            //string host = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
            //return host == null ? "" : host.Trim().ToString();
        }

        public static string GetBrowserVersion()
        {
            try
            {
                string Browser = System.Web.HttpContext.Current.Request.Browser.Browser;
                string Version = System.Web.HttpContext.Current.Request.Browser.Version;
                if (String.IsNullOrEmpty(Browser))
                    Browser = string.Empty;
                if (String.IsNullOrEmpty(Version))
                    Version = string.Empty;
                return Browser + Version;
            }
            catch
            {
                return "";
            }
        }
    }
}
