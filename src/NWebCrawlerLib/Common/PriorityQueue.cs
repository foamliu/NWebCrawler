using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NWebCrawlerLib.Common
{
    // foamliu, 2009/12/27, 优先队列.
    // 把爬虫队列从先入先出换成优先队列, 即可得到带偏好的爬虫 (Preferential Crawler).
    // 带偏好的爬虫给尚未访问的页面链接赋予优先级, 该优先级根据对页面的估计计算而得.
    // 通常的估计会考虑页面的:
    // 1.拓扑属性, 如目标页面的入度.
    // 2.内容属性, 如程序目标和页面至今的相似度.


    /// <summary>
    /// Summary of running times
    ///             Linked  List        Binary Tree (Min-)Heap Fibonacci Heap 
    /// insert      O(1)                O(log n)    O(log n)    O(1) 
    /// accessmin   O(n)                O(1)        O(1)        O(1) 
    /// deletemin   O(n)                O(log n)    O(log n)    O(log n)* 
    /// decreasekey O(1)                O(log n)    O(log n)    O(1)* 
    /// delete      O(n)                O(n)        O(log n)    O(log n)* 
    /// merge       O(1)                O(m log(n+m)) O(m log(n+m)) O(1) 
    /// (*)Amortized time
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {        
        private BinaryHeap<T> m_list;
        
        public void Insert(T value)
        {            
            m_list.Insert(value);
        }
        
        public T Remove()
        {            
            return (T)m_list.Remove();
        }        

        public PriorityQueue(IComparer<T> strategy)
        {           
            m_list = new BinaryHeap<T>(strategy);
        }
    } 

    public class BinaryHeap<T>
    {
        private ArrayList m_list;
        private IComparer<T> m_comparer;

        public IComparer<T> Comparer
        {
            set { this.m_comparer = value; }
        }

        public BinaryHeap()
        {
            this.m_list = new ArrayList();
        }

        public BinaryHeap(int capacity)
        {
            this.m_list = new ArrayList(capacity);
        }

        public BinaryHeap(IComparer<T> comparer)
        {
            this.m_comparer = comparer;
            this.m_list = new ArrayList();
        }

        public virtual void Clear()
        {
            m_list.Clear();
        }

        public virtual void Insert(T value)
        {
            int pos = m_list.Add(value);
            if (pos == 0) return;

            while (pos > 0)
            {
                int nextPos = pos / 2;

                T toCheck = (T)m_list[nextPos];

                if (m_comparer.Compare(value, toCheck) > 0)
                {
                    m_list[pos] = toCheck;
                    pos = nextPos;
                }
                else
                    break;
            }

            m_list[pos] = value;

        }

        public virtual T Remove()
        {
            if (m_list.Count == 0)
                return default(T);
            T toReturn = (T)m_list[0];

            m_list.RemoveAt(0);

            if (m_list.Count > 1)
            {
                m_list.Insert(0, m_list[m_list.Count - 1]);
                m_list.RemoveAt(m_list.Count - 1);

                int current = 0, possibleSwap = 0;

                while (true)
                {
                    int leftChildPos = 2 * current + 1;
                    int rightChildPos = leftChildPos + 1;

                    if (leftChildPos < m_list.Count)
                    {
                        T entry1 = (T)m_list[current];
                        T entry2 = (T)m_list[leftChildPos];

                        if (m_comparer.Compare(entry2, entry1) > 0)
                            possibleSwap = leftChildPos;
                    }
                    else
                        break;

                    if (rightChildPos < m_list.Count)
                    {
                        T entry1 = (T)m_list[possibleSwap];
                        T entry2 = (T)m_list[rightChildPos];

                        if (m_comparer.Compare(entry2, entry1) > 0)
                            possibleSwap = rightChildPos;
                    }

                    if (current != possibleSwap)
                    {
                        object temp = m_list[current];
                        m_list[current] = m_list[possibleSwap];
                        m_list[possibleSwap] = temp;
                    }
                    else
                        break;

                    current = possibleSwap;
                }
            }

            return toReturn;

        }

        public virtual int Count
        {
            get
            {
                return m_list.Count;
            }
        }


    }
}
