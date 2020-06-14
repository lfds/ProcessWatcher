using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWatcher.Utils
{
    /// <summary>
    /// Логгер
    /// </summary>
    public static class Logger
    {
        public static void SaveLogFile(string message, Exception exception = null)
        {
            string location = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ProcessWatcher\";

            if (!File.Exists(location + @"log.txt"))
            {
                File.Create(location + @"log.txt");
            }

            try
            {
                //Opens a new file stream which allows asynchronous reading and writing
                using (StreamWriter sw = new StreamWriter(new FileStream(location + @"log.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    //Writes the method name with the exception and writes the exception underneath
                    sw.WriteLine(String.Format("{0} ({1})", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));

                    if (!string.IsNullOrEmpty(message))
                        sw.WriteLine(message);
                    
                    if (exception != null)
                        sw.WriteLine(exception.ToString());

                    sw.WriteLine("");
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debugger.Log(0, "error", "Exception in logger " + exc.Message);
            }
        }
    }
}
