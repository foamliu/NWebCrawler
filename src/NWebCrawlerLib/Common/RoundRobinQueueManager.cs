using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWebCrawlerLib.Interface;
using System.Collections;
using System.Threading;

namespace NWebCrawlerLib.Common
{
    public static class CircularLinkedList
    {
        public static LinkedListNode<DomainUrlBucket> NextOrFirst(this LinkedListNode<DomainUrlBucket> current)
        {
            if (current.Next == null)
                return current.List.First;
            return current.Next;
        }

        public static LinkedListNode<DomainUrlBucket> PreviousOrLast(this LinkedListNode<DomainUrlBucket> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            return current.Previous;
        }
    }

    /// <summary>
    /// 基本的URL队列 "UrlQueueManager"，主要问题在于：
    /// </summary>
    public class RoundRobinQueueManager : IQueueManager
    {
        private Dictionary<string, LinkedListNode<DomainUrlBucket>> m_Hashtable = new Dictionary<string, LinkedListNode<DomainUrlBucket>>();
        private LinkedList<DomainUrlBucket> m_CircularLinkedList = new LinkedList<DomainUrlBucket>();
        private LinkedListNode<DomainUrlBucket> m_CurrentNode;
        private object lockObject = new object();
        private int m_Count = 0;

        public int Count
        {
            get
            {
                return m_Count;
            }
        }

        public void Enqueue(string url)
        {
            lock (lockObject)
            {
                SynchronizedEnqueue(url);
                Interlocked.Increment(ref m_Count);
            }
        }

        private void SynchronizedEnqueue(string url)
        {
            string baseUri = Utility.GetBaseUri(url);

            if (m_Hashtable.ContainsKey(baseUri))
            {
                LinkedListNode<DomainUrlBucket> chain = m_Hashtable[baseUri];
                chain.Value.UrlQueue.Enqueue(url);
            }
            else
            {
                LinkedListNode<DomainUrlBucket> newNode = new LinkedListNode<DomainUrlBucket>(new DomainUrlBucket());
                m_Hashtable[baseUri] = newNode;
                newNode.Value.UrlQueue.Enqueue(url);

                if (m_CurrentNode == null)
                {
                    m_CircularLinkedList.AddFirst(newNode);
                    m_CurrentNode = newNode;
                }
                else
                {
                    m_CircularLinkedList.AddBefore(m_CurrentNode, newNode);
                }
            }
        }

        public string Dequeue()
        {
            lock (lockObject)
            {
                var result = SynchronizedDequeue();
                Interlocked.Decrement(ref m_Count);
                return result;
            }
        }

        private string SynchronizedDequeue()
        {
            if (m_CurrentNode == null)
            {
                if (m_CircularLinkedList.Count > 0)
                {
                    m_CurrentNode = m_CircularLinkedList.First;
                }
                else
                {
                    throw new InvalidOperationException("Dequeue from an empty url queue.");
                }
            }

            while (m_CircularLinkedList.Count > 0 && m_CurrentNode.Value.UrlQueue.Count == 0)
            {
                var temp = m_CurrentNode.NextOrFirst();
                m_CircularLinkedList.Remove(m_CurrentNode);
                m_CurrentNode = temp;
            }

            if (m_CurrentNode.Value.UrlQueue.Count == 0)
            {
                m_CurrentNode = null;
                throw new InvalidOperationException("Dequeue from an empty url queue.");
            }
            var result = m_CurrentNode.Value.UrlQueue.Dequeue();

            if (m_CurrentNode.Value.UrlQueue.Count == 0)
            {
                m_Hashtable.Remove(Utility.GetBaseUri(result));
            }
            m_CurrentNode = m_CurrentNode.NextOrFirst();

            return result;
        }

        public void Clear()
        {
            m_Hashtable.Clear();
            m_CircularLinkedList.Clear();
            m_CurrentNode = null;
            m_Count = 0;
        }
    }
}
