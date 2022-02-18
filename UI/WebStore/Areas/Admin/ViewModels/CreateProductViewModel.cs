using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Areas.Admin.ViewModels
{
    public class CreateProductViewModel
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [HiddenInput]
        public int Order { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Картинка")]
        public string ImageUrl { get; set; }

        [Display(Name = "Бренд")]
        public string Brand { get; set; }

        [Display(Name = "Категория")]
        public string Section { get; set; }
    }
}
