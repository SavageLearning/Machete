using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.ViewModel;
using Newtonsoft.Json;

namespace Machete.Web.Maps
{
    public class ReportDefinitionProfile : MacheteProfile
    {
        public ReportDefinitionProfile()
        {
            CreateMap<Domain.ReportDefinition, ViewModel.ReportDefinition>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.columns, opt => opt.MapFrom(d => JsonConvert.DeserializeObject(d.columnsJson)))
                ;
        }

    }
}