using System.Net.Http.Headers;
using System.Web.Http;

class WebApiConfig
{
    public static void Register(HttpConfiguration configuration)
    {
        // https://stackoverflow.com/questions/36274985/how-to-map-webapi-routes-correctly
        // http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#restful
        //configuration.Formatters.JsonFormatter.SupportedMediaTypes
        //        .Add(new MediaTypeHeaderValue("text/html"));
        //configuration.Routes.MapHttpRoute("2", "api/{controller}/{id}/{action}");
        configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
            new { id = RouteParameter.Optional });
        configuration.MapHttpAttributeRoutes();
    }

}