﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput (DisplayValue = false)]
        public int Id { get; set; }

        [Display (Name = "Фамилия")]
        [Required (ErrorMessage = "Фамилия обязательна")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 255")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 255")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(255, ErrorMessage = "Длина должна быть до 255")]
        [RegularExpression(@"(([А-ЯЁ][а-яё]+)|([A-Z][a-z]+))?", ErrorMessage = "Ошибка формата")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Зарплата")]
        public int Salary { get; set; }
    }
}
