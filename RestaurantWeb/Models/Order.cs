using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public int RestaurantId { get; set; }
        public int Status { get; set; }
        public DateTime PlanReturn { get; set; }
        public string FactReturn { get; set; }

        public virtual Product Product { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
