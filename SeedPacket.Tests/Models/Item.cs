using System;

namespace SeedPacket.Tests.Model
{
    public class Item
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int Number { get; set; }

        public DateTime? Created { get; set; }

        public override string ToString()
        {
            return $"{ItemName} ({ItemId})";
        }
    }
}
