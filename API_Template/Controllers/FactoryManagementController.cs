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

namespace API_Template.Controllers
{
    public class FactoryManagementController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        ArrayList opc = new ArrayList();

        public FactoryManagementController()
        {
        }


        #region GetHC Post
        /// <summary>
        /// QueryLvData
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetHC_Input), typeof(InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleLvData))]
        public ReturnMessage GetHC_QueryLvData(GetHC_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetHC_Helper.GetHC_QueryLvData(item);
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
        /// QueryDLBuffer
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetHCDLBuffer_Input), typeof(InputExampleDLBuffer))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleDLBuffer))]
        public ReturnMessage GetHC_QueryDLBuffer(GetHCDLBuffer_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetHC_Helper.GetHC_QueryDLBuffer(item);
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

        #region GetNSB
        /// <summary>
        /// GetNSB
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetNSB_Input), typeof(GetNSB_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetNSB_OutputExample))]
        public ReturnMessage GetNSB(GetNSB_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = string.Empty;
            try
            {
                Result = GetNSB_Helper.GetNSB(item);
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

        #region GetNUB
        /// <summary>
        /// GetNUB
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetNUB_Input), typeof(GetNUB_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetNUB_OutputExample))]
        public ReturnMessage GetNUB(GetNUB_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = string.Empty;
            try
            {
                Result = GetNUB_Helper.GetNUB(item);
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


    #region GetHC Example 
    public class InputExampleDLBuffer : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetHCDLBuffer_Input
            {
                BU = "OPTO",
                DEPT_ID = "SSD",
                OPTIONAL = ""
            };
        }

    }


    public class OutputExampleDLBuffer : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetHCDLBuffer_Output
            {
                DEPT_ID = "SSD",
                DL_DEMAND = "200",
                DL_ACT = "143",
            };

        }
    }
    public class InputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetHC_Input
            {
                BU = "OPTO",
            };
        }

    }

    public class OutputExampleLvData : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetHCLvData_Output
            {
                DEPT_ID = "SSD_IRPTR",
                G1LastMonth_Incumbency = "36",
                G1HC = "35",
                G1Cum = "0",
                G1CumRate = "0",
                G1VolTO = "0",
                G1VolRate = "0",
                G1DailyTO = "0",

                G2LastMonth_Incumbency = "25",
                G2HC = "22",
                G2Cum = "0",
                G2CumRate = "0",
                G2VolTO = "0",
                G2VolRate = "0",
                G2DailyTO = "0",

                G3LastMonth_Incumbency = "25",
                G3HC = "24",
                G3Cum = "0",
                G3CumRate = "0",
                G3VolTO = "0",
                G3VolRate = "0",
                G3DailyTO = "0",

                G4LastMonth_Incumbency = "30",
                G4HC = "25",
                G4Cum = "0",
                G4CumRate = "0",
                G4VolTO = "0",
                G4VolRate = "0",
                G4DailyTO = "0",

                G5LastMonth_Incumbency = "25",
                G5HC = "20",
                G5Cum = "0",
                G5CumRate = "0",
                G5VolTO = "0",
                G5VolRate = "0",
                G5DailyTO = "0",

            };

        }
    }

    #endregion

    #region GetNSB Example 
    public class GetNSB_InputExample : IExamplesProvider
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
    public class GetNSB_OutputExample : IExamplesProvider
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

    #region GetNUB Example 
    public class GetNUB_InputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetNUB_Input
            {
                WERKS = "2301",
                PRODH = "10060",
                OPTIONAL = ""
            };
        }

    }
    public class GetNUB_OutputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetNUB_Output
            {
                currentNUB = "10000",
                targetNUB = "10000"
            };

        }
    }

    #endregion

}

