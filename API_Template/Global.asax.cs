using API_Template.Filter;
using System.Web.Http;

namespace API_Template
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
           
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            string log4netPath = System.Configuration.ConfigurationManager.AppSettings["LogConfigFile"];
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4netPath));
            GlobalConfiguration.Configuration.Filters.Add(new AuthFilter());
            //支援跨網域呼叫，先註解起來，後續有需要使用在打開
            //GlobalConfiguration.Configuration.Filters.Add(new AllowCORS());
            
        }
    }
    
  
}
