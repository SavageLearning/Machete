using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Data.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = serv.getList()
                .Select(a => map.Map<ReportDefinition, ReportDefinitionVM>(a));

            return Ok(result);
        }

        [HttpGet("{id?}"), Authorize(Roles = "Administrator")]
        public ActionResult Get(
            [FromRoute] string id,
            [FromQuery] DateTime? beginDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? memberNumber
        )
        {
            MapperHelpers.ClientTimeZoneInfo = _clientTimeZoneInfo;
            endDate = endDate?.AddDays(1); // date passed does not reflect desired range, and is of type string...
            var result = serv.getQuery(
                new Service.DTO.SearchOptions {
                    idOrName = id,
                    endDate = endDate.ToUtcDatetime(),
                    beginDate = beginDate.ToUtcDatetime(),
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

    }
}
