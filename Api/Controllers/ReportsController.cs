using Api.ViewModels;
using AutoMapper;
using Machete.Service;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
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
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]

        public IHttpActionResult Get()
        {
            var result = serv.getList()
                .Select(a => map.Map<Domain.ReportDefinition, ReportDefinition>(a));

            return Json( new { data = result } );
        }
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public IHttpActionResult Get(string id)
        {

            var result = serv.Get(id);
            // TODO Use Automapper to return column deserialized
            return Json(new { data = result });
        }
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public IHttpActionResult Get(string id, DateTime? beginDate, DateTime? endDate)
        {
            return Get(id, beginDate, endDate, null);
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public IHttpActionResult Get(string id, DateTime? beginDate)
        {
            return Get(id, beginDate, null, null);
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]

        public IHttpActionResult Get(string id, int? memberNumber)
        {
            return Get(id, null, null, memberNumber);
        }

        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
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
        [HttpPost]
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public IHttpActionResult Post(ReportQuery data)
        {
            string query = data.query;
            if (string.IsNullOrEmpty(query)) {
                if (query == string.Empty) { // query is blank
                    return StatusCode(HttpStatusCode.NoContent);
                } else { // query is null; query cannot be null
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            try {
                var validationMessages = serv.validateQuery(query);
                if (validationMessages.Count == 0) {
                    // "no modification needed"; http speak good human
                    return StatusCode(HttpStatusCode.NotModified);
                } else {
                    // 200; we wanted validation messages, and got them.
                    return Json(new { data = validationMessages });
                }
            } catch (Exception ex) {
                // SQL errors are expected but they will be returned as strings (200).
                // in this case, something happened that we were not expecting; return 500.
                var message = ex.Message;
                var statusCode = HttpStatusCode.InternalServerError;
                var error = Request.CreateErrorResponse(statusCode, message);
                return ResponseMessage(error);
            }
        }

        // PUT api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { "Administrator" })]
        public void Delete(int id)
        {
        }
    }
}