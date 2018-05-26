﻿using NewLibrary.ForString;
using SeedPacket.Exceptions;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            if (sourceData != null && !identifier.isNullOrEmpty())
            {
                var list = sourceData.Descendants(identifier)
                    .Select(p => p.ToObject<T>())
                    .ToList();

                return list;
            }
            return new List<T>();
        }
    }

    public static class XmlDataSourceExtensions
    {
        public static T ToObject<T>(this XElement element) where T : class, new()
        {
            T instance = new T();
            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var xattribute = element.Attribute(property.Name);
                var xelement = element.Element(property.Name);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (xattribute == null && xelement == null)
                {
                    string message = $"Unable to parse XML value. Object '{element.Name}' does not contain either an attribute or an element called '{property.Name}'.";
                    throw new Exception(message);
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
                catch // (Exception ex) // If Error let the value remain default for that property type
                {
                    Console.WriteLine("Not able to parse XML value " + value + " for type '" + property.PropertyType + "' for property " + property.Name);
                }
            }

            if (instance == null)
                throw new Exception($"Not able to parse XML {element.Name}");

            return instance;
        }
    }
}
