using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string ElementNext (IGenerator generator, string identifier)
        {
            // Will loop back to beginning if rownumber is greater than number of elements in list

            List<string> strings = generator.Datasource.GetElementList(identifier);
            int count = strings.Count;
            if (count == 0)
                return "";

            int mod = (generator.RowNumber - 1) % count;
            int position = mod;

            return strings?.ElementAtOrDefault(position) ?? "";
        }
    }
}
