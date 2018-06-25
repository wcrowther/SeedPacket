﻿
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SeedPacket
{
    public static partial class RulesExtensions
    {
        public static void AddBasicRules (this IRules rules, bool overwrite = false)
        {
            var basicRules = new List<Rule>(){
                new Rule(typeof(string),        "", g => (g.CurrentProperty?.Name ?? "") + g.RowNumber.ToString(),      "String",    "Returns propertyName + RowNumber." ),
                new Rule(typeof(bool),          "", g => g.RowNumber % 2 == 0 ? true : false,                           "Bool",      "Returns alternating true & false."),
                new Rule(typeof(int),           "", g => g.RowNumber,                                                   "Int",       "Returns RowNumber"),
                new Rule(typeof(long),          "", g => (long) g.RowNumber,                                            "Long",      "Returns RowNumber" ),
                new Rule(typeof(double),        "", g => (double) g.RowNumber,                                          "Double",    "Returns RowNumber" ),
                new Rule(typeof(decimal),       "", g => (decimal) g.RowNumber,                                         "Decimal",   "Returns RowNumber" ),
                new Rule(typeof(DateTime),      "", g => g.BaseDateTime.AddDays(g.RowNumber),                           "DateTime",  "Returns BaseDateTime" ),
                new Rule(typeof(Guid),          "", g => Funcs.RandomGuid(g),                                           "Guid",      "Returns a Guid" ),
                new Rule(typeof(bool?),         "", g => Funcs.DiceRoll(g) == 1 ? null : (bool?)(g.RowNumber % 2 == 0), "Bool?",     "Returns alternating true & false (1 in 6 NULL)"),
                new Rule(typeof(int?),          "", g => Funcs.DiceRoll(g) == 1 ? null : (int?) g.RowNumber,            "Int?",      "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(long?),         "", g => Funcs.DiceRoll(g) == 1 ? null : (long?) g.RowNumber,           "Long?",     "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(double?),       "", g => Funcs.DiceRoll(g) == 1 ? null : (double?) g.RowNumber,         "Double?",   "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(decimal?),      "", g => Funcs.DiceRoll(g) == 1 ? null : (decimal?) g.RowNumber,        "Decimal?",  "Returns RowNumber (1 in 6 NULL)"),
                new Rule(typeof(DateTime?),     "", g => Funcs.DiceRoll(g) == 1 ? null : (DateTime?) g.BaseDateTime,    "DateTime?", "Returns BaseDateTime (1 in 6 NULL)")
                // new Rule(typeof(IList),      "", g => Funcs.EmptyList(g),                                            "EmptyList", "Returns an empty list"),            };
            };

            rules.AddRange(basicRules, overwrite);
        }
    }
}