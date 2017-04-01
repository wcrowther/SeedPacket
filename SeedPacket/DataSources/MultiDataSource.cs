using SeedPacket.Enums;
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
        private SeedInputType seedInputType;

        private string sourceFilePath;
        private string sourceString;
        private bool appendToDefault;

        public MultiDataSource(string sourcefilepath = null, string sourcestring = null, SeedInputType seedinputtype = SeedInputType.Auto)
        {
            sourceFilePath = sourcefilepath;
            sourceString = sourcestring;
            seedInputType = seedinputtype;
            appendToDefault = true; 

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
                xmlDataSource.Parse(sourceString, appendToDefault);
                sourceData = xmlDataSource;
            }
            else if (seedInputType == SeedInputType.XmlFile)
            {
                xmlDataSource.Load(sourceFilePath, appendToDefault);
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
                        xmlDataSource.Load(sourceFilePath, appendToDefault);
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
                        xmlDataSource.Parse(sourceString, appendToDefault);
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
            return sourceData.GetElementList(identifier);
        }
    }
}
