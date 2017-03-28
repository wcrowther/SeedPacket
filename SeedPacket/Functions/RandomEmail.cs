using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        public static string RandomEmail (IGenerator generator)
        {
            string userName         = generator?.CurrentRowValues.Get("UserName")?.ToString() ?? RandomUserName(generator);
            string fullCompanyName  = generator?.CurrentRowValues.Get("CompanyName")?.ToString() ?? RandomCompany(generator).Replace(" ", "");
            string domain           = generator?.CurrentRowValues.Get("DomainExtension")?.ToString() ?? ".com";

            return $"{userName}@{fullCompanyName}{domain}".ToLower();
        }
    }
}
