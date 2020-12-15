using System;
using System.ComponentModel;
using System.IO;
using System.ServiceProcess;


namespace DataWatcherService
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller ServiceInstaller { get; }
        ServiceProcessInstaller ProcessInstaller { get; }

        public Installer1()
        {
            InitializeComponent();
            ServiceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Manual,
                ServiceName = "Service1",
                DisplayName = "DataWatcherService"
            };
            ProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            Installers.Add(ProcessInstaller);
            Installers.Add(ServiceInstaller);
        }
    }
}
