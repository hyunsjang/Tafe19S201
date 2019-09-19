using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    public class ShoppingList
    {
        [PrimaryKey]
        public string ItemID { get; set; }

        [NotNull]
        public string ShopName { get; set; }

        [NotNull]
        public string NameOfItem { get; set; }

        [NotNull]
        public string ShoppingDate { get; set; }

        [NotNull]
        public string PriceQuoted { get; set; }
    }
}
