using System;
using System.Collections.Generic;

namespace SeedPacket.Interfaces
{
    public interface IDataSource 
    {
        List<T> GetElementList<T>(string identifier);
    }
} 

 
