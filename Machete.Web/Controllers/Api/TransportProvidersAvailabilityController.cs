using System.Collections.Generic;
using AutoMapper;
using Machete.Service;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportProvidersAvailabilityController 
        : MacheteApi2Controller<Machete.Domain.TransportProviderAvailability,TransportProviderAvailabilityVM> 
    {
        public TransportProvidersAvailabilityController(ITransportProvidersAvailabilityService serv, IMapper map) : base(serv, map) {}

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<TransportProviderAvailabilityVM>> Get() 
        { 
            return base.Get(new ApiRequestParams() {AllRecords = true}); 
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<TransportProviderAvailabilityVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderAvailabilityVM> Post([FromBody]TransportProviderAvailabilityVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderAvailabilityVM> Put(int id, [FromBody]TransportProviderAvailabilityVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }
    }
}
