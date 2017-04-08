using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        // Simulates rolling a 1 on a 6-sided dice (16.6%). Higher diceRange is smaller chance of true (linear).
        public static bool DiceRoll (IGenerator generator, int diceRange = 7)
        {
            return generator.RowRandom.Next(1, diceRange) == 1;
        }
    }
}
