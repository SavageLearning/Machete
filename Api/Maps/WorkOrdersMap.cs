﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Api.ViewModel;
using Newtonsoft.Json;
using System.Globalization;

namespace Machete.Api.Maps
{
    public class WorkOrdersMap : MacheteProfile
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public WorkOrdersMap()
        {
            CreateMap<Service.DTO.WorkOrdersList, WorkOrder>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            CreateMap<Domain.WorkOrder, WorkOrder>()
                .ForMember(v => v.dateTimeofWork, opt => opt.MapFrom(d => d.dateTimeofWork.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.datecreated, opt => opt.MapFrom(d => d.datecreated.ToString("o", CultureInfo.InvariantCulture)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => d.dateupdated.ToString("o", CultureInfo.InvariantCulture)))
                ;
            //CreateMap<Domain.WorkOrder, Service.DTO.WorkOrdersList>()
            //    .ForMember(v => v.workers, opt => opt.MapFrom(d => d.workerRequests ?? new List<Domain.WorkerRequest>()))
            //    ;
            CreateMap<WorkOrder, Domain.WorkOrder>()
                .ForMember(v => v.datecreated, opt => opt.Ignore())
                .ForMember(v => v.dateupdated, opt => opt.Ignore())
                .ForMember(v => v.createdby, opt => opt.Ignore())
                .ForMember(v => v.updatedby, opt => opt.Ignore())
                .ForMember(v => v.ID, opt => opt.Ignore())
                ;
        }

    }
}