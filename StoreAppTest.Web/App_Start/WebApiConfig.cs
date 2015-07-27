
using System.Web.Http;

namespace StoreAppTest.Web
{
    using System.Net.Http.Headers;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional , action = RouteParameter.Optional}
            );

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            //config.Formatters.XmlFormatter.UseXmlSerializer = true;

        }
    }
}
