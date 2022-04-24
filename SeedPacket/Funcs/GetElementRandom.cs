using SeedPacket.Interfaces;
using System;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        // Wrap parameter no longer used and will be removed.
        public static string GetElementRandom (this IGenerator generator, string identifier = null, bool nullIfEmpty = false, bool wrap = true)
        {
            // Get propertyName from generator to use if identfier not set
            string propertyName = identifier ?? generator.CustomName ?? generator?.CurrentProperty?.Name ?? "";
            string defaultValue = nullIfEmpty ? null : propertyName + generator.RowNumber.ToString();

            var elementList = generator.Datasource.GetElementList(propertyName);
            int index = generator.RowRandom.Next(elementList.Count);
            
            return elementList?.ElementInOrDefault(index) ?? defaultValue; 
        }

        public static dynamic GetElementRandom (this IGenerator generator, string identifier, TypeCode typeCode, bool wrap = true)
        {
            string element = generator.GetElementRandom(identifier, true, wrap);

            return Convert.ChangeType(element, typeCode);
        }
    }
}
