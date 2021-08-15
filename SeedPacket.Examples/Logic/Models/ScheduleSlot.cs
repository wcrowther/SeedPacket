
namespace SeedPacket.Examples.Logic.Models
{
    public class ScheduleSlot
    {
        public int SeasonWeek { get; set; }

        public GameType GameType { get; set; } // In, Out, Division, Extra

        public override string ToString()
        {
            return $"Week {SeasonWeek} {GameType}";
        }
    }
}
