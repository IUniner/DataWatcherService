using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;

namespace DataWatcherService
{
    class Logger
    {
        readonly List<FileSystemWatcher> watchers;
        readonly object obj = new object();
        bool enabled = true;
        readonly string SourceDirectory = "C:\\ClientData\\data\\";
        readonly string TargetDirectory = "C:\\ServerData\\archive\\";
        readonly private byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        public Logger()
        {
            watchers = new List<FileSystemWatcher>
                {
                    new FileSystemWatcher(SourceDirectory),
                    new FileSystemWatcher(TargetDirectory)
                };
            watchers[0].Deleted += ChangeWatcher_Deleted;
            watchers[1].Deleted += ChangeWatcher_Deleted;
            watchers[0].Created += ChangeWatcher_Created;
            watchers[1].Created += ChangeWatcher_Created;
            /*watchers[0].Changed += ChangeWatcher_Changed;
            watchers[1].Changed += ChangeWatcher_Changed;
            watchers[0].Renamed += ChangeWatcher_Renamed;
            watchers[1].Renamed += ChangeWatcher_Renamed;*/
        }

        public void Start()
        {
            watchers[0].EnableRaisingEvents = true;
            watchers[1].EnableRaisingEvents = true;
            watchers[0].IncludeSubdirectories = true;
            watchers[1].IncludeSubdirectories = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watchers[0].EnableRaisingEvents = false;
            watchers[1].EnableRaisingEvents = false;
            watchers[0].IncludeSubdirectories = false;
            watchers[1].IncludeSubdirectories = false;
            enabled = false;
        }
        private void ChangeWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "created";
            string filePath = e.FullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            RecordEntry(fileEvent, filePath, watcherName[i]);
            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "changed";
            string filePath = e.FullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            RecordEntry(fileEvent, filePath, watcherName[i]);
            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "renamed to" + e.FullPath;
            string filePath = e.OldFullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            RecordEntry(fileEvent, filePath, watcherName[i]);
            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "deleted";
            string filePath = e.FullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            RecordEntry(fileEvent, filePath, watcherName[i]);
        }
        private void FileHandler(string eFPath)
        {
            try
            {
                FileInfo currentFile;
                if (eFPath.Contains(".txt") && eFPath.Contains(SourceDirectory))
                {
                    currentFile = new FileInfo(eFPath);
                    if (!eFPath.Contains("AES.txt")) currentFile = Encryption(currentFile);
                    if (currentFile.Exists)
                    {
                        FileInfo oldFile = currentFile;
                        currentFile = Compress(oldFile);
                        oldFile.Delete();
                    }

                    if (currentFile.FullName.Contains("AES.gz"))
                    {
                        int ifSpace = currentFile.Name.IndexOf("_") + 1;
                        DirectoryInfo DirGen = new DirectoryInfo(TargetDirectory + currentFile.Name.Substring(ifSpace, 4)
                                                            + "\\" + currentFile.Name.Substring(ifSpace + 5, 2)
                                                            + "\\" + currentFile.Name.Substring(ifSpace + 8, 2));
                        if (!DirGen.Exists) DirGen.Create();
                        if (!new FileInfo(DirGen.FullName + "\\" + currentFile.Name).Exists) 
                            File.Move(currentFile.FullName, DirGen.FullName + "\\" + currentFile.Name);
                        if (new FileInfo(DirGen.FullName.Replace(TargetDirectory, SourceDirectory) + "\\" + currentFile.Name).Exists) 
                            File.Delete(currentFile.FullName);                        
                    }
                }
                if (eFPath.Contains("AES.gz") && eFPath.Contains(TargetDirectory)) Decryption(Decompress(new FileInfo(eFPath)));                
            }
            catch (FileNotFoundException ex) 
            {
                RecordEntry("FileHandler exeption: " + ex.Message);
            }
        }
        private FileInfo Encryption(FileInfo fileToEncryption)
        {
            try
            {
                FileInfo currentFile = fileToEncryption;
                if (fileToEncryption.Extension == ".txt")
                {
                    using (FileStream originalFileStream = fileToEncryption.OpenRead())
                    {
                        using (FileStream cryptedFileStream = File.Create(fileToEncryption.FullName.Replace(".txt", "_AES.txt")))
                        {
                            Aes aes = Aes.Create();
                            aes.Key = key;
                            byte[] iv = aes.IV;

                            cryptedFileStream.Write(iv, 0, iv.Length);
                            using (CryptoStream encryptionStream = new CryptoStream(cryptedFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                originalFileStream.CopyTo(encryptionStream);
                            }
                        }
                    }
                    currentFile = new FileInfo(fileToEncryption.FullName.Replace(".txt", "_AES.txt"));
                }
                return currentFile;
            }
            catch (FileNotFoundException ex)
            {
                RecordEntry("Encryption error:" + ex.Message);
                return fileToEncryption;
            }
        }
        private FileInfo Compress(FileInfo fileToCompress)
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
                return currentArchive;
            }
            catch (FileNotFoundException ex)
            {
                RecordEntry("Compress error:" + ex.Message);
                return fileToCompress;
            }
        }
        private FileInfo Decompress(FileInfo fileToDecompress)
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
                    fileToDecompress.Delete();
                }
                return currentArchive;
            }
            catch (FileNotFoundException ex)
            {
                RecordEntry("Decompress error:" + ex.Message);
                return fileToDecompress;
            }
        }
        private FileInfo Decryption(FileInfo fileToDecryption)
        {
            try
            {
                FileInfo currentFile = fileToDecryption;
                if (fileToDecryption.Name.Contains("AES.txt"))
                {
                    using (FileStream originalFileStream = fileToDecryption.OpenRead())
                    {
                        Aes aes = Aes.Create();
                        byte[] iv = new byte[aes.IV.Length];

                        originalFileStream.Read(iv, 0, iv.Length);
                        using (FileStream decryptedFileStream = File.Create(fileToDecryption.FullName.Replace("_AES.txt", ".txt")))
                        {
                            using (CryptoStream decryptionFileStream = new CryptoStream(originalFileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                            {
                                decryptionFileStream.CopyTo(decryptedFileStream);
                            }
                        }
                    }
                    currentFile = new FileInfo(fileToDecryption.FullName.Replace("_AES.txt", ".txt"));
                    fileToDecryption.Delete();
                }
                return currentFile;
            }
            catch (FileNotFoundException ex)
            {
                RecordEntry("Decryption error:" + ex.Message);
                return fileToDecryption;
            }
        }
        private void RecordEntry(string fileEvent, string filePath, string watcherName = "default")
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("D:\\services\\LogFile.txt", true))
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
        private void RecordEntry(string message)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("D:\\services\\LogFile.txt", true))
                {
                    writer.WriteLine(String.Format(message));
                    writer.Flush();
                }
            }
        }
    }
}
