using System;
using System.Collections.Generic;
using System.Globalization;
using NewLibrary.ForType;
using SeedPacket.Interfaces;
using SeedPacket.Functions;

namespace SeedPacket.Generators
{
    public class BasicGenerator : Generator, IGenerator 
    {
        // BasicGenerator never populates the datasource so it is always null

        public BasicGenerator ()
        {
            base.Rules.AddRange(GetRules());
        }

        private List<Rule> GetRules()
        {
             return new List<Rule>(){
                    new Rule(typeof(string),     "", g => g.CurrentProperty.Name + g.RowNumber.ToString(),          "Basic string",     "Returns propertyName + RowNumber."),
                    new Rule(typeof(bool),       "", g => g.RowNumber % 2 == 0 ? true : false,                      "Basic bool",       "Alternating true & false."),
                    new Rule(typeof(int),        "", g => g.RowNumber,                                              "Basic int",        "Returns RowNumber"),
                    new Rule(typeof(long),       "", g => (long) g.RowNumber,                                       "Basic long",       "Returns RowNumber"),
                    new Rule(typeof(double),     "", g => (double) g.RowNumber,                                     "Basic double",     "Returns RowNumber"),
                    new Rule(typeof(decimal),    "", g => (decimal) g.RowNumber,                                    "Basic decimal",    "Returns RowNumber"),
                    new Rule(typeof(DateTime),   "", g => g.BaseDateTime,                                           "Basic DateTime",   "Uses BaseDateTime"),
                    new Rule(typeof(Guid),       "", g => func.RandomGuid(g),                                       "Basic Guid",       "Returns a Guid"),

                    new Rule(typeof(bool?),      "", g => func.DiceRoll(g) ? null : (bool?)(g.RowNumber % 2 == 0),  "Basic bool?",      "Returns alternating true & false (1 in 6 NULL)"),                                 
                    new Rule(typeof(int?),       "", g => func.DiceRoll(g) ? null : (int?) g.RowNumber,             "Basic int?",       "Returns RowNumber (1 in 6 NULL)"),                                 
                    new Rule(typeof(long?),      "", g => func.DiceRoll(g) ? null : (long?) g.RowNumber,            "Basic long?",      "Returns RowNumber (1 in 6 NULL)"),
                    new Rule(typeof(double?),    "", g => func.DiceRoll(g) ? null : (double?) g.RowNumber,          "Basic double?",    "Returns RowNumber (1 in 6 NULL)"),
                    new Rule(typeof(decimal?),   "", g => func.DiceRoll(g) ? null : (decimal?) g.RowNumber,         "Basic decimal?",   "Returns RowNumber (1 in 6 NULL)"),                                 
                    new Rule(typeof(DateTime?),  "", g => func.DiceRoll(g) ? null : (DateTime?) g.BaseDateTime,     "Basic DateTime?",  "Returns BaseDateTime (1 in 6 NULL)")                             
                };
        }
    }
} 
