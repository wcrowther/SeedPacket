using NewLibrary.ForString;
using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SeedPacket.DataSources
{
    public class XmlDataSource : IDataSource
    {
        private XDocument sourceData;
        private const string defaultXml = "SeedPacket.Source.XmlGeneratorSource.xml";

        public XmlDataSource()
        {
            LoadDefaultData();
        }

        public void Parse(string xml)
        {
            try
            {
                sourceData = XDocument.Parse(xml); 
            }
            catch
            {
                throw new InvalidSourceStringException("xml");
            }
        }

        public void Load(string sourceFilePath)
        {
            string pathToFile = null;
            try
            {
                pathToFile = sourceFilePath.StartsWith("~") ? sourceFilePath.ToMapPath() : sourceFilePath;
                string xml = File.ReadAllText(pathToFile);
                Parse(xml);
            }
            catch
            {
                throw new InvalidSourceFileException("xml", pathToFile);
            }
        }

        public void LoadDefaultData()
        {
            try
            {
                // Gets embedded xml file Update 'Build Action' property to 'embedded Resource'
                System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
                using (var xmlStream = a.GetManifestResourceStream(defaultXml))
                { 
                    sourceData = XDocument.Load(xmlStream);
                }
            }
            catch
            {
                throw new InvalidDefaultDataException();
            }
        }

        public List<string> GetElementList(string identifier)
        {
            if (sourceData != null && !identifier.isNullOrEmpty())
            {
                return sourceData.Descendants(identifier).Select(x => (string)x).ToList();
            }
            return new List<string>();
        }
    }
}
