using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_WriteTraceLog
{
    public static class WriteTraceLog
    {
        //记录错误日志
        public static void Error(string msg)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(msg);
        }
        //记录一般信息
        public static void Info(string msg)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(msg);
        }
        //记录调试信息
        public static void Debug(string msg)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug(msg);
        }
        //记录严重错误
        public static void Fatal(string msg)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Fatal(msg);
        }
        //记录警告信息
        public static void Warn(string msg)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Warn(msg);
        }
    }
}
