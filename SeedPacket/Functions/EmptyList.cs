using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static IEnumerable<object> EmptyList(IGenerator generator)
        {
            dynamic emptyList = Activator.CreateInstance(generator.CurrentProperty.PropertyType);

            return emptyList;
        }
    }
}
