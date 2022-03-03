using System;
using System.Collections.Generic;

namespace WebApplication10.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public byte[] ProductImg { get; set; }
        public int? Category { get; set; }

        public virtual Category CategoryNavigation { get; set; }
    }
}
