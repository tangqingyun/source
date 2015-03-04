using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Basement.Framework.Net
{
    public static class RemotePicture
    {

        /// <summary>
        /// 下载远程图片
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="savedir">保存目录</param>
        /// <param name="isrename">是否重命名</param>
        public static void DownRemoteImg(string address, string savedir, bool isrename = false)
        {
            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(savedir))
                return;
            string htmlContent = HttpWeb.Get(address);
            MatchImgs(htmlContent, savedir, isrename);
        }
        /// <summary>
        /// 从内容中提取图片
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="savedir">保存目录</param>
        /// <param name="isrename">是否重命名</param>
        public static void TakeImgToLocal(string content, string savedir, bool isrename = false)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
            MatchImgs(content, savedir, isrename);
        }

        private static void MatchImgs(string htmlContent, string savedir, bool isrename = false)
        {
            MatchCollection matchs = Regex.Matches(htmlContent, "<img([^>]*)>");
            int i = 1;
            foreach (Match match in matchs)
            {
                int len = match.Value.IndexOf("src=");
                string fh = match.Value.Substring(len + 4, 1);
                string pattern = string.Format("src={0}{1}{2}", fh, @"[\s\S]*?", fh);
                string strsrc = Regex.Match(match.Value, pattern).Value;
                if (string.IsNullOrWhiteSpace(strsrc))
                    continue;
                string imgurl = strsrc.Replace("src=", "").Replace(fh, "");
                try
                {
                    TakeRemoteImg(imgurl, savedir, isrename ? i.ToString() : string.Empty);
                }
                catch (Exception)
                {
                }
                i++;
            }
        }

        /// <summary>
        /// 抓取web图片保存到本地
        /// </summary>
        /// <param name="imgurl">图片url</param>
        /// <param name="savedir">保存目录</param>
        /// <param name="rename">重命名</param>
        public static void TakeRemoteImg(string imgurl, string savedir, string rename = "")
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] bt = wc.DownloadData(imgurl);
                var ms = new MemoryStream(bt);
                Image img = Image.FromStream(ms);
                string filename = string.Empty;
                if (string.IsNullOrWhiteSpace(rename))
                    filename = Path.GetFileName(imgurl);
                else
                    filename = rename + Path.GetExtension(imgurl);

                if (!Directory.Exists(savedir))
                    Directory.CreateDirectory(savedir);
                img.Save(Path.Combine(savedir, filename));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
