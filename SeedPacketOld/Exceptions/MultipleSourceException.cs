using System;

namespace SeedPacket.Exceptions
{
    public class MultipleSourceException : Exception
    {
        public MultipleSourceException()
            : base("You can only define one source, either the path to a file OR explicitly pass in a string.")
        {
        }
    }
}
