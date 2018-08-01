using BlockingQueue;
using CommChannel;
using System;
using System.ServiceModel;
using System.Threading;
using System.Windows;

namespace CommChannel
{
    ///////////////////////////////////////////////////////////////////
    // Receiver hosts Communication service used by other Peers
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Receiver<T> : ICommunicator
    {
        static BlockingQueue<Message> rcvBlockingQ = null;
        ServiceHost service = null;

        public string name { get; set; }

        public Receiver()
        {
            if (rcvBlockingQ == null)
                rcvBlockingQ = new BlockingQueue<Message>();
        }

        public Thread start(ThreadStart rcvThreadProc)
        {
            Thread rcvThread = new Thread(rcvThreadProc);
            rcvThread.Start();
            return rcvThread;
        }

        public void Close()
        {
            service.Close();
        }

        //  Create ServiceHost for Communication service
       // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
        public void CreateRecvChannel(string address)
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            service = new ServiceHost(typeof(Receiver<T>), baseAddress);
            service.AddServiceEndpoint(typeof(ICommunicator), binding, baseAddress);
            service.Open();
            Console.Write("\n  Service is open listening on {0}", address);
        }

        // Implement service method to receive messages from other Peers

        public void PostMessage(Message msg)
        {
            //Console.Write("\n  service enQing message: \"{0}\"", msg.body);
            rcvBlockingQ.enQ(msg);
        }

        // Implement service method to extract messages from other Peers.
        // This will often block on empty queue, so user should provide
        // read thread.

        public Message GetMessage()
        {
            Message msg = rcvBlockingQ.deQ();
           Console.Write("\n  {0} dequeuing message from {1}", name, msg.from);
            return msg;
        }
    }
    ///////////////////////////////////////////////////////////////////
    // Sender is client of another Peer's Communication service

    public class Sender
    {
        public string name { get; set; }

        ICommunicator channel;
        string lastError = "";
        BlockingQueue<Message> sndBlockingQ = null;
        Thread sndThrd = null;
        int tryCount = 0, MaxCount = 10;
        string currEndpoint = "";

        //----< processing for send thread >-----------------------------

        void ThreadProc()
        {
            tryCount = 0;
            while (true)
            {
                Message msg = sndBlockingQ.deQ();
                if (msg.to != currEndpoint)
                {
                    currEndpoint = msg.to;
                    CreateSendChannel(currEndpoint);
                }
                while (true)
                {
                    try
                    {
                        channel.PostMessage(msg);
                        Console.Write("\n  posted message from {0} to {1}", name, msg.to);
                        tryCount = 0;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        Console.Write("\n  connection failed");
                        if (++tryCount < MaxCount)
                            Thread.Sleep(100);
                        else
                        {
                            Console.Write("\n  {0}", "can't connect\n");
                            currEndpoint = "";
                            tryCount = 0;
                            break;
                        }
                    }
                }
                if (msg.body == "quit")
                    break;
            }
        }

        //----< initialize Sender >--------------------------------------

        public Sender()
        {
            sndBlockingQ = new BlockingQueue<Message>();
            sndThrd = new Thread(ThreadProc);
            sndThrd.IsBackground = true;
            sndThrd.Start();
        }

        //----< Create proxy to another Peer's Communicator >------------

        public void CreateSendChannel(string address)
        {
            EndpointAddress baseAddress = new EndpointAddress(address);
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory<ICommunicator> factory
              = new ChannelFactory<ICommunicator>(binding, address);
            channel = factory.CreateChannel();
            Console.Write("\n  service proxy created for {0}", address);
        }

        //----< posts message to another Peer's queue >------------------
        /*
         *  This is a non-service method that passes message to
         *  send thread for posting to service.
         */
        public void PostMessage(Message msg)
        {
            sndBlockingQ.enQ(msg);
        }

        public string GetLastError()
        {
            string temp = lastError;
            lastError = "";
            return temp;
        }

        //----< closes the send channel >--------------------------------

        public void Close()
        {
            Console.Write("Closing Channel\n\n\n\n");
           ChannelFactory<ICommunicator> temp = (ChannelFactory<ICommunicator>)channel;
           
            temp.Close();
        }
    }
    ///////////////////////////////////////////////////////////////////
    // Comm class simply aggregates a Sender and a Receiver
    //
    public class Comm<T>
    {
        public string name { get; set; } = typeof(T).Name;

        public Receiver<T> rcvr { get; set; } = new Receiver<T>();

        public Sender sndr { get; set; } = new Sender();

        public Comm()
        {
            rcvr.name = name;
            sndr.name = name;
        }
        public static string makeEndPoint(string url, int port)
        {
            string endPoint = url + ":" + port.ToString() + "/ICommunicator";
            return endPoint;
        }
        //----< this thrdProc() used only for testing, below >-----------

        public void thrdProc()
        {
            while (true)
            {
                Message msg = rcvr.GetMessage();
                msg.showMsg();
                if (msg.body == "quit")
                {
                    break;
                }
            }
        }
        class TestDemoChannel
        {
            static void Main(string[] args)
            {
                Console.Write("\n  Demonstrating Project #4 Channel Prototype");
                Console.Write("\n ============================================\n");

               //ChannelDemo<Client> demo1 = new ChannelDemo<Client>();

             
               // Thread.Sleep(500);
               // Console.Write("\n  rcvThread1.state = {0}", rcvThread1.ThreadState.ToString());

                
                Console.Write("\n  passed 2nd Join");

                Console.Write("\n\n");
            }
        }
    }
}