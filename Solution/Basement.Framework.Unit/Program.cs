using Basement.Framework.Configuration;
using Basement.Framework.Data;
using Basement.Framework.Data.Configuration;
using Basement.Framework.EnumExt;
using Basement.Framework.IO;
using Basement.Framework.Logging;
using Basement.Framework.Logging.LogProvider;
using Basement.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Basement.Framework.Mongo;
using MongoDB.Bson;

namespace Basement.Framework.Unit
{
    class Program
    {
        static void Main(string[] args)
        {


            //MongoExt mongoHelper = new MongoExt("mongodb://192.168.6.102:30000", "zlrd2012");
            //BsonDocument document = mongoHelper.FindAll("zhaopin.resume_10386", null).SingleOrDefault();
            //long campusid = document["campus_id"].ToInt64();

            //string appName = FrameworkConfig.GetAppSetting("appName");
            //return;

            //IDatabase bll = Database.EMallUni;

            //var list = bll.ExecuteList<CompanyUni>("select * from CompanyGroup_Company");

            //CompanyUni model = new CompanyUni
            //{
            //    CompanyName = "测试2000000",
            //    CreateTime = DateTime.Now,
            //    CreateBy = Guid.NewGuid(),
            //    LinkMan = "zss",
            //    Mobile = "13260184476",
            //};
            //int n = bll.Insert<CompanyUni>(model);

            // string path=@"C:\Users\Administrator\Desktop\示例项目\Source\Solution\Basement.Framework.Unit\abc.txt";
            //FileExtension.WriterFile(path, "示例项目1");
            // string content=FileExtension.ReadFileAsString(path);

            //初始化Log4Net
            //new Basement.Framework.Configuration.Log4NetConfig();

            //ILogger loger = new LogSqlProvider("SysLogAdapter");
            //Entity_SqlLoger logEntity = new Entity_SqlLoger("日志000");
            //loger.Log(LogLevel.Info, logEntity);

            //ILogger loger = LogFactoryProvider.GetLogProvider(EnumLogType.Log4Net, "ErrorLogAdapter_RollingFile");
           // loger.Log(EnumLogLevel.Error, "日志内容qqq");

            //ILogger loger = new Log4NetProvider("HttpLogAdapter");
            //Entity_HttpLog httpLog = new Entity_HttpLog("", "", "", "", 1, EnumHttpMethod.Post, "", "");
            //loger.Log(EnumLogLevel.Info, httpLog);


            //ILogProvider loger = new LogIoFileProvider();
            //loger.Log(LogLevel.Info, "写入日志");
            //IDatabase bll = Database.EMall;
            // var model= bll.SingleAsModel<Company>("SELECT * FROM Uni2uni_EMall.dbo.companygroup_company WHERE CompanyId=265");

            //    DataTable dt=bll.GetPageScript();

            //Company model = new Company
            //{
            //    CompanyName = "测试00011",
            //    CreateTime = DateTime.Now,
            //    CreateBy = Guid.NewGuid(),
            //    LinkMan = "zss",
            //    Mobile = "13260184476",
            //};
            //int n = bll.Insert<Company>(model);
            //Expression<Func<User, object>> exp = u => u.Name == "dd";
            //IList<User> list = new List<User> { 
            //    new User(){ UserID=Guid.NewGuid(), Name="zhang"},
            //    new User(){ UserID=Guid.NewGuid(), Name="san"},
            //};
            //string ss = MyLambdaExpression.ResloveName<User>(exp);
            //  string ss = Configuration.FrameworkConfig.GetAppSetting("appName");
            //string abPath = @"C:\Users\Administrator\Desktop\示例项目\Source\Solution\Basement.Framework.Unit\2.xls";
            //Basement.Framework.Excels.ExcelExtension excel = new Excels.ExcelExtension(abPath);
            //DataTable dt = excel.ReadExcelAsDataTable(true);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<table border='1'>");
            //foreach (DataRow dr in dt.Rows) {
            //    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}M</td><td><img src='{3}'/></td></tr>", dr["id"], dr["name"], dr["distance"], dr["img"]);
            //}
            //sb.Append("</table>");

            Console.ReadKey();
        }
    }



    public class Entity_SystemLog
    {
        public Entity_SystemLog(string message)
        {
            Message = message;
            CreateDate = DateTime.Now;
            Category = string.Empty;
            IP = string.Empty;
            UrlFull = string.Empty;
            Thread = string.Empty;
            LogLevel = string.Empty;
            Logger = string.Empty;
            Title = string.Empty;
            Exception = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Category { set; get; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { set; get; }
        public string UrlFull { set; get; }
        public string Thread { set; get; }
        public string LogLevel { set; get; }
        public string Logger { set; get; }
        public string Title { set; get; }
        public string Exception { set; get; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class Entity_HttpLog
    {
        public Entity_HttpLog(string category, string shortUrl, string fullUrl, string thread, int totalTime, EnumHttpMethod httpMethod, string requestParams, string responseContent)
        {
            this.Category = category;
            this.IPStr = ClientIP.GetUserIP();
            this.UrlShort = shortUrl;
            this.UrlFull = fullUrl;
            this.Thread = thread;
            this.TotalTime = totalTime;
            this.HttpMethod = httpMethod.ToString();
            this.RequestParams = responseContent;
            this.ResponseContent = category;
            this.CreateTime = DateTime.Now;
        }
        public string Category { set; get; }
        public string IPStr { set; get; }
        public string UrlShort { set; get; }
        public string UrlFull { set; get; }
        public string Thread { set; get; }
        public int TotalTime { set; get; }
        public string HttpMethod { set; get; }
        public string RequestParams { set; get; }
        public string ResponseContent { set; get; }
        public DateTime CreateTime { set; get; }
    }

    public class User
    {
        public Guid UserID { set; get; }
        public string Name { set; get; }
    }


}
