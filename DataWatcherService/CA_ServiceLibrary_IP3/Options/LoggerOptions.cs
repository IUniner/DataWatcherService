namespace ServiceLibrary_IP3
{
    public class LoggerOptions : Options
    {
        public LoggerOptions()
        {

        }
        public LoggerOptions(string sourceDirectory, string targetDirectory, bool IsLoggerEnable)
                            : base(sourceDirectory, targetDirectory, IsLoggerEnable)
        {
        }
        public string LogFile { get; set; }
    }
}