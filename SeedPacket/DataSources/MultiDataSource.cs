using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeedPacket.DataSources
{
    public class MultiDataSource : IDataSource
    {
        private IDataSource sourceData;
        private JsonDataSource jsonDataSource;
        private XmlDataSource xmlDataSource;
        private DataInputType dataInputType;

        private string sourceFilePath;
        private string sourceString;

        public MultiDataSource(string sourcefilepath = null, string sourcestring = null, DataInputType datainputtype = DataInputType.Auto)
        {
            sourceFilePath = sourcefilepath;
            sourceString = sourcestring;
            dataInputType = datainputtype;
            jsonDataSource = new JsonDataSource();
            xmlDataSource = new XmlDataSource();
            LoadSource();
        }

        private void LoadSource()
        {
            if (dataInputType == DataInputType.JsonString)
            {
                jsonDataSource.Parse(sourceString);
                sourceData = jsonDataSource;
            }
            else if (dataInputType == DataInputType.JsonFile)
            {
                jsonDataSource.Load(sourceFilePath);
                sourceData = jsonDataSource;
            }
            else if (dataInputType == DataInputType.XmlString)
            {
                xmlDataSource.Parse(sourceString);
                sourceData = xmlDataSource;
            }
            else if (dataInputType == DataInputType.XmlFile)
            {
                xmlDataSource.Load(sourceFilePath);
                sourceData = xmlDataSource;
            }
            else if (dataInputType == DataInputType.Default)
            {
                xmlDataSource.LoadDefaultData();
                sourceData = xmlDataSource;
            }
            else  // if DataInputType.Auto determine best option
            {
                AutoSource();
            }
        }

        private void AutoSource()
        {
            if (sourceFilePath != null && sourceString != null)
            {
                throw new MultipleSourceException();
            }
            else if (!string.IsNullOrWhiteSpace(sourceFilePath))
            {
                if (sourceFilePath.StartsWith("~"))
                {
                    throw new InvalidTildePathException(sourceFilePath);
                };

                try
                {
                    jsonDataSource.Load(sourceFilePath);
                    sourceData = jsonDataSource;
                }
                catch (InvalidSourceException)
                {
                    // If no valid JSON file try XML
                    try
                    {
                        xmlDataSource.Load(sourceFilePath);
                        sourceData = xmlDataSource;
                    }
                    catch
                    {
                        throw new InvalidSourceException("", sourceFilePath);
                    }
                }
            }
            else if (sourceString != null)
            {
                try
                {
                    jsonDataSource.Parse(sourceString);
                    sourceData = jsonDataSource;
                }
                catch (InvalidSourceException)
                {
                    try
                    {
                        xmlDataSource.Parse(sourceString);
                        sourceData = xmlDataSource;
                    }
                    catch
                    {
                        throw new InvalidSourceException();
                    }
                }
            }
            else
            {
                xmlDataSource.LoadDefaultData();
                sourceData = xmlDataSource;
            }
        }

        public List<string> GetElementList(string identifier)
        {
            return sourceData.GetElementList(identifier);
        }

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            return sourceData.GetObjectList<T>(identifier);
        }
    }
}
