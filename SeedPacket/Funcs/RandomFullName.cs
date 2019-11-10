using SeedPacket.Interfaces;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomFullName(this IGenerator generator) // First-Initial LastName
        {
            string firstName    = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? GetElementRandom(generator, "FirstName") ?? $"FirstName{generator.RowNumber}";
            string middleName   = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? GetElementRandom(generator, "FirstName") ?? $"T";
            string lastName     = generator.CurrentRowValues.Get("LastName")?.ToString()  ?? GetElementRandom(generator, "LastName")  ?? $"LastName{generator.RowNumber}";

            return $"{firstName} {middleName.FirstOrDefault()}. {lastName}";
        }
    }
}
