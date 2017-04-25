using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SeedPacket.Model
{
    public class ItemGroup
    {
        public int GroupId { get; set; }
        public List<Item> ItemList { get; set; }
    }
}
