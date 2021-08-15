using SeedPacket.Examples.Logic.Models;
using System;

namespace SeedPacket.Examples.Logic.Interfaces
{
    public interface ITeamsManager
    {
        FootballInfo GetGamesInfo( Random random = null, DateTime? openingSunday = null);
    }
}
