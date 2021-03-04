using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
            Employees = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Site { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
