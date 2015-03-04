using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Basement.Framework.Mvc;

namespace Basement.Framework.Mvc
{
    public class BaseWebApiApplication : System.Web.HttpApplication
    {

        public void ApplicationStart()
        {
            AreaRegistration.RegisterAllAreas();

            BaseWebApiConfig.Register(GlobalConfiguration.Configuration);
            BaseFilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BaseRouteConfig.RegisterRoutes(RouteTable.Routes);
            BaseBundleConfig.RegisterBundles(BundleTable.Bundles);

            var dateTimeConverter = new IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(dateTimeConverter);

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings = serializerSettings;
            jsonFormatter.MediaTypeMappings.Add(new RequestHeaderMapping("accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, new MediaTypeHeaderValue("text/html")));
            GlobalConfiguration.Configuration.Formatters.Insert(0, jsonFormatter);

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("xml", "true", "application/xml"));

            JsonFormatter formatter = new JsonFormatter();
            formatter.MediaTypeMappings.Add(new QueryStringMapping("jsonp", "true", "application/javascript"));
            GlobalConfiguration.Configuration.Formatters.Add(formatter);

            
        }

        public void Start()
        {
            this.ApplicationStart();
        }
    }
}
