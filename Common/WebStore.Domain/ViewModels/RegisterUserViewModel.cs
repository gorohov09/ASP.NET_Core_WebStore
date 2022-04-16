using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Фамилия пользователя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Display(Name = "О себе")]
        public string AboutMySelf { get; set; }

        [Required]
        [Display(Name = "Логин пользователя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
