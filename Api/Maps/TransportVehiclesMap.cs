using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.Maps
{
    public class TransportVehiclesMap : MacheteProfile
    {
        public TransportVehiclesMap()
        {
            CreateMap<Domain.TransportVehicle, ViewModel.TransportVehicle>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                ;
        }
    }
    public class TransportVehiclesScheduleMap : MacheteProfile
    {
        public TransportVehiclesScheduleMap()
        {
            CreateMap<Domain.TransportVehicleSchedule, ViewModel.TransportVehicleSchedule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.TransportVehicleName, opt => opt.MapFrom(d => d.TransportVehicle.Name))
                ;
        }
    }
}