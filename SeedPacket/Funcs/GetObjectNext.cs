using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static T GetObjectNext<T> (this IGenerator generator, string identifier = null, int offset = 0) where T : class, new()
        {
            // Get propertyName from generator to use if identfier not set
            var propertyName = identifier ?? generator.CustomName ?? generator?.CurrentProperty?.Name ?? "";

            // Will loop back to beginning if rownumber is greater than number of elements in list
            List<T> objects = generator.Datasource.GetObjectList<T>(propertyName);

            if (objects.Count == 0)
                return null;

            // By default goes to first record in 0-based list. Offset allows to start offset from the row.
            //  Will always add 1 for each row so it remains sequential. Will loop back if past the end.
            int rowNumberWithOffset = (generator.RowNumber - 1) + offset;
            int mod = (rowNumberWithOffset) % objects.Count;
            int position = mod;

            return objects?.ElementAtOrDefault(position);
        }
    }
}
