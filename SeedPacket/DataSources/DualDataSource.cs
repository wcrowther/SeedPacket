using SeedPacket.Enums;
using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeedPacket.DataSources
{
    public class DualDataSource : IDataSource
    {
        private IDataSource sourceData;
        private JsonDataSource jsonDataSource;
        private XmlDataSource xmlDataSource;
        private SeedInputType seedInputType;

        private string sourceFilePath;
        private string sourceString;

        public DualDataSource(string sourcefilepath = null, string sourcestring = null, SeedInputType seedinputtype = SeedInputType.Auto)
        {
            sourceFilePath = sourcefilepath;
            sourceString = sourcestring;
            seedInputType = seedinputtype;
            jsonDataSource = new JsonDataSource();
            xmlDataSource = new XmlDataSource();
            LoadSource();
        }

        private void LoadSource()
        {
            if (seedInputType == SeedInputType.JsonString)
            {
                jsonDataSource.Parse(sourceString);
                sourceData = jsonDataSource;
            }
            else if (seedInputType == SeedInputType.JsonFile)
            {
                jsonDataSource.Load(sourceFilePath);
                sourceData = jsonDataSource;
            }
            else if (seedInputType == SeedInputType.XmlString)
            {
                xmlDataSource.Parse(sourceString);
                sourceData = xmlDataSource;
            }
            else if (seedInputType == SeedInputType.XmlFile)
            {
                xmlDataSource.Load(sourceFilePath);
                sourceData = xmlDataSource;
            }
            else if (seedInputType == SeedInputType.Default)
            {
                xmlDataSource.LoadDefaultData();
                sourceData = xmlDataSource;
            }
            else  // if SeedInputType.Auto determine best option
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
                catch (InvalidSourceStringException ex)
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

        public List<T> GetElementList<T>(string identifier)
        {
            // TODO ADD CACHING OF LIST?
            return sourceData.GetElementList<T>(identifier);
        }
    }
}
