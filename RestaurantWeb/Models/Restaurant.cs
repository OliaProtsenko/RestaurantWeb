using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name ="Назва")]
        public string Name { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Сайт")]
        public string Site { get; set; }
        public string? GeoLong { get; set; }
        public string? GeoLat { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
