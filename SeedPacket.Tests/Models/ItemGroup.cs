using System.Collections.Generic;

namespace SeedPacket.Tests.Model
{
    public class ItemGroup
    {
        public int GroupId { get; set; }

        public List<Item> ItemList { get; set; } = new List<Item>();

        public override string ToString()
        {
            return $"{GroupId} Count: {ItemList.Count}";
        }
    }
}
