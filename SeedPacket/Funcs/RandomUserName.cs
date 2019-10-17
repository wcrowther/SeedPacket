using SeedPacket.Interfaces;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomUserName (this IGenerator generator) // First-Initial LastName
        {
            string firstName    = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? GetElementRandom(generator, "FirstName") ?? "F";
            string lastName     = generator.CurrentRowValues.Get("LastName")?.ToString()  ?? GetElementRandom(generator, "LastName") ?? $"LastName{generator.RowNumber}";

            return $"{firstName.FirstOrDefault()}{lastName}";
        }
    }
}
