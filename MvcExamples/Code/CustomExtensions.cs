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
            public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin = 1, int seedEnd = 10, string filePath = null) where T : new()
            {
                var generator = new DualGenerator(filePath) {
                    SeedBegin = seedBegin,
                    SeedEnd = seedEnd,
                    BaseDateTime = DateTime.Now,
                    
                    
                };
                var seedCore = new SeedCore(generator);

                generator.Rules.AddRange
                (
                    new List<Rule> {
                        new Rule(typeof(List<Item>), "", g => ExampleRules.AddItems<Item>(g))
                        // OTHER CUSTOM RULES HERE
                    }
                );
                return seedCore.SeedList(iEnumerable);
            }
     }
 }


