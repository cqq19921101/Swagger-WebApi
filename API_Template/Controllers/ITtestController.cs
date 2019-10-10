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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace API_Template.Controllers
{
    public class ITtestController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        ArrayList opc = new ArrayList();

        public ITtestController()
        {
        }



        #region GetNSB
        /// <summary>
        /// GetNSB  ----------> 異步處理 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetNSB_Input), typeof(GetNSB_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetNSB_OutputExample))]
        //public async Task<IHttpActionResult> GetNSB([FromBody]GetNSB_Input para) => Ok(await GetNSB_Helper.GetNSB(para));
        public ReturnMessage GetNSB(GetNSB_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = string.Empty;
            try
            {
                Result = GetNSB_Helper.GetNSB(item);
                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Status = "success";
                rm.Command = "GetNSB";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Status = "Error";
            }


            return rm;
        }
        #endregion

    }



    #region GetNSB Example 
    public class Test_InputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetNUB_Input
            {
                WERKS = "2301",
                PRODH = "10060",
                OPTIONAL = "",
            };
        }

    }
    public class Test_OutputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetNSB_Output
            {
                currentNSB = "10000",
                targetNSB = "10000"
            };

        }
    }

    #endregion



}

