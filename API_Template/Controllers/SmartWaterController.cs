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
using System.Threading.Tasks;

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
        /// 抓取当天,当月累计的用水量
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartWater_Input), typeof(InputExampleW))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleW))]
        public async Task<IHttpActionResult> GetWater([FromBody]SmartWater_Input para) => Ok(await GetWater_Helper.GetWater(para));


        /// <summary>
        /// 抓取每天用水量异常记录 最新的一笔 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SmartWaterAlert_Input), typeof(InputExampleAlert))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleAlert))]
        public async Task<IHttpActionResult> GetWaterAlert([FromBody] SmartWaterAlert_Input para) => Ok(await GetWater_Helper.GetWaterAlert(para));

        #endregion


    }


    #region Example Model Water Real
    public class InputExampleW : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWater_Input
            {
                did = "A1S",
                functiontype = "Day"
            };
        }

    }
    public class OutputExampleW : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWater_Output
            {
                did = "A1S",
                ActValue = "100",
                TargetValue = "200"
            };

        }
    }
    #endregion

    #region Example Model Water Day
    public class InputExampleAlert : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWaterAlert_Input
            {
                did = "A1S"
            };
        }

    }
    public class OutputExampleAlert : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SmartWaterAlert_Output
            {
                did = "A1S",
                type = "R",
                dt = "2019-06-20 00:00:00.000",
                UPDATE_TIME = "01:00:00",
                ActValue = "300",
                TargetValue = "200"
            };

        }
    }
    #endregion

}

