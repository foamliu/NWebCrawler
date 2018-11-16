using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;

namespace NWebCrawlerLib
{
    class Program
    {
        public static void Main()
        {
            //string url = "http://www.cnn.com/";
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //req.Timeout = Settings.ConnectionTimeout;
            //HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            //string cotentType = response.ContentType;
            //Encoding enc = Encoding.Default;
            //StreamReader inStream = new StreamReader(response.GetResponseStream(), enc);
            //string html = inStream.ReadToEnd();
            //response.Close();
            //inStream.Close();

            //Console.WriteLine(cotentType);

            //string md5 = Utility.MD5(url);
            //string folder = Path.Combine(Settings.DownloadFolder, md5);

            //Console.WriteLine(md5);
            //Console.WriteLine(folder);

            //Console.WriteLine(CrawlerThread.GetExtensionByMimeType("text/html"));

            Settings.ThreadCount = 15;
            Console.WriteLine(Settings.ThreadCount);
            
        }
    }
}
