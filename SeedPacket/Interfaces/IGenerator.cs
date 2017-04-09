using System.Collections.Generic;
using SeedPacket.Generators;
using System;
using NewLibrary.ForType;
using SeedPacket.Interfaces;

namespace SeedPacket.Interfaces
{
    public interface IGenerator
    {
        IRules Rules { get; }

        // Seed Range
        int SeedBegin { get; set; }
        int SeedEnd { get; set; }

        // Base 
        Random BaseRandom { get; }
        DateTime BaseDateTime { get; }
        bool Debugging { get; set; }

        // Cache (dynamic)
        dynamic Cache { get; set; }

        // DataSource (dynamic)
        IDataSource Datasource { get;  }

        // Row Info
        int RowNumber { get; set; }
        int RowRandomNumber { get; }
        Random RowRandom { get; }
        int RowCount { get; }
        Dictionary<string, object> CurrentRowValues { get; }
        void GetNextRowRandom ();

        // Current CurrentProperty
        MetaProperty CurrentProperty { get; set; }
    }
}
