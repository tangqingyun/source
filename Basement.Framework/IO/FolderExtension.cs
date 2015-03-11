using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Basement.Framework.IO
{
    /// <summary>
    /// 目录操作
    /// </summary>
    public class FolderExtension
    {

        public static FileInfo[] GetDirectoryFiles(string folderFullName, string searchPattern = null)
        {

            DirectoryInfo theFolder = new DirectoryInfo(folderFullName);
            if (searchPattern == null)
            {
                return theFolder.GetFiles();
            }
            return theFolder.GetFiles(searchPattern);
        }


        ///<summary>  ///直接删除指定目录下的所有文件及文件夹(保留目录) 
        ///</summary>  ///<param name="strPath">文件夹路径</param> 
        ///<returns>执行结果</returns> 
        public static bool DeleteDir(string strPath)
        {
            try
            {
                strPath = @strPath.Trim().ToString();// 清除空格  
                if (System.IO.Directory.Exists(strPath)) // 判断文件夹是否存在  
                {
                    // 获得文件夹数组  
                    string[] strDirs = System.IO.Directory.GetDirectories(strPath);
                    // 获得文件数组  
                    string[] strFiles = System.IO.Directory.GetFiles(strPath);

                    // 遍历所有子文件夹  
                    foreach (string strFile in strFiles)
                    {
                        // 删除文件夹  
                        System.IO.File.Delete(strFile);
                    }
                    // 遍历所有文件  
                    foreach (string strdir in strDirs)
                    {
                        // 删除文件  
                        System.IO.Directory.Delete(strdir, true);
                    }
                }
                // 成功  
                return true;

            }
            catch (Exception Exp) // 异常处理   
            {
                // 异常信息  
                System.Diagnostics.Debug.Write(Exp.Message.ToString()); // 失败 
                return false;
            }
        }

        public static void IEclear()
        {
            Process process = new Process();
            process.StartInfo.FileName = "RunDll32.exe";
            process.StartInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess 255";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
        }

    }

    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }
}
