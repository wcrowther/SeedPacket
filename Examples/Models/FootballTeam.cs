using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examples.Models
{
    public class FootballTeam
    {
        public string Location { get; set; }

        public string Name { get; set; }

        public string Conference { get; set; }

        public string Division { get; set; }

        public override string ToString()
        {
            return $"{Location} {Name}";
        }
    }
}