﻿///////////////////////////////////////////////////////////////////////
// CommChannel.cs - Demonstrate use of channel with a single process //
// Ver 1.0                                                           //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2016   //
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * The ChannelDemo package defines one class, ChannelDemo, that uses 
 * the Comm<Client> and Comm<Server> classes to pass messages to one 
 * another.
 * 
 * Public Interface:
 * -----------------
 * rcvThreadproc()
 * 
 * Required Files:
 * ---------------
 * - ChannelDemo.cs
 * - ICommunicator.cs, CommServices.cs
 * - Messages.cs, MessageTest, Serialization
 *
 * Maintenance History:
 * --------------------
 * Ver 1.0 : 10 Nov 2016
 * - first release 
 *  
 */
using System;
using System.Collections.Generic;
using System.Threading;
using Utilities;

namespace CommChannel
{
    ///////////////////////////////////////////////////////////////////
    // ChannelDemo class
    // - Shows how to define Sender and Receiver classes using packages
    //   CommService.cs, ICommunicator.cs, BlockingQueue.cs, Messages.cs.
    // - Each endpoint would have one of each.
    // - This demo does that, but simply sends messages to itself
    //
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
        public static string makeTestRequest()
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver("td1.dll");
            te1.addCode("t1.dll");
            te1.addCode("t2.dll");
            TestElement te2 = new TestElement("test2");
            te2.addDriver("td2.dll");
            te2.addCode("tc3.dll");
            te2.addCode("tc4.dll");
            TestRequest tr = new TestRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            return tr.ToXml();
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
   // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class ChannelDemo<T>
    {
        public Comm<T> comm { get; set; } = new Comm<T>();

        public string name { get; set; } = typeof(T).Name;

        //----< intialize sender and receiver >--------------------------

        public ChannelDemo()
        {
        }
        //----< define receive thread processing >-----------------------

        public void rcvThreadProc()
        {
            Message msg = new Message();
            while (true)
            {
                msg = comm.rcvr.GetMessage();
                Console.Write("\n  getting message on rcvThread {0}", Thread.CurrentThread.ManagedThreadId);
                if (msg.type == "TestRequest")
                {
                    TestRequest tr = msg.body.FromXml<TestRequest>();
                    if (tr != null)
                    {
                        Console.Write(
                          "\n  {0}\n  received message from:  {1}\n{2}\n  deserialized body:\n{3}",
                          msg.to, msg.from, msg.body.shift(), tr.showThis()
                          );
                        if (msg.body == "quit")
                            break;
                    }
                }
                else
                {
                    Console.Write("\n  {0}\n  received message from:  {1}\n{2}", msg.to, msg.from, msg.body.shift());
                    if (msg.body == "quit")
                        break;
                }
            }
            Console.Write("\n  receiver {0} shutting down\n", msg.to);
        }
        
        public string makeTestRequest()
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver("td1.dll");
            te1.addCode("t1.dll");
            te1.addCode("t2.dll");
            TestElement te2 = new TestElement("test2");
            te2.addDriver("td2.dll");
            te2.addCode("tc3.dll");
            te2.addCode("tc4.dll");
            TestRequest tr = new TestRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            return tr.ToXml();
        }
    }
    class Client { }
    class Server { }

    class TestDemoChannel
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Project #4 Channel Prototype");
            Console.Write("\n ============================================\n");

            ChannelDemo<Client> demo1 = new ChannelDemo<Client>();

            string sndrEndPoint1 = Comm<Client>.makeEndPoint("http://localhost", 8080);
            string rcvrEndPoint1 = Comm<Server>.makeEndPoint("http://localhost", 8080);
            demo1.comm.rcvr.CreateRecvChannel(rcvrEndPoint1);
            Thread rcvThread1 = demo1.comm.rcvr.start(demo1.rcvThreadProc);
            Console.Write("\n  rcvr thread id = {0}", rcvThread1.ManagedThreadId);
            Console.WriteLine();

            ChannelDemo<Server> demo2 = new ChannelDemo<Server>();

            string sndrEndPoint2 = Comm<Client>.makeEndPoint("http://localhost", 8081);
            string rcvrEndPoint2 = Comm<Server>.makeEndPoint("http://localhost", 8081);
            demo2.comm.rcvr.CreateRecvChannel(rcvrEndPoint2);
            Thread rcvThread2 = demo2.comm.rcvr.start(demo2.rcvThreadProc);
            Console.Write("\n  rcvr thread id = {0}", rcvThread2.ManagedThreadId);
            Console.WriteLine();

            // make a TestRequest message and send five times to two different endpoints

            Message msg = null;
            string rcvrEndPoint;

            for (int i = 0; i < 5; ++i)
            {
                msg = new Message(demo1.makeTestRequest());
                msg.type = "TestRequest";
                msg.from = sndrEndPoint1;
                if (i < 3)
                    msg.to = rcvrEndPoint = rcvrEndPoint1;
                else
                    msg.to = rcvrEndPoint = rcvrEndPoint2;
                msg.author = "Fawcett";
                msg.time = DateTime.Now;

                demo1.comm.sndr.PostMessage(msg);

                Console.Write("\n  {0}\n  posting message with body:\n{1}", msg.from, msg.body.shift());
                Thread.Sleep(20);
            }
            msg = new Message();
            msg.from = sndrEndPoint1;
            msg.to = rcvrEndPoint2;
            msg.body = "quit";

            demo1.comm.sndr.PostMessage(msg);
            rcvThread2.Join();
            Console.Write("\n  rcvThread1.state = {0}", rcvThread1.ThreadState.ToString());
            Console.Write("\n  rcvThread2.state = {0}", rcvThread2.ThreadState.ToString());
            msg = new Message();
            msg.from = sndrEndPoint2;
            msg.to = rcvrEndPoint1;
            msg.body = "quit";

            demo2.comm.sndr.PostMessage(msg);
            Thread.Sleep(500);
            Console.Write("\n  rcvThread1.state = {0}", rcvThread1.ThreadState.ToString());

            rcvThread1.Join();
            Console.Write("\n  passed 2nd Join");

            Console.Write("\n\n");
        }
    }
}
