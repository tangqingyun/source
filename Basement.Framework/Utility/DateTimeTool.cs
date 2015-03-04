using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basement.Framework.Utility
{
    public class DateTimeTool
    {
        public static string ShowDate(DateTime t)
        {
            DateTime now = DateTime.Now;
            if (t.Year == now.Year)
            {
                return t.ToString("MM/dd");
            }
            else
            {
                return t.ToString("MM/dd/yyyy");
            }
        }

        public static string ShowDateTime(DateTime t)
        {
            DateTime now = DateTime.Now;
            if (t.Year == now.Year)
            {
                return t.ToString("MM/dd HH:mm");
            }
            else
            {
                return t.ToString("MM/dd/yyyy HH:mm");
            }
        }

        public static string ShowUtcDateTime(DateTime t)
        {
            DateTime now = DateTime.Now;
            if (t.Year == now.Year)
            {
                return t.ToUniversalTime().ToString("MM/dd HH:mm") + " (GMT)";
            }
            else
            {
                return t.ToUniversalTime().ToString("MM/dd/yyyyy HH:mm") + " (GMT)";
            }
        }

        public static string ShowUtcDate(DateTime t)
        {
            DateTime now = DateTime.Now;
            if (t.Year == now.Year)
            {
                return t.ToUniversalTime().ToString("MM/dd") + " (GMT)";
            }
            else
            {
                return t.ToUniversalTime().ToString("MM/dd/yyyy") + " (GMT)";
            }
        }

        public static string RelativeDateTime_zh_cn(DateTime time)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - time;
            if (span.TotalDays > 1)
            {
                return span.Days + "天前";
            }
            else if (span.TotalHours > 1)
            {
                return span.Hours + "小时前";
            }
            else if (span.TotalMinutes > 1)
            {
                return span.Minutes + "分钟前";
            }
            else
            {
                return span.Seconds + "秒前";
            }
        }

        public static string RelativeDateTime_en(DateTime time)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - time;
            if (span.TotalDays >= 2)
            {
                return span.Days + " days ago";
            }
            else if (span.TotalDays >= 1 && span.TotalDays < 2)
            {
                return span.Days + " day ago";
            }
            else if (span.TotalHours >= 2)
            {
                return span.Hours + " hours ago";
            }
            else if (span.TotalHours >= 1 && span.TotalHours < 2)
            {
                return span.Hours + " hour ago";
            }
            else if (span.TotalMinutes >= 2)
            {
                return span.Minutes + " minutes ago";
            }
            else if (span.TotalMinutes >= 1 && span.TotalMinutes < 2)
            {
                return span.Minutes + " minute ago";
            }
            else if (span.TotalSeconds >= 2)
            {
                return span.Seconds + " seconds ago";
            }
            else
            {
                return span.Seconds + " second ago";
            }
        }

        public static string TimeSpan_zh_cn(DateTime t1, DateTime t2)
        {
            TimeSpan span = t1 - t2;
            if (span.TotalDays > 1)
            {
                return span.Days + "天";
            }
            else if (span.TotalHours > 1)
            {
                return span.Hours + "小时";
            }
            else if (span.TotalMinutes > 1)
            {
                return span.Minutes + "分钟";
            }
            else
            {
                return span.Seconds + "秒";
            }
        }

        public static string TimeSpan_en(DateTime t1, DateTime t2)
        {
            TimeSpan span = t1 - t2;
            if (span.TotalDays >= 2)
            {
                return span.Days + " days";
            }
            else if (span.TotalDays >= 1 && span.TotalDays < 2)
            {
                return span.Days + " day";
            }
            else if (span.TotalHours >= 2)
            {
                return span.Hours + " hours";
            }
            else if (span.TotalHours >= 1 && span.TotalHours < 2)
            {
                return span.TotalHours + " hour";
            }
            else if (span.TotalMinutes >= 2)
            {
                return span.Minutes + " minutes";
            }
            else if (span.TotalMinutes >= 1 && span.TotalMinutes < 2)
            {
                return span.Minutes + " minute";
            }
            else if (span.TotalSeconds >= 2)
            {
                return span.Seconds + " seconds";
            }
            else
            {
                return span.Seconds + " second";
            }
        }

        public static string TimeSpanPositive_zh_cn(DateTime t1, DateTime t2)
        {
            if (t1 <= t2)
            {
                return "0秒";
            }
            return TimeSpan_zh_cn(t1, t2);
        }

        public static string TimeSpanPositive_en(DateTime t1, DateTime t2)
        {
            if (t1 <= t2)
            {
                return "0 second";
            }
            return TimeSpan_en(t1, t2);
        }

        /// <summary>
        /// 退回今天开始00:00
        /// </summary>
        public static DateTime BackToTodayStart(DateTime now)
        {
            DateTime todayStart = now.Subtract(now.TimeOfDay);
            return todayStart;
        }

        /// <summary>
        /// 去明天开始00:00
        /// </summary>
        public static DateTime GoToNextDayStart(DateTime now)
        {
            DateTime nextDayStart = BackToTodayStart(now.AddDays(1));
            return nextDayStart;
        }

        /// <summary>
        /// 退回本周开始，即上一个周日00:00，一周按周日至周六算
        /// </summary>
        public static DateTime BackToWeekStart(DateTime now)
        {
            DateTime weekStart = now.AddDays(-1 * (int)now.DayOfWeek);
            weekStart = BackToTodayStart(weekStart);
            return weekStart;
        }

        /// <summary>
        /// 退回上周开始
        /// </summary>
        public static DateTime BackToLastWeekStart(DateTime now)
        {
            DateTime curWeekStart = BackToWeekStart(now);
            DateTime lastWeekStart = BackToWeekStart(curWeekStart.AddDays(-1));
            return lastWeekStart;
        }

        /// <summary>
        /// 返回sqlserver最小值时间
        /// </summary>
        /// <returns></returns>
        public static DateTime BackSqlServerMinDatetime()
        {
            return new DateTime(1900, 01, 01);
        }

        /// <summary>
        /// 返回给定时间对应的UTC时间的当天开始时候
        /// 返回的是UTC时间表示（t.Kind == DateTimeKind.Utc，日期各部分皆为UTC时间对应的值）
        /// 比如，如果传入的时间是2013-3-19 5:18(local)，返回的时间是2013-3-18 00:00(Utc)
        /// </summary>
        public static DateTime BackToUTCDateStart(DateTime now)
        {
            DateTime t = now.ToUniversalTime();//转换为UTC时间，此时t.Kind == DateTimeKind.Utc，日期各部分皆为UTC时间对应的值
            t = BackToTodayStart(t);//仍然为UTC时间
            return t;
        }

        /// <summary>
        /// 将一个时间转化为整数，返回从1970-01-01至这个时间的秒数
        /// </summary>
        public static long DateTimeToLong(DateTime t)
        {
            DateTime baseTime = new DateTime(1970, 1, 1);
            TimeSpan s = t - baseTime;
            return (long)Math.Round(s.TotalSeconds);
        }

        /// <summary>
        /// 将一个整数转换为时间，输入从1970-01-01至这个时间的秒数
        /// </summary>
        public static DateTime LongToDateTime(long num_t)
        {
            DateTime baseTime = new DateTime(1970, 1, 1);
            return baseTime.AddSeconds(num_t);
        }

        /// <summary>
        /// 得到TimeStamp时间戳（从1970年到现在的秒数）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <summary>
        /// 得到指定日期的时间戳（从1970年到现在的秒数）
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime datetime)
        {
            TimeSpan ts = datetime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt32(ts.TotalSeconds);
        }
    }
}
