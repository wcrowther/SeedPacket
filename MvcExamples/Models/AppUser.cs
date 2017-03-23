using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcExamples.Models
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Item> Items { get; set; }
        public DateTime Created { get; set; }
    }
}