using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        /// <summary>Simulates rolling a x-sided dice y number of times where x is diceSides and y is numberOfDice.<br/><br/> 
        /// Use a higher numberOfDice to decrease chances in a linear progression.<br/>
        /// Increasing numberOfDice above 1 creates a bell-curve of probability.<br/><br/>
        /// ie: rolling 3 6-sided dice (3d6) returns from 3 to 18 (with 7 the most likely possiblity)
        /// </summary>
        /// <param name="generator">IGenerator</param>
        /// <param name="diceSides">Number of sides of dice ie: standard 6-sided, 10-sided, etc;</param>
        /// <param name="numberOfDice">How many dice to role</param>
        /// <returns>int</returns>
        public static int DiceRoll (IGenerator generator, int diceSides = 6, int numberOfDice = 1)
        {
            int diceSum = 0;

            for (int i = 1; i <= numberOfDice; i++)
            {
                // Add +1 as Random takes values less than max ie: 1,7 returns 1-6
                diceSum += generator.RowRandom.Next(1, diceSides + 1 );
            }
            return diceSum;
        }
    }
}
