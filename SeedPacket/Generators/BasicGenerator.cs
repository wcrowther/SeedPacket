using SeedPacket.Interfaces;
using System;

namespace SeedPacket.Generators
{
    public class BasicGenerator : Generator, IGenerator 
    {
        // BasicGenerator never populates the datasource so it is always null

        public BasicGenerator(IRules rules = null, Random baseRandom = null, DateTime? baseDateTime = null)
        {
            base.RowNumber = SeedBegin;
            base.rules = rules ?? new Rules();
            base.baseRandom = baseRandom ?? new Random(defaultSeed);
            base.baseDateTime = baseDateTime;
            GetNextRowRandom();

            Rules.AddBasicRules();
        }
    }
} 
