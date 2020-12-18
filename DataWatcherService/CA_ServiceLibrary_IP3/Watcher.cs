//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ServiceLibrary_IP3
{
    public class Watcher
    {
        readonly OptionsManager Manager;
        readonly List<FileSystemWatcher> watchers;
        object obj;
        bool enabled;
        readonly WatcherOptions Options;
        readonly string SourceDirectory;
        readonly string TargetDirectory;
        readonly bool IsLoggerEnable;
        readonly Logger logger;
        readonly Cryptor Cryptor;
        readonly Archive Archive;

        public Watcher()
        {
            Manager = new OptionsManager(true);
            Options = Manager.GetOptions<WatcherOptions>(Options);
            SourceDirectory = Options.SourceDirectory;
            TargetDirectory = Options.TargetDirectory;
            IsLoggerEnable = Options.IsLoggerEnable;
            enabled = true;
            watchers = new List<FileSystemWatcher>
                {
                    new FileSystemWatcher(SourceDirectory),
                    new FileSystemWatcher(TargetDirectory)
                };
            /*
            ArchiveOptions ArchiveSet = new ArchiveOptions() { IsCompressEnable = true, CompressionLevel = (System.IO.Compression.CompressionLevel)1, SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            CryptingOptions CryptorSet = new CryptingOptions() { IsEncryptEnable = true, SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            LoggerOptions LoggerSet = new LoggerOptions() { LogFile = "D:\\services\\LogFile.txt", SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            WatcherOptions WatcherSet = new WatcherOptions() {  SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            Options DefaultOptionsSet = new Options() { SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            EtlJsonOptions EtlContent = new EtlJsonOptions(ArchiveSet, CryptorSet, LoggerSet, WatcherSet, DefaultOptionsSet);
            string json = JsonConvert.SerializeObject(EtlContent);
           //FileStream sr = new FileStream(File.WriteAllText() @"C:\Users\Asus\Desktop\Универ\3 Семестр. 2 Курс\asp.net.C#\LaboratoryProjects.С#\LP3\DataWatcherService\DataWatcherService\apptest.json")
            File.WriteAllText(@"C:\Users\Asus\Desktop\Универ\3 Семестр. 2 Курс\asp.net.C#\LaboratoryProjects.С#\LP3\DataWatcherService\DataWatcherService\apptest.json",json);
            */
            watchers[0].Deleted += ChangeWatcher_Deleted;
            watchers[1].Deleted += ChangeWatcher_Deleted;
            watchers[0].Created += ChangeWatcher_Created;
            watchers[1].Created += ChangeWatcher_Created;
            /*
            watchers[0].Changed += ChangeWatcher_Changed;
            watchers[1].Changed += ChangeWatcher_Changed;
            watchers[0].Renamed += ChangeWatcher_Renamed;
            watchers[1].Renamed += ChangeWatcher_Renamed;
            */
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
            if (IsLoggerEnable)
            {
                logger.RecordEntry(fileEvent, filePath, watcherName[i]);
            }

            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "changed";
            string filePath = e.FullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            if (IsLoggerEnable)
            {
                logger.RecordEntry(fileEvent, filePath, watcherName[i]);
            }

            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "renamed to" + e.FullPath;
            string filePath = e.OldFullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            if (IsLoggerEnable)
            {
                logger.RecordEntry(fileEvent, filePath, watcherName[i]);
            }

            FileHandler(e.FullPath);
        }
        private void ChangeWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string[] watcherName = { "sender", "receiver" };
            string fileEvent = "deleted";
            string filePath = e.FullPath;
            _ = filePath.Contains(SourceDirectory) ? 0 : 1;
            int i = filePath.Contains(TargetDirectory) ? 1 : 0;
            if (IsLoggerEnable)
            {
                logger.RecordEntry(fileEvent, filePath, watcherName[i]);
            }
        }
        private void FileHandler(string eFPath)
        {
            try
            {
                FileInfo currentFile;
                if (eFPath.Contains(".txt") && eFPath.Contains(SourceDirectory))
                {
                    currentFile = new FileInfo(eFPath);
                    if (!eFPath.Contains("AES.txt"))
                    {
                        currentFile = Cryptor.Encryption(currentFile);
                    }

                    if (currentFile.Exists)
                    {
                        FileInfo oldFile = currentFile;
                        currentFile = Archive.Compress(oldFile);
                        oldFile.Delete();
                    }

                    if (currentFile.FullName.Contains("AES.gz"))
                    {
                        int ifSpace = currentFile.Name.IndexOf("_") + 1;
                        DirectoryInfo DirGen = new DirectoryInfo(TargetDirectory + currentFile.Name.Substring(ifSpace, 4)
                                                            + "\\" + currentFile.Name.Substring(ifSpace + 5, 2)
                                                            + "\\" + currentFile.Name.Substring(ifSpace + 8, 2));
                        if (!DirGen.Exists)
                        {
                            DirGen.Create();
                        }

                        if (!new FileInfo(DirGen.FullName + "\\" + currentFile.Name).Exists)
                        {
                            File.Move(currentFile.FullName, DirGen.FullName + "\\" + currentFile.Name);
                        }

                        if (new FileInfo(DirGen.FullName.Replace(TargetDirectory, SourceDirectory) + "\\" + currentFile.Name).Exists)
                        {
                            File.Delete(currentFile.FullName);
                        }
                    }
                }
                if (eFPath.Contains("AES.gz") && eFPath.Contains(TargetDirectory))
                {
                    Cryptor.Decryption(Archive.Decompress(new FileInfo(eFPath)));
                }
            }
            catch (FileNotFoundException ex)
            {
                if (IsLoggerEnable)
                {
                    logger.RecordEntry("FileHandler exeption: " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            if (!(obj is null))
            {
                obj = null;
            }

            throw new NotImplementedException();
        }
    }
}
