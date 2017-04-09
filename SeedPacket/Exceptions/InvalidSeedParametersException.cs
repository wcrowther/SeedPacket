using System;

namespace SeedPacket.Exceptions
{
    public class InvalidSeedParametersException : Exception
    {
        public InvalidSeedParametersException () 
            : base("Invalid Parameters: SeedBegin must be less than or equal to SeedEnd.")
        {
        }
    }
}
