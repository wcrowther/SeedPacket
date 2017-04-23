﻿using SeedPacket;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcExamples.Extensions
{
    public static class CustomSeedExtensions
    {
        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, int seedEnd = 100, string propertyName = "")
        {
            var gen = new MultiGenerator("~/SourceFiles/xmlSeedSourcePlus.xml", dataInputType: DataInputType.XmlFile)
            {
                SeedEnd = seedEnd,
                BaseRandom = new Random(34561),
                Rules =  // Adds rules
                {                         
                   new Rule(typeof(string),"%Sport%",   g => Funcs.ElementRandom(g, "Sport"), "Sport", "Custom sport from XML file" )
                },
                CurrentPropertyName = propertyName
            };
            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }
    }
}


