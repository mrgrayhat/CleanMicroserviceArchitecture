using System.Collections.Generic;
using AutoMapper;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Features.Contents.Commands.CreateContent;
using StorageManagement.Application.Features.Contents.Queries.GetAllContents;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<List<Item>, List<ItemDto>>().ReverseMap();

            CreateMap<GetAllContentsQuery, GetAllContentsParameter>();

            CreateMap<CreateContentCommand, Item>();

        }
    }
}
