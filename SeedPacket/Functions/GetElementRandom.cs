using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string GetElementRandom (IGenerator generator, string identifier = null, bool nullIfEmpty = false)
        {
            // Get propertyName from generator to use if identfier not set
            var propertyName = identifier ?? generator.CustomPropertyName ?? generator?.CurrentProperty?.Name ?? "";
            string defaultValue = nullIfEmpty ? null : propertyName + generator.RowNumber.ToString();

            var elementList = generator.Datasource.GetElementList(propertyName);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);
            string element = elementList?.ElementAtOrDefault(index) ?? defaultValue;

            return element;
        }

        public static dynamic GetElementRandom (IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = generator.Datasource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }
    }
}
