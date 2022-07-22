namespace WebStore.Domain.ViewModels
{
    public class SelectableBrandsViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }

        public int? BrandId { get; set; }
    }
}
