using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Machete.Web.Controllers.Api
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : MacheteApi2Controller<ReportDefinition, ReportDefinitionVM>
    {
        private readonly IReportsV2Service serv;
        private TimeZoneInfo _clientTimeZoneInfo;

        public ReportsController(IReportsV2Service serv, ITenantService tenantService, IMapper map) : base(serv, map)
        {
            this.serv = serv;
            _clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tenantService.GetCurrentTenant().Timezone);
        }

        // GET api/<controller>
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<ReportDefinitionVM>> Get()
        {
            var result = serv.GetList()
                .Select(a => map.Map<ReportDefinition, ReportDefinitionVM>(a));

            return Ok(new {data = result});
        }

        // GET api/<controller>/definition/ReportId
        [HttpGet("definition/{id?}"), Authorize(Roles = "Administrator")]
        public ActionResult<ReportDefinitionVM> Get([FromRoute] string id)
        {
            if (!serv.Exists(id)) return NotFound();
            var result = map.Map<ReportDefinitionVM>(serv.Get(id));
            return Ok(new {data = result});
        }

        [HttpGet("{id?}"), Authorize(Roles = "Administrator")]
        public ActionResult<List<dynamic>> Get(
            [FromRoute] string id,
            [FromQuery] DateTime? beginDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? memberNumber
        )
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            endDate = endDate?.AddDays(1); // date passed does not reflect desired range, and is of type string...
            var result = serv.GetQuery(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate.ToUtcDatetime(),
                    beginDate = beginDate.ToUtcDatetime(),
                    dwccardnum = memberNumber
                });
            return Ok(new { data = result });
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

        // PUT api/values
       [Authorize(Roles = "Administrator")]
       [HttpPut("{id}")]
       public ActionResult<ReportDefinitionVM> Put([FromRoute] string id, [FromBody] ReportDefinitionVM reportDefinitionVM)
       {
           if (!ModelState.IsValid) return BadRequest(ModelState);
           if (!serv.Exists(id)) return NotFound();

           string query = reportDefinitionVM.sqlquery;
           try {
               var validationMessages = serv.ValidateQuery(query);
               if (validationMessages.Count == 0) {

                   serv.Save(map.Map<ReportDefinition>(reportDefinitionVM), UserEmail);
                //    return StatusCode((int)HttpStatusCode.OK);
                   return Ok(new { data = map.Map<ReportDefinitionVM>(serv.Get(id))});
               } else {
                   // 400; we wanted validation messages, and got them.
                   return BadRequest(new { data = validationMessages });
               }
           } catch (Exception ex) {
               // SQL errors are expected but they will be returned as strings (400).
               // in this case, something happened that we were not expecting; return 500.
               var message = ex.Message;
               return StatusCode((int)HttpStatusCode.InternalServerError, message);
           }
       }

        // POST api/values
       [Authorize(Roles = "Administrator")]
       [HttpPost]
       public ActionResult<ReportDefinitionVM> Create([FromBody] ReportDefinitionVM reportDefinitionVM)
       {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           // if common name converted to name as ID already exists
           if (serv.Exists(MapperHelpers.ToNameAsId(reportDefinitionVM.commonName)))
            return BadRequest("Report with this name already exists");

           string query = reportDefinitionVM.sqlquery;
           try {
               var validationMessages = serv.ValidateQuery(query);
               if (validationMessages.Count == 0) {

                   var createdRecord = serv.Create(map.Map<ReportDefinition>(reportDefinitionVM), UserEmail);
                //    return StatusCode((int)HttpStatusCode.OK);
                return CreatedAtAction(nameof(Get), new {createdRecord.name}, new {data = map.Map<ReportDefinitionVM>(createdRecord)});
               } else {
                   // 400; we wanted validation messages, and got them.
                   return BadRequest(new { data = validationMessages });
               }
           } catch (Exception ex) {
               // SQL errors are expected but they will be returned as strings (400).
               // in this case, something happened that we were not expecting; return 500.
               var message = ex.Message;
               return StatusCode((int)HttpStatusCode.InternalServerError, message);
           }
       }

       // DELETE
       [Authorize(Roles = "Administrator")]
       [HttpDelete("{id}")]
       public ActionResult Delete([FromRoute] string id)
       {
           if (!serv.Exists(id)) return BadRequest();
           var record = serv.Get(id);
           serv.Delete(record.ID, UserEmail);
           return Ok();
       }
    }
}
