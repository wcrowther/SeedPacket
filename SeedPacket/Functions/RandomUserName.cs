using SeedPacket.Interfaces;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomUserName (IGenerator generator) // First-Initial LastName
        {
            string firstName    = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? Funcs.ElementRandom(generator, "FirstName") ?? "F";
            string lastName     = generator.CurrentRowValues.Get("LastName")?.ToString() ?? Funcs.ElementRandom(generator, "LastName") ?? $"LastName{generator.RowNumber}";

            return $"{firstName.FirstOrDefault()}{lastName}";
        }
    }
}
