using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace Basement.Framework.Serialization
{
    public class JsonSerializer
    {

        /// 使用JSON.NET 转换对象到JSON字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertToJosnString(object item)
        {
            return item != null ? JsonConvert.SerializeObject(item) : "";
        }

        /// <summary>
        /// 使用JSON.NET 转换JSON字符串到.NET对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T ConvertToObject<T>(string strJson)
        {
            return !string.IsNullOrEmpty(strJson) ? JsonConvert.DeserializeObject<T>(strJson) : default(T);
        }

        /// <summary>
        /// Json序列化,用于发送到客户端
        /// </summary>
        public static string ToJsJson(object item)
        {

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Json反序列化,用于接收客户端Json后生成对应的对象
        /// </summary>
        public static T FromJsonTo<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T jsonObject = (T)ser.ReadObject(ms);
            ms.Close();
            return jsonObject;
        }

        /// <summary>
        ///  序列化DataTable为Json,用于发送到客户端
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dataTable)
        {
            StringBuilder objSb = new StringBuilder();
            JavaScriptSerializer objSer = new JavaScriptSerializer();
            Dictionary<string, object> resultMain = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dataTable.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                resultMain.Add(index.ToString(), result);
                index++;
            }
            objSer.Serialize(resultMain, objSb);
            return objSb.ToString();
        }

        /// <summary>
        /// 序列化DataRow为Json,用于发送到客户端
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string DataRowToJson(DataRow dataRow)
        {
            StringBuilder objSb = new StringBuilder();
            JavaScriptSerializer objSer = new JavaScriptSerializer();
            var dataTable = dataRow.Table;         
            var result = dataTable.Columns.Cast<DataColumn>().ToDictionary<DataColumn, string, object>(dc => dc.ColumnName, dc => dataRow[dc].ToString());
            objSer.Serialize(result, objSb);
            return objSb.ToString();
        }

        public static Dictionary<string, object> JsonToDictionary(string jsonStr)
        {
            try
            {
                var javaScriptSerializer = new JavaScriptSerializer();
                var value =
                    javaScriptSerializer.DeserializeObject(jsonStr) as Dictionary<string, object>;
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ObjectToJson<T>(T o)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            var value = javaScriptSerializer.Serialize(o);
            return value;
        }

        public static string TryObjectToJson<T>(T o)
        {
            try
            {
                var javaScriptSerializer = new JavaScriptSerializer();
                var value = javaScriptSerializer.Serialize(o);
                return value;
            }
            catch
            {
                return string.Empty;
            }

        }

        public static T JsonToObject<T>(string str)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            var value = javaScriptSerializer.Deserialize<T>(str);
            return value;
        }


        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string body = JsonConvert.SerializeObject(obj);
            return body;            
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json) where T : class, new()
        {
            var model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }

    }
}