using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Web.ViewModel;
using Newtonsoft.Json;

namespace Machete.Web.Maps
{
    public class EmployersMap : MacheteProfile
    {
        public EmployersMap()
        {
            CreateMap<Service.DTO.EmployersList, ViewModel.Api.Employer>();
            CreateMap<Domain.Employer, ViewModel.Api.Employer>();
        }

    }
}