using System;

namespace SeedPacket.Exceptions
{
    public class NoSourceException : Exception
    {
        public NoSourceException() 
            : base("You MUST either define the path to a source file OR explicitly pass in a string.")
        {
        }
    }
}
