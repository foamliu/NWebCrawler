using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWebCrawlerLib;
using NWebCrawlerLib.Common;
using NWebCrawlerLib.Interface;

namespace NWebCrawler.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for RoundRobinQueueManagerTest and is intended
    ///to contain all RoundRobinQueueManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RoundRobinQueueManagerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Dequeue
        ///</summary>
        [TestMethod()]
        public void DequeueTest()
        {
            IQueueManager queueManager = new RoundRobinQueueManager(); 
            string url;
            try
            {
                url = queueManager.Dequeue();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }
            Assert.AreEqual(0, queueManager.Count);

            queueManager.Enqueue("http://stackoverflow.com/questions/716256/creating-a-circually-linked-list-in-c");
            queueManager.Enqueue("http://stackoverflow.com/questions/1293058/round-robin-tournament-algorithm-in-c-sharp");
            queueManager.Enqueue("http://www.ltaaa.com/wtfy/4624.html");

            Assert.AreEqual(3, queueManager.Count);
            queueManager.Enqueue("http://www.ltaaa.com/wtfy/3848.html");
            queueManager.Enqueue("http://msdn.microsoft.com/en-us/library/64syzecx.aspx");
            queueManager.Enqueue("http://msdn.microsoft.com/en-us/library/dd831853");

            url = queueManager.Dequeue();
            Assert.AreEqual("http://stackoverflow.com/questions/716256/creating-a-circually-linked-list-in-c", url);
            url = queueManager.Dequeue();
            Assert.AreEqual("http://www.ltaaa.com/wtfy/4624.html", url);
            url = queueManager.Dequeue();
            Assert.AreEqual("http://msdn.microsoft.com/en-us/library/64syzecx.aspx", url);
            Assert.AreEqual(3, queueManager.Count);

            url = queueManager.Dequeue();
            Assert.AreEqual("http://stackoverflow.com/questions/1293058/round-robin-tournament-algorithm-in-c-sharp", url);
            url = queueManager.Dequeue();
            Assert.AreEqual("http://www.ltaaa.com/wtfy/3848.html", url);
            url = queueManager.Dequeue();
            Assert.AreEqual("http://msdn.microsoft.com/en-us/library/dd831853", url);
            Assert.AreEqual(0, queueManager.Count);

            try
            {
                url = queueManager.Dequeue();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }
        }
    }
}
