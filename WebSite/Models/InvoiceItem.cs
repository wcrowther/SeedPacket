using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}