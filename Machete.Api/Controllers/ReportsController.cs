using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Service.Tenancy;
using Machete.Domain;
using Machete.Service;
using Machete.Api.Helpers;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Machete.Api.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : MacheteApi2Controller<ReportDefinition, ReportDefinitionVM>
    {
        private readonly IReportsV2Service serv;
        private ITenantService _tenantService;

        public ReportsController(IReportsV2Service serv, ITenantService tenantService, IMapper map) : base(serv, map)
        {
            this.serv = serv;
            this._tenantService = tenantService;
        }

        // GET api/<controller>
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<ReportDefinitionVM>> Get()
        {
            var result = serv.GetList()
                .Select(a => map.Map<ReportDefinition, ReportDefinitionVM>(a));

            return Ok(new { data = result });
        }

        // GET api/<controller>/definition/ReportId
        [HttpGet("definition/{id?}"), Authorize(Roles = "Administrator")]
        public ActionResult<ReportDefinitionVM> Get([FromRoute] string id)
        {
            if (!serv.Exists(id)) return NotFound();
            var result = map.Map<ReportDefinitionVM>(serv.Get(id));
            return Ok(new { data = result });
        }

        [HttpGet("{id?}"), Authorize(Roles = "Administrator")]
        public ActionResult<List<dynamic>> Get(
            [FromRoute] string id,
            [FromQuery] DateTime? beginDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? memberNumber
        )
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(this._tenantService.GetCurrentTenant().Timezone);
            endDate = endDate?.AddDays(1); // date passed does not reflect desired range, and is of type string...
            var result = serv.GetQuery(
                new Service.DTO.SearchOptions
                {
                    idOrName = id,
                    endDate = endDate.ToUtcDatetime(timezone),
                    beginDate = beginDate.ToUtcDatetime(timezone),
                    dwccardnum = memberNumber
                });
            return Ok(new { data = result });
        }

        // PUT api/values
        /// <summary>
        /// Validates report definitions based on the View Model.
        /// Also validates the query and returns errors
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reportDefinitionVM"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult<ReportDefinitionVM> Put([FromRoute] string id, [FromBody] ReportDefinitionVM reportDefinitionVM)
        {
            /***
                 Applies to both Put and Create
                 Return all errors in the format:
                     new { errors = errorLabel, errorsList) }
                 where label is a string and error list is a list of strings
                 You can use the private method ArrangeReportErrors() to simply return
                 new { errors = ArrangeReportErrors("sqlQuery", validationMessages) }
                 The frontend client expects this as it is also the format that is returned
                 with ModelState errors.
            ***/
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!serv.Exists(id)) return NotFound();

            string query = reportDefinitionVM.sqlquery;
            try
            {
                var validationMessages = serv.ValidateQuery(query);
                if (validationMessages.Count == 0)
                {

                    serv.Save(map.Map<ReportDefinition>(reportDefinitionVM), UserEmail);
                    //    return StatusCode((int)HttpStatusCode.OK);
                    return Ok(new { data = map.Map<ReportDefinitionVM>(serv.Get(reportDefinitionVM.id)) });
                }
                else
                {
                    // 400; we wanted validation messages, and got them.
                    return BadRequest(new { errors = ArrangeReportErrors("sqlQuery", validationMessages) });
                }
            }
            catch (Exception ex)
            {
                // SQL errors are expected but they will be returned as strings (400).
                // in this case, something happened that we were not expecting; return 500.
                return StatusCode((int)HttpStatusCode.InternalServerError,
                     new { errors = ArrangeReportErrors("Server Error", new List<string>() { ex.Message }) });
            }
        }

        [NonAction]
        private Dictionary<string, List<string>> ArrangeReportErrors(string label, List<string> errors) =>
             new Dictionary<string, List<string>>()
             {
                {label, errors}
             };

        // POST api/values
        /// <summary>
        /// Validates report definitions based on the View Model.
        /// Also validates the query and returns errors
        /// </summary>
        /// <param name="reportDefinitionVM"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<ReportDefinitionVM> Create([FromBody] ReportDefinitionVM reportDefinitionVM)
        {
            /***
                Applies to both Put and Create
                Return all errors in the format:
                    new { errors = errorLabel, errorsList) }
                where label is a string and error list is a list of strings
                You can use the private method ArrangeReportErrors() to simply return
                new { errors = ArrangeReportErrors("sqlQuery", validationMessages) }
                The frontend client expects this as it is also the format that is returned
                with ModelState errors.
           ***/
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // if common name converted to name as ID already exists
            if (serv.Exists(MapperHelpers.NormalizeName(reportDefinitionVM.commonName)))
                return BadRequest("Report with this name already exists");

            string query = reportDefinitionVM.sqlquery;
            try
            {
                var validationMessages = serv.ValidateQuery(query);
                if (validationMessages.Count == 0)
                {

                    var createdRecord = serv.Create(map.Map<ReportDefinition>(reportDefinitionVM), UserEmail);
                    //    return StatusCode((int)HttpStatusCode.OK);
                    return CreatedAtAction(nameof(Get), new { createdRecord.name }, new { data = map.Map<ReportDefinitionVM>(createdRecord) });
                }
                else
                {
                    // 400; we wanted validation messages, and got them.
                    return BadRequest(new { errors = ArrangeReportErrors("sqlQuery", validationMessages) });
                }
            }
            catch (Exception ex)
            {
                // SQL errors are expected but they will be returned as strings (400).
                // in this case, something happened that we were not expecting; return 500.
                var message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError,
                     new { errors = ArrangeReportErrors("Server Error", new List<string>() { ex.Message }) });
            }
        }

        // DELETE
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] string id)
        {
            if (!serv.Exists(id))
                return BadRequest(new
                {
                    errors = ArrangeReportErrors("Server Error", new List<string>()
                    {
                        $"Error, delete failed: report '{id}' not found"
                    })
                });
            var record = serv.Get(id);
            serv.Delete(record.ID, UserEmail);
            return Ok();
        }
    }
}
