using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomCompany (IGenerator generator)
        {
            return Funcs.ElementRandom(generator, "CompanyName") + Funcs.ElementRandom(generator, "CompanySuffix") ?? "CompanyName" + generator.RowNumber;
        }
    }
}
