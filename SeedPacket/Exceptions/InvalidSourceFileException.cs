using System;

namespace SeedPacket.Exceptions
{
    public class InvalidSourceFileException : Exception
    {
        public InvalidSourceFileException(string source = "", string filepath = "") 
            : base($"Not able to retrieve valid {source} data from the supplied file path (\"{filepath}\").")
        {
        }
    }
}
