﻿using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    public class Contact
    {
        [PrimaryKey]
        public string ContactID { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        [NotNull]
        public string CompanyName { get; set; }

        [NotNull]
        public string MobilePhone { get; set; }
    }
}
