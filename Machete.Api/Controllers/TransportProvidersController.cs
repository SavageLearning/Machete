using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportProviderVM = Machete.Api.ViewModel.TransportProviderVM;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportProvidersController : MacheteApiController<TransportProvider, TransportProviderVM, TransportProviderListVM>
    {
        private ITransportProvidersService serv;

        public TransportProvidersController(ITransportProvidersService serv, IMapper map) : base(serv, map)
        {
            this.serv = serv;
        }

        // GET: api/TransportRule
        [HttpGet, Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        public ActionResult<TransportProviderListVM> Get()
        {
            try
            {
                var result = service.GetMany(w => w.active)
                    .ToList()
                    .AsEnumerable();
                return Ok(new { data = map.Map<IEnumerable<TransportProviderVM>>(result) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // GET: api/TransportProvider/5
        [HttpGet("{id}"), Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        public new ActionResult<TransportProviderVM> Get(int id) { return base.Get(id); }

        // POST: api/TransportProvider
        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderVM> Post([FromBody] TransportProviderVM value) { return base.Post(value); }

        // PUT: api/TransportProvider/5
        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderVM> Put(int id, [FromBody] TransportProviderVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }
        //
        // TransportProvider Availabilities
        [Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        [HttpGet("{tpid}/availabilities")]
        public ActionResult<TransportProviderAvailabilityVM> ARGet(int tpid)
        {
            try
            {
                var result = service.Get(tpid).AvailabilityRules
                    .Select(e =>
                    map.Map<TransportProviderAvailability, TransportProviderAvailabilityVM>(e))
                    .AsEnumerable();
                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // POST: api/TransportProvider/{tpid}/AvailabilityRules
        [HttpPost("{tpid}/availabilities"), Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<TransportProviderAvailabilityVM>> ARPost(int tpid, [FromBody] TransportProviderAvailabilityVM value)
        {
            var domain = map.Map<TransportProviderAvailabilityVM, TransportProviderAvailability>(value);

            try
            {
                var entity = serv.CreateAvailability(tpid, domain, UserEmail);
                var result = map.Map<TransportProviderAvailability, TransportProviderAvailabilityVM>(entity);
                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
