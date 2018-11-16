using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWebCrawlerLib.Interface;

namespace NWebCrawlerLib.Common
{
    /// <summary>
    /// 支持5种不同的优先级
    /// </summary>
    public enum FrontierQueuePriority
    {
        Low,
        BelowNormal,
        Normal,
        AboveNormal,
        High,
    }

    /// <summary>
    /// 简单的URL队列，支持五种优先级。
    /// </summary>
    public class BasicQueueManager : IQueueManager
    {
        private Queue<string> lowQueue = new Queue<string>();
        private Queue<string> belowQueue = new Queue<string>();
        private Queue<string> normalQueue = new Queue<string>();
        private Queue<string> aboveQueue = new Queue<string>();
        private Queue<string> highQueue = new Queue<string>();

        /// <summary>
        /// 返回队列长度
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                count = lowQueue.Count
                    + belowQueue.Count
                    + normalQueue.Count
                    + aboveQueue.Count
                    + highQueue.Count;
                return count;
            }

        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            lowQueue.Clear();
            belowQueue.Clear();
            normalQueue.Clear();
            aboveQueue.Clear();
            highQueue.Clear();
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="url"></param>
        public void Enqueue(string url)
        {
            normalQueue.Enqueue(url);
        }

        /// <summary>
        /// 带优先级入队
        /// </summary>
        /// <param name="url"></param>
        /// <param name="priority"></param>
        public void Enqueue(string url, FrontierQueuePriority priority)
        {
            switch (priority)
            {
                case FrontierQueuePriority.Low:
                    SynchronizedEnqueue(lowQueue, url);
                    break;
                case FrontierQueuePriority.BelowNormal:
                    SynchronizedEnqueue(belowQueue, url);
                    break;
                case FrontierQueuePriority.Normal:
                    SynchronizedEnqueue(normalQueue, url);
                    break;
                case FrontierQueuePriority.AboveNormal:
                    SynchronizedEnqueue(aboveQueue, url);
                    break;
                case FrontierQueuePriority.High:
                    SynchronizedEnqueue(highQueue, url);
                    break;
                default:
                    SynchronizedEnqueue(normalQueue, url);
                    break;
            }

        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public string Dequeue()
        {

            if (highQueue.Count > 0)
            {
                return SynchronizedDequeue(highQueue);
            }
            else if (aboveQueue.Count > 0)
            {
                return SynchronizedDequeue(aboveQueue);
            }
            else if (normalQueue.Count > 0)
            {
                return SynchronizedDequeue(normalQueue);
            }
            else if (belowQueue.Count > 0)
            {
                return SynchronizedDequeue(belowQueue);
            }
            else
                return SynchronizedDequeue(lowQueue);

        }

        /// <summary>
        /// 线程安全的入队
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="item"></param>
        private static void SynchronizedEnqueue(Queue<string> queue, string item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// 线程安全的出队
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static string SynchronizedDequeue(Queue<string> queue)
        {
            lock (queue)
            {
                return queue.Dequeue();
            }
        }
    }
}
