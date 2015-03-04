using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Basement.Framework.IO
{
    public class FileExtension
    {
        #region - ReadText -
        public static string ReadText(string path, Encoding encoding)
        {
            FileInfo fi = new FileInfo(path);
            StringBuilder str = new StringBuilder();
            if (fi.Exists == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path, encoding))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            str.Append(line + "\n");
                        }
                    }
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    str.Append("The file could not be read:");
                    str.Append(e.Message);
                }

                return str.ToString();

            }

            return "File is not Exists";

        }

        public static string ReadText(string path)
        {
            return ReadText(path, Encoding.GetEncoding("GB2312"));
        }
        #endregion

        #region - WriteText -
      
        public static void WriteText(string FileName_FullPath, string strContent,Encoding encoding=null, bool IsAppend=false )
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            FileInfo fi = new FileInfo(FileName_FullPath);
            if (fi.Exists == false)
            {
                DirectoryInfo di = new DirectoryInfo(fi.DirectoryName);

                if (di.Exists == false)
                {
                    di.Create();
                }
                using (FileStream fs = fi.Create())
                {
                }

                IsAppend = false;
            }

            if (IsAppend == true)
            {
                using (StreamWriter sw = fi.AppendText())
                {
                    sw.Write(strContent);
                    sw.Flush();
                    sw.Close();
                }

            }
            else
            {
                using (StreamWriter sw = new StreamWriter(FileName_FullPath, false, encoding))
                {

                    sw.Write(strContent);
                    sw.Flush();
                    sw.Close();
                }
            }

        }
        #endregion

        #region - Compress -
        public static bool Compress(string sourceFile, string destinationFile)
        {
            // make sure the source file is there
            if (File.Exists(sourceFile) == false)
                return false;

            // Create the streams and byte arrays needed
            byte[] buffer = null;
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream compressedStream = null;

            try
            {
                // Read the bytes from the source file into a byte array
                sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Read the source stream values into the buffer
                buffer = new byte[sourceStream.Length];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);

                if (checkCounter != buffer.Length)
                {
                    return false;
                }

                // Open the FileStream to write to
                destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);

                // Create a compression stream pointing to the destiantion stream
                compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);

                // Now write the compressed data to the destination file
                compressedStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();

                if (compressedStream != null)
                    compressedStream.Close();

                if (destinationStream != null)
                    destinationStream.Close();
            }

            return true;
        }
        #endregion

        #region - Decompress -
        public static bool Decompress(string sourceFile, string destinationFile)
        {
            // make sure the source file is there
            if (File.Exists(sourceFile) == false)
                return false;

            // Create the streams and byte arrays needed
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream decompressedStream = null;
            byte[] quartetBuffer = null;

            try
            {
                // Read in the compressed source stream
                sourceStream = new FileStream(sourceFile, FileMode.Open);

                // Create a compression stream pointing to the destiantion stream
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);

                // Read the footer to determine the length of the destiantion file
                quartetBuffer = new byte[4];
                int position = (int)sourceStream.Length - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);

                byte[] buffer = new byte[checkLength + 100];

                int offset = 0;
                int total = 0;

                // Read the compressed data into the buffer
                while (true)
                {
                    int bytesRead = decompressedStream.Read(buffer, offset, 100);

                    if (bytesRead == 0)
                        break;

                    offset += bytesRead;
                    total += bytesRead;
                }

                // Now write everything to the destination file
                destinationStream = new FileStream(destinationFile, FileMode.Create);
                destinationStream.Write(buffer, 0, total);

                // and flush everyhting to clean out the buffer
                destinationStream.Flush();
            }
            catch
            {
                return false;
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();

                if (decompressedStream != null)
                    decompressedStream.Close();

                if (destinationStream != null)
                    destinationStream.Close();
            }
            return true;
        }
        #endregion

        #region - GetExtension -
        /// <summary>
        /// 获取路径字符串中的文件后缀,
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>小写的后缀名称</returns>
        [Obsolete]
        public static string GetExtension(string filename)
        {
            return Path.GetExtension(filename).ToLower().Replace(".", string.Empty);
            //FileInfo fi = new FileInfo(filename);

            //return fi.Extension.Replace(".", string.Empty).ToLower();
        }
        #endregion

        #region - CopyTo -
        /// <summary>
        /// 将 DirectoryInfo 实例及其内容复制到新路径
        /// </summary>
        /// <param name="sourceDirName">The di.</param>
        /// <param name="destDirName">要将此目录移动到的目标位置的名称和路径。目标不能是另一个具有相同名称的磁盘卷或目录。它可以是您要将此目录作为子目录添加到其中的一个现有目录。 </param>
        /// <remarks>2006-6-30</remarks>
        public static void CopyTo(string sourceDirName, string destDirName)
        {
            DirectoryInfo di = new DirectoryInfo(sourceDirName);
            if (destDirName == null)
            {
                throw new ArgumentNullException("destDirName");
            }
            if (destDirName.Length == 0)
            {
                throw new ArgumentException("Argument_EmptyFileName");
            }

            if (destDirName.EndsWith(@"\") == false)
            {
                destDirName += @"\";
            }

            if (Directory.Exists(destDirName) == false)
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                fi.CopyTo(destDirName + fi.Name);
            }

            foreach (DirectoryInfo cdi in di.GetDirectories())
            {
                CopyTo(cdi.FullName, destDirName + cdi.Name);
            }
        }
        #endregion

    }
}
