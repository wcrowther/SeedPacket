using System;

namespace Examples.Exceptions
{
    public class InvalidWeekSequence : Exception
    {
        public InvalidWeekSequence()
            : base("WeekSequence must contain 6 'd' Division games, 4 'i' InConference games, 2 'e' ExtraInConference games, and 4 'o' OutOfConference games.")
        {
        }
    }
}
