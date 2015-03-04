using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Basement.Framework.IO;
using HtmlAgilityPack;

namespace Basement.Framework.UnitTest.Models
{
    public class TaobaoGoods : Goods
    {

        private HtmlDocument doc = new HtmlDocument();
        public TaobaoGoods(string goodurl)
        {
            this.goodsurl = goodsurl;
            string path = @"F:\qingyun.tang\My.Test.Projects\Source\Solution\Basement.Framework.UnitTest\goods1.html";
            string text = FileExtension.ReadText(path);
            doc.LoadHtml(text);
            base.title = GetGoodsTitle();
            base.attrlist = GetAttrList();
            base.propertys = GetPropertys();
        }
        /// <summary>
        /// 获取商品标题
        /// </summary>
        /// <returns></returns>
        private string GetGoodsTitle()
        {
            HtmlNode main_title = doc.DocumentNode.SelectSingleNode("//h3[@class='tb-main-title']");
            string goodsName = string.Empty;
            if (main_title != null)
            {
                goodsName = main_title.InnerText.Trim();
            }
            return goodsName;
        }
        /// <summary>
        /// 获取商品属性li
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetAttrList()
        {
            HtmlNodeCollection attributes_list = doc.DocumentNode.SelectNodes("//ul[@class='attributes-list']/li");
            Dictionary<string, string> attrlist = new Dictionary<string, string>();
            foreach (HtmlNode node in attributes_list)
            {
                string[] arr = node.InnerHtml.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                attrlist.Add(arr[0], arr[1]);
            }
            return attrlist;
        }
        /// <summary>
        /// 获取商品属性sku数据
        /// </summary>
        /// <returns></returns>
        private List<PropertyData> GetPropertys()
        {

            HtmlNodeCollection data_property = doc.DocumentNode.SelectNodes("//ul[@data-property]");
            List<PropertyData> dataproperty = new List<PropertyData>();
            foreach (HtmlNode node in data_property)
            {
                PropertyData property = new PropertyData();
                Match m1 = Regex.Match(node.OuterHtml, string.Format("<ul data-property=\"{0}\"", @"[\s\S]*?"));
                if (!string.IsNullOrWhiteSpace(m1.Value))
                {
                    property.PropertyName = m1.Value.Replace("\"", "").Replace("<ul data-property=", "");
                }
                string InnerHtml = node.InnerHtml;
                MatchCollection matchs = Regex.Matches(InnerHtml, @"<span>[\s\S]*?</span>");
                List<string> values = new List<string>();
                foreach (Match match in matchs)
                {
                    values.Add(match.Value.Replace("<span>", "").Replace("</span>", ""));
                }
                property.ValueList = values;
                dataproperty.Add(property);
            }
            return dataproperty;
        }

        private List<string> GetMainImgList()
        {
            return null;
        }

        private List<string> GetDetailImgList()
        {
            return null;
        }

    }
}
