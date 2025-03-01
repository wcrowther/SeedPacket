
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;

namespace SeedPacket
{
	public static partial class RulesExtensions
    {
        public static void AddCommonRules (this IRules rules, bool overwrite =  true )
        {
            var commonRules = new List<Rule>(){
                new (typeof(string),    "",                                 g => g.GetElementRandom(),                  "String",               "Random string from data or default" ),
                new (typeof(string),    "%firstname%,%givenname%",          g => g.GetElementRandom("FirstName"),       "FirstName",            "Random firstName" ),
                new (typeof(string),    "%lastname%,%surname%",             g => g.GetElementRandom("LastName"),        "LastName",             "Random lastname" ),
                new (typeof(string),    "%fullname%",                       g => g.RandomFullName(),                    "FullName",             "Random fullName" ),
                new (typeof(string),    "%user%",                           g => g.RandomUserName(),                    "User",                 "Random username" ),
                new (typeof(string),    "%address%",                        g => g.RandomAddress(),                     "Address",              "Random address" ),
                new (typeof(string),    "%city%",                           g => g.GetElementRandom("City"),            "City",                 "Random city" ),
                new (typeof(string),    "%county%",                         g => g.GetElementRandom("County"),          "County",               "Random county" ),
                new (typeof(string),    "%state%",                          g => g.GetElementRandom("State"),           "State",                "Random state" ),
                new (typeof(string),    "%statename%",                      g => g.GetElementRandom("StateName"),       "StateName",            "Random state name" ),
                new (typeof(string),    "%country%",                        g => g.GetElementRandom("Country"),         "Country",              "Random country" ),
                new (typeof(string),    "%zip%",                            g => g.RandomZip(),                         "Zip",                  "Random zip" ),
                new (typeof(decimal),   "%fee%,%tax%",                      g => g.RandomFee(),                         "Fee/Tax",              "Random fee/tax" ),
                new (typeof(decimal),   "%cost%,%price%,%worth%",           g => g.RandomCost(),                        "Cost/Price",           "Random cost/price" ),
                new (typeof(string),    "%company%,%business%,%account%",   g => g.RandomCompany(),                     "Company/Business",     "Random company/business" ),
                new (typeof(string),    "%product%,%item%",                 g => g.GetElementRandom("ProductName"),     "Product/Item",         "Random product/item" ),
                new (typeof(string),    "%email%",                          g => g.RandomEmail(),                       "Email",                "Random email" ),
                new (typeof(int),       "%random%",                         g => g.RowRandomNumber,                     "RowRandomNumber",      "Random number"),
                new (typeof(string),    "%phone%,%cell%,%mobile%,%fax%",    g => g.RandomPhone(),                       "Phone/Cell/Mobile/Fax","Random phone/cell/mobile/fax" ),
                new (typeof(DateTime),  "",                                 g => g.RandomDateTime(-17521, 17521),       "DateTime",             "Random DateTime base +/- 2yr by hour" ),
                new (typeof(DateTime?), "",                                 g => g.RandomDateTimeNull(-17521, 17521),   "DateTime?",            "Random DateTime? base +/- 2yr by hour" ),
                new (typeof(string),    "%text%,%note%,%lorem%",            g => g.RandomLoremText(),                   "Text/Note/Lorem",      "Random lorem text paragraph" ),
                new (typeof(IList<string>),"%body%,%copy%",                 g => g.RandomBodyCopy(),                    "RandomBodyCopy",       "Random copy list" )
            };

            rules.AddRange(commonRules, overwrite);
        }
    }
}
