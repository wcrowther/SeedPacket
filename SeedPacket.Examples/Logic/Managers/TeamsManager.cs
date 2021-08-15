using Microsoft.AspNetCore.Hosting;
using SeedPacket.Examples.Logic.Generators;
using SeedPacket.Examples.Logic.Interfaces;
using SeedPacket.Examples.Logic.Models;
using System;
using System.Diagnostics;

namespace SeedPacket.Examples.Logic.Managers
{
    public class TeamsManager : ITeamsManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamsManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public FootballInfo GetGamesInfo(Random random = null, DateTime? openingDate = null)
        {
            var footballInfo = new FootballInfo(openingDate);
            string footballSource = $"{_webHostEnvironment.ContentRootPath}/Logic/SourceFiles/FootballSource.xml";             

            var stopwatch = Stopwatch.StartNew();

            var gen = new FootballGenerator (footballInfo.OpeningSunday, footballSource, random);

            stopwatch.Stop();

            footballInfo.Teams = gen.Teams; 
            footballInfo.Games = gen.Games;       

            footballInfo.ElapsedTime    = stopwatch.Elapsed.TotalSeconds.ToString("#.0000");
            Debug.WriteLine($"ElapsedTime: { footballInfo.ElapsedTime }");

            return footballInfo;
        }
    }
}
