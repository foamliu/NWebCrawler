using System.IO;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NWebCrawlerLib.Common
{
    public class NWebResponse
    {
        public Socket socket;
        public Uri ResponseUri;
        public WebHeaderCollection Headers;
        public string Header;
        public string ContentType;
        public int ContentLength;
        public bool KeepAlive;

        public NWebResponse()
        {
        }

        public void Connect(NWebRequest request)
        {
            ResponseUri = request.RequestUri;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(ResponseUri.Host).AddressList[0], ResponseUri.Port);
            socket.Connect(remoteEP);
        }

        public void SetTimeout(int Timeout)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, Timeout * 1000);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout * 1000);
        }

        public void SendRequest(NWebRequest request)
        {
            ResponseUri = request.RequestUri;

            request.Header = request.Method + " " + ResponseUri.PathAndQuery + " HTTP/1.0\r\n" + request.Headers;
            socket.Send(Encoding.ASCII.GetBytes(request.Header));
        }

        public void ReceiveHeader()
        {
            Header = "";
            Headers = new WebHeaderCollection();

            byte[] bytes = new byte[10];
            while (socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
            {
                Header += Encoding.ASCII.GetString(bytes, 0, 1);
                if (bytes[0] == '\n' && Header.EndsWith("\r\n\r\n"))
                    break;
            }
            MatchCollection matches = new Regex("[^\r\n]+").Matches(Header.TrimEnd('\r', '\n'));
            for (int n = 1; n < matches.Count; n++)
            {
                string[] strItem = matches[n].Value.Split(new char[] { ':' }, 2);
                if (strItem.Length > 0)
                    Headers[strItem[0].Trim()] = strItem[1].Trim();
            }
            // check if the page should be transfered to another location
            if (matches.Count > 0 && (
                matches[0].Value.IndexOf(" 302 ") != -1 ||
                matches[0].Value.IndexOf(" 301 ") != -1))
                // check if the new location is sent in the "location" header
                if (Headers["Location"] != null)
                {
                    try { ResponseUri = new Uri(Headers["Location"]); }
                    catch { ResponseUri = new Uri(ResponseUri, Headers["Location"]); }
                }
            ContentType = Headers["Content-Type"];
            if (Headers["Content-Length"] != null)
                ContentLength = int.Parse(Headers["Content-Length"]);
            KeepAlive = (Headers["Connection"] != null && Headers["Connection"].ToLower() == "keep-alive") ||
                        (Headers["Proxy-Connection"] != null && Headers["Proxy-Connection"].ToLower() == "keep-alive");
        }

        public void Close()
        {
            socket.Close();
        }

        public byte[] GetResponseStream()
        {
            MemoryStream streamOut = new MemoryStream();
            {
                using (BinaryWriter writer = new BinaryWriter(streamOut))
                {
                    // receive response buffer
                    byte[] RecvBuffer = new byte[10240];
                    int nBytes, nTotalBytes = 0;
                    // loop to receive response buffer
                    while ((nBytes = socket.Receive(RecvBuffer, 0, 10240, SocketFlags.None)) > 0)
                    {
                        // increment total received bytes
                        nTotalBytes += nBytes;
                        // write received buffer to file
                        writer.Write(RecvBuffer, 0, nBytes);
                        // check if connection Keep-Alive to can break the loop if response completed
                        if (KeepAlive && nTotalBytes >= ContentLength && ContentLength > 0)
                            break;
                    }
                }
            }
            return streamOut.ToArray();
        }
    }
}
