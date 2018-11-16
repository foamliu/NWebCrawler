using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWebCrawlerLib
{
    public class CrawlHistroyEntry
    {
        public string Url { get; set; }
        public DateTime Timestamp { get; set; } // 时间戳  
        public long Size { get; set; }
    }
}
