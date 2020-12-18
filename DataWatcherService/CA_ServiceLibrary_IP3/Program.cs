using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServiceLibrary_IP3
{
    class Program
    {
        static void Main(string[] args)
        {
            ArchiveOptions ArchiveSet = new ArchiveOptions() { IsCompressEnable = true, CompressionLevel = (System.IO.Compression.CompressionLevel)1, SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            CryptingOptions CryptorSet = new CryptingOptions() { IsEncryptEnable = true, SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            LoggerOptions LoggerSet = new LoggerOptions() { LogFile = "D:\\services\\LogFile.txt", SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            WatcherOptions WatcherSet = new WatcherOptions() { SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            Options DefaultOptionsSet = new Options() { SourceDirectory = "C:\\ServerData\\archive\\", TargetDirectory = "C:\\ClientData\\data\\", IsLoggerEnable = true };
            EtlXmlOptions EtlContent = new EtlXmlOptions(ArchiveSet, CryptorSet, LoggerSet, WatcherSet, DefaultOptionsSet);
            //string json = JsonConvert.SerializeObject(EtlContent);
            string json = @"{
  'ArchiveOptions' {
    'CompressionLevel': 1,
    'CompressEnable': true,
    'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
  },
  'CryptingOptions': {
                'IsEncryptEnable': true,
    'Key': 'AQIDBAUGBwgJEBESExQVFg==',
    'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
  },
  'LoggerOptions': {
                'LogFile': 'D:\\services\\LogFile.txt',
    'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
  },
  'WatcherOptions': {
                'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
  },
  'DefaultOptions': {
                'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
  },
  'SourceDirectory': 'C:\\ServerData\\archive\\',
    'TargetDirectory': 'C:\\ClientData\\data\\',
    'IsLoggerEnable': true
}";
            //filestream sr = new filestream(file.writealltext() @"c:\users\asus\desktop\универ\3 семестр. 2 курс\asp.net.c#\laboratoryprojects.с#\lp3\datawatcherservice\datawatcherservice\apptest.json")
            //file.writealltext(@"c:\users\asus\desktop\универ\3 семестр. 2 курс\asp.net.c#\laboratoryprojects.с#\lp3\datawatcherservice\datawatcherservice\jsondata.txt", json);
            //json = @"c:\users\asus\desktop\универ\3 семестр. 2 курс\asp.net.c#\laboratoryprojects.с#\lp3\datawatcherservice\datawatcherservice\jsondata.json";
            //system.text.json
            //jsonserializer.deserialize<etljsonoptions>();
            //EtlContent = JsonConvert.DeserializeObject<EtlJsonOptions>(json);
            //Console.WriteLine(EtlContent.SourceDirectory + EtlContent.TargetDirectory + EtlContent.IsLoggerEnable);
            XmlSerializer formatter = new XmlSerializer(typeof(EtlXmlOptions));
            
            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, EtlContent);
            }
            
            //string v = nameof(Options);
            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {

                EtlContent = (EtlXmlOptions)formatter.Deserialize(fs);
            }
            Console.ReadKey();
        }
    }
}
