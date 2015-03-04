using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Basement.Framework.Web
{
    public class HttpLogModule : IHttpModule
    {
        private DateTime requestBeginTime = DateTime.Now;
        private string responseContent = null;
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Application_BeginRequest;
            context.EndRequest += Application_EndRequest;
        }
       
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            requestBeginTime = DateTime.Now;
            var application = (HttpApplication)source;
            //装配过滤器
            application.Response.Filter = new ResponseFilter(application.Response.Filter);
            //绑定过滤器事件
            var filter = (ResponseFilter)application.Response.Filter;
            filter.Writed += Writed;
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            var totalTime = (int)(DateTime.Now - requestBeginTime).TotalMilliseconds;
            try
            {
               // HttpLogHelper.Write(totalTime, responseContent ?? "");
            }
            catch (Exception ex)
            {
              //  SystemLogHelper.Error("HttpLogModule写日志异常", ex.Message, ex);
            }
        }

        private void Writed(string content)
        {
            responseContent = content;
        }

        public void Dispose(){
        }

    }


    //自定义过滤器
    public class ResponseFilter : Stream
    {
        readonly Stream _responseStream;
        long _position;
        public delegate void DelegateWrited(string content);
        public DelegateWrited Writed;
        public ResponseFilter(Stream inputStream)
        {
            _responseStream = inputStream;
        }

        //实现Stream 虚方法Filter Overrides
        #region Filter Overrides
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override void Close()
        {
            _responseStream.Close();
        }
        public override void Flush()
        {
            _responseStream.Flush();
        }
        public override long Length
        {
            get
            {
                return 0;
            }
        }
        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _responseStream.Seek(offset, origin);
        }
        public override void SetLength(long length)
        {
            _responseStream.SetLength(length);
        }
        #endregion
        //关键的点，在HttpResponse 输入内容的时候，一定会调用此方法输入数据，所以要在此方法内截获数据
        public override void Write(byte[] buffer, int offset, int count)
        {
            var strBuffer = Encoding.UTF8.GetString(buffer, offset, count);
            Writed(strBuffer);
            _responseStream.Write(buffer, offset, count);
        }
    }

}
