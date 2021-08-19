using System;
using AutoMapper;
using Machete.Domain;
using Machete.Web.Helpers;
using Machete.Web.ViewModel.Api;
using Newtonsoft.Json;

namespace Machete.Web.Maps.Api
{
    public class ReportDefinitionsMap : Profile
    {
        public ReportDefinitionsMap()
        {
            CreateMap<ReportDefinition, ReportDefinitionVM>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.columns, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.columnsJson)))
                .ForMember(v => v.inputs, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.inputsJson)))
                ;
            CreateMap<ReportDefinitionVM, ReportDefinition>()
                .ForMember(d => d.ID, opt => opt.MapFrom(v => v.id))
                .ForMember(d => d.inputsJson, opt => opt.MapFrom(v => JsonConvert.SerializeObject(v.inputs)))
                .ForMember(d => d.columnsJson, opt => opt.MapFrom(v => JsonConvert.SerializeObject(v.columns)))
                .ForMember(d => d.name, opt => opt.MapFrom(v => MapperHelpers.ToNameAsId(v.commonName)))
                ;
        }
    }
}
