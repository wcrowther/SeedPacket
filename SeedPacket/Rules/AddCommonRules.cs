using NewLibrary.ForString;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SeedPacket
{
    public static partial class RulesExtensions
    {
        public static void AddCommonRules (this IRules rules, bool overwrite =  false )
        {
            var advancedRules = new List<Rule>(){
                new Rule(typeof(string),    "",                                 g => func.ElementRandom(g),                     "String",               "Random string from data or default" ),
                new Rule(typeof(string),    "%firstname%,%givenname%",          g => func.ElementRandom(g, "FirstName"),        "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",             g => func.ElementRandom(g, "LastName"),         "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%user%",                           g => func.RandomUserName(g),                    "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                        g => func.RandomAddress(g),                     "Address",              "Random address" ),
                new Rule(typeof(string),    "%city%",                           g => func.ElementRandom(g, "City"),             "City",                 "Random city" ),
                new Rule(typeof(string),    "%county%",                         g => func.ElementRandom(g, "County"),           "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                          g => func.ElementRandom(g, "State"),            "State",                "Random state" ),
                new Rule(typeof(string),    "%statename%",                      g => func.ElementRandom(g, "StateName"),        "StateName",            "Random state name" ),
                new Rule(typeof(string),    "%country%",                        g => func.ElementRandom(g, "Country"),          "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                            g => func.RandomZip(g),                         "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                      g => func.RandomFee(g),                         "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",           g => func.RandomCost(g),                        "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%,%account%",   g => func.RandomCompany(g),                     "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",                 g => func.ElementRandom(g, "ProductName"),      "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                          g => func.RandomEmail(g),                       "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                         g => g.RowRandomNumber,                         "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",    g => func.RandomPhone(g),                       "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                                 g => func.RandomDateTime(g, -17521, 17521),     "DateTime",             "Random Override of DateTime" ),
                new Rule(typeof(DateTime?), "",                                 g => func.RandomDateTimeNull(g, -17521, 17521), "DateTime?",            "Random Override of DateTime?" )
            };

            rules.AddRange(advancedRules, true);
        }
    }
}
