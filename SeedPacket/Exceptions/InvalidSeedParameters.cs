using System;

namespace SeedPacket.Exceptions
{
    public class InvalidSeedParameters : Exception
    {
        public InvalidSeedParameters () 
            : base("Invalid Parameters: SeedBegin must be less than or equal to SeedEnd.")
        {
        }
    }
}
