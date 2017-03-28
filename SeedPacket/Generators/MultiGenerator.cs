using NewLibrary.ForString;
using SeedPacket.DataSources;
using SeedPacket.Functions;
using SeedPacket.Enums;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Generators
{
    public class MultiGenerator : Generator, IGenerator 
    {

        public MultiGenerator(  string sourceFilepath = null,
                                string sourceString = null, 
                                SeedInputType seedInputType = SeedInputType.Auto,
                                RulesSet rulesSet = RulesSet.Advanced
                                )
        {
            dataSource = new MultiDataSource(sourceFilepath, sourceString, seedInputType);
            GetRules(rulesSet);
        }

        public MultiGenerator ( IDataSource datasource, RulesSet rulesSet = RulesSet.Advanced )
        {
            dataSource = datasource;
            GetRules(rulesSet);
        }

    #region Get Rules

        private void GetRules (RulesSet ruleSet)
        {
            switch (ruleSet)
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

        protected List<Rule> GetBasicRules ()
        {
            // Base type seeding with no filters
            return new List<Rule>(){
                new Rule(typeof(string),        "", g => g.CurrentProperty.Name.ifBlank() + g.RowNumber.ToString(),    "String",    "Returns propertyName + RowNumber." ),
                new Rule(typeof(bool),          "", g => g.RowNumber % 2 == 0 ? true : false,                          "Bool",      "Returns alternating true & false."),
                new Rule(typeof(int),           "", g => g.RowNumber,                                                  "Int",       "Returns RowNumber"),
                new Rule(typeof(long),          "", g => (long) g.RowNumber,                                           "Long",      "Returns RowNumber" ),
                new Rule(typeof(double),        "", g => (double) g.RowNumber,                                         "Double",    "Returns RowNumber" ),
                new Rule(typeof(decimal),       "", g => (decimal) g.RowNumber,                                        "Decimal",   "Returns RowNumber" ),
                new Rule(typeof(DateTime),      "", g => g.BaseDateTime,                                               "DateTime",  "Returns BaseDateTime" ),
                new Rule(typeof(Guid),          "", g => func.RandomGuid(g),                                           "Guid",      "Returns a Guid" ),
                new Rule(typeof(bool?),         "", g => func.DiceRoll(g) ? null : (bool?)(g.RowNumber % 2 == 0),      "Bool?",     "Returns alternating true & false (1 in 6 NULL)"),
                new Rule(typeof(int?),          "", g => func.DiceRoll(g) ? null : (int?) g.RowNumber,                 "Int?",      "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(long?),         "", g => func.DiceRoll(g) ? null : (long?) g.RowNumber,                "Long?",     "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(double?),       "", g => func.DiceRoll(g) ? null : (double?) g.RowNumber,              "Double?",   "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(decimal?),      "", g => func.DiceRoll(g) ? null : (decimal?) g.RowNumber,             "Decimal?",  "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(DateTime?),     "", g => func.DiceRoll(g) ? null : (DateTime?) g.BaseDateTime,         "DateTime?", "Returns BaseDateTime (1 in 6 NULL)")
            };
        }

        protected List<Rule> GetAdvancedRules ()
        {
            // Advanced random seeding of common English patterns and DateTimes.
            return new List<Rule>(){
                new Rule(typeof(string),    "",                                 g => func.RandomElement(g),                     "String",               "Random string from data or default" ),
                new Rule(typeof(string),    "%firstname%,%givenname%",          g => func.RandomElement(g, "FirstName"),        "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",             g => func.RandomElement(g, "LastName"),         "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%user%",                           g => func.RandomUserName(g),                    "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                        g => func.RandomAddress(g),                     "Address",              "Random address" ),
                new Rule(typeof(string),    "%county%",                         g => func.RandomElement(g, "City"),             "City",                 "Random city" ),
                new Rule(typeof(string),    "%country%",                        g => func.RandomElement(g, "CountyName"),       "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                          g => func.RandomElement(g, "StateName"),        "State",                "Random state" ),
                new Rule(typeof(string),    "%country%",                        g => func.RandomElement(g, "Country"),          "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                            g => func.RandomZip(g),                         "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                      g => func.RandomFee(g),                         "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",           g => func.RandomCost(g),                        "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%,%account%",   g => func.RandomCompany(g),                     "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",                 g => func.RandomElement(g, "ProductName"),      "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                          g => func.RandomEmail(g),                       "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                         g => g.RowRandomNumber,                         "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",    g => func.RandomPhone(g),                       "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                                 g => func.RandomDateTime(g, -17521, 17521),     "DateTime",             "Random Override of DateTime" ),
                new Rule(typeof(DateTime?), "",                                 g => func.RandomDateTimeNull(g, -17521, 17521), "DateTime?",        "Random Override of DateTime?" )
            };
        }

    #endregion

    }
} 

 
