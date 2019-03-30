using System;

namespace Tests.SeedPacket.Core.Model
{
    public class Unknown
    {
        public int UnknownId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{Name} ({UnknownId})";
        }
    }
}
