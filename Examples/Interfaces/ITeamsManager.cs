using Examples.Models;
using System;

namespace Examples.Interfaces
{
    public interface ITeamsManager
    {
        FootballInfo GetGamesInfo( Random random = null, DateTime? openingSunday = null);
    }
}
