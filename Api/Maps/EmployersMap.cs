using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Machete.Api.ViewModel;

namespace Machete.Api.Maps
{
    public class EmployersMap : MacheteProfile
    {
        public EmployersMap()
        {
            CreateMap<Service.DTO.EmployersList, Employer>();
            CreateMap<Domain.Employer, Employer>();
        }

    }
}