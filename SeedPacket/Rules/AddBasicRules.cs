
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;

namespace SeedPacket
{
	public static partial class RulesExtensions
    {
        public static void AddBasicRules (this IRules rules, bool overwrite = false)
        {
            var basicRules = new List<Rule>(){
                new (typeof(string),        "", g => (g.CurrentProperty?.Name ?? "") + g.RowNumber.ToString(),  "String",    "Returns propertyName + RowNumber." ),
                new (typeof(bool),          "", g => g.RowNumber % 2 == 0 ? true : false,                       "Bool",      "Returns alternating true & false."),
                new (typeof(int),           "", g => g.RowNumber,                                               "Int",       "Returns RowNumber"),
                new (typeof(long),          "", g => (long) g.RowNumber,                                        "Long",      "Returns RowNumber" ),
                new (typeof(double),        "", g => (double) g.RowNumber,                                      "Double",    "Returns RowNumber" ),
                new (typeof(decimal),       "", g => (decimal) g.RowNumber,                                     "Decimal",   "Returns RowNumber" ),
                new (typeof(DateTime),      "", g => g.BaseDateTime.AddDays(g.RowNumber),                       "DateTime",  "Returns BaseDateTime" ),
                new (typeof(Guid),          "", g => g.RandomGuid(),                                            "Guid",      "Returns a Guid" ),
                new (typeof(bool?),         "", g => g.OnceEvery(6) ? null : (bool?)(g.RowNumber % 2 == 0),     "Bool?",     "Returns alternating true & false (1 in 6 NULL)"),
                new (typeof(int?),          "", g => g.OnceEvery(6) ? null : (int?) g.RowNumber,                "Int?",      "Returns RowNumber (1 in 6 NULL)"),
                new (typeof(long?),         "", g => g.OnceEvery(6) ? null : (long?) g.RowNumber,               "Long?",     "Returns RowNumber (1 in 6 NULL)"),
                new (typeof(double?),       "", g => g.OnceEvery(6) ? null : (double?) g.RowNumber,             "Double?",   "Returns RowNumber (1 in 6 NULL)"),
                new (typeof(decimal?),      "", g => g.OnceEvery(6) ? null : (decimal?) g.RowNumber,            "Decimal?",  "Returns RowNumber (1 in 6 NULL)"),
                new (typeof(DateTime?),     "", g => g.OnceEvery(6) ? null : (DateTime?) g.BaseDateTime,        "DateTime?", "Returns BaseDateTime (1 in 6 NULL)")
            };

            rules.AddRange(basicRules, overwrite);
        }
    }
}
