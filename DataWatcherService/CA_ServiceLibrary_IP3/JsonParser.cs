//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ServiceLibrary_IP3
{
    public class JsonParser
    {
        //static EtlJsonOptions JsonOptions;
        private readonly Dictionary<string, string> jsonDictionary;
        public JsonParser()
        {
            jsonDictionary = new Dictionary<string, string>();

        }
        public Dictionary<string, string> Parse(string jsonFile = null)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonFile))
                {
                    throw new ArgumentException($"'{nameof(jsonFile)}' cannot be null or empty", nameof(jsonFile));
                }
                string jsonDoc = File.ReadAllText(jsonFile);
                JsonDocument json = JsonDocument.Parse(jsonDoc);
                JsonElement entity = json.RootElement;

                var properties = entity.EnumerateObject();
                foreach (var property in properties)
                {
                    jsonDictionary.Add(property.Name, property.Value.ToString());
                }
                return jsonDictionary;
                /*
                while (properties.MoveNext())
                {
                    var property = properties.Current;
                    jsonDictionary.Add(property.Name, property.Value.ToString());
                }
            
                
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    sw.Write(ResultBlock.Text);
                }

                using (FileStream decompressedFileStream = File.Create(fileToDecompress.FullName.Replace(".gz", ".txt")))
                {
                    using (FileStream decompressionStream = new FileStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }*/
                //sr.
                //                string json = @"{
                //  'ArchiveOptions' {
                //    'CompressionLevel': 1,
                //    'CompressEnable': true,
                //    'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //  },
                //  'CryptingOptions': {
                //                'IsEncryptEnable': true,
                //    'Key': 'AQIDBAUGBwgJEBESExQVFg==',
                //    'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //  },
                //  'LoggerOptions': {
                //                'LogFile': 'D:\\services\\LogFile.txt',
                //    'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //  },
                //  'WatcherOptions': {
                //                'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //  },
                //  'DefaultOptions': {
                //                'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //  },
                //  'SourceDirectory': 'C:\\ServerData\\archive\\',
                //    'TargetDirectory': 'C:\\ClientData\\data\\',
                //    'IsLoggerEnable': true
                //}";
                //                //new string("{ ArchiveOptions\":{ "CompressionLevel":1,"IsCompressEnable":true,"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"CryptingOptions":{ "IsEncryptEnable":true,"Key":"AQIDBAUGBwgJEBESExQVFg == ","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"LoggerOptions":{ "LogFile":"D:\\services\\LogFile.txt","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"WatcherOptions":{ "SourceDirectory":"C:\\ServerData\\archive\\",'TargetDirectory':'C:\\ClientData\\data\\','IsLoggerEnable':true},'DefaultOptions':{ 'SourceDirectory':'C:\\ServerData\\archive\\','TargetDirectory':'C:\\ClientData\\data\\','IsLoggerEnable':true},'SourceDirectory':'C:\\ServerData\\archive\\','TargetDirectory':'C:\\ClientData\\data\\','IsLoggerEnable':true}");
                //                //@"{ 'ArchiveOptions':\\{ 'CompressionLevel":1,"IsCompressEnable":true,"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"CryptingOptions":{ "IsEncryptEnable":true,"Key":"AQIDBAUGBwgJEBESExQVFg==","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"LoggerOptions":{ "LogFile":"D:\\services\\LogFile.txt","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"WatcherOptions":{ "SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"DefaultOptions":{ "SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true}"
                //                //jsonFile = String.Format("{"ArchiveOptions":{"CompressionLevel":1,"IsCompressEnable":true,"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"CryptingOptions":{"IsEncryptEnable":true,"Key":"AQIDBAUGBwgJEBESExQVFg == ","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"LoggerOptions":{"LogFile":"D:\\services\\LogFile.txt","SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"WatcherOptions":{"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"DefaultOptions":{"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true},"SourceDirectory":"C:\\ServerData\\archive\\","TargetDirectory":"C:\\ClientData\\data\\","IsLoggerEnable":true}");
                //                //Type jsons = json.GetType();
                //                //jsonString = JsonSerializer.Serialize(weatherForecast);
                //                //File.WriteAllText(fileName, jsonString);
                //                //string jsonString = File.ReadAllText(fileName);
                //                json = @"C:\Users\Asus\Desktop\Универ\3 Семестр. 2 Курс\asp.net.C#\LaboratoryProjects.С#\LP3\DataWatcherService\DataWatcherService\JsonData.json";
                //                //return JsonSerializer.Deserialize<EtlJsonOptions>(json);
                //                return JsonConvert.DeserializeObject<EtlJsonOptions>(json);

                /*
                var streamReader = new StreamReader(jsonFilePath);
                var inputJson = streamReader.ReadToEnd();
                streamReader.Close();
                var pattern0 = @"^{\s*
                                 ""(?<AllConfName>[^""]*)""\s*:\s*
                                 {\s*(?<Content>[\w\W]*)}\s*}$";
                var regexAllConfWithContent = new Regex(pattern0, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                var allConfName = regexAllConfWithContent.Match(inputJson).Groups["AllConfName"].Value;
                inputJson = regexAllConfWithContent.Match(inputJson).Groups["Content"].Value;

                var pattern1 = @"""(?<ClassName>[^""]*)""\s*:\s*
                                 \{(?<Content>[^}]*)\}";
                var regexClassNameWithContent = new Regex(pattern1, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

                var pattern2 = @"""(?<FieldName>[^""]*)""\s*:\s*[@""]*
                                    (?<Content>[^,""]*)\s*[@""]*";
                var regexDetailedContent = new Regex(pattern2, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                foreach (Match matchClassNameWithContent in regexClassNameWithContent.Matches(inputJson))
                {
                    var groupsClassNameWithContent = matchClassNameWithContent.Groups;
                    groupsClassNameWithContent[""].Value
                    var classBuilder = new CBuilder(groupsClassNameWithContent["ClassName"].Value);
                    var content = groupsClassNameWithContent["Content"].Value;
                    foreach (Match matchDetailedContent in regexDetailedContent.Matches(content))
                    {
                        var groupDetailedContent = matchDetailedContent.Groups;
                        var actualType = Utils.FigureOutType(groupDetailedContent["Content"].Value);
                        classBuilder.AddField(actualType,
                                              groupDetailedContent["FieldName"].Value,
                                              Convert.ChangeType(groupDetailedContent["Content"].Value, actualType));
                    }

                    JsonSettings.Add(classBuilder.CreateClass());
                }

                var classBuilder1 = new CBuilder(allConfName);
                foreach (var e in JsonSettings)
                {
                    classBuilder1.AddField(e.GetType(), e.Name, e);
                }
                JsonSettings.Add(classBuilder1.CreateClass());

                return JsonOptions;
                */



            }
            catch (FormatException e)
            {
                throw new ArgumentException("Path cannot be null or empty", e.Message);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format("Json file has wrong format: {0}", e.Message));
            }
        }

        public string GetJsonElement(string key)
        {
            return jsonDictionary[key];
        }
    }
}
