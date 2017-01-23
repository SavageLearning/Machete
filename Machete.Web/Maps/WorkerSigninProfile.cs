using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class WorkerSigninProfile : MacheteProfile
    {
        public WorkerSigninProfile()
        {
            CreateMap<Domain.WorkerSignin, Service.DTO.WorkerSigninList>()
                .ForMember(v => v.lotterySequence, opt => opt.MapFrom(d => d.lottery_sequence))
                .ForMember(v => v.englishlevel, opt => opt.MapFrom(d => d == null ? 0 : d.worker.englishlevelID))
                .ForMember(v => v.waid, opt => opt.MapFrom(d => d.WorkAssignmentID))
                .ForMember(v => v.skill1, opt => opt.MapFrom(d => d == null ? null : d.worker.skill1))
                .ForMember(v => v.skill2, opt => opt.MapFrom(d => d == null ? null : d.worker.skill2))
                .ForMember(v => v.skill3, opt => opt.MapFrom(d => d == null ? null : d.worker.skill3))
                .ForMember(v => v.fullname, opt => opt.MapFrom(d =>
                    d.worker.Person.firstname1 + " " +
                    d.worker.Person.firstname2 + " " +
                    d.worker.Person.lastname1 + " " +
                    d.worker.Person.lastname2))
                .ForMember(v => v.firstname1, opt => opt.MapFrom(d => d.worker.Person.firstname1))
                .ForMember(v => v.firstname2, opt => opt.MapFrom(d => d.worker.Person.firstname2))
                .ForMember(v => v.lastname1, opt => opt.MapFrom(d => d.worker.Person.lastname1))
                .ForMember(v => v.lastname2, opt => opt.MapFrom(d => d.worker.Person.lastname2))
                .ForMember(v => v.expirationDate, opt => opt.MapFrom(d => d.worker.memberexpirationdate))
                .ForMember(v => v.memberStatusID, opt => opt.MapFrom(d => d.worker.memberStatusID))
                .ForMember(v => v.memberStatusEN, opt => opt.MapFrom(d => d.worker.memberStatusEN))
                .ForMember(v => v.memberStatusES, opt => opt.MapFrom(d => d.worker.memberStatusES))
                .ForMember(v => v.memberExpired, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpired ? true : false))
                .ForMember(v => v.memberInactive, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iInactive ? true : false))
                .ForMember(v => v.memberSanctioned, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iSanctioned ? true : false))
                .ForMember(v => v.memberExpelled, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpelled ? true : false))
            ;
            CreateMap<Domain.WorkerSignin, ViewModel.WorkerSignin>()
                .ForMember(v => v.memberExpired, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpired ? true : false))
                .ForMember(v => v.memberInactive, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iInactive ? true : false))
                .ForMember(v => v.memberSanctioned, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iSanctioned ? true : false))
                .ForMember(v => v.memberExpelled, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpelled ? true : false))

            ;
            CreateMap<Service.DTO.WorkerSigninList, ViewModel.WorkerSigninList>()
                .ForMember(v => v.expirationDate, opt => opt.MapFrom(d => d.expirationDate.ToShortDateString()))
                .ForMember(v => v.memberStatus, opt => opt.MapFrom(d => getCI() == "ES" ? d.memberStatusES : d.memberStatusEN))
                .ForMember(v => v.dateforsigninstring, opt => opt.MapFrom(d => d.dateforsignin.ToShortTimeString()))
            ;
        }
    }

    //return what's left to datatables
    //var result = from p in was.query select new 
    //{  
    //    WSIID = p.ID,
    //    recordid = p.ID.ToString(),
    //    dwccardnum = p.dwccardnum,
    //    fullname = p.fullname,
    //    firstname1 = p.firstname1,
    //    firstname2 = p.firstname2,
    //    lastname1 = p.lastname1,
    //    lastname2 = p.lastname2, 
    //    dateforsignin = p.dateforsignin.AddHours(Convert.ToDouble(WebConfigurationManager.AppSettings["TimeZoneDifferenceFromPacific"])).ToString(),
    //    dateforsigninstring = p.dateforsignin.AddHours(Convert.ToDouble(WebConfigurationManager.AppSettings["TimeZoneDifferenceFromPacific"])).ToShortTimeString(),
    //    WAID = p.waid ?? 0,
    //    memberStatus = lcache.textByID(p.memberStatus, CI.TwoLetterISOLanguageName),
    //    memberInactive = p.w.isInactive,
    //    memberSanctioned = p.w.isSanctioned,
    //    memberExpired = p.w.isExpired,
    //    memberExpelled = p.w.isExpelled,
    //    imageID = p.imageID,
    //    lotterySequence = p.lotterySequence,
    //    expirationDate = p.expirationDate.ToShortDateString(),
    //    skills = _getSkillCodes(p.englishlevel, p.skill1, p.skill2, p.skill3),
    //    program = lcache.getByID(p.typeOfWorkID).ltrCode
    //};

    //TODO: rework into model 
    //private string _getSkillCodes(int eng, int? sk1, int? sk2, int? sk3)
    //{
    //    string rtnstr = "E" + eng + " ";
    //    if (sk1 != null)
    //    {
    //        var lookup = lcache.getByID((int)sk1);
    //        rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
    //    }
    //    if (sk2 != null)
    //    {
    //        var lookup = lcache.getByID((int)sk2);
    //        rtnstr = rtnstr + lookup.ltrCode + lookup.level + " ";
    //    }
    //    if (sk3 != null)
    //    {
    //        var lookup = lcache.getByID((int)sk3);
    //        rtnstr = rtnstr + lookup.ltrCode + lookup.level;
    //    }
    //    return rtnstr;
    //}

}