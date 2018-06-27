
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SeedPacket
{
    public static partial class RulesExtensions
    {
        public static void AddCommonRules (this IRules rules, bool overwrite =  true )
        {
            var commonRules = new List<Rule>(){
                new Rule(typeof(string),    "",                                 g => Funcs.GetElementRandom(g),                     "String",               "Random string from data or default" ),
                new Rule(typeof(string),    "%firstname%,%givenname%",          g => Funcs.GetElementRandom(g, "FirstName"),        "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",             g => Funcs.GetElementRandom(g, "LastName"),         "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%user%",                           g => Funcs.RandomUserName(g),                       "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                        g => Funcs.RandomAddress(g),                        "Address",              "Random address" ),
                new Rule(typeof(string),    "%city%",                           g => Funcs.GetElementRandom(g, "City"),             "City",                 "Random city" ),
                new Rule(typeof(string),    "%county%",                         g => Funcs.GetElementRandom(g, "County"),           "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                          g => Funcs.GetElementRandom(g, "State"),            "State",                "Random state" ),
                new Rule(typeof(string),    "%statename%",                      g => Funcs.GetElementRandom(g, "StateName"),        "StateName",            "Random state name" ),
                new Rule(typeof(string),    "%country%",                        g => Funcs.GetElementRandom(g, "Country"),          "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                            g => Funcs.RandomZip(g),                            "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                      g => Funcs.RandomFee(g),                            "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",           g => Funcs.RandomCost(g),                           "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%,%account%",   g => Funcs.RandomCompany(g),                        "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",                 g => Funcs.GetElementRandom(g, "ProductName"),      "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                          g => Funcs.RandomEmail(g),                          "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                         g => g.RowRandomNumber,                             "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",    g => Funcs.RandomPhone(g),                          "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                                 g => Funcs.RandomDateTime(g, -17521, 17521),        "DateTime",             "Random DateTime base +/- 2yr by hour" ),
                new Rule(typeof(DateTime?), "",                                 g => Funcs.RandomDateTimeNull(g, -17521, 17521),    "DateTime?",            "Random DateTime? base +/- 2yr by hour" ),
                new Rule(typeof(string),    "%text%,%note%,%lorem%",            g => Funcs.RandomLoremText(g),                      "Text/Note/Lorem",      "Random lorem text paragraph" ),
                new Rule(typeof(IList<string>),"%body%,%copy%",                 g => Funcs.RandomBodyCopy(g),                       "RandomBodyCopy",       "Random copy list" )
            };

            rules.AddRange(commonRules, overwrite);
        }
    }
}
