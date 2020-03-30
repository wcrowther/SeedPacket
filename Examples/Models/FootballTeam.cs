using Examples.Generators;
using Examples.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Examples.Models
{
    public class FootballTeam
    {
        private int rank = 0;

        [XmlIgnore]
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

        [XmlIgnore]
        public int Rank
        {
            get { return rank == 0 ? TeamId : rank; }
            set { rank = value; }
        }

        [XmlIgnore]
        public List<ScheduleSlot> Schedule { get; set; } = new List<ScheduleSlot>().GetBaseSlots();

        public override string ToString()
        {
            return $"{Location} {Name} {Id}";
        }
    }
}
