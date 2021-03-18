using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name ="Назва")]
        public string Name { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
