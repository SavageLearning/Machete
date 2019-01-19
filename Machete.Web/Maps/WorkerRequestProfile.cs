namespace Machete.Web.Maps
{
    public class WorkerRequestProfile : MacheteProfile
    {
        public WorkerRequestProfile()
        {
            CreateMap<Domain.WorkerRequest, ViewModel.WorkerRequest>()
                .ForMember(vo => vo.idString, opt => opt.Ignore()) // prefix specified in views
                .ForMember(vo => vo.workOrder, opt => opt.Ignore())
                .MaxDepth(3);
        }
    }
}
