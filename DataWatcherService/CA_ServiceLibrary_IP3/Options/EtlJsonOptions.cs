namespace ServiceLibrary_IP3
{
    public class EtlJsonOptions : Options
    {
        public ArchiveOptions ArchiveOptions;
        public CryptingOptions CryptingOptions;
        public LoggerOptions LoggerOptions;
        public WatcherOptions WatcherOptions;
        public Options DefaultOptions;

        public EtlJsonOptions() { }
        public EtlJsonOptions(ArchiveOptions archiveOptions, CryptingOptions cryptingOptions, LoggerOptions loggerOptions, WatcherOptions watcherOptions, Options defaultOptions)
        {
            ArchiveOptions = archiveOptions;
            CryptingOptions = cryptingOptions;
            LoggerOptions = loggerOptions;
            WatcherOptions = watcherOptions;
            DefaultOptions = defaultOptions;
            SourceDirectory = DefaultOptions.SourceDirectory;
            TargetDirectory = DefaultOptions.TargetDirectory;
            IsLoggerEnable = DefaultOptions.IsLoggerEnable;
        }

    }
}
