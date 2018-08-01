/////////////////////////////////////////////////////////////////////
// ITest.cs - declares ITest interface                             //
//                                                                 //
//  Platform:     Windows 10 Ultimate, SP2           //
// Application: CSE784 - Software Studio, Final Project Prototype  //
// Author:       Akshay                                            //
/////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ==================
 * This module provides the ITest interface, declaring bool test().
 * 
 * Public Interface:
 * =================
 * bool test();
 * void getlog();
 * string logfilepath(string fp);
 */
/*
 * Build Process:
 * ==============
 * Files Required:
 *   ITest.cs
 *  Command : csc Itest.cs
 *  
 * Maintence History:
 * ==================
 * ver 1.0 : 05 Oct 2016
 *   - first release
 *   ver 2.0 : 14th November 2016
 *      - Changed getlog();
 * 
 */

namespace CommChannel
{
    public interface ITest
    {

        // void logfilepath(string fp);
        // void getlog();
         string showlog();
        bool test();
    }
}
