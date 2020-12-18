using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceLibrary_IP3
{
    public class XmlParser
    {
        private readonly Dictionary<string, string> xmlDictionary;
        static EtlXmlOptions Options;

        public XmlParser()
        {
            xmlDictionary = new Dictionary<string, string>();

        }
        public Dictionary<string, string> Parse(string xmlFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(xmlFilePath))
                {
                    throw new ArgumentException($"'{nameof(xmlFilePath)}' cannot be null or empty", nameof(xmlFilePath));
                }
                //XmlSerializer formatter = new XmlSerializer(typeof(EtlXmlOptions));
                //using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
                //{
                //    Options = (EtlXmlOptions)formatter.Deserialize(fs);
                //}               

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);

                foreach (XmlNode node in doc.DocumentElement)
                {
                    xmlDictionary.Add(node.Name, node.InnerText);
                }
                return xmlDictionary;
            }
            catch (FormatException e)
            {
                throw new ArgumentException("Path cannot be null or empty", e.Message);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format("Xml file has wrong format: {0}", e.Message));
            }
        }

        public string GetXmlElement(string key)
        {
            return xmlDictionary[key];
        }
    }
}
