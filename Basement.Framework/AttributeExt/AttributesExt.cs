using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.AttributeExt
{
    public static class AttributesExt
    {
        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="TAttri">特性标识</typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<TAttributes> GetCustomAttributes<TAttributes>(Type t)
        {
            var att = t.GetCustomAttributes(false);
            return att.OfType<TAttributes>();
        }
        /// <summary>
        /// 判断类是否存在某属性
        /// </summary>
        /// <typeparam name="TAttri">特性标识</typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool ExitsAttributes<TAttributes>(Type t)
        {
            var list = GetCustomAttributes<TAttributes>(t);
            return list.Count() > 0 ? true : false;
        }

    }
}
