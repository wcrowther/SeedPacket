using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomZip (IGenerator generator)
        {
            return generator.RowRandom.Next(10001, 100000).ToString();
        }
    }
}
