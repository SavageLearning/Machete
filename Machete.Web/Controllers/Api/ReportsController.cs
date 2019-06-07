using System;
using System.Linq;
using AutoMapper;
using Machete.Service;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : MacheteApiController
    {
        private readonly IReportsV2Service serv;
        private readonly IMapper map;
        public ReportsController(IReportsV2Service serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET api/<controller>
        //[Authorize(Roles = "Administrator, Manager")]
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            var result = serv.getList()
                .Select(a => map.Map<Domain.ReportDefinition, ViewModel.Api.ReportDefinition>(a));

            return new JsonResult( new { data = result } );
        }
        
        /// <summary>
        /// Get the report definition.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult: { "data": [] }</returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(string id)
        {
            var result = serv.Get(id);
            return new JsonResult(new { data = result });
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id?}")]
        public ActionResult Get(
            [FromRoute] string id,
            [FromQuery] DateTime? beginDate, 
            [FromQuery] DateTime? endDate,
            [FromQuery] int? memberNumber
        )
        {
            endDate = endDate?.AddDays(1); // date passed by TS does not reflect desired range, and is of type string...
            var result = serv.getQuery(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate,
                    beginDate = beginDate,
                    dwccardnum = memberNumber
                });
            return new JsonResult(new { data = result });
        }

        // POST api/values
//        [Authorize(Roles = "Administrator")]
//        [HttpPost("{data}")]
//        public ActionResult Post(Machete.Web.ViewModel.Api.ReportQuery data)
//        {
//            string query = data.query;
//            if (string.IsNullOrEmpty(query)) {
//                if (query == string.Empty) { // query is blank
//                    return StatusCode((int)HttpStatusCode.NoContent);
//                } else { // query is null; query cannot be null
//                    return StatusCode((int)HttpStatusCode.BadRequest);
//                }
//            }
//            try {
//                var validationMessages = serv.validateQuery(query);
//                if (validationMessages.Count == 0) {
//                    // "no modification needed"; http speak good human
//                    return StatusCode((int)HttpStatusCode.NotModified);
//                } else {
//                    // 200; we wanted validation messages, and got them.
//                    return new JsonResult(new { data = validationMessages });
//                }
//            } catch (Exception ex) {
//                // SQL errors are expected but they will be returned as strings (200).
//                // in this case, something happened that we were not expecting; return 500.
//                var message = ex.Message;
//                return StatusCode((int)HttpStatusCode.InternalServerError, message);
//            }
//        }
        [Authorize(Roles = "Administrator")]
        [HttpPost("{data}")]
        public void Post(ReportQuery data)
        {
            // I commented this out due to security concerns for which I did not have time to test.
            // The functionality was never implemented in the UI.
        }

        // PUT api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}