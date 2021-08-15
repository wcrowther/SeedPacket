using Newtonsoft.Json.Linq;
using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SeedPacket.DataSources
{
    public class JsonDataSource : IDataSource
    {
        private JObject jsonData;
        private readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        private readonly ListOfList<string, string> Data = new ListOfList<string, string>(); // For change

        public JsonDataSource()
        {
            LoadDefaultData();
        }

        public void Parse(string json, string source = null)
        {
            try
            {
                jsonData = JObject.Parse(json);
            }
            catch
            {
                throw new InvalidSourceException("json", source);
            }
        }

        public void Load(string sourceFilePath)
        {
            string jsonString;

            if (sourceFilePath.StartsWith("~"))
            {
                throw new InvalidTildePathException(sourceFilePath);
            };
 
            try
            {
                jsonString = File.ReadAllText(sourceFilePath);
            }
            catch
            {
                throw new InvalidFilePathException(sourceFilePath);
            }

            Parse(jsonString, "file");
        }

        public void LoadDefaultData()
        {
            try
            {
                // Gets embedded json file Update 'Build Action' property to 'embedded Resource'
                Assembly a = Assembly.GetExecutingAssembly();

                using (var stream = a.GetManifestResourceStream(GetDefaultJsonResource()))
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

        private string GetDefaultJsonResource()
        {
            string sourceName;
            switch (currentCulture.Name)
            {
                case "en-US": sourceName = "SeedPacket.Source.JsonGeneratorSource.json"; break;

                // Potentially other languages as
                // case "en-GB":  sourceName = "SeedPacket.Source.JsonGeneratorSource.json"; break;

                default: sourceName = "SeedPacket.Source.JsonGeneratorSource.json"; break;
            }
            return sourceName;
        }
    }
}
