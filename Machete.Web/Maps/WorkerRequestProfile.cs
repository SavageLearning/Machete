using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class WorkerRequestProfile : MacheteProfile
    {
        public WorkerRequestProfile()
        {
            CreateMap<Domain.WorkerRequest, ViewModel.WorkerRequest>()
                .ForMember(vo => vo.workOrder, opt => opt.Ignore())
                .MaxDepth(3);
        }
    }
}