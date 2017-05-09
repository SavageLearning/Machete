using Machete.Service;
using Machete.Service.DTO.Reports;
using Machete.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            var result = serv.getList();
            var result2 = result.Select(a => new {
                id = a.ID,
                name = a.name,
                description = a.description,
                category = a.category,
                subcategory = a.subcategory,
                columnLbaelsJson = a.columnLabelsJson
            });
            return Json( new { data = result2 } );
        }

        public IHttpActionResult Get(string id)
        {

            var result = serv.Get(id);
            return Json(new { data = result });
        }

        public IHttpActionResult Get(string id, DateTime? beginDate, DateTime? endDate)
        {
            var result = serv.getSimpleAggregate(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate,
                    beginDate = beginDate
                });
            return Json(new { data = result });
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