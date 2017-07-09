using AutoMapper;
using Machete.Service;
using System;
using System.Linq;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [Authorize]
    public class ReportsController : ApiController
    {
        private readonly IReportsV2Service serv;
        private readonly IMapper map;
        public ReportsController(IReportsV2Service serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET api/<controller>
        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get()
        {
            var result = serv.getList()
                .Select(a => map.Map<Domain.ReportDefinition, ReportDefinition>(a));

            return Json( new { data = result } );
        }
        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id)
        {

            var result = serv.Get(id);
            // TODO Use Automapper to return column deserialized
            return Json(new { data = result });
        }

        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id, DateTime? beginDate, DateTime? endDate)
        {
            return Get(id, beginDate, endDate, null);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id, DateTime? beginDate)
        {
            return Get(id, beginDate, null, null);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id, int? memberNumber)
        {
            return Get(id, null, null, memberNumber);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id, DateTime? beginDate, DateTime? endDate, int? memberNumber)
        {
            var result = serv.getQuery(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate,
                    beginDate = beginDate,
                    dwccardnum = memberNumber
                });
            return Json(new { data = result });
        }

        // POST api/values
        [Authorize(Roles = "Administrator")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [Authorize(Roles = "Administrator")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Authorize(Roles = "Administrator")]
        public void Delete(int id)
        {
        }
    }
}