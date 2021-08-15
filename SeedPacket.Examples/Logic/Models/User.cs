using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public string Notes { get; set; }

        public List<string> BodyCopy { get; set; }
    }
}