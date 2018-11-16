using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NWebCrawlerLib
{
    /// <summary>
    /// foamliu, 2010/1/9, 考虑仿照StreamInsight写一个事件流类，以提高性能.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventStream<T>
    {
        class ETPair
        {
            public T Event
            {
                get;
                set;
            }

            public DateTime Timestamp
            {
                get;
                set;
            }

            public ETPair(T e)
            {
                this.Event = e;
                this.Timestamp = DateTime.UtcNow;
            }
        }

        private Queue<ETPair> m_eventQueue;

        public void Insert(T e)
        {            
            m_eventQueue.Enqueue(new ETPair(e));
        }

        public EventStream()
        {
            m_eventQueue = new Queue<ETPair>();
        }
    }
}
