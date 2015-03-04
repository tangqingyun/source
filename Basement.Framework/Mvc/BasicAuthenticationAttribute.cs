using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Basement.Framework.Mvc
{
     /// <summary>   
    /// 基本验证Attribtue，用以WebApi请求的权限处理   
    /// </summary>   
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>   
        /// 检查用户是否有该WebApi请求执行的权限   
        /// </summary>   
        /// <param name="actionContext"></param>   
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //var accessToken = actionContext.Request.Headers.GetCookies("access_token");
            //if (accessToken.Count > 0)
            //{
            //    //解密用户token值，看有没有登录
            //    var tokenValue = accessToken[0]["access_token"].Value;

            //    if (CheckToken(tokenValue))
            //    {
            //        base.OnActionExecuting(actionContext);
            //    }
            //    else
            //    {
            //        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //        actionContext.Response.Content = new StringContent("The request has no token, is invalid !", Encoding.UTF8, "text/plain");
            //    }
            //}
            //else
            //{
            //    //检查web.config配置是否要求权限校验   
            //    bool isRquired = (WebConfigurationManager.AppSettings["WebApiAuthenticatedFlag"].ToString() == "true");
            //    if (isRquired)
            //    {
            //        //如果请求Header不包含token，则判断是否是匿名调用   
            //        var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            //        bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

            //        //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息   
            //        if (isAnonymous)
            //        {
            //            base.OnActionExecuting(actionContext);
            //        }
            //        else
            //        {
            //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //            actionContext.Response.Content = new StringContent("The request is Unauthorized, is not allow!", Encoding.UTF8, "text/plain");
            //        }
            //    }
            //    else
            //    {
            //        base.OnActionExecuting(actionContext);
            //    }
            //}
        }

        /// <summary>
        /// 验证是否登录授权
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var model = string.Empty;// sessionPool.Get(token);
                return model != null ? true : false;
            }
            return false;
        }
    }
}
