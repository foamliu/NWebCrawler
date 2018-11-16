using NWebCrawlerLib.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NWebCrawler.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for NWebRequestTest and is intended
    ///to contain all NWebRequestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NWebRequestTest
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
        ///A test for GetResponse
        ///</summary>
        [TestMethod()]
        public void GetResponseTest()
        {
            Uri uri = new Uri("http://www.sohu.com");
            bool bKeepAlive = true;
            NWebRequest request = new NWebRequest(uri, bKeepAlive);
            NWebResponse response = request.GetResponse();
            Assert.AreEqual("text/html", response.ContentType);
            Assert.IsTrue(response.ContentLength > 360000);
            Assert.AreEqual(true, response.KeepAlive);
        }
    }
}
