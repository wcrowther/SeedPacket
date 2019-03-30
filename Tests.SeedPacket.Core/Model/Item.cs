using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tests.SeedPacket.Core.Model
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
