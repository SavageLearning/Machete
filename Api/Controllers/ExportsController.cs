using AutoMapper;
using Machete.Service;
using Machete.Service.DTO;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Machete.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable UnusedMember.Global
namespace Machete.Api.Controllers
{
    public enum ValidTableNames {
        Activities,
        ActivitySignins,
        Emails,
        Employers,
        Events,
        Lookups,
        Persons,
        ReportDefinitions,
        WorkAssignments,
        WorkerRequests,
        Workers,
        WorkerSignins,
        WorkOrders
    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class ExportsController : ControllerBase
    {
        private readonly IReportsV2Service serv;
        private readonly IMapper map;
        public ExportsController(IReportsV2Service serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        //  GET api/<controller>
        // [Authorize(Roles = "Administrator, Manager")]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get()
        {
            var tables = new[] {
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

            return new JsonResult(new { data = result });
        }
        //[Authorize(Roles = "Administrator, Manager")]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public ActionResult Get(string tableName)
        {
            Enum.TryParse<ValidTableNames>(tableName, out var name); // validate that we've only received a table name
            var result = serv.getColumns(tableName);
            return new JsonResult(new { data = result });
        }

        //[Authorize(Roles = "Administrator")]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        // https://stackoverflow.com/questions/36274985/how-to-map-webapi-routes-correctly
        // http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#restful
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        //
        // The ZZtablename is a more unique namespace than ID; an ID is getting sent from the domain, which this method
        // has to comply with. ZZTablename is less likely to cause a collison.
        [HttpGet, Route("{ZZtablename}/execute")]
        public HttpResponseMessage Execute(string ZZtablename, string filterField, DateTime? beginDate, DateTime? endDate)
        {
            var includeOptions = Request.Query.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()); // TODO so sketch
            includeOptions.Remove("filterField");
            includeOptions.Remove("beginDate");
            includeOptions.Remove("endDate");
            var o = new SearchOptions
            {
                name = ZZtablename,
                exportFilterField = filterField == "undefined" ? null : filterField,
                beginDate = beginDate,
                endDate = endDate,
                exportIncludeOptions = includeOptions
            };
            // http://epplus.codeplex.com/wikipage?title=WebapplicationExample
            // https://stackoverflow.com/questions/30570336/export-to-excel-as-a-response-in-web-api
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
        //[Authorize(Roles = "Administrator")]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        //[Authorize(Roles = "Administrator")]
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { "Administrator" })]
        public void Delete(int id)
        {
        }
    }
}
