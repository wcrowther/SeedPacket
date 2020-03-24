
using SeedPacket.Exceptions;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WildHare.Extensions;

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
    }

    // TODO  - MOVE TO WILDHARE EXTENSIONS
    public static class XmlDataSourceExtensions
    {
        public static T ToObject<T>(this XElement element) where T : class, new()
        {
            var instance = new T();
            var typeProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); //.GetMetaProperties();

            foreach (var property in typeProperties)
            {
                var xattribute = element.Attribute(property.Name);
                var xelement = element.Element(property.Name);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (xattribute == null && xelement == null)
                {
                    if (property.CustomAttributes.Where(a => a.AttributeType.Name == "XmlIgnoreAttribute").Count() == 0)
                    {
                        string message = $"Unable to parse XML value. Object '{element.Name}' does not contain either an attribute or an element called '{property.Name}'.";
                        Debug.WriteLine(message);
                    }
                    continue;
                }

                var value = xattribute?.Value ?? xelement.Value;
                try
                {
                    if (value != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(instance, Convert.ChangeType(value, propertyType));
                        }
                    }
                }
                catch (Exception ex) // If Error let the value remain default for that property type
                {
                    Debug.WriteLine( $"Not able to parse XML value {value} for type '{property.PropertyType}' " +
                                     $"for property {property.Name}. Ex: {ex.Message}" );
                }
            }

            if (instance == null)
                throw new Exception($"Not able to parse XML {element.Name}");

            return instance;
        }
    }
}
