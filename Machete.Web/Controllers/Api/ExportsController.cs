using AutoMapper;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Machete.Web.Controllers.Api
{
    [ElmahHandleError]
    [Authorize]
    [RoutePrefix("api/exports")]
    public class ExportsController : ApiController
    {
        private readonly IReportsV2Service serv;
        private readonly IMapper map;
        public ExportsController(IReportsV2Service serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

       //  GET api/<controller>
        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get()
        {
            var tables = new string[] {
                "Activities",
                "ActivitySignins",
                "Emails",
                "Employers",
                "Events",
                "Lookups",
                "Persons",
                "ReportDefinitions",
                "WorkAssignments",
                "WorkerRequests",
                "Workers",
                "WorkerSignins",
                "WorkOrders"
            };
            var result = tables.Select(a => new { name = a });

            return Json(new { data = result });
        }
        [Authorize(Roles = "Administrator, Manager")]
        public IHttpActionResult Get(string id)
        {

            var result = serv.getColumns(id);
            // TODO Use Automapper to return column deserialized
            return Json(new { data = result });
        }

        [Authorize(Roles = "Administrator")]
        //public HttpResponseMessage Get(string id, string filterField, DateTime? beginDate, DateTime? endDate, [FromBody]string value)
        [Route("{ZZtablename}/execute")]
        [HttpGet]
        public HttpResponseMessage Execute(string ZZtablename, string filterField, DateTime? beginDate, DateTime? endDate)
        {
            var includeOptions = this.Request.GetQueryNameValuePairs()
                .ToDictionary(kv => kv.Key, kv => kv.Value,
                            StringComparer.OrdinalIgnoreCase);
            includeOptions.Remove("filterField");
            includeOptions.Remove("beginDate");
            includeOptions.Remove("endDate");
            var o = new SearchOptions
            {
                name = ZZtablename,
                exportFilterField = filterField,
                beginDate = beginDate,
                endDate = endDate,
                exportIncludeOptions = includeOptions
            };

            byte[] bytes = null;
            serv.getXlsxFile(o, ref bytes);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = ZZtablename + ".xlsx";
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            result.Content.Headers.ContentLength = bytes.Count();
            result.StatusCode = System.Net.HttpStatusCode.OK;
            return result;
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
