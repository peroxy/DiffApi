using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DiffApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DiffRoute",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { action = "GetDiff" }
            );

            config.Routes.MapHttpRoute(
                name: "LeftRoute",
                routeTemplate: "v1/{controller}/{id}/left",
                defaults: new { action = "PutLeft" }
            );

            config.Routes.MapHttpRoute(
                name: "RightRoute",
                routeTemplate: "v1/{controller}/{id}/right",
                defaults: new { action = "PutRight" }
            );
        }
    }
}
