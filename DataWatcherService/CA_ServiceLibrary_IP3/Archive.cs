using System.IO;
using System.IO.Compression;

namespace ServiceLibrary_IP3
{
    public class Archive
    {
        readonly OptionsManager Manager;
        private readonly ArchiveOptions Options;
        private readonly Logger logger;
        public Archive()
        {
            Manager = new OptionsManager(true);
            Options = Manager.GetOptions<ArchiveOptions>(Options);
        }
        public Archive(ArchiveOptions options)
        {
            Options = options;
        }

        public FileInfo Compress(FileInfo fileToCompress)
        {
            try
            {
                FileInfo currentArchive = fileToCompress;
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden)
                        != FileAttributes.Hidden &
                   fileToCompress.Extension != ".gz")
                {
                    using (FileStream originalFileStream = fileToCompress.OpenRead())   // or new FileStream(fileToCompress.FullName, FileMode.OpenOrCreate))
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName.Replace(".txt", ".gz")))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                    }
                    currentArchive = new FileInfo(fileToCompress.FullName.Replace(".txt", ".gz"));
                }
                if (Options.IsLoggerEnable)
                {
                    logger.RecordEntry($"File {0} was compressed", fileToCompress.Name);
                }

                return currentArchive;
            }
            catch (FileNotFoundException ex)
            {
                if (Options.IsLoggerEnable)
                {
                    logger.RecordEntry("Compress error:" + ex.Message);
                }

                return fileToCompress;
            }
        }
        public FileInfo Decompress(FileInfo fileToDecompress)
        {
            try
            {
                FileInfo currentArchive = fileToDecompress;
                if (fileToDecompress.Extension == ".gz")
                {
                    using (FileStream originalFileStream = fileToDecompress.OpenRead())
                    {
                        using (FileStream decompressedFileStream = File.Create(fileToDecompress.FullName.Replace(".gz", ".txt")))
                        {
                            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                    }
                    currentArchive = new FileInfo(fileToDecompress.FullName.Replace(".gz", ".txt"));
                    if (Options.IsLoggerEnable)
                    {
                        logger.RecordEntry($"File {0} was decompressed", fileToDecompress.Name);
                    }

                    fileToDecompress.Delete();
                }
                return currentArchive;
            }
            catch (FileNotFoundException ex)
            {
                if (Options.IsLoggerEnable)
                {
                    logger.RecordEntry("Decompress error:" + ex.Message);
                }

                return fileToDecompress;
            }
        }
    }
}

