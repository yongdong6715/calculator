using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
namespace Calculator.Utilities
{
    /// <summary>
    /// Error Log
    /// 
    /// For logging important information about an error 
    /// </summary>
    class ErrorLog
    {
        private static List<String>errorList = new List<String>();

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="msg"></param>
        public static void logErrorMessage(String msg)
        {
            errorList.Add("Date: " + DateTime.Today.ToString());
            errorList.Add("Local Time: " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString());
            errorList.Add("Details: ");
            errorList.Add(msg);
        }
        /// <summary>
        /// Write error info to a local file 
        /// </summary>
        /// <param name="info"></param>
        public static void writeToFile()
        {
           
           System.IO.File.WriteAllLines("error_log.txt", errorList.ToArray(), Encoding.UTF8);

               
            
         
        }
    }
}
