using SeedPacket.Exceptions;
using SeedPacket.Extensions;
using SeedPacket.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using WildHare.Extensions;

namespace SeedPacket.DataSources
{
    public class XmlDataSource : IDataSource
    {
        private XDocument sourceData;
        private readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

        public XmlDataSource()
        {
            LoadDefaultData();
        }

        public void Parse(string xml, string source = null)
        {
            try
            {
                sourceData = XDocument.Parse(xml);
            }
            catch
            {
                throw new InvalidSourceException("xml", source);
            }
        }

        public void Load(string sourceFilePath)
        {
            string xmlstring;

            if (sourceFilePath.StartsWith("~"))
            {
                throw new InvalidTildePathException(sourceFilePath);
            };

            try
            {
                xmlstring = File.ReadAllText(sourceFilePath);
            }
            catch
            {
                throw new InvalidFilePathException(sourceFilePath);
            }

            Parse(xmlstring, "file");
        }

        public void LoadDefaultData()
        {
            try
            {
                // Gets embedded xml file Update 'Build Action' property to 'embedded Resource'
                Assembly a = Assembly.GetExecutingAssembly();
                using (var xmlStream = a.GetManifestResourceStream(GetDefaultXmlResource()))
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
            if (sourceData != null && !identifier.IsNullOrEmpty())
            {
                return sourceData.Descendants(identifier).Select(x => (string)x).ToList();
            }
            return new List<string>();
        }

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            if (sourceData != null && !identifier.IsNullOrEmpty())
            {
                var list = sourceData.Descendants(identifier)
                    .Select(p => p.ToObject<T>())
                    .ToList();

                return list;
            }
            return new List<T>();
        }

        private string GetDefaultXmlResource()
        {
            string sourceName;
            switch (currentCulture.Name)
            {
                // Potentially other languages as
                // case "en-GB":  sourceName = "SeedPacket.Source.JsonGeneratorSource.json"; break;

                default: sourceName = "SeedPacket.Source.XmlGeneratorSource.xml"; break;  // "en-US"
            }
            return sourceName;
        }
    }
}
