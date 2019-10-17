using System;

namespace SeedPacket.Exceptions
{
    public class InvalidSourceException : Exception
    {
        public InvalidSourceException(string sourceType = "", string source = null) 
            : base($"Not able to retrieve valid {sourceType} data from the supplied source {source ?? "string"}.")
        {
        }
    }
}
