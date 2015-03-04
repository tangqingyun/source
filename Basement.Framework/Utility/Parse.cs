using System;
using System.IO;
using System.Text;
//using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Json;
namespace Basement.Framework.Utility
{
    /// <summary>
    /// 各种转化类
    /// </summary>
    public class Parse
    {
        public static int ParseInt(string snum, int def)
        {
            int num;
            return int.TryParse(snum, out num) ? num : def;
        }

        public static long ParseLong(string snum, long def)
        {
            long num;
            return long.TryParse(snum, out num) ? num : def;
        }

        public static float Parsefloat(string snum, float def)
        {
            float num;
            return float.TryParse(snum, out num) ? num : def;
        }

        public static decimal ParseDecimal(string snum, decimal def)
        {
            decimal num;
            return decimal.TryParse(snum, out num) ? num : def;
        }

        public static bool ParseBool(string snum, bool def)
        {
            bool num;
            return bool.TryParse(snum, out num) ? num : def;
        }

        public static DateTime ParseTime(string sTime)
        {
            DateTime time;
            return DateTime.TryParse(sTime, out time) ? time : new DateTime();
        }

        public static string Serialize(object item)
        {
            var json = new DataContractJsonSerializer(item.GetType());
            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, item);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static T JsonParse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }
    }
}
