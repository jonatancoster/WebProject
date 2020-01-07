﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.WebHost;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
