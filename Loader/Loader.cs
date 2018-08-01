
/////////////////////////////////////////////////////////////////////////////
//  Loader.cs - Simulates Loader                                           //
//  ver 4.0                                                                //
//  Language:     C#, VS 2015                                              //
//   Platform:     Windows 10 Ultimate, SP2                                //
//  Application:  Demonstration for CSE681 - Software Modeling & Analysis  //
//  Author:       Akshay , 708 S Beech Street Syracuse NY                  //
//                  (210)-610-7540 akshay@syr.edu 
// Reference :   Jim Fawcett, CSE681 - Software Modeling and Analysis, 
//                    Fall 2016
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   This Package simulates the loader of the test Harness , it will dequeue and send the details to the appdomainmanager
 *  
 *    
 * 
 *   
 *   Public Interface
 *   ----------------
 *  callapm()
 *  rcvthreadproc()
 *  
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   RepoService.cs,IRepoService.cs,HiResTimer.cs
 *   - Compiler command: csc  RepoService.cs,IRepoService.cs,HiResTimer.cs
 *   
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 16 November 2016
 *     - first release
 * 
 */
using RepoService1;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Threading;
using Utilities;
using XmlCreator;


namespace CommChannel
{
    class Server
    {
        public Comm<Server> comm { get; set; } = new Comm<Server>();

        public string endPoint { get; } = Comm<Server>.makeEndPoint("http://localhost", 8080);

        private Thread rcvThread = null;
        List<Thread> t_; 
        public Server()
        {
            comm.rcvr.CreateRecvChannel(endPoint);
            rcvThread = comm.rcvr.start(rcvThreadProc);
             t_ = new List<Thread>();
        }

        public void wait()
        {
            rcvThread.Join();
        }
        public Message makeMessage(string author, string fromEndPoint, string toEndPoint)
        {
            Message msg = new Message();
            msg.author = author;
            msg.from = fromEndPoint;
            msg.to = toEndPoint;
            return msg;
        }
        static void CallApm(object n)
        {
            Message msg = (Message)n;
            Console.Write("\nThreadname is:" + Thread.CurrentThread.ManagedThreadId + "-----Req 4\n");
            msg.showMsg();
           
           // stwr.WriteLine(msg.body);
           // stwr.Close();
            XmlCreator.TestRequest tr = msg.body.FromXml<XmlCreator.TestRequest>();
           
            if (tr != null)
            {
                foreach (XmlCreator.TestElement t in tr.tests)
                {
                    
                  
                    AppDomainManager1 apm = new AppDomainManager1();
                   string x= apm.LoadTests(t, tr.author);
                    string testreqfilename = msg.author + DateTime.Now.Month+DateTime.Now.Day+DateTime.Now.Year+".log";
                    StreamWriter stwr = new StreamWriter("../../../ServerMemory/" + testreqfilename);
                    stwr.WriteLine(x);
                    msg.body = x;
                    stwr.Close();
                    string remoteEndPoint=null;
                    if (msg.author == "Client2")
                    {
                        remoteEndPoint = Comm<Server>.makeEndPoint("http://localhost", 8085);
                    }
                    else if (msg.author == "Client1")
                    {
                         remoteEndPoint = Comm<Server>.makeEndPoint("http://localhost", 8081); }
                    Sender s = new Sender();
                    s.CreateSendChannel(msg.from);
                    msg.from = msg.to;
                    msg.to = remoteEndPoint;
                    Console.Write("\n\nSending test results to Client ---req 8\n\n");
                    s.PostMessage(msg);
                    Console.Write("\n\nSending log file to repository----Req 8\n\n");
                    upload("../../../ServerMemory/" + testreqfilename);
                 
                }
            }
           
                
           


        }
        
        static IFTService CreateServiceChannel(string url)
        {
            BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;

            BasicHttpBinding binding = new BasicHttpBinding(securityMode);
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = 500000000;
            EndpointAddress address = new EndpointAddress(url);

            ChannelFactory<IFTService> factory
              = new ChannelFactory<IFTService>(binding, address);
            return factory.CreateChannel();
        }
       // 
       static IFTService channel;
        //IStreamService channel;
        //string ToSendPath = "..\\..\\..\\Repository";
        string SavePath = "..\\..\\..\\Servermemory";
        int BlockSize = 1024;
        byte[] block;
       static void upload(string filename)
        {
            // byte[] block;
            string ToSendPath = "../../../Repository";
            channel = CreateServiceChannel("http://localhost:8000/StreamService");
           byte[] block = new byte[1024];
            //int totalBytes = 0;
            string fqname = System.IO.Path.Combine(ToSendPath, filename);
            try
                {
                    //hrt.Start();
                    using (var inputStream = new FileStream(filename, FileMode.Open))
                    {
                        FileTransferMessage msg = new FileTransferMessage();
                        msg.filename = filename;
                        msg.transferStream = inputStream;
                        channel.upLoadFile(msg);
                    }
                    // hrt.Stop();

                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }

            }
            
        void download(string filename)
        {
            channel = CreateServiceChannel("http://localhost:8000/StreamService");
            block = new byte[BlockSize];
            int totalBytes = 0;
            try
            {
               // hrt.Start();
                Stream strm = channel.downLoadFile(filename);
                string rfilename = Path.Combine(SavePath, filename);
                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);
                using (var outputStream = new FileStream(rfilename, FileMode.Create))
                {
                    while (true)
                    {
                        int bytesRead = strm.Read(block, 0, BlockSize);
                        totalBytes += bytesRead;
                        if (bytesRead > 0)
                            outputStream.Write(block, 0, bytesRead);
                        else
                            break;
                    }
                }
               // hrt.Stop();
                //ulong time = hrt.ElapsedMicroseconds;
                Console.Write("\n  Recieved file ");
            }
            catch (Exception ex)
            {
                Console.Write("\n  {0}", ex.Message);
            }
        }


        void rcvThreadProc()
        {
            ThreadPool.SetMaxThreads(4, 4);
            int x1 = 0, y1 = 0;
            int num = 8;
            bool y = true;
            while (y==true )
            {
                //ThreadPool.GetAvailableThreads(out x1, out y1);
                Console.Write(x1.ToString(), y1);
                Message msg = comm.rcvr.GetMessage();
                XmlCreator.TestRequest tr = msg.body.FromXml<XmlCreator.TestRequest>();
               
                {
                    foreach(XmlCreator.TestElement t in tr.tests)
                    {
                        if (t.testDriver != null)
                        {
                            this.download(t.testDriver.ToString());
                            foreach(string x in t.testCodes)
                            {
                                this.download(x);
                            }
                            
                        }
                        
                    }
                }
                if (msg==null)
                {
                    y = false;
                    break;
                }
                msg.time = DateTime.Now;
               // while (msg != null)
               // {
                    Console.Write("\n  {0} received message:", comm.name);

                // msg.showMsg();

                if (msg.body == "quit")
                    break;
                else
                {
                    ThreadStart rts = () =>
                    {
                        CallApm(msg);
                    };
                    Thread th = new Thread(rts);
                    x1++;
                    th.Start();
                    if (x1 == num)
                        y = false;
                    t_.Add(th);
                  
                }
                foreach(Thread t in t_)
                {
                    t.Join();
                    

                }
                
            }
           

        }
        void sendtoclient(string msg , string to)
        {
           // Server s = new Server();
            Message msg1 = this.makeMessage("Server", this.endPoint, to);
            msg1.body = msg;
            this.comm.sndr.PostMessage(msg1);
            //sthiswait();
            
        }
        

        static void Main(string[] args)
        {
            Console.Write("\n  Testing Server Demo");
            Console.Write("\n =====================\n");

            Server server = new Server();
            Message msg = server.makeMessage("Fawcett", server.endPoint, server.endPoint);
            
            
            Console.Write("\n  press key to exit: ");
            Console.ReadKey();
           // msg.to = server.endPoint;
           // msg.body = "quit";
           // server.comm.sndr.PostMessage(msg);
            server.wait();
            Console.Write("\n\n");
        }
    }
}
