using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWebCrawlerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NWebCrawler.Test
{
    [TestClass()]
    public class NormalizeTest
    {
        
        /// <summary>
        /// 相对URL转换成绝对URL
        /// </summary>
        [TestMethod()]
        public void Test1()
        {
            string baseUri = "http://www.example.com/";
            string testUri = "/foamliu/faq.html";
            Utility.Normalize(baseUri, ref testUri);
            Assert.AreEqual("http://www.example.com/foamliu/faq.html", testUri);
        }

        /// <summary>
        /// 删除默认端口号
        /// </summary>
        [TestMethod()]
        public void Test2()
        {
            string testUri1 = "http://www.example.com:80/bar.html";
            Utility.Normalize(ref testUri1);
            Assert.AreEqual("http://www.example.com/bar.html", testUri1);

            string testUri2 = "http://www.example.com:81/";
            Utility.Normalize(ref testUri2);
            Assert.AreEqual("http://www.example.com:81/", testUri2);
        }

        /// <summary>
        /// 根目录添加斜杠
        /// </summary>
        [TestMethod()]
        public void Test3()
        {
            string testUri = "http://www.example.com";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/", testUri);
        }

        /// <summary>
        /// 猜测的目录添加尾部斜杠
        /// </summary>
        [TestMethod()]
        public void Test4()
        {
            string testUri = "http://www.example.com/foamliu";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/foamliu/", testUri);
        }         

        /// <summary>
        /// 删除分块
        /// </summary>
        [TestMethod()]
        public void Test5()
        {
            string testUri = "http://www.example.com/bar.html#section1";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/bar.html", testUri);
        }
       
        /// <summary>
        /// 解析路径
        /// </summary>
        [TestMethod()]
        public void Test6()
        {
            string testUri = "http://www.example.com/../a/b/../c/./d.html";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/a/c/d.html", testUri);
        }

        /// <summary>
        /// 删除缺省名字
        /// </summary>
        [TestMethod()]
        public void Test7()
        {
            string testUri1 = "http://www.example.com/default.asp";
            Utility.Normalize(ref testUri1);
            Assert.AreEqual("http://www.example.com/", testUri1);
        
            string testUri2 = "http://www.example.com/a/index.html";
            Utility.Normalize(ref testUri2);
            Assert.AreEqual("http://www.example.com/a/", testUri2);
        }

        /// <summary>
        /// 解码转义字符
        /// </summary>
        [TestMethod()]
        public void Test8()
        {
            string testUri = "http://www.example.com/%7Efoamliu/";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/~foamliu/", testUri);
        }
        
        /// <summary>
        /// 当查询字符串为空时去掉问号"?"
        /// </summary>
        [TestMethod()]
        public void Test9()
        {
            string testUri = "http://www.example.com/?";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/", testUri);
        }

        /// <summary>
        /// 转换为小写
        /// </summary>
        [TestMethod()]
        public void Test10()
        {
            string testUri = "HTTP://www.Example.com/";
            Utility.Normalize(ref testUri);
            Assert.AreEqual("http://www.example.com/", testUri);
        }

        [TestMethod()]
        public void BaseUriTest()
        {
            string testUri1 = "http://www.example.com/foamliu/default.asp";
            string baseUri = Utility.GetBaseUri(testUri1);
            Assert.AreEqual("http://www.example.com/", baseUri);
        }
    }
}
