using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Web.Maps
{
    public class LookupsMap : MacheteProfile
    {
        public LookupsMap()
        {
            CreateMap<Domain.Lookup, ViewModel.Api.Lookup>();
        }
    }
}