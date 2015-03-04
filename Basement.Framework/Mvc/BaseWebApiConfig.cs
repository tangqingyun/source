using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Basement.Framework.Mvc
{
    public static class BaseWebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
