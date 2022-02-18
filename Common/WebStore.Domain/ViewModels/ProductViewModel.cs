using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя товара")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Картинка")]
        public string ImageUrl { get; set; }

        [Display(Name = "Бренд")]
        public string? Brand { get; set; }

        [Display(Name = "Категория")]
        public string Section { get; set; }
    }
}
