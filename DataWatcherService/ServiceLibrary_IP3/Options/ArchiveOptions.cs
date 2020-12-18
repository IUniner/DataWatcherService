using System.IO.Compression;

namespace ServiceLibrary_IP3
{
    public class ArchiveOptions : Options
    {
        // Compress"IsCompressEnable": true,    "CompressionLevel": 1 - Json
        // Compress"IsCompressEnable": true,    "CompressionLevel": 1 - Xml
        public ArchiveOptions() { }
        public ArchiveOptions(bool isCompressEnable, CompressionLevel compressionLevel, string sourceDirectory, string targetDirectory, bool IsLoggerEnable)
                            : base(sourceDirectory, targetDirectory, IsLoggerEnable)
        {
            IsCompressEnable = isCompressEnable;
            CompressionLevel = compressionLevel;
        }
        public CompressionLevel CompressionLevel { get; set; }
        public bool IsCompressEnable { get; set; }
    }
}
