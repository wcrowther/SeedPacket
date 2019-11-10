
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
                new Rule(typeof(string),    "",                                 g => g.GetElementRandom(),                  "String",               "Random string from data or default" ),
                new Rule(typeof(string),    "%firstname%,%givenname%",          g => g.GetElementRandom("FirstName"),       "FirstName",            "Random firstName" ),
                new Rule(typeof(string),    "%lastname%,%surname%",             g => g.GetElementRandom("LastName"),        "LastName",             "Random lastname" ),
                new Rule(typeof(string),    "%fullname%",                       g => g.RandomFullName(),                    "FullName",             "Random fullName" ),
                new Rule(typeof(string),    "%user%",                           g => g.RandomUserName(),                    "User",                 "Random username" ),
                new Rule(typeof(string),    "%address%",                        g => g.RandomAddress(),                     "Address",              "Random address" ),
                new Rule(typeof(string),    "%city%",                           g => g.GetElementRandom("City"),            "City",                 "Random city" ),
                new Rule(typeof(string),    "%county%",                         g => g.GetElementRandom("County"),          "County",               "Random county" ),
                new Rule(typeof(string),    "%state%",                          g => g.GetElementRandom("State"),           "State",                "Random state" ),
                new Rule(typeof(string),    "%statename%",                      g => g.GetElementRandom("StateName"),       "StateName",            "Random state name" ),
                new Rule(typeof(string),    "%country%",                        g => g.GetElementRandom("Country"),         "Country",              "Random country" ),
                new Rule(typeof(string),    "%zip%",                            g => g.RandomZip(),                         "Zip",                  "Random zip" ),
                new Rule(typeof(decimal),   "%fee%,%tax%",                      g => g.RandomFee(),                         "Fee/Tax",              "Random fee/tax" ),
                new Rule(typeof(decimal),   "%cost%,%price%,%worth%",           g => g.RandomCost(),                        "Cost/Price",           "Random cost/price" ),
                new Rule(typeof(string),    "%company%,%business%,%account%",   g => g.RandomCompany(),                     "Company/Business",     "Random company/business" ),
                new Rule(typeof(string),    "%product%,%item%",                 g => g.GetElementRandom("ProductName"),     "Product/Item",         "Random product/item" ),
                new Rule(typeof(string),    "%email%",                          g => g.RandomEmail(),                       "Email",                "Random email" ),
                new Rule(typeof(int),       "%random%",                         g => g.RowRandomNumber,                     "RowRandomNumber",      "Random number"),
                new Rule(typeof(string),    "%phone%,%cell%,%mobile%,%fax%",    g => g.RandomPhone(),                       "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new Rule(typeof(DateTime),  "",                                 g => g.RandomDateTime(-17521, 17521),       "DateTime",             "Random DateTime base +/- 2yr by hour" ),
                new Rule(typeof(DateTime?), "",                                 g => g.RandomDateTimeNull(-17521, 17521),   "DateTime?",            "Random DateTime? base +/- 2yr by hour" ),
                new Rule(typeof(string),    "%text%,%note%,%lorem%",            g => g.RandomLoremText(),                   "Text/Note/Lorem",      "Random lorem text paragraph" ),
                new Rule(typeof(IList<string>),"%body%,%copy%",                 g => g.RandomBodyCopy(),                    "RandomBodyCopy",       "Random copy list" )
            };

            rules.AddRange(commonRules, overwrite);
        }
    }
}
