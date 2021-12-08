using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RestaurantWeb
{
    public partial class Dish
    {
        public Dish()
        {
            Usings = new HashSet<Using>();
        }

        public int Id { get; set; }
        [Display(Name = "Фото")]
        public string Image { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name="Страва")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name="Тип")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Вага")]
        public int Weight { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ціна")]
        public int Price { get; set; }
        [Display(Name = "Примітки")]
        public string Remarks { get; set; }
        
        public int RestaurantId { get; set; }
        [Display(Name = "Ресторан")]
    
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Using> Usings { get; set; }
    }
}
