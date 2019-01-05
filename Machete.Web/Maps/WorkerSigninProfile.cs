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
                .ForMember(v => v.memberExpired, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpired))
                .ForMember(v => v.memberInactive, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iInactive))
                .ForMember(v => v.memberSanctioned, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iSanctioned))
                .ForMember(v => v.memberExpelled, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpelled))
                .ForMember(v => v.imageRef, opt => opt.MapFrom(d => d.worker.ImageID == null ? "/Content/images/NO-IMAGE-AVAILABLE.jpg" : "/Image/GetImage/" + d.worker.ImageID))
                .ForMember(v => v.imageID, opt => opt.MapFrom(d => d.worker.ImageID))
                .ForMember(v => v.typeOfWorkID, opt => opt.MapFrom(d => d.worker.typeOfWorkID))
                .ForMember(v => v.signinID, opt => opt.MapFrom(d => d.ID))
            ;
            CreateMap<Domain.WorkerSignin, ViewModel.WorkerSignin>()
                .ForMember(v => v.memberExpired, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpired))
                .ForMember(v => v.memberInactive, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iInactive))
                .ForMember(v => v.memberSanctioned, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iSanctioned))
                .ForMember(v => v.memberExpelled, opt => opt.MapFrom(d => d.worker.memberStatusID == Domain.Worker.iExpelled))
                .ForMember(v => v.imageRef, opt => opt.MapFrom(d => d.worker.ImageID == null ? "/Content/images/NO-IMAGE-AVAILABLE.jpg" : "/Image/GetImage/" + d.worker.ImageID))
                .ForMember(v => v.message, opt => opt.MapFrom(src => "success"))
                .ForMember(v => v.worker, opt => opt.Ignore())
                .ForMember(v => v.def, opt => opt.Ignore())
                .ForMember(v => v.idString, opt => opt.Ignore())
                .ForMember(v => v.tabref, opt => opt.Ignore())
                .ForMember(v => v.tablabel, opt => opt.Ignore())
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
}
