using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string ElementRandom (IGenerator generator, string identifier = null, bool nullIfEmpty = false)
        {
            // Set default in case it is needed
            var propertyName = generator.CurrentPropertyName ?? "";
            string defaultValue = nullIfEmpty ? null : propertyName + generator.RowNumber.ToString();

            var elementList = generator.Datasource.GetElementList(identifier ?? propertyName);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);
            string element = elementList?.ElementAtOrDefault(index) ?? defaultValue;

            return element;
        }

        public static dynamic ElementRandom (IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = generator.Datasource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }
    }
}
