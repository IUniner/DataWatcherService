namespace ServiceLibrary_IP3
{
    public class Options
    {
        public Options() { }
        public Options(string sourceDirectory, string targetDirectory, bool IsLoggerEnable)
        {
            SourceDirectory = sourceDirectory;

            TargetDirectory = targetDirectory;

            this.IsLoggerEnable = IsLoggerEnable;
        }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public bool IsLoggerEnable { get; set; }
    }
}
