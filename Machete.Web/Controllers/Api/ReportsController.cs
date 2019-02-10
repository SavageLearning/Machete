using System;
using System.Linq;
using System.Net;
using AutoMapper;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]

        public ActionResult Get()
        {
            var result = serv.getList()
                .Select(a => map.Map<Domain.ReportDefinition, Machete.Web.ViewModel.Api.ReportDefinition>(a));

            return new JsonResult( new { data = result } );
        }
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get(string id)
        {

            var result = serv.Get(id);
            // TODO Use Automapper to return column deserialized
            return new JsonResult(new { data = result });
        }
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get(string id, DateTime? beginDate, DateTime? endDate)
        {
            return Get(id, beginDate, endDate, null);
        }

        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get(string id, DateTime? beginDate)
        {
            return Get(id, beginDate, null, null);
        }

        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]

        public ActionResult Get(string id, int? memberNumber)
        {
            return Get(id, null, null, memberNumber);
        }

        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get(string id, DateTime? beginDate, DateTime? endDate, int? memberNumber)
        {
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
        [HttpPost]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Post(Machete.Web.ViewModel.Api.ReportQuery data)
        {
            string query = data.query;
            if (string.IsNullOrEmpty(query)) {
                if (query == string.Empty) { // query is blank
                    return StatusCode((int)HttpStatusCode.NoContent);
                } else { // query is null; query cannot be null
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            try {
                var validationMessages = serv.validateQuery(query);
                if (validationMessages.Count == 0) {
                    // "no modification needed"; http speak good human
                    return StatusCode((int)HttpStatusCode.NotModified);
                } else {
                    // 200; we wanted validation messages, and got them.
                    return new JsonResult(new { data = validationMessages });
                }
            } catch (Exception ex) {
                // SQL errors are expected but they will be returned as strings (200).
                // in this case, something happened that we were not expecting; return 500.
                var message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, message);
            }
        }

        // PUT api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public void Delete(int id)
        {
        }
    }
}