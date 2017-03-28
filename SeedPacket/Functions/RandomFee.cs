using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static decimal RandomFee (IGenerator generator)
        {
            decimal fee = generator.RowRandom.Next(1, 200);
            decimal dec = generator.RowRandom.Next(0, 100);
            return fee + (dec * .01M);
        }
    }
}
