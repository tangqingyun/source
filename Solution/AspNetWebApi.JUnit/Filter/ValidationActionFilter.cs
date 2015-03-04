using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace AspNetWebApi.JUnit.Filter
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                var values = modelState.Values.FirstOrDefault();
                if (values != null && values.Errors.Count() > 0)
                {
                    var err = values.Errors.Last();
                    string errorMessage = err.ErrorMessage;
                    var baseResult = new 
                    {
                        message = errorMessage,
                        status = "fail"
                    };
                    //SystemLogHelper.Error("ValidationActionFilter.OnActionExecuting自动捕获", errorMessage, err.Exception);
                    actionContext.Response = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(baseResult), Encoding.UTF8, "text/json"),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

            }

        }
    }
}