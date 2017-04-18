using Machete.Service;
using Machete.Service.DTO.Reports;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    [ElmahHandleError]
    [Authorize]
    public class ReportsController : ApiController
    {
        private readonly IReportsV2Service serv;

        public ReportsController(IReportsV2Service serv)
        {
            this.serv = serv;
        }

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Get(new DateTime(1753,1,1), DateTime.MaxValue);
        }

        public IHttpActionResult Get(DateTime? beginDate, DateTime? endDate)
        {
            var result = serv.getJobsDispatchedCount(new Service.DTO.SearchOptions {
                endDate = endDate,
                beginDate = beginDate
            });
            return Ok(new { data = result });
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}