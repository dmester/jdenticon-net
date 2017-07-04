using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Jdenticon.WebApi.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "IconApi",
                routeTemplate: "api/icon/{name}/{size}",
                defaults: new { controller = "IconApi", action = "Icon" }
            );
        }
    }
}
