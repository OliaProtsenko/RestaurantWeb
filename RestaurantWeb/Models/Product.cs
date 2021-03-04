using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
            Usings = new HashSet<Using>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? QuantityAvailabale { get; set; }
        public DateTime? BestBefore { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Using> Usings { get; set; }
    }
}
