using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin.ViewModels
{
    public class CreateProductViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Order { get; set; }

        public string Section { get; set; }

        public string? Brand { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
    }
}
