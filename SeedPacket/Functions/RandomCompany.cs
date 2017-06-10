using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomCompany (IGenerator generator)
        {
            return Funcs.GetElementRandom(generator, "CompanyName") + " " + Funcs.GetElementRandom(generator, "CompanySuffix") ?? "Inc";
        }
    }
}
