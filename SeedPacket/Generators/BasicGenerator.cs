using System;
using System.Collections.Generic;
using System.Globalization;
using NewLibrary.ForType;
using SeedPacket.Interfaces;

namespace SeedPacket.Generators
{
    public class BasicGenerator : Generator, IGenerator 
    {
        public BasicGenerator ( DateTime? BaseDateTime = null, Random BaseRandom = null, bool Debug = false) 
                                : base(BaseDateTime, BaseRandom)
        {

        }

        public List<Rule> GetRules()
        {
             return new List<Rule>(){
                    new Rule(typeof(string),     "", g => g.CurrentProperty.Name + g.RowNumber.ToString(),   "Basic string",   "Returns propertyName + RowNumber."),
                    new Rule(typeof(bool),       "", g => g.RowNumber % 2 == 0 ? true : false,        "Basic bool",     "Alternating true & false."),
                    new Rule(typeof(int),        "", g => g.RowNumber,                                "Basic int",      "Returns RowNumber"),
                    new Rule(typeof(long),       "", g => (long) g.RowNumber,                         "Basic long",     "Returns RowNumber"),
                    new Rule(typeof(double),     "", g => (double) g.RowNumber,                       "Basic double",   "Returns RowNumber"),
                    new Rule(typeof(decimal),    "", g => (decimal) g.RowNumber,                      "Basic decimal",  "Returns RowNumber"),
                    new Rule(typeof(DateTime),   "", g => g.BaseDateTime,                             "Basic DateTime", "Uses BaseDateTime"),
                    new Rule(typeof(Guid),       "", g => RandomGuid(g),                              "Basic Guid",     "Returns a Guid"),

                    new Rule(typeof(bool?),      "", g => g.RowRandom.Next(1, 6) == 5 ? (bool?) null : g.RowNumber % 2 == 0,        "Basic bool?",      "Returns alternating true & false (1 in 5 NULL)"),                                 
                    new Rule(typeof(int?),       "", g => g.RowRandom.Next(1, 6) == 5 ? (int?) null  : (int?) g.RowNumber,          "Basic int?",       "Returns RowNumber (1 in 5 NULL)"),                                 
                    new Rule(typeof(long?),      "", g => g.RowRandom.Next(1, 6) == 5 ? (long?) null  : (long?) g.RowNumber,        "Basic long?",      "Returns RowNumber (1 in 5 NULL)"),
                    new Rule(typeof(double?),    "", g => g.RowRandom.Next(1, 6) == 5 ? (double?) null  : (double?) g.RowNumber,    "Basic double?",    "Returns RowNumber (1 in 5 NULL)"),
                    new Rule(typeof(decimal?),   "", g => g.RowRandom.Next(1, 6) == 5 ? (decimal?) null  : (decimal?) g.RowNumber,  "Basic decimal?",   "Returns RowNumber (1 in 5 NULL)"),                                 
                    new Rule(typeof(DateTime?),  "", g => g.RowRandom.Next(1, 6) == 5 ? (DateTime?) null  : g.BaseDateTime,         "Basic DateTime?",  "Returns BaseDateTime (1 in 5 NULL)")                             
                };
        }

        public Guid RandomGuid(IGenerator generator)
        {
            int seed = generator.RowRandom.Next();
            var random = new Random(seed);
            var guid = new byte[16];
            random.NextBytes(guid);

            return new Guid(guid);
        }
    }
} 
