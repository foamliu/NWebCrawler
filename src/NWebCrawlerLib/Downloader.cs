
namespace NWebCrawlerLib
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using NWebCrawlerLib.Common;
    using NWebCrawlerLib.Interface;

    public delegate void DownloaderStatusChangedEventHandler(object sender, DownloaderStatusChangedEventArgs e);
    public class DownloaderStatusChangedEventArgs : EventArgs
    {

    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum DownloaderStatusType
    {
        NotStarted,
        Running,
        Suspended,
        Stopped,
    }

    public enum DownloaderEventType
    {

    }

    /// <summary>
    /// 下载者
    /// </summary>
    public class Downloader : IDownloader
    {
        #region Privates

        public event DownloaderStatusChangedEventHandler StatusChanged;

        // foamliu, 2009/12/27.
        // 尚未访问的URL列表, 使用先进先出 （First-in-first-out, FIFO) 的队列， 
        // 对应的爬虫就是宽度优先爬虫 (Breadth-first crawler).
        // 
        //public Queue<string> UrlsQueueFrontier = /*Queue.Synchronized(*/new Queue<string>();
        private IQueueManager m_queue;

        // 为了避免重复下载或陷入爬虫陷阱， 只有未被访问的URL才会被加入到队列中去.
        // 这就需要一个爬取历史这样的数据结构来维护已访问过的URL.        
        public HashSet<string> m_urlSet;

        // 爬取历史是包含有时间戳的URL列表.
        public IList<CrawlHistroyEntry> m_history;

        public long TotalSize { get; set; }
        public object TotalSizelock = new object();

        public long Errors = 0L;

        private Collection<CrawlerThread> m_crawlerThreads;

        private DownloaderStatusType m_status;
        private bool m_dirty;

        #endregion

        #region Properties

        public IQueueManager UrlsQueueFrontier
        {
            get
            {
                return m_queue;
            }
            set
            {
                m_queue = value;
            }
        }

        public IEnumerable<CrawlerThread> Crawlers
        {
            get
            {
                return m_crawlerThreads;
            }
        }

        public DownloaderStatusType Status
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
                if (null != StatusChanged)
                {
                    StatusChanged(this, null);
                }
            }
        }

        public HashSet<string> CrawledUrlSet
        {
            get
            {
                return m_urlSet;
            }
            set
            {
                m_urlSet = value;
            }
        }

        public IList<CrawlHistroyEntry> CrawleHistroy
        {
            get
            {
                return m_history;
            }
            set
            {
                m_history = value;
            }
        }

        public bool Dirty
        {
            get { return m_dirty; }
            set { m_dirty = value; }
        }

        #endregion

        #region ctor

        public Downloader()
        {
            m_status = DownloaderStatusType.NotStarted;
            m_queue = new RoundRobinQueueManager();
            m_urlSet = new HashSet<string>();
            m_history = new List<CrawlHistroyEntry>();

            m_dirty = false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seeds">
        /// 通常爬虫是从一系列种子(Seed)网页开始,然后使用这些网页中的链接去获取其他页面.
        /// </param>
        public void InitSeeds(string[] seeds)
        {
            UrlsQueueFrontier.Clear();
            // 使用种子URL进行队列初始化
            foreach (string s in seeds)
                UrlsQueueFrontier.Enqueue(s);
        }

        public void Start()
        {
            // 如果已经启动则退出
            if (null != m_crawlerThreads) return;

            m_crawlerThreads = new Collection<CrawlerThread>();

            for (int i = 0; i < MemCache.ThreadCount; i++)
            {
                CrawlerThread crawler = new CrawlerThread(this);
                crawler.StatusChanged += new CrawlerStatusChangedEventHandler(CrawlerStatusChanged);
                crawler.Start();

                m_crawlerThreads.Add(crawler);
            }

            this.Status = DownloaderStatusType.Running;

        }

        public void Suspend()
        {
            if (null == m_crawlerThreads) return;

            foreach (CrawlerThread crawler in m_crawlerThreads)
            {
                crawler.Suspend();
            }

            this.Status = DownloaderStatusType.Suspended;

        }

        public void Resume()
        {
            if (null == m_crawlerThreads) return;

            foreach (CrawlerThread crawler in m_crawlerThreads)
            {
                crawler.Resume();
            }

            this.Status = DownloaderStatusType.Running;
        }

        public void Abort()
        {
            if (null == m_crawlerThreads) return;

            foreach (CrawlerThread crawler in m_crawlerThreads)
            {
                crawler.Abort();
            }
        }

        private void CrawlerStatusChanged(object sender, CrawlerStatusChangedEventArgs e)
        {
            this.m_dirty = true;
        }

        public double GetDownloadSpeed()
        {
            long totalSize = 0;
            DateTime now = DateTime.UtcNow;
            int window = 15; // seconds
            lock (CrawleHistroy)
            {
                for (int i = CrawleHistroy.Count - 1; i >= 0; i--)
                {
                    CrawlHistroyEntry entry = CrawleHistroy[i];
                    if ((now - entry.Timestamp) <= TimeSpan.FromSeconds(window))
                        totalSize += entry.Size;
                    else
                        break;
                }

            }
            double speed = 1.0 * totalSize / window / 1024;
            return speed;
        }

        public void Dump(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.CreateNew)))
            {
                while (UrlsQueueFrontier.Count > 0)
                {
                    string url = UrlsQueueFrontier.Dequeue();
                    writer.WriteLine(url);
                }
            }
        }




    }
}
