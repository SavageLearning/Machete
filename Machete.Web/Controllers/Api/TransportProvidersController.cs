using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportProvider = Machete.Web.ViewModel.Api.TransportProvider;

namespace Machete.Web.Controllers.Api
{
    [Route("api/transportproviders")]
    [ApiController]
    public class TransportProvidersController : MacheteApiController
    {
        private readonly ITransportProvidersService serv;
        private readonly IMapper map;

        public TransportProvidersController(ITransportProvidersService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/TransportRule
        [Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            try
            {
                var result = serv.GetMany(w => w.active)
                    .Select(e => 
                    map.Map<Domain.TransportProvider, TransportProvider>(e))
                    .AsEnumerable();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        // GET: api/TransportProvider/5
        [Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.TransportProvider, TransportProvider>(serv.Get(id));
            if (result == null) return NotFound();

            return new JsonResult(new { data = result });
        }

        // POST: api/TransportProvider
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPost("")]
        public void Post([FromBody]TransportProvider value)
        {
            var domain = map.Map<TransportProvider, Domain.TransportProvider>(value);
            serv.Save(domain, UserEmail);
        }

        // PUT: api/TransportProvider/5
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]TransportProvider value)
        {
            var domain = serv.Get(value.id);
            // TODO employers must only be able to edit their record
            map.Map<TransportProvider, Domain.TransportProvider>(value, domain);
            serv.Save(domain, UserEmail);
        }

        // DELETE: api/TransportProvider/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //
        // TransportProvider Availabilities

        [Authorize(Roles = "Administrator, Check-in, Hirer, Manager, PhoneDesk, Teacher, User")]
        [HttpGet]
        [Route("{tpid}/availabilities")]
        public ActionResult ARGet(int tpid)
        {
            try
            {
                var result = serv.Get(tpid).AvailabilityRules
                    .Select(e =>
                    map.Map<Domain.TransportProviderAvailabilities, TransportProviderAvailabilities>(e))
                    .AsEnumerable();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        // POST: api/TransportProvider/{tpid}/AvailabilityRules
        [Authorize(Roles = "Administrator")]
        [HttpPost("{tpid}/availabilities")]
        public ActionResult ARPost(int tpid, [FromBody]TransportProviderAvailabilities value)
        {
            var domain = map.Map<TransportProviderAvailabilities, Domain.TransportProviderAvailabilities>(value);

            try
            {
                var entity = serv.CreateAvailability(tpid, domain, UserEmail);
                var result = map.Map<Domain.TransportProviderAvailabilities, TransportProviderAvailabilities>(entity);
                return new JsonResult(new { data = result });

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new {
                    Content = new StringContent(e.Message),
                    ReasonPhrase = "Create new availability failed"
                });
            }
        }
    }
}
