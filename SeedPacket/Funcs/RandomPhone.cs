using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomPhone (IGenerator generator)
        {
            int r1 = generator.RowRandom.Next(100, 1000);
            int r2 = generator.RowRandom.Next(100, 1000);
            int r3 = generator.RowRandom.Next(1000, 10000);

            return $"({r1}) {r2}-{r3}";
        }
    }
}
