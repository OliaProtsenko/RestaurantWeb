using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Доступна к-сть (кг)")]
        public int? QuantityAvailabale { get; set; }
        [Display(Name = "Вжити до")]
        public DateTime? BestBefore { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Using> Usings { get; set; }
    }
}
