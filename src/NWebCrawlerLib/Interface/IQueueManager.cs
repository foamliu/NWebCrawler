using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWebCrawlerLib.Interface
{
    public interface IQueueManager
    {
        int Count { get; }
        void Enqueue(string url);
        string Dequeue();
        void Clear();
    }
}
