using System;

namespace SeedPacket.Exceptions
{
    public class InvalidTildePathException : Exception
    {
        public InvalidTildePathException(string filepath) 
            : base($"Tilde root paths like ({filepath}) are not currently supported. " +
                    "Please use a full physical file path instead.")
        {
        }
    }
}
