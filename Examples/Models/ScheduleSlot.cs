using System.ComponentModel.DataAnnotations;

namespace Examples.Models
{
    public class ScheduleSlot
    {
        public bool IsHomeGame { get; set; } 

        [StringLength(10)]
        public string GameType { get; set; } // In, Out, Division, Extra, Bye
	}
}
