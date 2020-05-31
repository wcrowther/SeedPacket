using System;
using System.Collections.Generic;
using Examples.Helpers;

namespace Examples.Models
{
    public class FootballInfo
    {

        public FootballInfo(DateTime? openingDate = null)
        {
            if (openingDate != null)
                OpeningSunday = openingDate.Value.SecondSundayInSeptember();
        }

        public DateTime OpeningSunday { get; set; } = DateTime.Now.SecondSundayInSeptember();

        // public Dictionary<string, List<FootballGame>> TeamGames { get; set; }

        // public Dictionary<string, List<FootballGame>> FootballWeeks { get; set; }

        public List<FootballTeam> Teams { get; set; } = new List<FootballTeam>();

        public List<FootballGame> Games { get; set; } = new List<FootballGame>();

        public string ElapsedTime { get; set; }

        //public List<DateTime> OpeningSundayList => new List<DateTime>().GetOpeningSundayList();
   }
}
