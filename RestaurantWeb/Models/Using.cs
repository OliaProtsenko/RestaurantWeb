using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace RestaurantWeb
{
    public partial class Using
    {
        public int Id { get; set; }
        [Display(Name ="Кількість (г)")]
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int DishId { get; set; }
        [Display(Name = "Страва")]
        public virtual Dish Dish { get; set; }
        [Display(Name = "Продукт")]
        public virtual Product Product { get; set; }
    }
}
