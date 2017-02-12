using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Machete.Service.DTO;

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
                .ForMember(v => v.program, opt => opt.MapFrom(d => d.worker.typeOfWork))
                .ForMember(v => v.skillCodes, opt => opt.MapFrom(d => d.worker.skillCodes))
                .ForMember(v => v.lotterySequence, opt => opt.MapFrom(d => d.lottery_sequence))
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
                .ForMember(v => v.imageRef, opt => opt.MapFrom(d => d.worker.ImageID == null ? "/Content/images/NO-IMAGE-AVAILABLE.jpg" : "/Image/GetImage/" + d.worker.ImageID))
                .ForMember(v => v.message, opt => opt.UseValue("success"))
                .ForMember(v => v.worker, opt => opt.Ignore())
            ;

            CreateMap<Service.DTO.WorkerSigninList, ViewModel.WorkerSigninList>()
                .ForMember(v => v.recordid, opt => opt.MapFrom(d => d.ID))
                .ForMember(v => v.WSIID, opt => opt.MapFrom(d => d.ID))
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



    //public class signinView : Record
    //{
    //    public int dwccardnum { get; set; }
    //    public string firstname1 { get; set; }
    //    public string firstname2 { get; set; }
    //    public string lastname1 { get; set; }
    //    public string lastname2 { get; set; }
    //    public string fullname
    //    {
    //        get
    //        {
    //            return firstname1 + " " +
    //                    firstname2 + " " +
    //                    lastname1 + " " +
    //                    lastname2;
    //        }
    //        set{}
    //    }
    //    public int signinID { get; set; }
    //    public DateTime dateforsignin { get; set; }
    //    public int? imageID { get; set; }
    //    public DateTime expirationDate { get; set; }
    //    public int memberStatus { get; set; }
    //    public Person p { get; set; }
    //    public Worker w { get; set; }
    //    public Signin s { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="per"></param>
    //    /// <param name="sign"></param>
    //    public signinView(Person per, Signin sign)
    //    {
    //        p = per;
    //        w = p.Worker;
    //        s = sign;
    //        ID = s.ID;
    //        firstname1 = p == null ? null : p.firstname1;
    //        firstname2 = p == null ? null : p.firstname2;
    //        lastname1 = p == null ? null : p.lastname1;
    //        lastname2 = p == null ? null : p.lastname2;
    //        dateforsignin = s.dateforsignin;
    //        dwccardnum = s.dwccardnum;
    //        signinID = s.ID;
    //        dateupdated = s.dateupdated;
    //        datecreated = s.datecreated;
    //        createdby = s.createdby;
    //        updatedby = s.updatedby;
    //        imageID = p == null ? null : p.Worker.ImageID;
    //        expirationDate = p == null ? DateTime.MinValue : p.Worker.memberexpirationdate;
    //        memberStatus = p == null ? 0 : p.Worker.memberStatusID;

    //    }
    //    public signinView() { }
    //}
}