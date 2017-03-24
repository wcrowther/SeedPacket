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
    }
}
