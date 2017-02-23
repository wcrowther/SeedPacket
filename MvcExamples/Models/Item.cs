using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcExamples.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime Created { get; set; }
    }
}