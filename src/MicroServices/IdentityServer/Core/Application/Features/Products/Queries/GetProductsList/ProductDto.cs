﻿using STS.Application.Common.Mappings;
using STS.Domain.Entities;
using AutoMapper;

namespace STS.Application.Features.Products.Queries.GetProductsList
{
    public class ProductDto : IMapFrom<Product>
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? SupplierId { get; set; }

        public string SupplierCompanyName { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool Discontinued { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(d => d.SupplierCompanyName, opt => opt.MapFrom(s => s.Supplier != null ? s.Supplier.CompanyName : string.Empty))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.CategoryName : string.Empty));
        }
    }
}