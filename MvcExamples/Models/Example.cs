using System;
using System.Collections.Generic;

namespace MvcExamples.Models
{
    public class Example
    {
        private List<Item> properties;
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal NetWorth { get; set; }
        public decimal? Investments { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public Guid ExampleGuid { get; set; }
        public DateTime Created { get; set; }
        public int SelectedProperty { get; set; }
        public List<Item> Properties {
            get { return properties ?? new List<Item>(); }
            set { properties = value; }
        }
    }
}