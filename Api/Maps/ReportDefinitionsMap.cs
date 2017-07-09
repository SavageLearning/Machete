using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
using Newtonsoft.Json;

namespace Machete.Api.Maps
{
    public class ReportDefinitionsMap : MacheteProfile
    {
        public ReportDefinitionsMap()
        {
            CreateMap<Domain.ReportDefinition, ReportDefinition>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.columns, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.columnsJson)))
                .ForMember(v => v.inputs, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.inputsJson)))
                ;
            CreateMap<Service.DTO.SearchOptions, Data.DTO.SearchOptions>()
                .ForMember(v => v.beginDate, opt => opt.MapFrom(d => d.beginDate ?? new DateTime(1753, 1, 1)))
                .ForMember(v => v.endDate, opt => opt.MapFrom(d => d.endDate ?? DateTime.MaxValue))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.dwccardnum ?? 0))
            ;
        }

    }
}