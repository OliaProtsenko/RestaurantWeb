using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantWeb
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Salary { get; set; }
        public string HomeAddress { get; set; }
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
