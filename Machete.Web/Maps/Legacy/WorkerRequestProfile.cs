using AutoMapper;
using static Machete.Web.Helpers.Extensions;

namespace Machete.Web.Maps
{
    public class WorkerRequestProfile : Profile
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
