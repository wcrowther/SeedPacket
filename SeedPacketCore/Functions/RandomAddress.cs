using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomAddress (IGenerator generator)
        {
            int streetNumber = generator.RowRandom.Next(1, 10000);
            string streetName = Funcs.GetElementRandom(generator, "StreetName") ?? "StreetName" + generator.RowNumber ;
            string roadType = Funcs.GetElementRandom(generator, "RoadType");

            return $"{streetNumber} {streetName} {roadType}";
        }
    }
}
