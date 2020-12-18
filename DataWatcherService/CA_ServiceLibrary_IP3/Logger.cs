using System;
using System.IO;

namespace ServiceLibrary_IP3
{
    public class Logger
    {
        readonly OptionsManager Manager;
        readonly LoggerOptions Options;
        readonly object obj;


        public Logger()
        {
            Manager = new OptionsManager(true);
            Options = Manager.GetOptions<LoggerOptions>(Options);
            obj = new object();
        }
        public void RecordEntry(string fileEvent, string filePath, string watcherName = "default")
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(Options.LogFile, true))
                {
                    Console.WriteLine(String.Format("{0} file {1} has been {2} by {3}",
                                                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                                    filePath, fileEvent, watcherName));
                    writer.WriteLine(String.Format("{0} file {1} has been {2} by {3}",
                                                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                                    filePath, fileEvent, watcherName));
                    writer.Flush();
                }
            }
        }
        public void RecordEntry(string message)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(Options.LogFile, true))
                {
                    writer.WriteLine(String.Format(message));
                    writer.Flush();
                }
            }
        }
    }
}
