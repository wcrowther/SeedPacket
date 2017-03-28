using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomElement (IGenerator generator, string identifier = null)
        {
            // Set default in case it is needed
            var propertyName = generator.CurrentProperty?.Name ?? "";
            string defaultValue = propertyName + generator.RowNumber.ToString();

            var elementList = generator.Datasource.GetElementList(identifier ?? propertyName);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);
            string element = elementList?.ElementAtOrDefault(index) ?? defaultValue;

            return element;
        }

        public static dynamic RandomElement (IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = generator.Datasource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }
    }
}
