using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Basement.Framework.Utility
{
    public class WebClientFile
    {
        /// <summary>
        /// WebClient上传文件至服务器
        /// </summary>
        /// <param name="fileNamePath">文件名，全路径格式</param>
        /// <param name="uriString">服务器文件夹路径(IIS,需打开Webdev扩展)</param>
        /// <returns>文件服务器地址</returns>
        public string UpLoadFile(string fileNamePath, string uriString)
        {
            string fileName = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);
            string[] nameArray = fileName.Split(char.Parse("."));

            string NewFileName = nameArray[0] + "_" + DateTime.Now.ToString("yyMMddhhmmss") + "_" + DateTime.Now.Millisecond.ToString() + fileNamePath.Substring(fileNamePath.LastIndexOf("."));

            string fileNameExt = fileNamePath.Substring(fileNamePath.LastIndexOf(".") + 1);
            if (uriString.EndsWith("/") == false) uriString = uriString + "/";

            uriString = uriString + NewFileName;
            /**/
            /// 创建WebClient实例
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            myWebClient.Headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            myWebClient.Headers.Add("User-Agent", "Microsoft Internet Explorer");
            // 要上传的文件
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            try
            {
                //使用UploadFile方法可以用下面的格式
                //myWebClient.UploadFile(uriString,"PUT",fileNamePath);
                byte[] postArray = r.ReadBytes((int)fs.Length);
                Stream postStream = myWebClient.OpenWrite(uriString, "PUT");
                if (postStream.CanWrite)
                {
                    postStream.Write(postArray, 0, postArray.Length);
                }
                else
                {       
                    uriString = "error：文件目前不可写！";

                }
                
                postStream.Close();
                r.Close();
                fs.Close();
                myWebClient.Dispose();
            }
            catch
            {
                uriString = "error：文件上传失败，请稍候重试~";
            }
            return uriString;
        }

        public string UpLoadFileNotChangeName(string fileNamePath, string uriString)
        {
            
            /// 创建WebClient实例
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            myWebClient.Headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            myWebClient.Headers.Add("User-Agent", "Microsoft Internet Explorer");
            // 要上传的文件
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            try
            {
                //使用UploadFile方法可以用下面的格式
                //myWebClient.UploadFile(uriString,"PUT",fileNamePath);
                byte[] postArray = r.ReadBytes((int)fs.Length);
                Stream postStream = myWebClient.OpenWrite(uriString, "PUT");
                if (postStream.CanWrite)
                {
                    postStream.Write(postArray, 0, postArray.Length);
                }
                else
                {
                    uriString = "error：文件目前不可写！";

                }

                postStream.Close();
                r.Close();
                fs.Close();
                myWebClient.Dispose();

            }
            catch
            {
                uriString = "error：文件上传失败，请稍候重试~";
            }
            return uriString;
        }

        /**/
        /// <summary>
        /// 下载服务器文件至客户端
        /// </summary>
        /// <param name="URL">被下载的文件地址，绝对路径</param>
        /// <param name="Dir">另存放的目录</param>
        /// <param name="sLocalName">本地存放文件名</param>
        public void Download(string URL, string Dir, string sLocalName)
        {
            WebClient client = new WebClient();
            string fileName = URL.Substring(URL.LastIndexOf("/") + 1); //被下载的文件名
            string Path = Dir + fileName;
            if (sLocalName.Length > 0)
            {
                Path = Dir + sLocalName;   //另存为的绝对路径＋文件名
            }
            
            try
            {
                WebRequest myre = WebRequest.Create(URL);
                myre.Abort();
            }
            catch
            {
                //MessageBox.Show(exp.Message,"Error"); 
            }

            try
            {
                DirectoryInfo di = new DirectoryInfo(Dir);
                if (di.Exists == false)
                {
                    di.Create();
                }
                client.DownloadFile(URL, Path);

                client.Dispose();
            }
            catch
            {
                //MessageBox.Show(exp.Message,"Error");
            }
        }

    }
}
