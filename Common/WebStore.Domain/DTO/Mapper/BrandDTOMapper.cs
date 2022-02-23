using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mapper
{
    public static class BrandDTOMapper
    {
        public static BrandDTO? ToDTO(this Brand? brand)
        {
            if (brand is null)
                return null;
            else
                return new BrandDTO
                {
                    Id = brand.Id,
                    Order = brand.Order,
                    Name = brand.Name,
                };
        }

        public static Brand? FromDTO(this BrandDTO? brand)
        {
            if (brand is null)
                return null;
            else
                return new Brand
                {
                    Id = brand.Id,
                    Order = brand.Order,
                    Name = brand.Name,
                };
        }

        public static IEnumerable<BrandDTO?> ToDTO(this IEnumerable<Brand?> brands) => brands.Select(ToDTO);

        public static IEnumerable<Brand?> FromDTO(this IEnumerable<BrandDTO?> brands) => brands.Select(FromDTO);
    }
}
