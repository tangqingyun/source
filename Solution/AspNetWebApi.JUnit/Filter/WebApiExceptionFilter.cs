using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace AspNetWebApi.JUnit.Filter
{
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exMessage = context.Exception.Message;
            if (context.Exception.InnerException != null)
            {
                exMessage = context.Exception.InnerException.Message;
            }
            var result = new 
            {
                status = "fail",
                message = exMessage
            };

          //  SystemLogHelper.Error("WebApiExceptionFilter.OnException 自动捕获", exMessage, context.Exception);
            context.Response = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "text/json"),
                StatusCode = System.Net.HttpStatusCode.OK,
            };
            base.OnException(context);
        }
    }
}