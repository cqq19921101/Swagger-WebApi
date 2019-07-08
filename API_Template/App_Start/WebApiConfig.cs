using System.Web.Http;
using API_Template.Filter;
namespace API_Template
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "liteonApi",
                routeTemplate: "liteonApi/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
            );
            //紀錄Web API log 這是for action
            config.Filters.Add(new LogFilter());

        }
    }
}
