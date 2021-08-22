using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Models
{
    public class Word
	{
	    public Word()
		{
            this.Links = new List<Link>();
		}

		public int WordId { get; set; }
		public string WordString { get; set; }
		public Nullable<decimal> Commonality { get; set; }
		public string PartOfSpeech { get; set; }
		public bool Archived { get; set; }
		public virtual ICollection<Link> Links { get; set; }
	}
}

