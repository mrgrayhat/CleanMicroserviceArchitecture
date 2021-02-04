using System;
using System.ComponentModel;
using AutoMapper;
using LogModule.Application.Features.Logs.Queries.GetAllLogs;

namespace LogModule.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //TODO: Fix timestamp mappings
            CreateMap<Domain.Entities.Log, GetAllLogsViewModel>()
                /*.ReverseMap()*/;
            //CreateMap<CreateLogCommand, Domain.Entities.Log>();
            CreateMap<GetAllLogsQuery, GetAllLogsParameter>();
        }
    }
}
