using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mapper
{
    public static class SectionDTOMapper
    {
        public static SectionDTO? ToDTO(this Section? section)
        {
            if (section is null)
                return null;
            else
                return new SectionDTO
                {
                    Id = section.Id,
                    Order = section.Order,
                    Name = section.Name,
                    ParentId = section.ParentId,
                };
        }

        public static Section? FromDTO(this SectionDTO? section)
        {
            if (section is null)
                return null;
            else
                return new Section
                {
                    Id = section.Id,
                    Order = section.Order,
                    Name = section.Name,
                    ParentId= section.ParentId,
                };
        }

        public static IEnumerable<SectionDTO?> ToDTO(this IEnumerable<Section?> sections) => sections.Select(ToDTO);

        public static IEnumerable<Section?> FromDTO(this IEnumerable<SectionDTO?> sections) => sections.Select(FromDTO);
    }
}
