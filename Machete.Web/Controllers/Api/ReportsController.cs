using AutoMapper;
using Machete.Service;
using Machete.Service.DTO.Reports;
using Machete.Web.Helpers;
using Newtonsoft.Json;
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
                .Select(a => map.Map<Domain.ReportDefinition, Web.ViewModel.ReportDefinition>(a));
            //new
            //{
            //    id = a.ID,
            //    name = a.name,
            //    title = a.title,
            //    commonName = a.commonName,
            //    description = a.description,
            //    sqlquery = a.sqlquery,
            //    category = a.category,
            //    subcategory = a.subcategory,
            //    columns = JsonConvert.DeserializeObject(a.columnsJson)
            //}
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
            var result = serv.getQuery(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate,
                    beginDate = beginDate
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