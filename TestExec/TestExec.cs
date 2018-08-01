/////////////////////////////////////////////////////////////////////////////
//  TestExec.cs - Simulates TestDriver for the TestHarness                 //
//  ver 4.0                                                                //
//  Language:     C#, VS 2015                                              //
//   Platform:     Windows 10 Ultimate, SP2              //
//  Application:  Demonstration for CSE681 - Software Modeling & Analysis  //
//  Author:       Akshay , 708 S Beech Street Syracuse NY                  //
//                  (210)-610-7540 akshay@syr.edu                          //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   This Package simulates the Testdriver for the Test harness 
 *   which tests the logging function and checks if the 
 *   directory is being created.
 * 
 *   
 *   Public Interface
 *   ----------------
 *   class Testexec {} - main class of the testdriver
 *   bool test();
 *    
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   TestExec.cs, Logger.cs , Itest.cs
 *   - Compiler command: csc  TestExec.cs  Logger.cs Itest.cs
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 5 October 2016
 *     - first release
 *     ver 1.1 : 15 November 2016
 *     - Changed to use with WCF , THreads , WPF applications
 *     - Removed tests4 and test5
 * 
 */
using CommChannel;
using Logger.Properties;
using System;
using System.IO;
namespace CommChannel
{
    public class Testexec : MarshalByRefObject,ITest
    {
        string filepath;
        AppDomain ad = null;
        Logger log = new Logger();
        public Testexec()
        {
            ad = AppDomain.CurrentDomain;
            Console.Write("\nSpeaking from :" + ad.FriendlyName);

        }

        public bool test1()
        {
            log.add("Test1 : Always returns true , Test is always Successfull").showCurrentItem();
            Console.Write("\nTest1 always passes");

            return true;
        }
        public bool test2()
        {
            log.add("Test2 : Always returns false , Test will always fail").showCurrentItem();
            Console.Write("\ntest2 always fails");

            return false;
        }
        public void test3()
        {

            try
            {
                throw new System.ArgumentException("THIS IS AN EXCEPTION");
            }
            catch (Exception ex)
            {
                log.add("Test3 : Always returns throw exception " + ex.ToString()).showCurrentItem();



            }

        }
        public bool test4()
        {
            try
            {
                System.IO.Path.GetDirectoryName(filepath);
                if (Directory.Exists(Path.GetDirectoryName(filepath)))
                {
                    log.add("Test4:Check if the the  Directory has been created -- -- - PASSED!").showCurrentItem();

                    return true;
                }
                else
                {
                    log.add("TEst4 fail . ").showCurrentItem();

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                log.add(ex.ToString());
                return false;
            }

        }
        public bool test5()
        {
            try
            {
                System.IO.Path.GetFileName(filepath);
                if (File.Exists(filepath))
                {
                    log.add("LOG FILE IS PRESENT!");
                    return true;
                }
                else
                {
                    log.add("LOG FILE MISSING --FAILED");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return true;
        }
        public void logfilepath(string fp)
        {
            filepath = fp;


        }

        public void runtest()
        {


            if (test1() == true)
            {
                Console.Write("\nTEST PASSED!");
               // log.savetofile(filepath);
            }
            else
                Console.Write("\nTESTFAILED");

            if (test2() == true)
                Console.Write("\nTEST Passed!");
            else
                Console.Write("\nTESTFAILED");

            test3();
            //if (test4() == true)
            //{
            //    Console.Write("Test4Passed");
            //}
            //log.savetofile(filepath);
            //if (test5())
            //{
            //    Console.Write("pass");
            //}
            //else
            //{
            //    Console.Write("fail");
            //}


            log.showAll();
            


        }
        public string showlog()
        {
            return log.getlog();

        }
        //public void getlog()
        //{

        //    StreamReader str = new StreamReader(filepath);
        //    string x = str.ReadLine();
        //    while (x != null)
        //    {
        //        Console.Write(x);
        //        x = str.ReadLine();
        //    }
        //    str.Close();
        //}
        public bool test()
        {
            runtest();
            return true;
        }
        static void main(string[] args)
        {
            Testexec ex = new Testexec();
            ex.test();
            //ex.getlog();
        }
    }


}
