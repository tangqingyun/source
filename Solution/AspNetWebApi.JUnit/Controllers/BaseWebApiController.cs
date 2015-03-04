using AspNetWebApi.JUnit.Filter;
using Basement.Framework.Configuration;
using Basement.Framework.WebApi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Script.Serialization;




namespace AspNetWebApi.JUnit.Controllers
{
    [BaseAuthenticationAttribute]
    public class BaseWebApiController : BaseApiController
    {
        private string siteWebResource = "http://117.79.235.214:10901/api/WebResource/UploadImage";
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            #region Post、Get 请求的处理，支持Json传输。
            string postContentData = string.Empty;
            string contentType = string.Empty;
            string token = string.Empty;
            if (controllerContext.Request.Method != HttpMethod.Get)
            {
                
                //----------------获取Post参数--------------------
                NameValueCollection postParam = null;
                //【处理】content-type:application/x-www-form-urlencoded
                if (controllerContext.Request.Content.IsFormData())
                {
                    postParam = controllerContext.Request.Content.ReadAsFormDataAsync().Result;
                    postContentData = postParam.ToString();
                    token = postParam["token"] ?? "";
                    contentType = "application/x-www-form-urlencoded";
                }
                //【处理】 content-type:application/json 
                else
                {
                    contentType = "application/json";
                    if (!controllerContext.Request.Content.IsMimeMultipartContent("form-data"))
                    {
                        postContentData = controllerContext.Request.Content.ReadAsStringAsync().Result;
                        JavaScriptSerializer json = new JavaScriptSerializer();
                        var tt = System.Web.HttpUtility.UrlDecode(postContentData);
                        dynamic tokenDic = json.Deserialize(postContentData, typeof(object));
                        if (postContentData.Contains("token"))
                        {
                            token = Convert.ToString(tokenDic["token"]);
                        }
                    }
                    else
                    {
                        contentType = "multipart/form-data";
                        var collPostParamData = controllerContext.Request.RequestUri.ParseQueryString();
                        //----------------同时处理Post中的Url参数请求-------------------------------
                        if (!string.IsNullOrWhiteSpace(collPostParamData["token"]))
                        {
                            token = System.Web.HttpUtility.UrlDecode(collPostParamData["token"]);
                        }
                    }
                }
                //----------------设置回FormData参数----------------------
                if (!controllerContext.Request.Content.IsMimeMultipartContent("form-data"))
                {
                    StringContent content = new StringContent(postContentData, Encoding.UTF8, contentType);
                    controllerContext.Request.Content = content;
                }
            }
            else
            {
                var getParam = controllerContext.Request.RequestUri.ParseQueryString();
                //----------------Get请求-------------------------------
                if (!string.IsNullOrWhiteSpace(getParam["token"]))
                {
                    token = System.Web.HttpUtility.UrlDecode(getParam["token"]);
                }
            }
            #endregion

            base.Initialize(controllerContext);
        }

        #region 调用图片资源上传
        //[NonAction]
        //public string InvokeUploadImageSource<T>(out T t, string createImageSizes, string fileDir, string fileName) where T : new()
        //{
        //    var data = InvokeUploadImage<T>(out t, createImageSizes, fileDir, fileName);
        //    return data.data.BusinessContent == null ? "" : data.data.BusinessContent.ToString();
        //}

        ///// <summary>
        ///// 资源上传
        ///// </summary>
        ///// <param name="CreateImageSizes">上传图片Size,支持多个尺寸{ "320*320|120*120|240*340" }</param>
        ///// <param name="FileDir">文件路径{ "/Mobile/Images/App/" }</param>
        ///// <param name="FileName">文件名{ "absksdfklsl"}</param>
        ///// <returns></returns>
        //[NonAction]
        //public WebApiResultBase InvokeUploadImage<T>(out T t, string createImageSizes, string fileDir, string fileName) where T : new()
        //{
        //    //-----------序列化对象-----------------------
        //    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //    WebApiResultBase uploadModel = null;
        //    string defaultTempPath = "/Temp/";
        //    string fileSrc = string.Empty;//--返回的图片对象--

        //    //-----------图片上传---------------------
        //    fileSrc = CommonUpload(defaultTempPath, Guid.NewGuid().ToString("N"), jsonSerializer, out  t, out  uploadModel);


        //    if (!string.IsNullOrWhiteSpace(fileSrc))
        //    {
        //        #region 上传图片到资源服务器
        //        HttpClient client = new HttpClient();
        //        MultipartFormDataContent form = new MultipartFormDataContent();
        //        StreamContent fileContent = new StreamContent(File.OpenRead(HttpContext.Current.Server.MapPath(fileSrc)));
        //        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/jpeg");
        //        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //        fileContent.Headers.ContentDisposition.FileName = Path.GetFileName(fileSrc);
        //        //---------------资源对象----
        //        form.Add(fileContent);

        //        //---------------具体对象----
        //        var values = new[] {
        //          new KeyValuePair<string, string>("CreateImageSizes", createImageSizes) ,
        //          new KeyValuePair<string, string>("FileDir",fileDir),
        //          new KeyValuePair<string, string>("FileName",fileName)
        //        };
        //        values.Each(i =>
        //        {
        //            form.Add(new StringContent(i.Value), String.Format("\"{0}\"", i.Key));
        //        });
        //        //--------------异步上传-------
        //        HttpResponseMessage res = client.PostAsync(siteWebResource, form).Result;
        //        //uploadModel = jsonSerializer.Deserialize<WebApiResultBase>(res.Content.ReadAsStringAsync().Result);
        //        //--------------上传成功,删除临时目录文件--------
        //        if (uploadModel.status == "success" && uploadModel.data.Result == "success")
        //            File.Delete(HttpContext.Current.Server.MapPath(fileSrc));
        //        #endregion
        //    }
        //    return uploadModel;
        //}

        //[NonAction]
        //public string CommonUpload<T>(string fileDir, string fileName, JavaScriptSerializer jsonser, out T t, out WebApiResultBase modelBase) where T : new()
        //{
        //    //-----检查是否是上传资源文件----------
        //    if (!Request.Content.IsMimeMultipartContent("form-data"))
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //    //----文件保存目录路径 
        //    string SaveTempPath = fileDir;
        //    String dirTempPath = string.Empty;
        //    string fileSavePath = string.Empty;
        //    string appFileFolder = string.Empty;
        //    string finalPath = HttpContext.Current.Server.MapPath(fileDir);

        //    //------------------获取 MultipartFormDataStreamProvider 对象-------------------------------
        //    var provider = GetMultipartFormData<T>(out t, out modelBase);

        //    #region  ==获取资源文件==
        //    if (provider.FileData.Count > 0)
        //    {
        //        var file = provider.FileData[0];//--获取资源数据--
        //        string formDataFileName = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
        //        FileInfo fileinfo = new FileInfo(file.LocalFileName);

        //        string fileExt = Path.GetExtension(formDataFileName);
        //        fileSavePath = SaveTempPath + fileName + fileExt;
        //        fileinfo.CopyTo(Path.Combine(HttpContext.Current.Server.MapPath(SaveTempPath), fileName + fileExt), true);
        //        fileinfo.Delete();
        //    }
        //    #endregion

        //    return fileSavePath;
        //}

        //[NonAction]
        //public MultipartFormDataStreamProvider GetMultipartFormData<T>(out T t, out WebApiResultBase modelBase) where T : new()
        //{
        //    JavaScriptSerializer jsonser = new JavaScriptSerializer();
        //    //modelBase = new WebApiResultBase();
        //    t = new T();
        //    var provider = new MultipartFormDataStreamProvider(HttpContext.Current.Server.MapPath("/Temp/"));

        //    //---------阻止异步操作,强制同步,获取报文数据--------------------------------------------
        //    IEnumerable<HttpContent> parts = null;
        //    Task.Factory
        //        .StartNew(() => parts = Request.Content.ReadAsMultipartAsync(provider).Result.Contents,
        //            CancellationToken.None,
        //            TaskCreationOptions.LongRunning,
        //            TaskScheduler.Default)
        //        .Wait();

        //    try
        //    {
        //        //--获取文本数据--
        //        if (provider.FormData.Count > 0)
        //        {
        //            string stringSerialObj = jsonser.Serialize(provider.FormData.Cast<string>().Select(s => new { Key = s, Value = provider.FormData[s] }).ToDictionary(p => p.Key, p => p.Value));
        //            t = jsonser.Deserialize<T>(stringSerialObj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       // modelBase = new WebApiResultBase() { data = new WebApiResultData { Msg = "您的传参有误!" } };
        //    }

        //    return provider;
        //}
        #endregion

    }
}