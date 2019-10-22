using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace FTPTool
{
    class Para
    {

        public static DataSet dsSysMailLoop = null;

        public static DataSet dsSiteProfile = null;

        public static string MAIL_SERVER = null;
        public static string MAIL_DB = null;


        public static string MAIL_RECEIVER = "";
        public static string MAIL_FROM = null;
        public static string MAIL_SMTP = null;
        public static string MAIL_ACCOUNT = null;
        public static string MAIL_PWD = null;

        public static string NORMALLOGPATH = "";
        public static string ERRORLOGFILEPATH = "";
        public static string ERRORLOGFILE = "";

        public static int RECONNECTTIMES = 20;
    }
}
