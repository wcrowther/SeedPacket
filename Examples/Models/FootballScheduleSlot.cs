using System;
using System.ComponentModel.DataAnnotations;

namespace Examples.Models
{
    public class FootballScheduleSlot
    {
        public int StartYear { get; set; } // 2019, 2020, etc.

        public int Week { get; set; } // Week 1 - 17

        public DateTime SundayDate { get; set; }

        public DateTime GameDate { get; set; }

        public int DateOffset {
            get
            {   // -1 for Sat, -3 for Thurs, 1 for Mon

                if(SundayDate == null || GameDate == null)
                    return 0;

                return (GameDate - SundayDate).Days;
            }
        }  

        [StringLength(10)]
        public string GameType { get; set; } // In, Out, Division, Extra, Bye

        public FootballGame Game { get; set; }

	}
}
