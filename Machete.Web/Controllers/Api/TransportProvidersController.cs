using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;
using TransportProvider = Machete.Web.ViewModel.Api.TransportProvider;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Any })]
        public ActionResult Get()
        {
            try
            {
                var result = serv.GetMany(w => w.active == true)
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Any })]
        public ActionResult Get(int id)
        {
            var result = map.Map<Domain.TransportProvider, TransportProvider>(serv.Get(id));
            if (result == null) return NotFound();

            return new JsonResult(new { data = result });
        }

        // POST: api/TransportProvider
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager })]
        public void Post([FromBody]TransportProvider value)
        {
            var domain = map.Map<TransportProvider, Domain.TransportProvider>(value);
            serv.Save(domain, userEmail);
        }

        // PUT: api/TransportProvider/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Manager })]
        public void Put(int id, [FromBody]TransportProvider value)
        {
            var domain = serv.Get(value.id);
            // TODO employers must only be able to edit their record
            map.Map<TransportProvider, Domain.TransportProvider>(value, domain);
            serv.Save(domain, userEmail);
        }

        // DELETE: api/TransportProvider/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        public void Delete(int id)
        {
        }

        //
        // TransportProvider Availabilities

        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Any })]
        [Route("api/transportproviders/{tpid}/availabilities")]
        [HttpGet]
        public ActionResult ARGet(int tpid)
        {
            try
            {
                var result = serv.Get(tpid).AvailabilityRules
                    .Select(e =>
                    map.Map<Domain.TransportProviderAvailability, TransportProviderAvailability>(e))
                    .AsEnumerable();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        // POST: api/TransportProvider/{tpid}/AvailabilityRules
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin })]
        [Route("api/transportproviders/{tpid}/availabilities")]
        [HttpPost]
        public ActionResult ARPost(int tpid, [FromBody]TransportProviderAvailability value)
        {
            var domain = map.Map<TransportProviderAvailability, Domain.TransportProviderAvailability>(value);

            try
            {
                var entity = serv.CreateAvailability(tpid, domain, userEmail);
                var result = map.Map<Domain.TransportProviderAvailability, TransportProviderAvailability>(entity);
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
