using SeedPacket;
using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System;
using SeedPacket.Enums;
using MvcExamples.Models;
using SeedPacket.Examples;

namespace MvcExamples.Code
{
    public static class CustomExtensions
    {
            public static IList<T> Seed<T> (this IList<T> iList, int seedBegin = 1, int seedEnd = 10, string filePath = null) where T : new()
            {
                var generator = new MultiGenerator(filePath) {
                    SeedBegin = seedBegin,
                    SeedEnd = seedEnd,
                    BaseDateTime = DateTime.Now,
                    
                    
                };
                var seedCore = new SeedCore(generator);

                generator.Rules.AddRange
                (
                    new List<Rule> {
                        new Rule(typeof(List<Item>), "", g => ExampleRules.AddItems<Item>(g), "ListOfItems")
                        // OTHER CUSTOM RULES HERE
                    }
                );
                return seedCore.SeedList(iList);
            }
     }
 }


