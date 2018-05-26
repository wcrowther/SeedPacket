using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SeedPacket.Exceptions;
using System.Linq;
using System.IO;
using NewLibrary.ForString;

namespace SeedPacket.DataSources
{
    public class JsonDataSource : IDataSource
    {
        private JObject jsonData;
        private const string defaultJson = "SeedPacket.Source.JsonGeneratorSource.json";

        public JsonDataSource()
        {
            LoadDefaultData();
        }

        public void Parse(string json)
        {
            try
            {
                jsonData = JObject.Parse(json);
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

        public void LoadDefaultData()
        {
            try
            {
                // Gets embedded json file Update 'Build Action' property to 'embedded Resource'
                System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();

                using (var stream = a.GetManifestResourceStream(defaultJson))
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Parse(result);
                }
            }
            catch
            {
                throw new InvalidDefaultDataException();
            }
        }

        public List<string> GetElementList(string identifier)
        {
            if (jsonData != null)
            {
                return jsonData
                    .SelectToken($"..{identifier}")
                    ?.Select(p => p.Value<string>())
                    ?.ToList() ?? new List<string>();
            }
            return new List<string>();
        }

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            if (jsonData != null)
            {
                var list = jsonData.SelectToken($"..{identifier}")
                    .Select(p => p.ToObject<T>())
                    .ToList() ?? new List<T>();

                int parsingErrors = list.Count(l => l == null);
                if (parsingErrors > 0)
                {
                    throw new Exception($"Error parsing {parsingErrors} objects with identifier '{identifier}'."); 
                }

                return list;
            }
            return new List<T>();
        }
    }
}
