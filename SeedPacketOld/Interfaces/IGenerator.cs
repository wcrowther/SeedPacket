﻿using System.Collections.Generic;
using SeedPacket.Generators;
using System;

using SeedPacket.Interfaces;
using WildHare.Models;

namespace SeedPacket.Interfaces
{
    public interface IGenerator
    {
        IRules Rules { get; }

        // Seed Range
        int SeedBegin { get; set; }

        int SeedEnd { get; set; }

        // Base 
        Random BaseRandom { get; set; }

        DateTime BaseDateTime { get; set; }

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

        // Use this to set rule name of single element like list<string>
        string CustomName { get; set; }
    }
}