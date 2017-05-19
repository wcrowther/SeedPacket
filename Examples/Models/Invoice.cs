using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examples.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int AccountId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime Created { get; set; }
        public virtual List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}