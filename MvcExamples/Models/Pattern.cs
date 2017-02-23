using System;
using System.Collections.Generic;

namespace MvcExamples.Models
{
	public class Pattern
	{
        public Pattern()
        {
            this.PatternDetails = new List<PatternDetail>();
        }

        public int PatternId { get; set; }
        public string PatternName { get; set; }
        public string Match { get; set; }
        public string Note { get; set; }
        public int MemberCount { get; set; }
        public int SortOrder { get; set; }
        public bool Archived { get; set; }  
        public virtual ICollection<PatternDetail> PatternDetails { get; set; }
	}
}

