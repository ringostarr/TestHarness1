/////////////////////////////////////////////////////////////////////////////
//  Logger.cs - Simulates Logger and Testcode for the TestHarnes         //
//  ver 4.0                                                                //
//  Language:     C#, VS 2015                                              //
//  Platform:     Dell Dimension 8100, Windows 10 Ultimate, SP2            //
//  Application:  Demonstration for CSE681 - Software Modeling & Analysis  //
//  Author:       Akshay , 708 S Beech Street Syracuse NY                  //
//                  (210)-610-7540 akshay@syr.edu                          //
// Reference :  Jim Fawcett, CST 2-187, Syracuse University                //
//                (315) 443-3948, jfawcett@twcny.rr.com                    //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   This Package simulates the logger for the testharness
 *   and also the testcode , we will be testing if the log 
 *   is being written on the correct filepath or not .
 * 
 *   
 *   Public Interface
 *   ----------------
 *   Ilogger add(string logtext);
 *   void showall () ;
 *   string showlog();  
 */
/*
 *   Build Process
 *   -------------
 *   - Required files: Logger.cs  
 *   - Compiler command: csc Logger.cs
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 5 October 2016
 *     - first release
 *     ver 2.0 : 18th November 2016
 *     - fixed for project 4
 * 
 */
using System;
using System.IO;
using System.Text;

namespace CommChannel
{
    public interface Ilogger
    {
        Ilogger add(string logitem);
        void showCurrentItem();
        void showAll();
    }
    public class Logger : Ilogger
    {
        StringBuilder logtext = new StringBuilder();
        string currentItem = null;
        public Logger()
        { //constructor
            string time = DateTime.Now.ToString();
            string title = "\n\n Log:" + time;
            logtext = new StringBuilder(title);
            logtext.Append("\n" + new string('=', title.Length));
        }
        public Ilogger add(string logitem) // add to string
        {
            currentItem = logitem;
            logtext.Append("\n" + logitem);
            return this;
        }
        public void showCurrentItem()
        {
            Console.Write("\n" + currentItem);
        }
        public string getlog()
        {
            return logtext.ToString();
        }
        public void showAll()
        {
            Console.Write(logtext + "\n");
        }
        public void savetofile(string fp)
        {
            try
            {
                StreamWriter stwr = new StreamWriter(fp);
                stwr.WriteLine(logtext);
                Console.Write("\n\nLOGS ARE SAVED AT : " + fp);
                stwr.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                logtext.Append(ex.Data);
            }
        }
        //Stub
        static void main(string[] args)
        {
            Logger x = new Logger();
            x.add("This is a stub \n");
            x.showAll();
        }
    }


}
