using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomCompany (IGenerator generator)
        {
            return func.RandomElement(generator, "CompanyName") + func.RandomElement(generator, "CompanySuffix") ?? "CompanyName" + generator.RowNumber;
        }
    }
}
