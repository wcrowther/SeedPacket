using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examples.Models
{
    public class Song
    {
        public int Ranking { get; set; }
        public string SongTitle { get; set; }
        public string AlbumName { get; set; }
        public string Artist { get; set; }
        public DateTime Released { get; set; }
    }
}