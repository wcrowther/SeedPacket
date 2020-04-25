using System;

namespace Examples.Exceptions
{
    public class InvalidWeekSequenceLength : Exception
    {
        public InvalidWeekSequenceLength()
            : base("WeekSequence must contain 17 comma-separated entries for the type of game played on a particular week in the season. " +
                   "16 weeks of games are generated. The games assigned to a team's bye week all get played on the Bye(b) week in the sequence.")
        {
        }
    }
}
