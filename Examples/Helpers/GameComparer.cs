
using Examples.Models;
using System.Collections.Generic;

namespace Examples.Helpers
{
    public class GameComparer : IEqualityComparer<FootballGame>
    {
        public bool Equals(FootballGame x, FootballGame y)
        {
            if (x.AwayTeam.Equals(y.HomeTeam) || x.HomeTeam.Equals(y.AwayTeam))
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(FootballGame obj)
        {
            return obj.GetHashCode();
        }
    }
}
