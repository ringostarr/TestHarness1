
/////////////////////////////////////////////////////////////////////////////
//  XmlCreator.cs - Simulates Repository Server                            //
//  ver 4.0                                                                //
//  Language:     C#, VS 2015                                              //
//   Platform:     Windows 10 Ultimate, SP2                                //
//  Application:  Demonstration for CSE681 - Software Modeling & Analysis  //
//  Author:       Akshay , 708 S Beech Street Syracuse NY                  //
//                  (210)-610-7540 akshay@syr.edu                          //
//Reference :     Jim FawcettCSE681 - Software Modeling & Analysis         //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   This Package is responsible for making the test requests
 *  
 *   Public Interface
 *   ----------------
 *  addcode()
 *  adddriver()
 *  TestElemet()
 *  TestRequest()
 *  TestMessage()
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   MainWindow.xaml.cs,MainWindow.xaml,CommChannel.cs,RepoService.cs , BlockingQueue.cs 
 *   - Compiler command: csc  MainWindow.xaml.cs,MainWindow.xaml,CommChannel.cs,RepoService.cs , BlockingQueue.cs
 *   
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 16 November 2016
 *     - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace XmlCreator
{
    public class TestElement
    {
        public string testName { get; set; }
        public string testDriver { get; set; }
        public List<string> testCodes { get; set; } = new List<string>();

        public TestElement() { }
        public TestElement(string name)
        {
            testName = name;
        }
        public void addDriver(string name)
        {
            testDriver = name;
        }
        public void addCode(string name)
        {
            testCodes.Add(name);
        }
        public override string ToString()
        {
            string te = "\ntestName:\t" + testName;
            te += "\ntestDriver:\t" + testDriver;
            foreach (string code in testCodes)
            {
                te += "\ntestCode:\t" + code;
            }
            return te += "\n";
        }
    }
    public class TestRequest
    {
        public TestRequest() { }
        public string author { get; set; }
        public List<TestElement> tests { get; set; } = new List<TestElement>();

        public override string ToString()
        {
            string tr = "\nAuthor:\t" + author + "\n";
            foreach (TestElement te in tests)
            {
                tr += te.ToString();
            }
            return tr;
        }
    }

    public class MessageTest
    {
        public static string makeTestRequest(string DriverName , List<string> testcode,string author)
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver(DriverName);
            foreach(string x in testcode)
            {
                
                te1.addCode(x.ToString());
                Console.Write(x);
            }
            //te1.addCode("t1.dll");
            //te1.addCode("t2.dll");
            //TestElement te2 = new TestElement("test2");
            //te2.addDriver("td2.dll");
            //te2.addCode("tc3.dll");
            //te2.addCode("tc4.dll");
            TestRequest tr = new TestRequest();
            tr.author = author;
            tr.tests.Add(te1);
           // tr.tests.Add(te2);
            return tr.ToXml();
        }
    }
#if (TEST_MESSAGETEST)
    static void Main(string[] args)
    {
      Message msg = new Message();
      msg.to = "http://localhost:8080/ICommunicator";
      msg.from = "http://localhost:8081/ICommunicator";
      msg.author = "Fawcett";
      msg.type = "TestRequest";

      Console.Write("\n  Testing Message with Serialized TestRequest");
      Console.Write("\n ---------------------------------------------\n");
      TestElement te1 = new TestElement("test1");
      te1.addDriver("td1.dll");
      te1.addCode("tc1.dll");
      te1.addCode("tc2.dll");
      TestElement te2 = new TestElement("test2");
      te2.addDriver("td2.dll");
      te2.addCode("tc3.dll");
      te2.addCode("tc4.dll");
      TestRequest tr = new TestRequest();
      tr.author = "Jim Fawcett";
      tr.tests.Add(te1);
      tr.tests.Add(te2);
      msg.body = tr.ToXml();

      Console.Write("\n  Serialized TestRequest:");
      Console.Write("\n -------------------------\n");
      Console.Write(msg.body.shift());

      Console.Write("\n  TestRequest Message:");
      Console.Write("\n ----------------------");
      msg.showMsg();

      Console.Write("\n  Testing Deserialized TestRequest");
      Console.Write("\n ----------------------------------\n");
      TestRequest trDS = msg.body.FromXml<TestRequest>();
      Console.Write(trDS.showThis());
    }
#endif
}

