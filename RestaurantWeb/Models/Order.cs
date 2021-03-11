using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace RestaurantWeb
{
    public partial class Order
    {
        public int Id { get; set; }
        [Display(Name ="Дата замовлення")]
        public DateTime Date { get; set; }
        [Display(Name = "Вартість")]
        public int Price { get; set; }
        [Display(Name = "Кількість")]
        public int Amount { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public int RestaurantId { get; set; }
        public int Status { get; set; }
        [Display(Name = " Запланована дата виконання")]
        public DateTime PlanReturn { get; set; }
        [Display(Name = "Фактична дата виконання")]
        public string FactReturn { get; set; }
        [Display(Name = " Продукт")]
        public virtual Product Product { get; set; }
        [Display(Name = "Постачальник")]
        public virtual Provider Provider { get; set; }
        [Display(Name = "Ресторан")]
        public virtual Restaurant Restaurant { get; set; }
    }
}
