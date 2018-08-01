
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
 *   This Package simulates the Repository  service and implements the IRepoService Interface
 *  
 *    
 * 
 *   
 *   Public Interface
 *   ----------------
 *  uploadFile(string filename)
 *  downloadFile(string filename)
 *  sendMessage()
 *  GetMessage()
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RepoService1
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StreamService : IFTService
    {
        string filename;
        string savePath = "../../../Repository";
        string ToSendPath = "../../../Repository";
        int BlockSize = 1024;
        byte[] block;
        HRTimer.HiResTimer hrt = null;

        StreamService()
        {
            block = new byte[BlockSize];
            hrt = new HRTimer.HiResTimer();
        }

        public void upLoadFile(FileTransferMessage msg)
        {
            int totalBytes = 0;
            hrt.Start();
            filename =Path.GetFileName( msg.filename);
            string rfilename = Path.Combine(savePath, filename);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = msg.transferStream.Read(block, 0, BlockSize);
                    totalBytes += bytesRead;
                    if (bytesRead > 0)
                        outputStream.Write(block, 0, bytesRead);
                    else
                        break;
                }
            }
            hrt.Stop();
            Console.Write(
              "\n  Received file \"{0}\" of {1} bytes in {2} microsec.",
              filename, totalBytes, hrt.ElapsedMicroseconds
            );
        }

        public Stream downLoadFile(string filename)
        {
            hrt.Start();
            string sfilename = Path.Combine(ToSendPath, filename);
            FileStream outStream = null;
            if (File.Exists(sfilename))
            {
                outStream = new FileStream(sfilename, FileMode.Open);
            }
            else
                throw new Exception("open failed for \"" + filename + "\"");
            hrt.Stop();
            Console.Write("\n  Sent \"{0}\" in {1} microsec.", filename, hrt.ElapsedMicroseconds);
            return outStream;
        }

       
    }
    [ServiceBehavior]
    public class RepoService : IBasicService
    {


        public void sendMessage()
        {
            Console.Write("Received signal to send testdrivers and codes\n");
        }

        public string[] getMessage()
        {
            string dir = "../../../Repository";
            string[] files = Directory.GetFiles(dir, "*.dll");
            string[] fileonly;
            int i = 0;
            fileonly = new string[files.Length];
            foreach (string f in files)
            {
                Console.Write(Path.GetFileName(f));
                fileonly[i] = (Path.GetFileName(f));
                i++;

                Console.Write("\n");
            }
            return fileonly;
        }
        static void Main(string[] args)
        {
            RepoService s = new RepoService();
            s.sendMessage();
        }

    }
}
