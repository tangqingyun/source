using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace Basement.Framework.Net
{
    #region NetHelper
    public class NetHelper
    {
        #region = BigFileDownload =
        /// <summary>
        /// 大文件下载
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="filepath">Identify the file to download including its path.</param>
        public void BigFileDownload(HttpContext Context, string filepath)
        {
            BigFileDownload(Context, filepath, filepath);
        }

        public void BigFileDownload(HttpContext Context, string filepath, string displayname)
        {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file name.
            string filename = System.IO.Path.GetFileName(filepath);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.Read);


                // Total bytes to read:
                dataToRead = iStream.Length;

                Context.Response.ContentType = "application/octet-stream";
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + displayname);

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (Context.Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        Context.Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        Context.Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                Context.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
            }

            Context.Response.Flush();
            Context.Response.End();
        }
        #endregion

        #region = FileDownLoad =
        public void FileDownload(HttpContext Context, FileInfo fi, string displayname)
        {
            displayname = System.Web.HttpUtility.UrlEncode(displayname);
            Context.Response.Clear();
            Context.Response.ClearContent();
            Context.Response.ClearHeaders();
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + displayname);
            Context.Response.AddHeader("Content-Length", fi.Length.ToString());
            Context.Response.AddHeader("Content-Transfer-Encoding", "binary");
            //Context.Response.ContentType = "application/octet-stream";//ms-excel";//
            //Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Context.Response.WriteFile(fi.FullName);
            //Context.Response.Flush();
            //Context.Response.End();
        }
        #endregion

        #region = RemoveParamFromUrl =
        public static string RemoveParamFromUrl(string url, string paramName)
        {
            string regexstr;
            regexstr = string.Format(@"{0}=\s*(.*)&", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, string.Empty, RegexOptions.IgnoreCase);
            }
            regexstr = string.Format(@"([&]|[?]){0}=\s*(.*)$", paramName);
            if (Regex.IsMatch(url, regexstr, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexstr, string.Empty, RegexOptions.IgnoreCase);
            }

            return url;
        }
        #endregion

    }
    #endregion
}
