using AutoMapper;

namespace Machete.Web.Maps
{
    public class WorkerRequestProfile : Profile
    {
        public WorkerRequestProfile()
        {
            CreateMap<Domain.WorkerRequest, ViewModel.WorkerRequest>()
                .ForMember(vo => vo.idString, opt => opt.Ignore()) // prefix specified in views
                .ForMember(vo => vo.workOrder, opt => opt.Ignore())
                .ForMember(v => v.workerRequested, opt => opt.MapFrom(d => d.workerRequested))
                .MaxDepth(3);
        }
    }
}
