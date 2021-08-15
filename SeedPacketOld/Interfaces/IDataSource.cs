using System;
using System.Collections.Generic;

namespace SeedPacket.Interfaces
{
    public interface IDataSource 
    {
        void Parse(string json, string source = null);

        void Load(string sourceFilePath);

        void LoadDefaultData();

        List<string> GetElementList(string identifier);

        List<T> GetObjectList<T>(string identifier) where T : class, new();
    }
} 

 
