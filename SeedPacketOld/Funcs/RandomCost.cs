using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static decimal RandomCost (this IGenerator generator)
        {
            decimal cost = generator.RowRandom.Next(0, 1000);
            decimal dec  = generator.RowRandom.Next(0, 100);
            return cost + (dec * .01M);
        }
    }
}
