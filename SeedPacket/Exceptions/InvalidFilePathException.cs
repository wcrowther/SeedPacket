using System;

namespace SeedPacket.Exceptions
{
    public class InvalidFilePathException : Exception
    {
        public InvalidFilePathException(string filepath = "") 
            : base($"Not able to find the supplied file path (\"{filepath}\").")
        {
        }
    }
}
