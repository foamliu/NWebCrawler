
namespace NWebCrawlerLib
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.IO;

    public class Utility
    {
        public static string Hash(string url)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(url);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }

        /// <summary>
        /// foamliu, 2009/12/27.
        /// 爬虫需要两个URL是否指向相同的页面这一点可以被迅速检测出来, 这就需要URL规范化.
        /// URL规范化做的主要的事情:
        /// 转换为小写
        /// 相对URL转换成绝对URL
        /// 删除默认端口号
        /// 根目录添加斜杠
        /// 猜测的目录添加尾部斜杠
        /// 删除分块
        /// 解析路径
        /// 删除缺省名字
        /// 解码禁用字符
        /// 
        /// 更多信息参照RFC3986:
        /// http://tools.ietf.org/html/rfc3986
        /// </summary>
        /// <param name="strURL"></param>
        public static void Normalize(string baseUri, ref string strUri)
        {
            // 相对URL转换成绝对URL
            if (strUri.StartsWith("/"))
            {
                strUri = baseUri + strUri.Substring(1);
            }

            // 当查询字符串为空时去掉问号"?"
            if (strUri.EndsWith("?"))
                strUri = strUri.Substring(0, strUri.Length - 1);

            // 转换为小写
            strUri = strUri.ToLower();

            // 删除默认端口号
            // 解析路径
            // 解码转义字符
            Uri tempUri = new Uri(strUri);
            strUri = tempUri.ToString();

            // 根目录添加斜杠
            int posTailingSlash = strUri.IndexOf("/", 8);
            if (posTailingSlash == -1)
                strUri += '/';

            // 猜测的目录添加尾部斜杠
            if (posTailingSlash != -1 && !strUri.EndsWith("/") && strUri.IndexOf(".", posTailingSlash) == -1)
                strUri += '/';

            // 删除分块
            int posFragment = strUri.IndexOf("#");
            if (posFragment != -1)
            {
                strUri = strUri.Substring(0, posFragment);
            }

            // 删除缺省名字
            string[] DefaultDirectoryIndexes = 
            {
                "index.html",
                "default.asp",
                "default.aspx",
            };
            foreach (string index in DefaultDirectoryIndexes)
            {
                if (strUri.EndsWith(index))
                {
                    strUri = strUri.Substring(0, (strUri.Length - index.Length));
                    break;
                }
            }
        }

        public static void Normalize(ref string strUri)
        {
            Normalize(string.Empty, ref strUri);
        }

        public static string GetBaseUri(string strUri)
        {
            string baseUri;
            Uri uri = new Uri(strUri);
            string port = string.Empty;
            if (!uri.IsDefaultPort)
                port = ":" + uri.Port;
            baseUri = uri.Scheme + "://" + uri.Host + port + "/";

            return baseUri;

        }

        public static string GetExtensionByMimeType(string mimeType)
        {
            int pos;
            if ((pos = mimeType.IndexOf('/')) != -1)
            {
                return mimeType.Substring(pos + 1);
            }
            return string.Empty;
        }

        public static void ExecuteCommandSync(string fileName, string arguments)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(fileName, arguments);
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.RedirectStandardOutput = false;
                procStartInfo.RedirectStandardInput = false;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
