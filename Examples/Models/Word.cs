using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace Examples.Models
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

