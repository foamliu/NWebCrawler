using System;
using System.IO;

namespace NWebCrawlerLib.Common
{
    /// <summary>
    /// 存储Web文件在本地文件系统
    /// </summary>
    public class FileSystemUtility
    {
        private static string fileFolder = MemCache.FileSystemFolder;

        public static void StoreWebFile(string url, byte[] resource)
        {
            FileStream fs = null;
            string filePath = null;
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            try
            {
                string fileName = url.Replace("/", "_").Replace(":", "").Replace("?", "");
                filePath = fileFolder + "\\" + fileName;
                fs = new FileStream(filePath, FileMode.Create);
               
                fs.Write(resource, 0, resource.Length);
                fs.Flush();
                //FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message + e.StackTrace);
                Logger.Error(filePath);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
