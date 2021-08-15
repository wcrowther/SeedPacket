using System;
using System.Diagnostics;

namespace SeedPacket.Examples.Logic.Exceptions
{
    public class InvalidTimeZoneException : Exception
    {
        public InvalidTimeZoneException(string teamName) 
            : base($"Invalid TimeZone for {teamName}")
        {
            Debug.WriteLine(Message);
        }
    }
}
