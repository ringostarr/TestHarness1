
/////////////////////////////////////////////////////////////////////////////
//  AppDomainManager.cs - Simulates appdomainmanager                       //
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
 *   This Package simulates the appdomainmanager of the test Harness , it will load the assemblies and create child app domains
 *  
 *    
 * 
 *   
 *   Public Interface
 *   ----------------
 *  showassem()
 *  loadtests()
 *  
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   appdomainmanager.cs 
 *   - Compiler command: csc  appdomainmanager.cs
 *   
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 16 November 2016
 *     - first release
 */
using CommChannel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XmlCreator;

namespace CommChannel
{
    public class AppDomainManager1
    {
        static string tnc;
        static void showAssemblies(AppDomain ad)
        {
            Assembly[] arrayOfAssems = ad.GetAssemblies();
            foreach (Assembly assem in arrayOfAssems)
            {
                Console.Write("\n  {0}", assem);
                Type[] types = assem.GetExportedTypes();
                foreach (Type t in types)
                {
                    if (t.IsClass && typeof(ITest).IsAssignableFrom(t))
                    {
                        Console.Write("\n\n" + t.ToString() + "derives from ITEST and loading it");
                        tnc = t.ToString();
                    }

                    else tnc = null;
                }
            }
        }
        public string LoadTests(TestElement te, string author)
        {
            try
            {
                string filp = Path.GetDirectoryName("../../../ServerMemory/" + te.testDriver.ToString());
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.ApplicationBase = "file:///" + System.Environment.CurrentDirectory;

                Evidence e = AppDomain.CurrentDomain.Evidence;
                AppDomain ad = AppDomain.CreateDomain(te.testDriver.ToString());

                ad.Load(Path.GetFileNameWithoutExtension(filp + "/" + te.testDriver.ToString()));
                showAssemblies(ad);



                ObjectHandle oh = ad.CreateInstance(Path.GetFileNameWithoutExtension(filp + "/" + te.testDriver.ToString()), tnc);
                object ob = oh.Unwrap();

                CommChannel.ITest h = (CommChannel.ITest)ob;
                // string newlogpath = pathdir + "\\log.log";
                // h.logfilepath(newlogpath);

                h.test();
                string l = h.showlog();
                Console.Write(l);
                Console.Write("\n\n Getting Log file from Location\n\n");

                Console.Write("\n\n\n");
                Console.Write("\nunloading domain :" + ad.FriendlyName);
                AppDomain.Unload(ad);
                Console.Write("\ncurrent domain:" + AppDomain.CurrentDomain.FriendlyName);
                Console.Write("\n\n\n");
                return l;

            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return "error";
            }

        }

        //stub
        static void main(string[] args)
        {
            TestElement te = new TestElement();
            te.addCode("abc.dll");
            te.addDriver("xyz.dll");

            AppDomainManager1 a = new AppDomainManager1();
            a.LoadTests(te, "dummyauthor");

        }
    }
}
