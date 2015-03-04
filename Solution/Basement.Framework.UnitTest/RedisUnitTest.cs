using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Basement.Framework.IO;
using Basement.Framework.Net;
using Basement.Framework.Redis;
using Basement.Framework.UnitTest.Models;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Basement.Framework.UnitTest
{
    [TestClass]
    public class RedisUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            RedisExt redis = new RedisExt("127.0.0.1", 6379);
            redis.Set<string>("name", "tang");
            string name = redis.Get<string>("name");
        }

        [TestMethod]
        public void TestDownLoadImges()
        {
            RemotePicture.TakeRemoteImg("http://img4.imgtn.bdimg.com/it/u=2325196701,2989572833&fm=23&gp=0.jpg", "D:/webimg");
        }

        [TestMethod]
        public void GetAllImg()
        {
            string url = "http://tuan.jd.com/team-13009194.html";
            RemotePicture.DownRemoteImg(url, "D:\\webimg", false);
        }

        [TestMethod]
        public void GetGoodsDetail()
        {
            string url = "http://item.jd.com/1307848885.html";
            // string html = HttpWeb.Get(url);
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument doc = htmlWeb.Load(url);
            HtmlNode pdetail = doc.DocumentNode.SelectSingleNode("//div[@id='product-detail-1']");
            HtmlNode detailList = doc.DocumentNode.SelectSingleNode("//ul[@class='detail-list']");
            //提取图片保存到本地
            //RemotePicture.TakeImgToLocal(pdetail.InnerHtml, "D:\\webimg", false);

        }

        [TestMethod]
        public void TestTaobaoGoods()
        {
            TaobaoGoods goods = new TaobaoGoods("");

        }

    }
}
