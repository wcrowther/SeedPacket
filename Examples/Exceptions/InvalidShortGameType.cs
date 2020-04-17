using System;

namespace Examples.Exceptions
{
    public class InvalidShortGameType : Exception
    {
        public InvalidShortGameType() 
            : base("Invalid short GameType. Use 'd' for Division, 'i' for InConference, 'e' for ExtraInConference, 'o' for OutOfConference.")
        {
        }
    }
}
