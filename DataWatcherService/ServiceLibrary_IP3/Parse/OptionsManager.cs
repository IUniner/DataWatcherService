using System;

namespace ServiceLibrary_IP3
{
    public class OptionsManager
    {

        readonly string JsonPath = @"D:\services\appsettings.json";
        readonly string XmlPath = @"D:\services\config.xml";
        readonly bool IsJson = true;
        private readonly EtlJsonOptions EtlJsonOptions;
        private readonly EtlXmlOptions EtlXmlOptions;
        private readonly JsonParser JsonReader;
        private readonly XmlParser XmlReader;

        public OptionsManager(bool isJson = true)
        {
            IsJson = isJson;
            JsonReader = new JsonParser();
            JsonReader.Parse(JsonPath);
            ArchiveOptions ArchiveSet = new ArchiveOptions()
            {
                IsCompressEnable = string.Format(JsonReader.GetJsonElement("IsCompressEnable")).Contains("true"),
                CompressionLevel = (System.IO.Compression.CompressionLevel)Int32.Parse(JsonReader.GetJsonElement("CompressionLevel")),
                SourceDirectory = JsonReader.GetJsonElement("SourceDirectory"),
                TargetDirectory = JsonReader.GetJsonElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(JsonReader.GetJsonElement("IsLoggerEnable"))
            };
            CryptingOptions CryptorSet = new CryptingOptions()
            {
                IsEncryptEnable = bool.Parse(JsonReader.GetJsonElement("IsEncryptEnable")),
                SourceDirectory = JsonReader.GetJsonElement("SourceDirectory"),
                TargetDirectory = JsonReader.GetJsonElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(JsonReader.GetJsonElement("IsLoggerEnable"))
            };
            LoggerOptions LoggerSet = new LoggerOptions()
            {
                LogFile = JsonReader.GetJsonElement("LogFile"),
                SourceDirectory = JsonReader.GetJsonElement("SourceDirectory"),
                TargetDirectory = JsonReader.GetJsonElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(JsonReader.GetJsonElement("IsLoggerEnable"))
            };
            WatcherOptions WatcherSet = new WatcherOptions()
            {
                SourceDirectory = JsonReader.GetJsonElement("SourceDirectory"),
                TargetDirectory = JsonReader.GetJsonElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(JsonReader.GetJsonElement("IsLoggerEnable"))
            };
            Options DefaultOptionsSet = new Options()
            {
                SourceDirectory = JsonReader.GetJsonElement("SourceDirectory"),
                TargetDirectory = JsonReader.GetJsonElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(JsonReader.GetJsonElement("IsLoggerEnable"))
            };
            EtlJsonOptions = new EtlJsonOptions(ArchiveSet, CryptorSet, LoggerSet, WatcherSet, DefaultOptionsSet);

            XmlReader = new XmlParser();
            XmlReader.Parse(XmlPath);
            ArchiveOptions ArchiveSetXml = new ArchiveOptions()
            {
                IsCompressEnable = string.Format(XmlReader.GetXmlElement("IsCompressEnable")).Contains("true"),
                CompressionLevel = (System.IO.Compression.CompressionLevel)Int32.Parse(XmlReader.GetXmlElement("CompressionLevel")),
                SourceDirectory = XmlReader.GetXmlElement("SourceDirectory"),
                TargetDirectory = XmlReader.GetXmlElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(XmlReader.GetXmlElement("IsLoggerEnable"))
            };
            CryptingOptions CryptorSetXml = new CryptingOptions()
            {
                IsEncryptEnable = bool.Parse(XmlReader.GetXmlElement("IsEncryptEnable")),
                SourceDirectory = XmlReader.GetXmlElement("SourceDirectory"),
                TargetDirectory = XmlReader.GetXmlElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(XmlReader.GetXmlElement("IsLoggerEnable"))
            };
            LoggerOptions LoggerSetXml = new LoggerOptions()
            {
                LogFile = JsonReader.GetJsonElement("LogFile"),
                SourceDirectory = XmlReader.GetXmlElement("SourceDirectory"),
                TargetDirectory = XmlReader.GetXmlElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(XmlReader.GetXmlElement("IsLoggerEnable"))
            };
            WatcherOptions WatcherSetXml = new WatcherOptions()
            {
                SourceDirectory = XmlReader.GetXmlElement("SourceDirectory"),
                TargetDirectory = XmlReader.GetXmlElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(XmlReader.GetXmlElement("IsLoggerEnable"))
            };
            Options DefaultOptionsSetXml = new Options()
            {
                SourceDirectory = XmlReader.GetXmlElement("SourceDirectory"),
                TargetDirectory = XmlReader.GetXmlElement("TargetDirectory"),
                IsLoggerEnable = bool.Parse(XmlReader.GetXmlElement("IsLoggerEnable"))
            };
            EtlXmlOptions = new EtlXmlOptions(ArchiveSetXml, CryptorSetXml, LoggerSetXml, WatcherSetXml, DefaultOptionsSetXml);
        }
        public ArchiveOptions GetOptions<Type>(ArchiveOptions o)
        {
            return IsJson ? EtlJsonOptions.ArchiveOptions : EtlXmlOptions.ArchiveOptions;
        }
        public CryptingOptions GetOptions<Type>(CryptingOptions o)
            => IsJson ? EtlJsonOptions.CryptingOptions : EtlXmlOptions.CryptingOptions;
        public LoggerOptions GetOptions<Type>(LoggerOptions o)
        {
            return IsJson ? EtlJsonOptions.LoggerOptions : EtlXmlOptions.LoggerOptions;
        }
        public WatcherOptions GetOptions<Type>(WatcherOptions o)
            => IsJson ? EtlJsonOptions.WatcherOptions : EtlXmlOptions.WatcherOptions;
        public EtlJsonOptions GetOptions<Type>(EtlJsonOptions o)
            => EtlJsonOptions;
        public EtlXmlOptions GetOptions<Type>(EtlXmlOptions o)
            => EtlXmlOptions;
        public Options GetOptions<Type>(Options o)
        {
            return IsJson ? EtlJsonOptions.DefaultOptions : EtlXmlOptions.DefaultOptions;
        }
    }

}

