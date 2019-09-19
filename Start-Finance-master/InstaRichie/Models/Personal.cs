using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    public class Personal
    {
        [PrimaryKey]
        public string ID { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        [NotNull]
        public string DOB { get; set; }

        [NotNull]
        public string Gender { get; set; }

        [NotNull]
        public string Email { get; set; }

        [NotNull]
        public string MobileNum { get; set; }
    }
}
