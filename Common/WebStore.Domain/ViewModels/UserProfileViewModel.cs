using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModels
{
    public class UserProfileViewModel
    {
        [Display(Name = "Фамилия пользователя")]
        public string? LastName { get; set; }

        [Display(Name = "Имя пользователя")]
        public string? FirstName { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }

        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        [Display(Name = "О себе")]
        public string? AboutMySelf { get; set; }

    }
}
