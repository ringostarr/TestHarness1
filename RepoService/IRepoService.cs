/////////////////////////////////////////////////////////////////////////////
//  IRepoService.cs - Interface Defining Repo Service                      //
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
 *   Interface Defines contracts and repository service 
 * 
 *   
 *   Public Interface
 *   ----------------
 * void sendmessage();
 * void getmessage();
 * void uploadfile();
 *        
 *   
 */
/*
 *   
 *   
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 17 November 2016
 *     - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RepoService1
{
    [ServiceContract(Namespace = "RepoService")]
    public interface IBasicService
    {
        [OperationContract]
        void sendMessage();

        [OperationContract]
        string[] getMessage();


    }

    [ServiceContract(Namespace = "RepoService")]
    public interface IFTService
    {
        [OperationContract(IsOneWay = true)]
        void upLoadFile(FileTransferMessage msg);
        [OperationContract]
        Stream downLoadFile(string filename);
    }

    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public string filename { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream transferStream { get; set; }
    }
}
