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
                try
                {
                    jsonDataSource.Load(sourceFilePath);
                    sourceData = jsonDataSource;
                }
                catch (InvalidSourceFileException)
                {
                    try
                    {
                        xmlDataSource.Load(sourceFilePath);
                        sourceData = xmlDataSource;
                    }
                    catch
                    {
                        throw new InvalidSourceFileException("XML", sourceFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw new InvalidSourceFileException(filepath: sourceFilePath);
                }
            }
            else if (sourceString != null)
            {
                try
                {
                    jsonDataSource.Parse(sourceString);
                    sourceData = jsonDataSource;
                }
                catch (InvalidSourceStringException)
                {
                    try
                    {
                        xmlDataSource.Parse(sourceString);
                        sourceData = xmlDataSource;
                    }
                    catch
                    {
                        throw new InvalidSourceStringException();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw new InvalidSourceStringException();
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
            // TODO ADD CACHING OF LIST?
            return sourceData.GetElementList(identifier);
        }
    }
}
