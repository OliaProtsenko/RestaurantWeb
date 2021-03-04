using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Type { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public string Remarks { get; set; }
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Using> Usings { get; set; }
    }
}
