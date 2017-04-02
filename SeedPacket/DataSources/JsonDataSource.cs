using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SeedPacket.Exceptions;
using System.Linq;
using System.IO;
using NewLibrary.ForString;
using Newtonsoft.Json;
using System.Reflection;

namespace SeedPacket.DataSources
{
    public class JsonDataSource : IDataSource
    {
        private JObject sourceData;
        private const string defaultJson = "SeedPacket.Source.JsonGeneratorSource.json";
        private bool appendToDefaultData;


        public JsonDataSource (bool appendtodefaultdata = false)
        {
            appendToDefaultData = appendtodefaultdata;
        }

        public void Parse(string json)
        {
            try
            {
                if (appendToDefaultData)
                {
                    var settings = new JsonMergeSettings {
                        MergeArrayHandling= MergeArrayHandling.Concat
                    };
                    LoadDefaultData();
                    var additionalJson = JObject.Parse(json);
                    sourceData.Merge(additionalJson, settings);
                }
                else
                {
                    sourceData = JObject.Parse(json);
                }
            }
            catch
            {
                throw new InvalidSourceStringException("json");
            }
        }

        public void Load(string sourceFilePath)
        {
            try
            {
                string pathToFile = sourceFilePath.StartsWith("~") ? sourceFilePath.ToMapPath() : sourceFilePath;
                string json = File.ReadAllText(pathToFile);
                Parse(json);
            }
            catch
            {
                throw new InvalidSourceFileException("json");
            }
        }

        public void LoadDefaultData ()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(defaultJson))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonString = reader.ReadToEnd();
                    sourceData = JObject.Parse(jsonString);
                }
            }
            catch
            {
                throw new InvalidDefaultDataException();
            }
        }

        public List<string> GetElementList(string identifier)
        {
            if (sourceData != null)
            {
                return sourceData
                    .SelectToken($"..{identifier}")
                    ?.Select(p => p.Value<string>())
                    ?.ToList() ?? new List<string>();
            }
            return new List<string>();
        }
    }
}
