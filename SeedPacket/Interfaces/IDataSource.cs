using System;
using System.Collections.Generic;

namespace SeedPacket.Interfaces
{
    public interface IDataSource 
    {
        List<string> GetElementList(string identifier);

        List<T> GetObjectList<T>(string identifier) where T : class, new();
    }
} 

 
