using System;

namespace SeedPacket.Exceptions
{
    public class InvalidSourceStringException : Exception
    {
        public InvalidSourceStringException(string source = "") 
            : base($"Not able to retrieve valid {source}data from the supplied source string.")
        {
        }
    }
}
