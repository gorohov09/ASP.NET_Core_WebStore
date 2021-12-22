using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }

        [Display(Name ="Фамилияя")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(255, MinimumLength =2, ErrorMessage = "Длина должна быть от 2 до 255 символов")]
        //[RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)")
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 255 символов")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(255, ErrorMessage = "Длина должна быть до 255 символов")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Зарплата")]
        public int Salary { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LastName.Length > 100)
                yield return new ValidationResult("Длина фамилии больше 100 символов");
            yield return ValidationResult.Success;
        }
    }
}
