using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static bool OnceEvery (IGenerator generator, int count = 10)
        {
            return generator.RowNumber % count == 0;
        }
    }
}
