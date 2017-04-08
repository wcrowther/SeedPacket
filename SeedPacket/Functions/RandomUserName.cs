using SeedPacket.Interfaces;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomUserName (IGenerator generator) // First-Initial LastName
        {
            string firstName    = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? func.ElementRandom(generator, "FirstName") ?? "F";
            string lastName     = generator.CurrentRowValues.Get("LastName")?.ToString() ?? func.ElementRandom(generator, "LastName") ?? $"LastName{generator.RowNumber}";

            return $"{firstName.FirstOrDefault()}{lastName}";
        }
    }
}
