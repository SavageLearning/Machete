using AutoMapper;
using Machete.Domain;
using Machete.Service;
using System;
using System.Linq;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class TransportVehiclesScheduleController : MacheteApiController
    {
        private readonly ITransportVehiclesScheduleService serv;
        private readonly IMapper map;

        public TransportVehiclesScheduleController(ITransportVehiclesScheduleService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/TransportRule
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Employer })]
        public IHttpActionResult Get()
        {

            try
            {
                var result = serv.GetAll()
                    .Select(e => map.Map<Domain.TransportVehicleSchedule, ViewModel.TransportVehicleSchedule>(e))
                    .AsEnumerable();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // GET: api/TransportRule/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TransportRule
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TransportRule/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TransportRule/5
        public void Delete(int id)
        {
        }
    }
}
