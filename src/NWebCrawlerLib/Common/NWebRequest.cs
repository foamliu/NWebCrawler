using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NWebCrawlerLib.Common
{
    public class NWebRequest
    {
        public string Method;
        public string Header;
        public WebHeaderCollection Headers;
        public Uri RequestUri;
        public int Timeout;
        public bool KeepAlive;
        public NWebResponse response;

        public NWebRequest(Uri uri, bool bKeepAlive)
        {
            Headers = new WebHeaderCollection();
            RequestUri = uri;
            Headers["Host"] = uri.Host;
            KeepAlive = bKeepAlive;
            if (KeepAlive)
                Headers["Connection"] = "Keep-Alive";
            Method = "GET";
        }

        public NWebResponse GetResponse()
        {
            if (response == null || response.socket == null || response.socket.Connected == false)
            {
                response = new NWebResponse();
                response.Connect(this);
                response.SetTimeout(Timeout);
            }
            response.SendRequest(this);
            response.ReceiveHeader();
            return response;
        }

        

        
    }
}
