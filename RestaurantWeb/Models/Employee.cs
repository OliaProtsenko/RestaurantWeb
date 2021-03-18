using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace RestaurantWeb
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Display(Name="Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Посада")]
        public string Position { get; set; }
        [Display(Name = "Дата народження")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Заробітна плата")]
        public int Salary { get; set; }
        [Display(Name = "Домашня адреса")]
        public string HomeAddress { get; set; }
        public int RestaurantId { get; set; }
        [Display(Name = "Ресторан")]
        public virtual Restaurant Restaurant { get; set; }
    }
}
