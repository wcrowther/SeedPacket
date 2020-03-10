using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examples.Models
{
    public class FootballTeam
    {
        private int rank = 0;

        public string Id
        {
            get { return $"{ConfId}.{DivId}.{TeamId}"; }
        }

        public string Location { get; set; }

        public string Name { get; set; }

        public int ConfId { get; set; }

        public string Conference { get; set; }

        public int DivId { get; set; }

        public string Division { get; set; }

        public int TeamId { get; set; }

        public int Rank
        {
            get { return rank == 0 ? TeamId : rank; }
            set { rank = value; }
        }


        public List<FootballScheduleSlot> Schedule { get; set; } = new List<FootballScheduleSlot>();

        public override string ToString()
        {
            return $"{Location} {Name} {Id}";
        }
    }
}
