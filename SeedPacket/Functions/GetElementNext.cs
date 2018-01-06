using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string GetElementNext (IGenerator generator, string identifier = null, int offset = 0)
        {
            var propertyName = generator.CurrentPropertyName ?? "";

            // Will loop back to beginning if rownumber is greater than number of elements in list
            List<string> strings = generator.Datasource.GetElementList(identifier ?? propertyName);
            int count = strings.Count;
            if (count == 0)
                return null;

            // By default goes to first record in 0-based list. Offset allows to start offset from the row.
            //  Will always add 1 for each row so it remains sequential. Will loop back if past the end.
            int rowNumberWithOffset = (generator.RowNumber - 1) + offset;
            int mod = (rowNumberWithOffset) % count;
            int position = mod;

            return strings?.ElementAtOrDefault(position);
        }
    }
}
