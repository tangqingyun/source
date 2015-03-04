using System;
using System.Web;


namespace Basement.Framework.Mvc
{
    public class WebErrorCatchModule : IHttpModule
    {
        public void Dispose() {}

        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(OnContextError);
        }

        public void OnContextError(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            Exception ex = context.Server.GetLastError();
           // Logger.Error(string.Format("[{0}],{1}", context.Request.Url.Host, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            context.Server.ClearError();
            //context.Response.Redirect("/404.html");
        }
    }
}
