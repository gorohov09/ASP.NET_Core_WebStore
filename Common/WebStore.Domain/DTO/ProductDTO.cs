﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public SectionDTO Section { get; set; }

        public BrandDTO Brand { get; set; }
    }

    public class SectionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }
    }

    public class BrandDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }

    public static class BrandDTOMapper
    {
        public static BrandDTO? ToDTO(this Brand? brand) => brand is null
            ? null
            : new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
            };

        public static Brand? FromDTO(this BrandDTO? brandDTO) => brandDTO is null
            ? null
            : new Brand
            {
                Id = brandDTO.Id,
                Name = brandDTO.Name,
                Order = brandDTO.Order,
            };

        public static IEnumerable<BrandDTO?> ToDTO(this IEnumerable<Brand?> brands) => brands.Select(ToDTO);

        public static IEnumerable<Brand?> FromDTO(this IEnumerable<BrandDTO?> brands) => brands.Select(FromDTO);
    }

    public static class SectionDTOMapper
    {
        public static SectionDTO? ToDTO(this Section? section) => section is null
            ? null
            : new SectionDTO
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = section.ParentId,
            };

        public static Section? FromDTO(this SectionDTO? sectionDTO) => sectionDTO is null
            ? null
            : new Section
            {
                Id = sectionDTO.Id,
                Name = sectionDTO.Name,
                Order = sectionDTO.Order,
                ParentId = sectionDTO.ParentId,
            };

        public static IEnumerable<SectionDTO?> ToDTO(this IEnumerable<Section?> sections) => sections.Select(ToDTO);

        public static IEnumerable<Section?> FromDTO(this IEnumerable<SectionDTO?> sections) => sections.Select(FromDTO);
    }

    public static class ProductDTOMapper
    {
        public static ProductDTO? ToDTO(this Product? product) => product is null
            ? null
            : new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Brand = product.Brand.ToDTO(),
                Section = product.Section.ToDTO(),
            };

        public static Product? FromDTO(this ProductDTO? productDTO) => productDTO is null
            ? null
            : new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                ImageUrl = productDTO.ImageUrl,
                Order = productDTO.Order,
                Price = productDTO.Price,
                Brand = productDTO.Brand.FromDTO(),
                Section = productDTO.Section.FromDTO(),
                BrandId = productDTO.Brand.Id,
                SectionId = productDTO.Section.Id,
            };

        public static IEnumerable<ProductDTO?> ToDTO(this IEnumerable<Product?> products) => products.Select(ToDTO);
        public static IEnumerable<Product?> FromDTO(this IEnumerable<ProductDTO?> products) => products.Select(FromDTO);
    }
}
