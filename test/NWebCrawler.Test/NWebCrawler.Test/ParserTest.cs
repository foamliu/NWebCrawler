using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NWebCrawlerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NWebCrawler.Test
{
    [TestClass()]
    public class ParserTest
    {
        [TestMethod()]
        public void ExtractLinksTest1()
        {
            string testData = @"..\data\sina_12_28.txt";
            StreamReader reader = new StreamReader(new FileStream(testData, FileMode.Open));
            string html = reader.ReadToEnd();
            string[] links = Parser.ExtractLinks("", html);
            //foreach(string link in links)
            //{
            //    Console.WriteLine(link);
            //}

            Assert.AreEqual(355, links.Length);
           
        }

        [TestMethod()]
        public void ExtractLinksTest2()
        {
            string testData = @"..\data\pdc_09.txt";
            StreamReader reader = new StreamReader(new FileStream(testData, FileMode.Open));
            string html = reader.ReadToEnd();
            string[] links = Parser.ExtractLinks("", html);
            foreach (string link in links)
            {
                Console.WriteLine(link);
            }

            //Assert.AreEqual(355, links.Length);
            

        }
    }
}
