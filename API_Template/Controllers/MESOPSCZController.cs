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
using System.Web.Caching;

namespace API_Template.Controllers
{
    public class MESOPSCZController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        ArrayList opc = new ArrayList();

        public MESOPSCZController()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">缓存key，全局唯一</param>
        /// <param name="value">缓存值</param>
        /// <param name="minutes">缓存时间(分钟)</param>
        /// <param name="useAbsoluteExpiration">是否绝对过期</param>
        public static void Cache_Add(string key, object value, int minutes, bool useAbsoluteExpiration)
        {
            if (key != null && value != null)
            {
                if (useAbsoluteExpiration)
                {
                    System.Web.HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
                }
                else
                {
                    System.Web.HttpContext.Current.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0));
                }
            }
        }

        public static object Cache_Remove(string key)
        {
            return System.Web.HttpContext.Current.Cache.Remove(key);
        }

        public static void Cache_RemoveAll()
        {
            System.Web.Caching.Cache _cache = System.Web.HttpRuntime.Cache;
            System.Collections.IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            if (CacheEnum == null) return;
            while (CacheEnum.MoveNext())
            {
                System.Web.HttpContext.Current.Cache.Remove(CacheEnum.Key.ToString());
            }
        }


        #region OAY Post
        /// <summary>
        /// GetOAY
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetOAY_Input), typeof(GetOAY_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetOAY_OutputExample))]
        public ReturnMessage GetOAY(GetOAY_Input item)
        {
            ReturnMessage rm = new ReturnMessage();  //new 一個返回的請求狀態類
            try
            {
                string Result = string.Empty;
                System.Web.Caching.Cache cache = new Cache();
                Result = Convert.ToString(cache["GetOAY"]);
                if (string.IsNullOrEmpty(Result))
                {
                    Result = GETOAY_Helper.GetOAY(item);
                    Cache_Add("GetOAY", Result, 1440, true);                    
                }

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


        #region  Quantity
        /// <summary>
        /// GetQuality
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetQuanlity_Input), typeof(GetQuality_InputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(GetQuality_OutputExample))]
        public ReturnMessage GetQuality(GetQuanlity_Input item)
        {
            ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
            try
            {
                string Result = string.Empty;
                System.Web.Caching.Cache cache = new Cache();
                Result = Convert.ToString(cache["GetQuality"]);
                if (string.IsNullOrEmpty(Result))
                {
                    Result = GetQuanlity_Helper.GetQuanlity(item);
                    Cache_Add("GetQuality", Result, 1440, true);    
                }

                JArray jArray = JArray.Parse(Result);
                rm.Success = true;
                rm.Status = "Success";
                rm.Command = "GetQuality";
                rm.Array = jArray;
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rm.Success = false;
                rm.Status = "Error";
                rm.Command = "GetQuality";
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

    #region GetQuality Example
    public class GetQuality_InputExample: IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetQuanlity_Input
            {
                factory = "2301",
                line = "ALL"
            };
        }
    }

    public class GetQuality_OutputExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetQuanlity_Output
            {
                currentQuanlity = "3",
                targetQuanlity = "99999"
            };
        }
    }
    #endregion

}