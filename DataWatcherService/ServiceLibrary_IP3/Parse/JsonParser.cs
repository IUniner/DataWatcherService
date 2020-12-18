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
