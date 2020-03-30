using Examples.Models;
using System;

namespace Examples.Interfaces
{
    public interface ITeamsManager
    {
        FootballInfo GetGamesInfo(DateTime seasonStart, Random random = null);
    }
}
