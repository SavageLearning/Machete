using System;
using AutoMapper;
using Newtonsoft.Json;
using ReportDefinitionViewModel = Machete.Web.ViewModel.Api.ReportDefinition;

namespace Machete.Web.Maps.Api
{
    public class ReportDefinitionsMap : Profile
    {
        public ReportDefinitionsMap()
        {
            CreateMap<Domain.ReportDefinition, ReportDefinitionViewModel>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.columns, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.columnsJson)))
                .ForMember(v => v.inputs, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.inputsJson)))
                ;
            CreateMap<Service.DTO.SearchOptions, Data.DTO.SearchOptions>()
                .ForMember(v => v.beginDate, opt => opt.MapFrom(d => d.beginDate ?? new DateTime(1753, 1, 1))) // SQL Server epoch
                .ForMember(v => v.endDate, opt => opt.MapFrom(d => d.endDate ?? DateTime.MaxValue))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.dwccardnum ?? 0))
            ;
        }
    }
}
