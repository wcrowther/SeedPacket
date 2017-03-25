using System.Collections.Generic;
using SeedPacket.Generators;
using System;
using NewLibrary.ForType;
using SeedPacket.Interfaces;

namespace SeedPacket.Interfaces
{
    public interface IGenerator
    {
        Rules Rules { get; }

        // Seed Range
        int SeedBegin { get; set; }
        int SeedEnd { get; set; }

        // Base 
        Random BaseRandom { get; }
        DateTime BaseDateTime { get; }
        bool Debugging { get; set; }

        // Cache (dynamic)
        dynamic Cache { get; set; }

        // Row Info
        int RowNumber { get; set; }
        int RowRandomNumber { get; }
        Random RowRandom { get; }
        Dictionary<string, object> CurrentRowValues { get; }
        int RowCount { get; }
        void GetNextRowRandom ();

        // Current CurrentProperty
        MetaProperty CurrentProperty { get; set; }
    }
}
