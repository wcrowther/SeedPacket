using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomAddress (IGenerator generator)
        {
            int streetNumber = generator.RowRandom.Next(1, 10000);
            string streetName = func.RandomElement(generator, "StreetName") ?? "StreetName" + generator.RowNumber ;
            string roadType = func.RandomElement(generator, "RoadTypes");

            return $"{streetNumber} {streetName} {roadType}";
        }
    }
}
