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
        protected IDataSource _dataSource; 
        protected RulesSet _rulesSet;

        public MultiGenerator(  string sourceFilepath = null,
                                string sourceString = null, 
                                SeedInputType seedInputType = SeedInputType.Auto,
                                RulesSet rulesSet = RulesSet.Advanced
                                )
        {
            _dataSource = new MultiDataSource(sourceFilepath, sourceString, seedInputType);
            _rulesSet = rulesSet;
            GetRules();
        }

        public MultiGenerator ( IDataSource datasource, RulesSet rulesSet = RulesSet.Advanced )
        {
            _dataSource = datasource;
            _rulesSet = rulesSet;
            GetRules();
        }

        private void GetRules()
        {
            switch (_rulesSet)
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
                new Rule(typeof(string),     "", g => g.CurrentProperty.Name.ifBlank() + g.RowNumber.ToString(),    "String",    "Returns propertyName + RowNumber." ),
                new Rule(typeof(bool),       "", g => g.RowNumber % 2 == 0 ? true : false,                          "Bool",      "Returns alternating true & false."),
                new Rule(typeof(int),        "", g => g.RowNumber,                                                  "Int",       "Returns RowNumber"),
                new Rule(typeof(long),       "", g => (long) g.RowNumber,                                           "Long",      "Returns RowNumber" ),
                new Rule(typeof(double),     "", g => (double) g.RowNumber,                                         "Double",    "Returns RowNumber" ),
                new Rule(typeof(decimal),    "", g => (decimal) g.RowNumber,                                        "Decimal",   "Returns RowNumber" ),
                new Rule(typeof(DateTime),   "", g => g.BaseDateTime,                                               "DateTime",  "Returns BaseDateTime" ),
                new Rule(typeof(Guid),       "", g => RandomGuid(g),                                                "Guid",      "Returns a Guid" ),
                new Rule(typeof(bool?),      "", g => DiceRoll(g) ? (bool?) null : (g.RowNumber % 2 == 0),          "Bool?",     "Returns alternating true & false (1 in 6 NULL)"),                                 
                new Rule(typeof(int?),       "", g => DiceRoll(g) ? (int?) null :  g.RowNumber,                     "Int?",      "Returns RowNumber (1 in 6 NULL)"),                                 
                new Rule(typeof(long?),      "", g => DiceRoll(g) ? (long?) null :  g.RowNumber,                    "Long?",     "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(double?),    "", g => DiceRoll(g) ? (double?) null :  g.RowNumber,                  "Double?",   "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(decimal?),   "", g => DiceRoll(g) ? (decimal?) null : g.RowNumber,                  "Decimal?",  "Returns RowNumber (1 in 6 NULL)"),                                 
                new Rule(typeof(DateTime?),  "", g => DiceRoll(g) ? (DateTime?) null : g.BaseDateTime,              "DateTime?", "Returns BaseDateTime (1 in 6 NULL)")                             
            };
        }

        protected List<Rule> GetAdvancedRules()
        {
            // Advanced random seeding of common English patterns and DateTimes.
            return new List<Rule>(){
                new Rule(typeof(string),    "",                             g => RandomStringOrDefault(g),             "String",               "Random string from data or default" ),
                new Rule(typeof(string),    "%firstname%,%givenname%",      g => RandomElement(g, "FirstName"),        "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",         g => RandomElement(g, "LastName"),         "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%user%",                       g => RandomUserName(g),                    "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                    g => RandomAddress(g),                     "Address",              "Random address" ),
                new Rule(typeof(string),    "%county%",                     g => RandomElement(g, "City"),             "City",                 "Random city" ),
                new Rule(typeof(string),    "%country%",                    g => RandomElement(g, "CountyName"),       "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                      g => RandomElement(g, "StateName"),        "State",                "Random state" ),
                new Rule(typeof(string),    "%country%",                    g => RandomElement(g, "Country"),          "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                        g => RandomZip(g),                         "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                  g => RandomFee(g),                         "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",       g => RandomCost(g),                        "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%",         g => RandomCompany(g),                     "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",             g => RandomElement(g, "ProductName"),      "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                      g => RandomEmail(g),                       "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                     g => g.RowRandomNumber,                    "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",g => RandomPhone(g),                       "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                             g => RandomDateTime(g, -17521, 17521),     "DateTime",             "Random Override of DateTime" ),
                new Rule(typeof(DateTime?), "",                             g => RandomNullDateTime(g, -17521, 17521), "DateTime?",            "Random Override of DateTime?" )
            };
        }

        #region Generic Random Generate Methods

        // Convert
        public string RandomElement (IGenerator generator, string identifier)
        {
            var elementList = _dataSource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return elementList?.ElementAtOrDefault(index);
        }

        public dynamic RandomElement (IGenerator generator, string identifier, TypeCode typeCode)
        {
            var elementList = _dataSource.GetElementList(identifier);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return Convert.ChangeType(elementList?.ElementAtOrDefault(index), typeCode);
        }

        public string RandomStringOrDefault (IGenerator generator)
        {
            var propertyName = generator.CurrentProperty?.Name ?? "";
            string defaultValue = propertyName + generator.RowNumber.ToString();
            var elementList = _dataSource.GetElementList(propertyName);
            int index = new Random(generator.RowRandomNumber).Next(elementList.Count);

            return elementList?.ElementAtOrDefault(index) ?? defaultValue;
        }

        public string NextElement (IGenerator generator, string identifier)
        {
            // Will loop back to beginning if rownumber is greater than number of elements in list

            List<string> strings = _dataSource.GetElementList(identifier);
            int count = strings.Count;
            if (count == 0)
                return "";

            int mod = (generator.RowNumber - 1) % count;
            int position = mod;

            return strings?.ElementAtOrDefault(position) ?? "";
        }

        public DateTime RandomDateTime (IGenerator generator, int hoursBefore, int hoursAfter)
        {
            // BaseDateTime +- 2 years by hour
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return generator.BaseDateTime.AddHours(randomHours);
        }

        public DateTime? RandomNullDateTime (IGenerator generator, int hoursBefore, int hoursAfter, int diceRange = 7)
        {
            // BaseDateTime + -2 years by hour is -17521, 17521
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return DiceRoll(generator, diceRange) ? (DateTime?)null : generator.BaseDateTime.AddHours(randomHours);
        }

        public Guid RandomGuid (IGenerator generator)
        {
            int seed = generator.RowRandom.Next();
            var r = new Random(seed);
            var guid = new byte[16];
            r.NextBytes(guid);

            return new Guid(guid);
        }

        // Simulates rolling a 1 on a 6-sided dice (16.6%). Higher diceRange is smaller chance of true (linear).
        public bool DiceRoll (IGenerator generator, int diceRange = 7)
        {
            return generator.RowRandom.Next(1, diceRange) == 1;
        }

        #endregion

        #region Random Generate Methods

        public string RandomUserName (IGenerator generator) // First-Initial LastName
        {
            string firstName    = generator.CurrentRowValues.Get("FirstName")?.ToString() ?? RandomElement(generator, "FirstName") ?? "F";
            string lastName     = generator.CurrentRowValues.Get("LastName")?.ToString() ?? RandomElement(generator, "LastName") ?? $"LastName{generator.RowNumber}";

            return $"{firstName.FirstOrDefault()}{lastName}"; 
        }

        public string RandomEmail(IGenerator generator)
        {
            string userName         = generator?.CurrentRowValues.Get("UserName")?.ToString() ?? RandomUserName(generator);
            string fullCompanyName  = generator?.CurrentRowValues.Get("CompanyName")?.ToString() ?? RandomCompany(generator).Replace(" ", "");
            string domain           = generator?.CurrentRowValues.Get("DomainExtension")?.ToString() ?? ".com";

            return $"{userName}@{fullCompanyName}{domain}".ToLower();
        }

        public string RandomCompany (IGenerator generator)
        {
            return RandomElement(generator, "CompanyName") + RandomElement(generator, "CompanySuffix") ?? "CompanyName" + generator.RowNumber;
        }

        public string RandomAddress(IGenerator generator)
        {
            int streetNumber = generator.RowRandom.Next(1, 10000);
            string streetName = RandomElement(generator, "StreetName") ?? "StreetName" + generator.RowNumber ;
            string roadType = RandomElement(generator, "RoadTypes");

            return $"{streetNumber} {streetName} {roadType}";
        }

        public string RandomZip(IGenerator generator)
        {
            return generator.RowRandom.Next(10001, 100000).ToString();
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

        public string RandomPhone(IGenerator generator)
        {
            int r1 = generator.RowRandom.Next(100, 1000);
            int r2 = generator.RowRandom.Next(100, 1000);
            int r3 = generator.RowRandom.Next(1000, 10000);

            return $"({r1}) {r2}-{r3}";
        }

        #endregion
    }

    public static class DictionaryHelper
    {
        public static TVal Get<TKey, TVal> (this Dictionary<TKey, TVal> dictionary, TKey key, TVal defaultVal = default(TVal))
        {
            TVal val;
            if (dictionary.TryGetValue(key, out val))
            {
                return val;
            }
            return defaultVal;
        }
    }
} 

 
