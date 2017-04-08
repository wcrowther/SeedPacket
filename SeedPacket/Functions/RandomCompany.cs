using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomCompany (IGenerator generator)
        {
            return func.ElementRandom(generator, "CompanyName") + func.ElementRandom(generator, "CompanySuffix") ?? "CompanyName" + generator.RowNumber;
        }
    }
}
