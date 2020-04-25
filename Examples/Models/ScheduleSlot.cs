
namespace Examples.Models
{
    public class ScheduleSlot
    {
        public int SeasonWeek { get; set; }

        public GameType GameType { get; set; } // Intra, Out, Division, Extra

        public override string ToString()
        {
            return $"Week {SeasonWeek} {GameType}";
        }
    }
}
