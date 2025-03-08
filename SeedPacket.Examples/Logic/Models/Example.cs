using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Models
{
    public class Company
    {
        private List<Item> products;
        
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Ceo { get; set; }

        public decimal? NetWorth { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public Guid ExampleGuid { get; set; }

        public DateTime Created { get; set; }

        public int SelectedProduct { get; set; }

        public List<Item> Products 
        {
            get { return products ?? []; }
            set { products = value; }
        }
        public string Notes { get; set; }

        public List<string> BodyCopy { get; set; }
    }
}