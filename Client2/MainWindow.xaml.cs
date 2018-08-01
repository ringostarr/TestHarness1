/////////////////////////////////////////////////////////////////////////////
//  MainWindow.xaml.cs - Simulates Client for the TestHarness              //
//  ver 4.0                                                                //
//  Language:     C#, VS 2015                                              //
//   Platform:     Windows 10 Ultimate, SP2                                //
//  Application:  Demonstration for CSE681 - Software Modeling & Analysis  //
//  Author:       Akshay , 708 S Beech Street Syracuse NY                  //
//                  (210)-610-7540 akshay@syr.edu                          //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   This Package simulates the client for the Test harness and enqueues  
 *   tests as TestRequests
 *    then into a loader
 * 
 *   
 *   Public Interface
 *   ----------------
 *   StringValue(string s); converts strings to defined datastructure so that they can be input into a datagrid
 *     Button_click() ; all button clicks functions
 *        
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

using CommChannel;

using RepoService1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IO;

using System.Runtime.InteropServices;
using System.ServiceModel;

using System.Threading;

using System.Windows;

using System.Windows.Controls.Primitives;


namespace Client2
{
    public class StringValue
    {
        public StringValue(string s)
        {
            _value = s;
        }
        public string Value { get { return _value; } set { _value = value; } }
        string _value;
    }
    class Client
    {
        public Comm<Client> comm { get; set; } = new Comm<Client>();
        public string endpoint { get; } = Comm<Client>.makeEndPoint("http://localhost", 8085);

        private Thread rcvThread = null;
        public Client()
        {
            comm.rcvr.CreateRecvChannel(endpoint);
            rcvThread = comm.rcvr.start(rcvThreadProc);
        }
        public void wait()
        {
            rcvThread.Join();
        }

        public Message makeMessage(string author, string fromendpoint, string toendpoint)
        {
            Message msg = new Message();
            msg.author = author;
            msg.from = fromendpoint;
            msg.to = toendpoint;
            return msg;
        }
        public void rt()
        {
            rcvThread.Abort();
            Console.Write("Aborting thread");
            // comm<Client>.rcvr.Close();
        }
        void rcvThreadProc()
        {
            while (true)
            {
                Message msg = comm.rcvr.GetMessage();
                msg.showMsg();
                msg.time = DateTime.Now;
                if (msg.body == "quit")
                    break;
            }
        }
        //static void main (string[] args)
        //{
        //    MainWindow mw = new MainWindow();

        //}

    }
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            
            InitializeComponent();
            this.Show();
            timer = new HRTimer.HiResTimer();
            timer.Start();
            button1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            gtefiles.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Createxmlbutton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Logresults.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            // Dispatcher.BeginInvoke(button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent)));

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
        
        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            

        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

               
                AllocConsole();
                Console.Write("\n  Testing Client Demo");
                Console.Write("\n =====================\n");
                Client cs = new Client();

                
                string rendpoint = Comm<Client>.makeEndPoint("http://localhost", 8080);
                
                Message msg = cs.makeMessage("Client2", cs.endpoint, cs.endpoint);
                msg.body = xmlviewer.Text;
                msg = msg.copy();
                msg.to = rendpoint;
                cs.comm.sndr.PostMessage(msg);
               


            }
            catch (Exception xe)
            {
                Console.Write(xe.ToString());
            }
            //cs.wait();
            Console.Write("\n\n");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IFTService channel;
            string ToSendPath = "upload";
            // string SavePath = "..\\..\\SavedFiles";
            int BlockSize = 1024;
            byte[] block;
            string[] filename = Directory.GetFiles(ToSendPath);
            
            //HRTimer.HiResTimer hrt = null;
            block = new byte[BlockSize];
            channel = CreateServiceChannel("http://localhost:8000/StreamService");
            //hrt = new HRTimer.HiResTimer();
            foreach (string x in filename)
            {
                string x1 = System.IO.Path.GetFileName(x);
                string fqname = System.IO.Path.Combine(ToSendPath, x1);
                try
                {
                    //hrt.Start();
                    using (var inputStream = new FileStream(fqname, FileMode.Open))
                    {
                        FileTransferMessage msg = new FileTransferMessage();
                        msg.filename = x1;
                        msg.transferStream = inputStream;
                        channel.upLoadFile(msg);
                    }
                    // hrt.Stop();
                    FTLabel.Content = " Uploaded file ";
                }
                catch
                {
                    FTLabel.Content = "Error";
                }
            }
            
        }

        private void Createxmlbutton_Click(object sender, RoutedEventArgs e)
        {
            //string[] files;// = new string("aaa"); ;
            List<string> fns = new List<string>();
            //int i = 0;
            List<string> fns2 = new List<string>();
            dataGrid.SelectAllCells();
            dataGrid1.SelectAllCells();
            foreach (StringValue x in dataGrid.Items)
            {

                //var a = dataGrid.SelectedItems[0]..ToString();

                fns.Add(x.Value.ToString());
            }
            foreach (StringValue x in dataGrid1.Items)
            {

                //var a = dataGrid.SelectedItems[0]..ToString();

                fns2.Add(x.Value.ToString());
            }

            string xml = XmlCreator.MessageTest.makeTestRequest(fns[0], fns2, "CLient2");
            xmlviewer.Text = xml;
            
        }
        HRTimer.HiResTimer timer;
        static IBasicService CreateProxy(string url)
        {
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(url);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            return factory.CreateChannel();
        }
        private void gtefiles_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:8001/MessagePassingService";
            IBasicService svc = null;
            svc = CreateProxy(url);
            try
            {
                // gtefiles.Content = "Sending signal to recieve dlls";
                svc.sendMessage();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            ObservableCollection<StringValue> fn = new ObservableCollection<StringValue>();
            ObservableCollection<StringValue> fn2 = new ObservableCollection<StringValue>();
            string[] filenames;
            filenames = svc.getMessage();
            // gt.Content = "recieved files";
            foreach (string f in filenames)
            {
                if (f == "TestExec.dll")
                {
                    StringValue x = new StringValue(f);
                    fn.Add(x);
                }

            }
            dataGrid.ItemsSource = fn;
            foreach (string f in filenames)
            {
                
                    StringValue x = new StringValue(f);
                    fn2.Add(x);
                

            }
            
            dataGrid1.ItemsSource = fn2;

        }

        private void Logresults_Click(object sender, RoutedEventArgs e)
        {
            Console.Write("\n\n Getting Log file from Repository");
            IFTService channel;
            int BlockSize = 1024;
            byte[] block;
            channel = CreateServiceChannel("http://localhost:8000/StreamService");
            block = new byte[BlockSize];
            int totalBytes = 0;
            string SavePath = "ClientDownLoad";
            string filename = "Client2" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Year+".log";
            try
            {
                // hrt.Start();
                Stream strm = channel.downLoadFile(filename);
                string rfilename = System.IO.Path.Combine(SavePath, filename);
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
            StreamReader rd = new StreamReader(SavePath + "/" + filename);
            string log = null;
            log += rd.ReadLine();
            while (rd.ReadLine()!=null)
            {

                log += rd.ReadLine();
            }
            Console.Write(log);
            LOGS.Text = log;
            timer.Stop();
            Timert.Content += timer.ElapsedMicroseconds.ToString();


        }
}

}
