using AutoMapper;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Any })]
        public IHttpActionResult Get()
        {
            try
            {
                var result = serv.GetMany(w => w.active == true)
                    .Select(e => 
                    map.Map<Domain.TransportProvider, ViewModel.TransportProvider>(e))
                    .AsEnumerable();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // GET: api/TransportProvider/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Any })]
        public IHttpActionResult Get(int id)
        {
            var result = map.Map<Domain.TransportProvider, ViewModel.TransportProvider>(serv.Get(id));
            if (result == null) return NotFound();

            return Json(new { data = result });
        }

        // POST: api/TransportProvider
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager })]
        public void Post([FromBody]ViewModel.TransportProvider value)
        {
            var domain = map.Map<ViewModel.TransportProvider, Domain.TransportProvider>(value);
            serv.Save(domain, userEmail);
        }

        // PUT: api/TransportProvider/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Manager })]
        public void Put(int id, [FromBody]ViewModel.TransportProvider value)
        {
            var domain = serv.Get(value.id);
            // TODO employers must only be able to edit their record
            map.Map<ViewModel.TransportProvider, Domain.TransportProvider>(value, domain);
            serv.Save(domain, userEmail);
        }

        // DELETE: api/TransportProvider/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        public void Delete(int id)
        {
        }

        //
        // TransportProvider Availabilities

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Any })]
        [Route("api/transportproviders/{tpid}/availabilities")]
        [HttpGet]
        public IHttpActionResult ARGet(int tpid)
        {
            try
            {
                var result = serv.Get(tpid).AvailabilityRules
                    .Select(e =>
                    map.Map<Domain.TransportProviderAvailability, TransportProviderAvailability>(e))
                    .AsEnumerable();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // POST: api/TransportProvider/{tpid}/AvailabilityRules
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin })]
        [Route("api/transportproviders/{tpid}/availabilities")]
        [HttpPost]
        public IHttpActionResult ARPost(int tpid, [FromBody]TransportProviderAvailability value)
        {
            var domain = map.Map<TransportProviderAvailability, Domain.TransportProviderAvailability>(value);

            try
            {
                var entity = serv.CreateAvailability(tpid, domain, userEmail);
                var result = map.Map<Domain.TransportProviderAvailability, TransportProviderAvailability>(entity);
                return Json(new { data = result });

            }
            catch (Exception e)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                        Content = new StringContent(e.Message),
                        ReasonPhrase = "Create new availability failed"
                    });
            }
        }
    }
}
