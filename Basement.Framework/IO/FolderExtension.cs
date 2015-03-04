using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Basement.Framework.IO
{
    /// <summary>
    /// 目录操作
    /// </summary>
    public class FolderExtension
    {

        public static FileInfo[] GetDirectoryFiles(string folderFullName, string searchPattern)
        {

            DirectoryInfo theFolder = new DirectoryInfo(folderFullName);
            return theFolder.GetFiles(searchPattern);
        }
    }
}
