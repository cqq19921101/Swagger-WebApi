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
        /// 抓取當天實時用電量 By did and functiontype
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartMeterKWH_Input), typeof(InputExampleKWH))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleKWH))]
        public ReturnMessage GetMeterKWHReal(SmartMeterKWH_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetMeter_Helper.GetMeterKWHReal(item);
                rm.Success = true;
                rm.Info = Result;
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
        /// 抓取每天的用電量 By did 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartMeterKWH_Input), typeof(InputExampleKWHDay))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleKWHDay))]
        public ReturnMessage GetMeterKWHDay(SmartMeterKWH_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetMeter_Helper.GetMeterKWHDay(item);
                rm.Success = true;
                rm.Info = Result;
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
                rm.Success = true;
                rm.Info = Result;
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


    #region Example Model KWH Real
    public class InputExampleKWH : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Input
            {
                did = "190124",
                functiontype = "normal"
            };
        }

    }
    public class OutputExampleKWH : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Output
                {
                     did = "1234",
                     type = "R",
                     dt = "2019-06-20 11:15:00.000",
                     total = "100",
                     BU = "OPS"
                };

        }
    }
    #endregion

    #region Example Model KWH Day
    public class InputExampleKWHDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Input
            {
                did = "190124",
                functiontype = "normal"
            };
        }

    }
    public class OutputExampleKWHDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartMeterKWH_Output
            {
                did = "1234",
                type = "D",
                dt = "2019-06-20 11:15:00.000",
                total = "100",
                BU = "OPS"
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

