using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Areas.Admin.ViewModels
{
    public class EditProductViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        public int Order { get; set; }

        [HiddenInput]
        public int SectionId { get; set; }

        [Display(Name = "Категория")]
        public string Section { get; set; }

        [HiddenInput]
        public int? BrandId { get; set; }

        [Display(Name = "Бренд")]
        public string? Brand { get; set; }

        [Display(Name = "Картинка")]
        public string ImageUrl { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }
    }
}
