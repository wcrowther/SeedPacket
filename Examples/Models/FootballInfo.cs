using Examples.Generators;
using System;
using System.Collections.Generic;
using Examples.Helpers;
using System.Web.Mvc;

namespace Examples.Models
{
    public class FootballInfo
    {

        public FootballInfo(DateTime? openingDate =  null)
        {
            if (openingDate != null)
                OpeningSunday = openingDate.Value.SecondSundayInSeptember();
        }

        public Dictionary<int, List<FootballGame>> FootballWeeks { get; set; }

        public Dictionary<string, List<FootballGame>> TeamGames { get; set; }

        public string ElapsedTime { get; set; }

        public DateTime OpeningSunday { get; set; } = DateTime.Now.SecondSundayInSeptember();

        public List<SelectListItem> OpeningSundayList => new List<SelectListItem>().GetOpeningSundayList();
   }
}
