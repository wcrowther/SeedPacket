﻿using System.Collections.Generic;
using SeedPacket.Generators;
using System;
using NewLibrary.ForType;
using SeedPacket.Interfaces;

namespace SeedPacket
{
    public interface IGenerator
    {
        Rules Rules { get; }

        // Base 
        DateTime BaseDateTime { get; }
        bool Debug { get; set; }

        // Row Info
        int RowNumber { get; set; }
        int RowRandomNumber { get; }
        Random RowRandom { get; }
        Dictionary<string, object> CurrentRowValues { get; }
        void GetNextRowRandom ();

        // Current Property
        MetaProperty Property { get; set; }
    }
}
