using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Basement.Framework.Mvc
{
    public class JsonFormatter : JsonMediaTypeFormatter
    {
        private string JsonpCallbackFunction;

        public JsonFormatter() { }

        public override bool CanWriteType(Type type){ return true; }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, System.Net.Http.HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            JsonFormatter formatter = new JsonFormatter()
            {
                JsonpCallbackFunction = GetJsonCallbackFunction(request)
            };

            formatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            formatter.SerializerSettings.Converters.Add(dateTimeConverter);

            formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return formatter;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(JsonpCallbackFunction))
                return base.WriteToStreamAsync(type, value, stream, content, transportContext);

            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(stream);
                writer.Write(JsonpCallbackFunction + "(");
                writer.Flush();
            }
            catch (Exception ex)
            {
                try
                {
                    if (writer != null)
                        writer.Dispose();
                }
                catch { }

                var taskSource = new TaskCompletionSource<object>();
                taskSource.SetException(ex);
                return taskSource.Task;
            }

            return base.WriteToStreamAsync(type, value, stream, content, transportContext)
                       .ContinueWith(innerTask =>
                       {
                           if (innerTask.Status == TaskStatus.RanToCompletion)
                           {
                               writer.Write(")");
                               writer.Flush();
                           }

                       }, TaskContinuationOptions.ExecuteSynchronously)
                        .ContinueWith(innerTask =>
                        {
                            writer.Dispose();
                            return innerTask;

                        }, TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
        }

        private string GetJsonCallbackFunction(HttpRequestMessage request)
        {
            if (request.Method != HttpMethod.Get) return null;
            if (request.RequestUri == null) return null;
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var queryValue = query["callback"];

            if (string.IsNullOrEmpty(queryValue)) return null;

            return queryValue;
        }
    }
}
