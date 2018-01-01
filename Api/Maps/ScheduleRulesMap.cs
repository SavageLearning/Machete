using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.ViewModels;

namespace Machete.Api.Maps
{
    public class ScheduleRulesMap : MacheteProfile
    {
        public ScheduleRulesMap()
        {
            CreateMap<Domain.ScheduleRule, ScheduleRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID));

        }
    }
}