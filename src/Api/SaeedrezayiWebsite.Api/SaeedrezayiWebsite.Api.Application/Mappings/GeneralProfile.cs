using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SaeedrezayiWebsite.Api.Application.Features.Products.Commands.CreateProduct;
using SaeedrezayiWebsite.Api.Application.Features.Products.Queries.GetAllProducts;
using SaeedrezayiWebsite.Api.Domain.Entities;

namespace SaeedrezayiWebsite.Api.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
