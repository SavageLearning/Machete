using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class ActivitySigninProfile : MacheteProfile
    {
        public ActivitySigninProfile()
        {
            CreateMap<Domain.ActivitySignin, ViewModel.ActivitySignin>()
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.tabref, opt => opt.Ignore())
                .ForMember(v => v.tablabel, opt => opt.Ignore())
            ;
            CreateMap<Domain.ActivitySignin, Service.DTO.ActivitySigninList>()
                .ForMember(v => v.firstname1, opt => opt.MapFrom(d => d.person.firstname1))
                .ForMember(v => v.firstname2, opt => opt.MapFrom(d => d.person.firstname2))
                .ForMember(v => v.lastname1, opt => opt.MapFrom(d => d.person.lastname1))
                .ForMember(v => v.lastname2, opt => opt.MapFrom(d => d.person.lastname2))
                .ForMember(v => v.fullname, opt => opt.MapFrom(d =>
                    d.person.firstname1 + " " + d.person.firstname2 + " " +
                    d.person.lastname1 + " " + d.person.lastname2))
                .ForMember(v => v.expirationDate, opt => opt.MapFrom(d => d.person.Worker.memberexpirationdate))
                .ForMember(v => v.imageID, opt => opt.MapFrom(d => d.person.Worker.ImageID))
                .ForMember(v => v.memberStatusID, opt => opt.MapFrom(d => d.memberStatusID))
                .ForMember(v => v.memberStatusEN, opt => opt.Ignore())
                .ForMember(v => v.memberStatusES, opt => opt.Ignore());

            CreateMap<Service.DTO.ActivitySigninList, ViewModel.ActivitySigninList>()
                .ForMember(v => v.WSIID, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID.ToString()))
                .ForMember(v => v.imageID, opt => opt.MapFrom(d => d.imageID))
                .ForMember(v => v.dwccardnum, opt => opt.MapFrom(d => Convert.ToString(d.dwccardnum)))
                .ForMember(v => v.expirationDate, opt => opt.MapFrom(d=> d.expirationDate.ToShortDateString()))
                .ForMember(v => v.memberStatus, opt => opt.MapFrom(d => getCI() == "ES" ? d.memberStatusES : d.memberStatusEN))
                .ForMember(v => v.dateforsignin, opt => opt.MapFrom(d => d.dateforsignin.ToShortDateString()))
                .ForMember(v => v.memberInactive, opt => opt.MapFrom(d => d.memberStatusID == Domain.Worker.iInactive ? true : false))
                .ForMember(v => v.memberExpelled, opt => opt.MapFrom(d => d.memberStatusID == Domain.Worker.iExpelled ? true : false))
                .ForMember(v => v.memberExpired, opt => opt.MapFrom(d => d.memberStatusID == Domain.Worker.iExpired ? true : false))
                .ForMember(v => v.memberSanctioned, opt => opt.MapFrom(d => d.memberStatusID == Domain.Worker.iSanctioned ? true : false))
                ;
        }
    }

    //var result = from p in was.query
    //             select new
    //             {
    //                 WSIID = p.ID,
    //                 recordid = p.ID.ToString(),
    //                 dwccardnum = p.dwccardnum,
    //                 fullname = p.fullname,
    //                 firstname1 = p.firstname1,
    //                 firstname2 = p.firstname2,
    //                 lastname1 = p.lastname1,
    //                 lastname2 = p.lastname2,
    //                 dateforsignin = p.dateforsignin,
    //                 dateforsigninstring = p.dateforsignin.ToShortDateString(),
    //                 memberStatus = lcache.textByID(p.memberStatus, CI.TwoLetterISOLanguageName),
    //                 memberInactive = p.w.isInactive,
    //                 memberSanctioned = p.w.isSanctioned,
    //                 memberExpired = p.w.isExpired,
    //                 memberExpelled = p.w.isExpelled,
    //                 imageID = p.imageID,
    //                 expirationDate = p.expirationDate.ToShortDateString(),
    //             };
}