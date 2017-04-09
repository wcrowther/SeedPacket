using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        // Simulates rolling a 6-sided dice (16.6%). Use a higher diceSides to decrease chances in a linear progression.
        // Increasing numberOfDice creates a bell-curve of probability ie: 3d6 is 3 to 18 w/ 7 most likely possiblity.
        public static int DiceRoll (IGenerator generator, int diceSides = 6, int numberOfDice = 1)
        {
            int diceSum = 0;

            for (int i = 1; i <= numberOfDice; i++)
            {
                // Add +1 as Random takes values less than max ie: 1,7 returns 1-6
                diceSum = diceSum + generator.RowRandom.Next(1, diceSides + 1 );
            }
            return diceSum;
        }
    }
}
