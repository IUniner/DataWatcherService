using ServiceLibrary_IP3;
using System;
using System.ServiceProcess;
using System.Threading;

namespace DataWatcherService
{
    public partial class Service1 : ServiceBase
    {
        readonly OptionsManager Manager;
        readonly Options Options;
        Watcher Watcher;
        readonly Logger logger = new Logger();
        public Service1()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
            Manager = new OptionsManager(true);
            Options = Manager.GetOptions<Options>(Options);
        }
        internal void TestStartupAndStop(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }
        protected override void OnStart(string[] args)
        {
            if (Options.IsLoggerEnable)
            {
                logger.RecordEntry("Service started...");
            }

            Watcher = new Watcher();
            Thread loggerThread = new Thread(new ThreadStart(Watcher.Start));
            loggerThread.Start();
            if (Options.IsLoggerEnable)
            {
                logger.RecordEntry("Service is ready");
            }
        }
        protected override void OnStop()
        {
            if (Options.IsLoggerEnable)
            {
                logger.RecordEntry("Service stopped.");
            }

            Watcher.Stop();
            Thread.Sleep(1000);
        }
    }


}
