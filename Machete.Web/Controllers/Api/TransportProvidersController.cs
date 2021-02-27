using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportProviderVM = Machete.Web.ViewModel.Api.TransportProviderVM;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportProvidersController : MacheteApi2Controller<TransportProvider, TransportProviderVM>
    {
        private ITransportProvidersService _serv;

        public TransportProvidersController(ITransportProvidersService serv, IMapper map) : base(serv, map) {
            _serv = serv;
        }

        // GET: api/TransportRule
        [HttpGet, Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        public ActionResult<IEnumerable<TransportProviderVM>> Get()
        {
            try
            {
                var result = service.GetMany(w => w.active)
                    .Select(e => 
                    map.Map<Domain.TransportProvider, TransportProviderVM>(e))
                    .AsEnumerable();
                return Ok(result);
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
        public new ActionResult<TransportProviderVM> Post([FromBody]TransportProviderVM value) { return base.Post(value); }

        // PUT: api/TransportProvider/5
        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderVM> Put(int id, [FromBody]TransportProviderVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<TransportProviderVM> Delete(int id) { return base.Delete(id); }
        //
        // TransportProvider Availabilities
        [Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        [HttpGet("{tpid}/availabilities")]
        public ActionResult<IEnumerable<TransportProviderAvailabilityVM>> ARGet(int tpid)
        {
            try
            {
                var result = service.Get(tpid).AvailabilityRules
                    .Select(e =>
                    map.Map<TransportProviderAvailability, TransportProviderAvailabilityVM>(e))
                    .AsEnumerable();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        // POST: api/TransportProvider/{tpid}/AvailabilityRules
        [HttpPost("{tpid}/availabilities"), Authorize(Roles = "Administrator")]
        public ActionResult<TransportProviderAvailabilityVM> ARPost(int tpid, [FromBody]TransportProviderAvailabilityVM value)
        {
            var domain = map.Map<TransportProviderAvailabilityVM, TransportProviderAvailability>(value);

            try
            {
                var entity = _serv.CreateAvailability(tpid, domain, UserEmail);
                var result = map.Map<TransportProviderAvailability, TransportProviderAvailabilityVM>(entity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
