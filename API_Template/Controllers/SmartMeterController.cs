using API.Model;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiteOn.EA.BLL;
using LiteOn.EA.DAL;
using System.Configuration;
using System.Data;
using System.Text;
using System.Collections;
using System.Web;
using Newtonsoft.Json.Linq;

namespace API_Template.Controllers
{
    public class SmartMeterController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        SqlDB sdb = new SqlDB(conn);
        ArrayList opc = new ArrayList();

        public SmartMeterController()
        {
        }


        #region SmartMeter Post
        /// <summary>
        /// 抓取當天預警的用電量 by 車間 最新的一條預警數據
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartMeterKWHAlert_Input), typeof(InputExampleKWHAlert))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleKWHAlert))]
        public ReturnMessage GetMeterKWHAlert(SmartMeterKWHAlert_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetMeter_Helper.GetMeterKWHAlert(item);
                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Info = "Success";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Info = "Error";
            }

            return rm;
        }


        /// <summary>
        /// 抓取每天/每月的用電量 累計的數據
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartMeterKWH_Input), typeof(InputExampleKWHDay))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleKWHDay))]
        public ReturnMessage GetMeterKWH(SmartMeterKWH_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetMeter_Helper.GetMeterKWH(item);
                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Info = "Success";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Info = "Error";
            }

            return rm;
        }


        /// <summary>
        /// 抓取每天UTS By PRODUCTLINECODE
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartMeterUTS_Input), typeof(InputExampleUTS))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleUTS))]
        public ReturnMessage GetMeterUTS(SmartMeterUTS_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetMeter_Helper.GetMeterUTS(item);
                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Info = "Success";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Info = "Error";
            }

            return rm;
        }

        #endregion


    }


    #region Example Model KWH ALERT
    public class InputExampleKWHAlert : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Input
            {
                did = "190124"
            };
        }

    }
    public class OutputExampleKWHAlert : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWHAlert_Output
                {
                     Line = "ALED",
                     did = "190124",
                     type = "R",
                     dt = "2019-06-20 11:15:00.000",
                     ActValue = "1000",
                     TargetValue = "980"
                };

        }
    }
    #endregion

    #region Example Model KWH Day and Month
    public class InputExampleKWHDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Input
            {
                did = "190124",
                functiontype = "Day"
            };
        }

    }
    public class OutputExampleKWHDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Output
            {
                Line = "ALED",
                did = "1234",
                ActValue = "1000",
                TargetValue = "200000"
            };

        }
    }
    #endregion

    #region Example Model UTS
    public class InputExampleUTS : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterUTS_Input
            {
                PLANTNO = "2301",
                PRODUCTLINECODE = "23"
            };
        }

    }
    public class OutputExampleUTS : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterUTS_Output
            {
                PLANTNO = "1234",
                PRODUCTLINECODE = "1",
                GR_DATE = "2019-06-19 00:00:00.000",
                QTY = "1000",
            };

        }
    }
    #endregion
}

