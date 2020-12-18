namespace ServiceLibrary_IP3
{
    public class WatcherOptions : Options
    {
        public WatcherOptions()
        {

        }
        public WatcherOptions(string sourceDirectory, string targetDirectory, bool IsLoggerEnable)
                            : base(sourceDirectory, targetDirectory, IsLoggerEnable)
        {
        }
    }
}
