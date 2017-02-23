using System;
using System.Collections.Generic;

namespace MvcExamples.Models
{
	public class PatternDetail
	{
        public int PatternDetailId { get; set; }
        public int PatternId { get; set; }
        public int AbstractId { get; set; }
        public int SortOrder { get; set; }
        public virtual Pattern Pattern { get; set; }
	}
}

