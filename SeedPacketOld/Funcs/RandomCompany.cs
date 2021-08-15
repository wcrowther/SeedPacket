using SeedPacket.Interfaces;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomCompany (this IGenerator generator)
        {
            return GetElementRandom(generator, "CompanyName") + GetElementRandom(generator, "CompanySuffix").AddStart(" ");
        }
    }
}
