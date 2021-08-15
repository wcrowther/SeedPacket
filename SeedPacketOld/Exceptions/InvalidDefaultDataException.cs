using System;

namespace SeedPacket.Exceptions
{
    public class InvalidDefaultDataException : Exception
    {
        public InvalidDefaultDataException() 
            : base($"Not able to retrieve valid default data from the embedded resource file.")
        {
        }
    }
}
