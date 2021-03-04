using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Using
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int DishId { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Product Product { get; set; }
    }
}
