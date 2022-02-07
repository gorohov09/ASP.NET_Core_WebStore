using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin.ViewModels
{
    public class EditProductViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        [HiddenInput]
        public int SectionId { get; set; }

        public string Section { get; set; }

        [HiddenInput]
        public int? BrandId { get; set; }

        public string? Brand { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
    }
}
