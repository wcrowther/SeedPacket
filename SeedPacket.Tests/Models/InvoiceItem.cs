using System;

namespace SeedPacket.Tests.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }

        public int InvoiceId { get; set; }

        public decimal Fee { get; set; }

        public string Product { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{InvoiceItemId} Product: {Product} Fee: {Fee}";
        }
    }
}
