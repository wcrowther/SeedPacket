using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;
using SeedPacket.DataSources;
using NewLibrary.ForString;
using SeedPacket.Enums;

namespace SeedPacket.Generators
{
    public class MultiGenerator : Generator, IGenerator 
    {
        protected IDataSource dataSource; 
        protected RulesSet rulesSet;

        public MultiGenerator(  string sourceFilepath = null, string sourceString = null, 
                                RulesSet rulesSet = RulesSet.Advanced,
                                DateTime? BaseDateTime = null, Random BaseRandom = null,
                                SeedInputType seedInputType = SeedInputType.Auto )
            : base( BaseDateTime, BaseRandom )
        {
            dataSource = new MultiDataSource(sourceFilepath, sourceString, seedInputType);
            this.rulesSet = rulesSet;
            GetRules();
        }

        private void GetRules()
        {
            switch (rulesSet)
            {
                case RulesSet.None:
                    // No rules loaded. Add rules manually
                    break;
                case RulesSet.Basic:
                    Rules.AddRange(GetBasicRules());
                    break;
                case RulesSet.Custom:
                    throw new NotImplementedException("For use in a derived custom Generator");
                default: 
                    // default AdvancedRules 
                    Rules.AddRange(GetBasicRules());
                    Rules.AddRange(GetAdvancedRules());
                    break;
            }
        }

        protected List<Rule> GetBasicRules()
        {
            // Base type seeding with no filters
            return new List<Rule>(){
                new Rule(typeof(string),     "", g => g.Property.Name.ifBlank() + g.RowNumber.ToString(),   "String",     "Returns propertyName + RowNumber." ),
                new Rule(typeof(bool),       "", g => g.RowNumber % 2 == 0 ? true : false,                  "Bool",       "Alternating true & false."),
                new Rule(typeof(int),        "", g => g.RowNumber,                                          "Int",        "Returns RowNumber"),
                new Rule(typeof(long),       "", g => (long) g.RowNumber,                                   "Long",       "Returns RowNumber" ),
                new Rule(typeof(double),     "", g => (double) g.RowNumber,                                 "Double",     "Returns RowNumber" ),
                new Rule(typeof(decimal),    "", g => (decimal) g.RowNumber,                                "Decimal",    "Returns RowNumber" ),
                new Rule(typeof(DateTime),   "", g => g.BaseDateTime,                                       "DateTime",   "Uses BaseDateTime" ),
                new Rule(typeof(Guid),       "", g => RandomGuid(g),                                        "Guid",       "Returns a Guid" ),

                new Rule(typeof(bool?),      "", g => g.RowRandom.Next(1, 6) == 5 ? (bool?) null : (g.RowNumber % 2 == 0),      "Bool?",     "Returns alternating true & false (1 in 5 NULL)"),                                 
                new Rule(typeof(int?),       "", g => g.RowRandom.Next(1, 6) == 5 ? (int?) null : (int?) g.RowNumber,           "Int?",      "Returns RowNumber (1 in 5 NULL)"),                                 
                new Rule(typeof(long?),      "", g => g.RowRandom.Next(1, 6) == 5 ? (long?) null : (long?) g.RowNumber,         "Long?",     "Returns RowNumber (1 in 5 NULL)"),
                new Rule(typeof(double?),    "", g => g.RowRandom.Next(1, 6) == 5 ? (double?) null : (double?) g.RowNumber,     "Double?",   "Returns RowNumber (1 in 5 NULL)"),
                new Rule(typeof(Decimal?),   "", g => g.RowRandom.Next(1, 6) == 5 ? (decimal?) null : (decimal?) g.RowNumber,   "Decimal?",  "Returns RowNumber (1 in 5 NULL)"),                                 
                new Rule(typeof(DateTime?),  "", g => g.RowRandom.Next(1, 6) == 5 ? (DateTime?) null : g.BaseDateTime,          "DateTime?", "Returns BaseDateTime (1 in 5 NULL)")                             
            };
        }

        protected List<Rule> GetAdvancedRules()
        {
            // Advanced random seeding of common English patterns and DateTimes.
            return new List<Rule>(){
                new Rule(typeof(string),    "%firstname%,%givenname%",      g => RandomElement(g, "FirstName"),         "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",         g => RandomElement(g, "LastName"),          "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%user%",                       g => RandomUserName(g),                     "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                    g => RandomAddress(g),                      "Address",              "Random address" ),
                new Rule(typeof(string),    "%county%",                     g => RandomElement(g, "City"),              "City",                 "Random city" ),
                new Rule(typeof(string),    "%country%",                    g => RandomElement(g, "CountyName"),        "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                      g => RandomElement(g, "StateName"),         "State",                "Random state" ),
                new Rule(typeof(string),    "%country%",                    g => RandomElement(g, "Country"),           "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                        g => RandomZip(g),                          "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                  g => RandomFee(g),                          "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",       g => RandomCost(g),                         "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%",         g => RandomCompany(g),                      "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",             g => RandomElement(g, "ProductName"),       "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                      g => RandomEmail(g),                        "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                     g => g.RowRandomNumber,                     "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",g => RandomPhone(g),                        "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                             g => RandomDateTime(g, -17521, -17521),     "DateTime",             "Random Override of DateTime" ),
                new Rule(typeof(DateTime?), "",                             g => RandomNullDateTime(g, -17521, -17521), "DateTime?",            "Random Override of DateTime?" )
                ,new Rule(typeof(string),   "",                            g => RandomStringOrDefault(g),               "String",               "Random string from data or default" )
            };
        }

        #region Random Generate Methods

        // Convert
        public string RandomElement (IGenerator generator, string identifier)
        {
            System.Diagnostics.Debug.WriteLine(generator?.Property?.Name ?? "Unknown");
            var elementList = dataSource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return elementList?.ElementAtOrDefault(index) ?? "";
        }

        public dynamic RandomElement (IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = dataSource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }

        public string NextElement (IGenerator generator, string identifier)
        {
            // Will loop back to beginning if rownumber is greater than number of elements in list

            List<string> strings = dataSource.GetElementList(identifier);
            int count = strings.Count;
            if (count == 0)
                return "";

            int mod = (generator.RowNumber - 1) % count; 
            int position = mod;

            return strings?.ElementAtOrDefault(position) ?? "";
        }

        public string RandomUserName(IGenerator generator)
        {
            var firstNames = dataSource.GetElementList("FirstName");
            var lastNames = dataSource.GetElementList("LastName");

            int r1 = generator.RowRandom.Next(firstNames.Count);
            int r2 = generator.RowRandom.Next(lastNames.Count);

            string initial = (generator.CurrentRowValues.ContainsKey("FirstName") ? generator.CurrentRowValues["FirstName"].ToString() : firstNames[r1]).FirstOrDefault().ToString();
            string lastName = generator.CurrentRowValues.ContainsKey("LastName") ? generator.CurrentRowValues["LastName"].ToString() : lastNames[r2];

            return string.Format($"{initial}{lastName}"); // First-Initial LastName
        }

        public string RandomEmail(IGenerator generator)
        {
            var companyNames = dataSource.GetElementList("CompanyName");
            var companySuffixes = dataSource.GetElementList("CompanySuffix");
            var domainExtensions = dataSource.GetElementList("DomainExtension");

            int r1 = generator.RowRandom.Next(companyNames.Count);
            int r2 = generator.RowRandom.Next(companySuffixes.Count);
            int r3 = generator.RowRandom.Next(domainExtensions.Count);

            string userName     =   generator.CurrentRowValues.ContainsKey("UserName") ? generator.CurrentRowValues["UserName"].ToString() : RandomUserName(generator);
            string companyName  =   generator.CurrentRowValues.ContainsKey("CompanyName") ? generator.CurrentRowValues["CompanyName"].ToString() : companyNames[r1] + companySuffixes[r2]; ;
            string domain       =   generator.CurrentRowValues.ContainsKey("DomainExtension") ? generator.CurrentRowValues["DomainExtension"].ToString() : domainExtensions[r3];

            return string.Format($"{userName}@{companyName}{domain}").ToLower();
        }

        public string RandomAddress(IGenerator generator)
        {
            var streetNames = dataSource.GetElementList("StreetName");
            var roadTypes = dataSource.GetElementList("RoadTypes");

            int streetNumber = generator.RowRandom.Next(1, 10000);
            int r1 = generator.RowRandom.Next(streetNames.Count);
            int r2 = generator.RowRandom.Next(roadTypes.Count);

            return string.Format($"{streetNumber} {streetNames[r1]} {roadTypes[r2]}");
        }

        public string RandomZip(IGenerator generator)
        {
            int i1 = generator.RowRandom.Next(10001, 100000);
            return i1.ToString();
        }

        public decimal RandomFee(IGenerator generator)
        {
            decimal fee = generator.RowRandom.Next(1, 200);
            decimal dec = generator.RowRandom.Next(0, 100);
            return fee + (dec * .01M);
        }

        public decimal RandomCost(IGenerator generator)
        {
            decimal cost = generator.RowRandom.Next(1, 1000);
            decimal dec = generator.RowRandom.Next(0, 100);
            return cost + (dec * .01M);
        }

        public string RandomCompany(IGenerator generator)
        {
            var companyNames = dataSource.GetElementList("CompanyName");
            var companySuffixes = dataSource.GetElementList("CompanySuffix");

            int r1 = generator.RowRandom.Next(companyNames.Count);
            int r2 = generator.RowRandom.Next(companySuffixes.Count);

            return ($"{companyNames[r1]} {companySuffixes[r2]}");
        }

        public string RandomPhone(IGenerator generator)
        {
            int r1 = generator.RowRandom.Next(100, 1000);
            int r2 = generator.RowRandom.Next(100, 1000);
            int r3 = generator.RowRandom.Next(1000, 10000);

            return string.Format("({r1}) {r2}-{r3}");
        }

        public DateTime RandomDateTime (IGenerator generator, int hoursBefore, int hoursAfter)
        {
            // BaseDateTime +- 2 years by hour
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return generator.BaseDateTime.AddHours(randomHours);
        }

        public DateTime? RandomNullDateTime (IGenerator generator, int hoursBefore, int hoursAfter)
        {
            // BaseDateTime + -2 years by hour
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);
            int diceroll = generator.RowRandom.Next(1, 7);

            return diceroll == 5 ? (DateTime?) null : generator.BaseDateTime.AddHours(randomHours);
        }

        public Guid RandomGuid(IGenerator generator)
        {
            int seed = generator.RowRandom.Next();
            var r = new Random(seed);
            var guid = new byte[16];
            r.NextBytes(guid);

            return new Guid(guid);
        }

        public string RandomStringOrDefault (IGenerator generator)
        {
            var propertyName = generator.Property?.Name ?? "";
            string defaultValue = propertyName + generator.RowNumber.ToString();
            var elementList = dataSource.GetElementList(propertyName);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return elementList?.ElementAtOrDefault(index) ?? defaultValue;
        }


        /* Add City StateName County Province */

        #endregion

    }
} 

 
