using AutoMapper;
using Machete.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Domain.Person, Service.DTO.PersonList>()
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => d.Worker.dwccardnum))
                .ForMember(v => v.workerStatus, opt => opt.MapFrom(d => d.Worker.memberStatus))

                ;
            CreateMap<Domain.Person, ViewModel.Person>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Person/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                ;
            CreateMap<Service.DTO.PersonList, ViewModel.PersonList>()
                .ForMember(v => v.tabref, opt => opt.MapFrom(d => "/Person/Edit/" + Convert.ToString(d.ID)))
                .ForMember(v => v.tablabel, opt => opt.MapFrom(d => d.firstname1 + ' ' + d.lastname1))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.active, opt => opt.MapFrom(d => d.active ? Shared.True : Shared.False))
                .ForMember(v => v.status, opt => opt.MapFrom(d => d.active))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => Convert.ToString(d.ID)))
                .ForMember(v => v.dateupdated, opt => opt.MapFrom(d => Convert.ToString(d.dateupdated)))
                ;
        }
    }
    //var result = from p in list.query
    //             select new
    //             {
    //                 tabref = "/Person/Edit/" + Convert.ToString(p.ID),
    //                 tablabel = p.firstname1 + ' ' + p.lastname1,
    //                 dwccardnum = p.Worker == null ? "" : p.Worker.dwccardnum.ToString(),
    //                 active = p.active ? Shared.True : Shared.False,
    //                 status = p.active,
    //                 workerStatus = p.Worker == null ? "Not a worker" : lcache.textByID(p.Worker.memberStatus, CI.TwoLetterISOLanguageName),
    //                 firstname1 = p.firstname1,
    //                 firstname2 = p.firstname2,
    //                 lastname1 = p.lastname1,
    //                 lastname2 = p.lastname2,
    //                 phone = p.phone,
    //                 dateupdated = Convert.ToString(p.dateupdated),
    //                 Updatedby = p.Updatedby,
    //                 recordid = Convert.ToString(p.ID)
    //             };
}