﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcExamples.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime Created { get; set; }
        public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}