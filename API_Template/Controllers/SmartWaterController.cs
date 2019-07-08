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
    public class SmartWaterController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        static string conn = ConfigurationManager.AppSettings["SmartMeterDBConnection"];
        SqlDB sdb = new SqlDB(conn);
        ArrayList opc = new ArrayList();

        public SmartWaterController()
        {
        }


        #region SmartWater Post
        /// <summary>
        /// 抓取當天實時用水量 By did and functiontype
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartWaterReal_Input), typeof(InputExampleReal))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleReal))]
        public ReturnMessage GetWaterReal(SmartWaterReal_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetWater_Helper.GetWaterReal(item);
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
        /// 抓取每天的用水量（當天抓取昨日的用水量） By did 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartWaterDay_Input), typeof(InputExampleDay))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleDay))]
        public ReturnMessage GetWaterDay(SmartWaterDay_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetWater_Helper.GetWaterDay(item);
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


    #region Example Model Water Real
    public class InputExampleReal : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWaterReal_Input
            {
                did = "A1S",
                functiontype = "normal"
            };
        }

    }
    public class OutputExampleReal : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWater_Output
            {
                did = "A1S",
                type = "R",
                dt = "2019-06-20 00:00:00.000",
                UPDATE_TIME = "01:00:00",
                totalA = "120180.5",
                total = "13.13",
                BU = "OPS"
            };

        }
    }
    #endregion

    #region Example Model Water Day
    public class InputExampleDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWaterDay_Input
            {
                did = "A1S",
                functiontype = "normal"
            };
        }

    }
    public class OutputExampleDay : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWater_Output
            {
                did = "A1S",
                type = "R",
                dt = "2019-06-20 00:00:00.000",
                UPDATE_TIME = "2018-06-20",
                totalA = "120180.5",
                total = "355.22",
                BU = "OPS"
            };

        }
    }
    #endregion

}

