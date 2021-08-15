using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static T GetObjectRandom<T> (this IGenerator generator, string identifier = null, bool nullIfEmpty = false) where T : class, new()
        {

            // Get propertyName from generator to use if identfier not set
            var propertyName = identifier ?? generator.CustomName ?? generator?.CurrentProperty?.Name ?? "";

            T defaultValue = default(T);

            var elementList = generator.Datasource.GetObjectList<T>(propertyName);
            int index = generator.RowRandom.Next(elementList.Count);
            T element = elementList?.ElementAtOrDefault(index) ?? defaultValue;

            return element;
        }
    }
}
