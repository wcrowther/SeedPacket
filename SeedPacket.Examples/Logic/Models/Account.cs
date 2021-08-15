using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public DateTime Created { get; set; }

        public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();

        public override string ToString()
        {
            return $"{AccountName} AccountId: {AccountId}";
        }
    }
}
