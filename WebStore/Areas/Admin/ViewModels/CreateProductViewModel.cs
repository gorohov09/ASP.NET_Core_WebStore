namespace WebStore.Areas.Admin.ViewModels
{
    public class CreateProductViewModel
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Brand { get; set; }

        public string Section { get; set; }
    }
}
