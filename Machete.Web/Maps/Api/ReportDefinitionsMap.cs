using System;
using AutoMapper;
using Machete.Domain;
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
        }
    }
}
