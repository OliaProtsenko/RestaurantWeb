using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Provider
    {
        public Provider()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
