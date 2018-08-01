using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Xml;

namespace XMLparser
{
    public class XMLparse
    {
        public string testName { get; set; }
        public string author { get; set; }
        public DateTime timeStamp { get; set; }
        public String testDriver { get; set; }
        public List<string> testCode { get; set; }

        public void show()
        {
            Console.Write("\n  {0,-12} : {1}", "test name", testName);
            Console.Write("\n  {0,12} : {1}", "author", author);
            Console.Write("\n  {0,12} : {1}", "time stamp", timeStamp);
            Console.Write("\n  {0,12} : {1}", "test driver", testDriver);
            foreach (string library in testCode)
            {

                Console.Write("\n  {0,12} : {1}", "library", library);
            }
        }



    }

    public class XMLtest
    {
        public XDocument doc;
        public List<XMLparse> testList_ = new List<XMLparse>();
        public XMLtest()
        {


        }
        public bool parse(System.IO.Stream xml)
        {
            doc = XDocument.Load(xml);
            if (doc == null)
                return false;
            string author = doc.Descendants("author").First().Value;
            XMLparse test = null;
            XElement[] xtests = doc.Descendants("tests").ToArray();
            int numTests = xtests.Count();
            for (int i = 0; i < numTests; ++i)
            {
                test = new XMLparse();
                test.testCode = new List<string>();
                test.author = author;
                test.timeStamp = DateTime.Now;
                test.testName = xtests[i].Attribute("testName").Value;
                test.testDriver = xtests[i].Element("testDriver").Value;
                IEnumerable<XElement> xtestCode = xtests[i].Elements("testCodes");
                foreach (var xlibrary in xtestCode)
                {
                    test.testCode.Add(xlibrary.Value);
                }
                testList_.Add(test);
            }
            return true;
        }
    }
}