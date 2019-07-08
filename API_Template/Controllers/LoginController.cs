using System.Web.Http;
using Swashbuckle.Examples;
using LiteOn.WebAPI.Auth;
using System;

namespace API_Template.Controllers
{
    public class LoginController : ApiController
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));
        private readonly string API_DB = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName_API"];
        private readonly string HR_DB = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName_HR"];
        private string Connection = string.Empty;
        public LoginController()
        {
            try
            {
                string flag = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnUserDefine"];
                if (flag.ToLower() == "true")
                {
                    Connection = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnUserDefineString"];
                }
                else
                {
                    Connection = BorG.SPM.DataTier.BPMCONFIG.GetDbConnectionString();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 驗證UserName、UserPassword、AppID
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(LogonRequest), typeof(LoginRequestExample))]
        public ResultModel Post(LogonRequest vRequest)
        {
            ResultModel rs = new ResultModel();
            try
            {
                LiteonSecurity sy = new LiteonSecurity(Connection, API_DB, HR_DB);
                rs = sy.getToken(vRequest.UserName, vRequest.UserPassword, vRequest.AppID);
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rs.Status = false;
                rs.Msg = ex.Message;

            }
            return rs;
        }

        /// <summary>
        /// 增加第四個參數Time，可設定秒數(token的時效)
        /// </summary>
        /// <param name="vRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(LogonPlusRequest), typeof(LogonPlusRequestExample))]
        public ResultModel PostPlus(LogonPlusRequest vRequest)
        {
            ResultModel rs = new ResultModel();
            try
            {
                LiteonSecurity sy = new LiteonSecurity(Connection, API_DB, HR_DB);
                rs = sy.getToken(vRequest.UserName, vRequest.UserPassword, vRequest.AppID, vRequest.Time);
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                rs.Status = false;
                rs.Msg = ex.Message;
            }
            return rs;
        }
        public class LoginRequestExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new LogonRequest
                {
                    AppID = "wf",
                    UserName = "HR_API",
                    UserPassword = "97",
                };
            }
        }

        public class LogonPlusRequestExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new LogonPlusRequest
                {
                    AppID = "wf",
                    UserName = "HR_API",
                    UserPassword = "97",
                    Time = "3600"
                };
            }
        }

    }

}