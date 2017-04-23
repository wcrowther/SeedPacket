using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static DateTime RandomDateTime (IGenerator generator, int hoursBefore, int hoursAfter)
        {
            // BaseDateTime +- 2 years by hour
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return generator.BaseDateTime.AddHours(randomHours);
        }

        public static DateTime? RandomDateTimeNull (IGenerator generator, int hoursBefore, int hoursAfter, int diceRange = 7)
        {
            // BaseDateTime +-2 years by hour is -17521, 17521
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return DiceRoll(generator, diceRange) == 1 ? (DateTime?)null : generator.BaseDateTime.AddHours(randomHours);
        }
    }
}
