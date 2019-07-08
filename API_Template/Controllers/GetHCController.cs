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
    public class GetHCController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        ArrayList opc = new ArrayList();

        public GetHCController()
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
        [SwaggerRequestExample(typeof(GetHC_Input), typeof(InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(OutputExampleDLBuffer))]
        public ReturnMessage GetHC_QueryDLBuffer(GetHC_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            string Result = "";
            try
            {
                Result = GetHC_Helper.GetHC_QueryDLBuffer(item);
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


    #region Example 
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
    public class OutputExampleDLBuffer : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetHCDLBuffer_Output
            {
                ORG = "PLANT",
                DEPT_ID = "SSD",
                SUB_DEPT = "SSD_IRPTR",
                DL_DEMAND = "138",
                DL_ACT = "143",
                IDL_ACT = "6",
                TTL_HC = "149",
                DL_CUM_NEW_HIRE = "9",
                DL_BUFFER = "3.62319%",
                G1_RATE = "28.67%",
                G2_RATE = "20.98%",
                G3_RATE = "44.06%",
                G4_RATE = "4.2%",
                G5_RATE = "2.1%",
                rowNumber = "1",

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

}

