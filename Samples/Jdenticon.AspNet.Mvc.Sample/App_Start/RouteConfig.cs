using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jdenticon.WebApi.Sample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Mvc", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Icon",
                url: "mvc/icon/{name}/{size}",
                defaults: new { controller = "Mvc", action = "Icon" }
            );
        }
    }
}
