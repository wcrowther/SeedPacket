using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examples.Models
{
    public class FootballGame
    {
        public FootballTeam HomeTeam { get; set; }

        public FootballTeam AwayTeam { get; set; }

        public override string ToString()
        {
            return $"{HomeTeam} vs {AwayTeam}";
        }
    }
}