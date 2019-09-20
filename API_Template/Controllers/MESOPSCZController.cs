using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using API.Model;
using System.Collections;
using Swashbuckle.Examples;
using Newtonsoft.Json.Linq;

namespace API_Template.Controllers
{
    public class MESOPSCZController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        ArrayList opc = new ArrayList();

        public MESOPSCZController()
        {
        }

        #region OAY Post
        /// <summary>
        /// QueryLvData
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetOAY_Input), typeof(GetOAY_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetOAY_OutputExample))]
        public ReturnMessage GetOAY(GetOAY_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = string.Empty;
            try
            {
                Result = GETOAY_Helper.GetOAY(item);
                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Status = "Success";
                rm.Command = "GetOAY";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Status = "Error";
                rm.Command = "GetOAY";
            }
            
            return rm;
        }

        #endregion


    }


    #region GetOAY Example 
    public class GetOAY_InputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetOAY_Input
            {
                factory = "2301",
                line = "70",
                series = "ALL",
                stage = "ALL"
            };
        }

    }
    public class GetOAY_OutputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetOAY_Output
            {
                line = "70",
                currentOAY = "9800",
                targetOAY = "10000"
            };

        }
    }

    #endregion


}