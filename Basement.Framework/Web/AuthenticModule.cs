using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Basement.Framework.Web
{
    public class AuthenticModule : IHttpModule
    {

        public void Init(HttpApplication application)
        {
            //application.BeginRequest += new EventHandler(application_BeginRequest);
            //application.EndRequest += new EventHandler(application_EndRequest);
            //application.PreRequestHandlerExecute += new EventHandler(application_PreRequestHandlerExecute);
            //application.PostRequestHandlerExecute += new EventHandler(application_PostRequestHandlerExecute);
            //application.ReleaseRequestState += new EventHandler(application_ReleaseRequestState);
            //application.AcquireRequestState += new EventHandler(application_AcquireRequestState);
            //application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
            //application.AuthorizeRequest += new EventHandler(application_AuthorizeRequest);
            //application.ResolveRequestCache += new EventHandler(application_ResolveRequestCache);
            //application.PreSendRequestHeaders += new EventHandler(application_PreSendRequestHeaders);
            //application.PreSendRequestContent += new EventHandler(application_PreSendRequestContent);
        }

        /// <summary>
        /// 指示请求处理开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_BeginRequest<br/>");
        }
        /// <summary>
        /// 在Http请求处理完成的时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_EndRequest<br/>");
        }

        /// <summary>
        /// 在Http请求进入HttpHandler之前触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_PreRequestHandlerExecute<br/>");
        }

        //进入Page页面执行

        /// <summary>
        /// 在Http请求进入HttpHandler之后触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_PostRequestHandlerExecute<br/>");
        }

        void application_ReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_ReleaseRequestState<br/>");
        }

        void application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_AcquireRequestState<br/>");
        }

        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_AuthenticateRequest<br/>");
        }

        void application_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_AuthorizeRequest<br/>");
        }

        void application_ResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_ResolveRequestCache<br/>");
        }

        /// <summary>
        /// 在向客户端发送Header之前触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_PreSendRequestHeaders<br/>");
        }

        /// <summary>
        /// 在向客户端发送内容之前触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_PreSendRequestContent(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Context.Response.Write("application_PreSendRequestContent<br/>");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
     
    }
}
