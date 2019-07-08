
using Liteon.WebAPI.Auth;
using System;
using System.Web.Http.Filters;

namespace API_Template.Filter
{
    public class AuthFilter : ActionFilterAttribute
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AuthFilter));
        private readonly string API_DB = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName_API"];
        private readonly string HR_DB = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName_HR"];
        private string Connection = string.Empty;
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
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
                LiteonAuthFilter af = new LiteonAuthFilter();

                //af.checkToken(actionContext, Connection, API_DB, HR_DB);//暫時注釋驗證Token的邏輯 方便調試
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
                throw new Exception(ex.Message);
            }
            base.OnActionExecuting(actionContext);
        }

    }
}