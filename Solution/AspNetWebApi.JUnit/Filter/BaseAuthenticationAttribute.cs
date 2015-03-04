using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AspNetWebApi.JUnit.Filter
{
    public class BaseAuthenticationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //如果请求Header不包含token，则判断是否是匿名调用   
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attr.Any(a => a != null);
            if (isAnonymous)
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(
                new 
                {
                    status = "fail",
                    message = "请求没有访问权限，请登陆"
                }), Encoding.UTF8, "text/json");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

    }
}