using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;

namespace SeedPacket.DataSources
{
    public class MultiDataSource : IDataSource
    {
        private IDataSource dataSource;
        private readonly DataInputType dataInputType;

        private readonly string sourceFilePath;
        private readonly string sourceString;

        public MultiDataSource( string sourcefilepath = null, string sourcestring = null,
                                DataInputType datainputtype = DataInputType.Auto)
        {
            sourceFilePath = sourcefilepath;
            sourceString = sourcestring;
            dataInputType = datainputtype;
            LoadSource();
        }

        private void LoadSource()
        {
            if (dataInputType == DataInputType.JsonString)
            {
                dataSource = GetDataSource("json");
                dataSource.Parse(sourceString);
            }
            else if (dataInputType == DataInputType.JsonFile)
            {
                dataSource = GetDataSource("json");
                dataSource.Load(sourceFilePath);
            }
            else if (dataInputType == DataInputType.XmlString)
            {
                dataSource = GetDataSource("xml");
                dataSource.Parse(sourceString);
            }
            else if (dataInputType == DataInputType.XmlFile)
            {
                dataSource = GetDataSource("xml");
                dataSource.Load(sourceFilePath);
            }
            else if (dataInputType == DataInputType.CsvString)
            {
                dataSource = GetDataSource("csv");
                dataSource.Parse(sourceString);
            }
            else if (dataInputType == DataInputType.CsvFile)
            {
                dataSource = GetDataSource("csv");
                dataSource.Load(sourceFilePath);
            }
            else if (dataInputType == DataInputType.Default)
            {
                dataSource = GetDataSource("xml");
                dataSource.LoadDefaultData();
            }
            else  // if DataInputType.Auto determine best option between JSON and XML
            {
                AutoSource();
            }
        }

        public void Parse(string str, string source = null)
        {
            throw new NotImplementedException("MultiDataSource does not implement 'Parse'. Pass in a dataSource string using the 'sourcestring' parameter in the constructor.");
        }

        public void Load(string sourceFilePath)
        {
            throw new NotImplementedException("MultiDataSource does not implement 'Load'. Pass in a dataSource filename using the 'sourcefilepath' parameter in the constructor.");
        }

        public void LoadDefaultData()
        {
            dataSource = new XmlDataSource();
            dataSource.LoadDefaultData();
        }

        public List<string> GetElementList(string identifier)
        {
            return dataSource.GetElementList(identifier);
        }

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            return dataSource.GetObjectList<T>(identifier);
        }

        // PRIVATE METHODS ======================================

        // AutoSource only works with Json & Xml. CSV must be directly selected.
        private void AutoSource()
        {
            if (sourceFilePath != null && sourceString != null)
            {
                throw new MultipleSourceException();
            }
            else if (!sourceFilePath.IsNullOrSpace())
            {
                if (sourceFilePath.StartsWith("~"))
                {
                    throw new InvalidTildePathException(sourceFilePath);
                };

                string fileExtension = Path.GetExtension(sourceFilePath);
                dataSource = GetDataSource(fileExtension);

                if (dataSource != null)
                {
                    dataSource.Load(sourceFilePath);
                }
                else
                {
                    throw new AutoSourceException(fileExtension);
                }

            }
            else if (sourceString != null)
            {
                try
                {
                    dataSource = GetDataSource("json");
                    dataSource.Parse(sourceString);
                }
                catch (InvalidSourceException)
                {
                    try
                    {
                        dataSource = GetDataSource("xml");
                        dataSource.Parse(sourceString);
                    }
                    catch
                    {
                       throw new InvalidSourceException();
                    }
                }

            }
            else
            {
               LoadDefaultData();
            }
        }

        private IDataSource GetDataSource(string dataSourceType)
        {
            string sourceType = dataSourceType.RemoveStart(".").ToLower();

            if (sourceType == "json")
            {
                return new JsonDataSource();
            }
            else if (sourceType == "xml")
            {
                return new XmlDataSource();
            }
            else if (sourceType == "csv")
            {
                return new CsvDataSource();
            }
            else
            {
                return null;
            }
        }

    }
}
