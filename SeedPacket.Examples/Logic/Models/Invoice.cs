using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int AccountId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime Created { get; set; }

        public virtual List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        
        public override string ToString()
        {
            return $"{InvoiceId} AccountId: {AccountId}";
        }
    }
}