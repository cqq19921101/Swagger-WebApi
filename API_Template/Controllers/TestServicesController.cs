using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace API_Template.Controllers
{
    public class TestServicesController : ApiController
    {
        public TestServicesController()
        {
        }

        /// <summary>
        /// 測試API服務是否運作中
        /// </summary>
        /// <param name="item">AppID=應用程式ID、Name=要顯示的訊息名稱、Num=要回傳的訊息次數
        /// </param>
        /// <remarks>
        /// 測試API服務是否運作中，傳入AppID、Name、Num參數<br />
        /// 若API有在服務中，將傳回Num指定次數的 Hello World 訊息
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(TestInput), typeof(TestInputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(TestOutputExample))]
        public List<TestOutput> getHello(TestInput item)
        {
            List<TestOutput> rs = new List<TestOutput>();

            string Msg = string.Empty;
            try
            {
                int count = 0;
                var list = new List<string>();
                if (int.TryParse(item.Num, out count))
                {
                    for (int i = 0; i < count; i++)
                    {
                        var hello = new TestOutput();
                        hello.Value = item.Name + ", Hello World " + (i + 1).ToString();
                        rs.Add(hello);
                    }
                }
                else
                {
                    var hello = new TestOutput();
                    hello.Value = item.Name + ", Hello World";
                    rs.Add(hello);
                }

            }
            catch (Exception ex)
            {


            }
            return rs;
        }


        /// <summary>
        /// 測試API服務是否運作中
        /// </summary>
        /// <param name="item">AppID=應用程式ID、Name=要顯示的訊息名稱、Num=要回傳的訊息次數
        /// </param>
        /// <remarks>
        /// 測試API服務是否運作中，傳入AppID、Name、Num參數<br />
        /// 若API有在服務中，將傳回Num指定次數的 Hello World 訊息
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(TestInput), typeof(TestInputExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(TestOutput2Example))]
        public List<TestOutput> getHello2(TestInput item)
        {
            List<TestOutput> rs = new List<TestOutput>();

            string Msg = string.Empty;
            try
            {
                int count = 0;
                var list = new List<string>();
                if (int.TryParse(item.Num, out count))
                {
                    for (int i = 0; i < count; i++)
                    {
                        var hello = new TestOutput();
                        hello.Value = item.Name + ", Hello World2 " + (i + 1).ToString();
                        rs.Add(hello);
                    }
                }
                else
                {
                    var hello = new TestOutput();
                    hello.Value = item.Name + ", Hello World2";
                    rs.Add(hello);
                }

            }
            catch (Exception ex)
            {


            }
            return rs;
        }
        #region input/output class
        public class TestInput
        {
            public string AppID { get; set; }
            public string Name { get; set; }
            public string Num { get; set; }

        }
        public class TestOutput
        {
            public string Value { get; set; }
        }
        public class TestOutput2
        {
            public string Value { get; set; }
        }
        #endregion

        #region Example Model
        public class TestInputExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new TestInput
                {
                    AppID = "App01",
                    Name = "AppName",
                    Num = "5"
                };
            }

        }
        public class TestOutputExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new List<TestOutput>()
                {
                 new TestOutput
                 {
                     Value="AppName, Hello World 1"
                 },
                  new TestOutput
                 {
                     Value="AppName, Hello World 2"
                 }
                };

            }
        }
        public class TestOutput2Example : IExamplesProvider
        {
            public object GetExamples()
            {
                return new List<TestOutput2>()
                {
                 new TestOutput2
                 {
                     Value="AppName, Hello World2 1"
                 },
                  new TestOutput2
                 {
                     Value="AppName, Hello World2 2"
                 }
                };

            }
        }
        #endregion
    }
}