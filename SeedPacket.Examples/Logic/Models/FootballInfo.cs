using System;
using System.Collections.Generic;
using SeedPacket.Examples.Logic.Helpers;

namespace SeedPacket.Examples.Logic.Models
{
    public class FootballInfo
    {
        public FootballInfo(DateTime? openingDate = null)
        {
            if (openingDate != null)
                OpeningSunday = openingDate.Value.SecondSundayInSeptember();
        }

        public DateTime OpeningSunday { get; set; } = DateTime.Now.SecondSundayInSeptember();

        public List<FootballTeam> Teams { get; set; } = new List<FootballTeam>();

        public List<FootballGame> Games { get; set; } = new List<FootballGame>();

        public string ElapsedTime { get; set; }

   }
}
