using System;

namespace SeedPacket.Examples.Logic.Exceptions
{
    public class InvalidWeekSequenceLength : Exception
    {
        public InvalidWeekSequenceLength()
            : base("WeekSequence must contain 16 comma-separated entries for the type of game played on a particular week in the season. " +
                   "Season week 17 is determined by the teams having a Bye week that is pushed to the last week of the season.")
        {
        }
    }
}
