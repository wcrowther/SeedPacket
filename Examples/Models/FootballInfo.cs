using Examples.Generators;
using System;
using System.Collections.Generic;

namespace Examples.Models
{
    public class FootballInfo
    {
        public Dictionary<int, List<FootballGame>> FootballWeeks { get; set; }

        public Dictionary<string, List<FootballGame>> TeamGames { get; set; }

        public string ElapsedTime { get; set; }
    }
}
