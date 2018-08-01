
/////////////////////////////////////////////////////////////////////////////
//  RepoServer.cs - Simulates Repository Server             //
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
 *   This Package simulates the Repository  
 *  
 *    
 * 
 *   
 *   Public Interface
 *   ----------------
 *   CreateChannel(url)
 *   CreateServiceChannel(url)
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
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepoService1
{
    public class RepoServer
    {
        class Host
        {
            static ServiceHost CreateChannel(string url)
            {
                WSHttpBinding binding = new WSHttpBinding();
                Uri address = new Uri(url);
                Type service = typeof(RepoService);
                ServiceHost host = new ServiceHost(service, address);
                host.AddServiceEndpoint(typeof(RepoService1.IBasicService), binding, address);
                return host;
            }
            static ServiceHost CreateServiceChannel(string url)
            {
                // Can't configure SecurityMode other than none with streaming.
                // This is the default for BasicHttpBinding.
                //   BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;
                //   BasicHttpBinding binding = new BasicHttpBinding(securityMode);

                BasicHttpBinding binding = new BasicHttpBinding();
                binding.TransferMode = TransferMode.Streamed;
                binding.MaxReceivedMessageSize = 50000000;
                Uri baseAddress = new Uri(url);
                Type service = typeof(StreamService);
                ServiceHost host = new ServiceHost(service, baseAddress);
                host.AddServiceEndpoint(typeof(IFTService), binding, baseAddress);
                return host;
            }
            static void Main(string[] args)
            {
                Console.Write("\n  Repository Sever ");
                Console.Write("\n =====================================\n");

                ServiceHost host = null;
                ServiceHost host1 = null;
                try
                {
                    host = CreateChannel("http://localhost:8001/MessagePassingService");
                    host1 = CreateServiceChannel("http://localhost:8000/StreamService");
                    host1.Open();
                  //  Console.Write("Opened StreamService Channel");
                    
                    //int millisec = 500;
                    // Console.Write("\n  waiting {0} ms to demonstrate client tolerance to repository server delay", millisec);
                    // Thread.Sleep(millisec);
                    host.Open();
                    Console.Write("\n  Started RepoService - Press key to exit:\n");
                   Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n\n", ex.Message);
                }
                finally
                {
                    host.Close();
                    host1.Close();
                }
               

               
            }
        }
    }
}
