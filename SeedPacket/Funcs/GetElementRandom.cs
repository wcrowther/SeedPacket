using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string GetElementRandom (this IGenerator generator, string identifier = null, bool nullIfEmpty = false)
        {
            // Get propertyName from generator to use if identfier not set
            string propertyName = identifier ?? generator.CustomName ?? generator?.CurrentProperty?.Name ?? "";
            string defaultValue = nullIfEmpty ? null : propertyName + generator.RowNumber.ToString();

            var elementList = generator.Datasource.GetElementList(propertyName);
            int index = generator.RowRandom.Next(elementList.Count);
            string element = elementList?.ElementAtOrDefault(index) ?? defaultValue;

            return element;
        }

        public static dynamic GetElementRandom (this IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = generator.Datasource.GetElementList(identifier);
            int index = generator.RowRandom.Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }
    }
}
