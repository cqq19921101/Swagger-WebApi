using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Liteon.WebAPI.Auth;

namespace API_Template.Filter
{
    public class LogFilter : ActionFilterAttribute
    {
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LogFilter));
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                LiteonLogFilter LogFilter = new LiteonLogFilter();
                string logStr = LogFilter.getLogString(actionContext);
                Log.Info(logStr);
            }
            catch (Exception ex)
            {
                Log.Error("Got error. " + ex.Message);
            }
            base.OnActionExecuting(actionContext);
        }

    }



}