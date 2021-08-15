using System;

namespace SeedPacket.Examples.Logic.Models
{
    public class FootballGame
    {
        public int GameId { get; set; }

        public GameType GameType { get; set; } // ie conference home, conference away, bye, etc.

        public int SeasonStartYear { get; set; } // ie 2020

        public int SeasonWeek { get; set; } = -1; // Week 1-17 (16 games, 1 bye)

        public DateTime GameDate { get; set; }

        public FootballTeam HomeTeam { get; set; }

        public FootballTeam AwayTeam { get; set; }

        public override string ToString()
        {
            return $"Week {SeasonWeek}: {HomeTeam} vs {AwayTeam}";
        }

    }
}
