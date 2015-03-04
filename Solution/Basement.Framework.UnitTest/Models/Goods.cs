using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.UnitTest.Models
{
    public class Goods
    {
        public Goods()
        {

        }
        /// <summary>
        /// 地址
        /// </summary>
        public string goodsurl { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { set; get; }
        /// <summary>
        /// 属性list
        /// </summary>
        public Dictionary<string, string> attrlist { set; get; }
        /// <summary>
        /// sku属性
        /// </summary>
        public List<PropertyData> propertys { set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { set; get; }
        /// <summary>
        /// 主图list
        /// </summary>
        public List<string> mainimg_list { set; get; }
        /// <summary>
        /// 商品详细图片list
        /// </summary>
        public List<string> detailimg_list { set; get; }
    }
}
