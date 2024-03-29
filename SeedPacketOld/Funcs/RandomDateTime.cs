using SeedPacket.Interfaces;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static DateTime RandomDateTime (this IGenerator generator, int hoursBefore, int hoursAfter)
        {
            // BaseDateTime +- 2 years by hour
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return generator.BaseDateTime.AddHours(randomHours);
        }

        public static DateTime? RandomDateTimeNull (this IGenerator generator, int hoursBefore, int hoursAfter, int diceRange = 6)
        {
            // BaseDateTime +-2 years by hour is -17520, 17521
            int randomHours = generator.RowRandom.Next(hoursBefore, hoursAfter);

            return DiceRoll(generator, diceRange) == 1 ? (DateTime?)null : generator.BaseDateTime.AddHours(randomHours);
        }
    }
}
