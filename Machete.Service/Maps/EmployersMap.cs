using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service
{
    public class EmployersMap : Profile
    {
        public EmployersMap()
        {
            CreateMap<Domain.Employer, Service.DTO.EmployersList>();
        }
    }
}
