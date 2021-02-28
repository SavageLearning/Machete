using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Machete.Web.Controllers.Api
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
    
    [Route("api/exports")]
    [ApiController]
    public class ExportsController : ControllerBase
    {
        private readonly IReportsV2Service serv;

        public ExportsController(IReportsV2Service serv)
        {
            this.serv = serv;
        }

        //  GET api/<controller>
        [Authorize(Roles = "Administrator, Manager")]
        [HttpGet]
        [Route("")]
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
        
        [Authorize(Roles = "Administrator, Manager")]
        [HttpGet]
        [Route("{tableName}")]
        public ActionResult Get(string tableName)
        {
            Enum.TryParse<ValidTableNames>(tableName, out var unused); // validate that we've only received a table name
            var result = serv.getColumns(tableName);
            return new JsonResult(new { data = result });
        }

        // https://stackoverflow.com/questions/36274985/how-to-map-webapi-routes-correctly
        // http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#restful
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        //
        // The tablename is a more unique namespace than ID; an ID is getting sent from the domain, which this method
        // has to comply with. tablename is less likely to cause a collision.
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{tablename}/execute")]
        public IActionResult Execute(
            [FromRoute] string tablename,
            [FromQuery] string filterField,
            [FromQuery] DateTime? beginDate,
            [FromQuery] DateTime? endDate)
        {
            Enum.TryParse<ValidTableNames>(tablename, out var unused); // validate that we've only received a table name

            var includeOptions = Request.Query.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()); // TODO so sketch
            includeOptions.Remove("filterField");
            includeOptions.Remove("beginDate");
            includeOptions.Remove("endDate");

            var o = new SearchOptions
            {
                name = tablename,
                exportFilterField = filterField == "undefined" ? null : filterField, // TODO Enum
                beginDate = beginDate.ToUtcDatetime(),
                endDate = endDate.ToUtcDatetime(),
                exportIncludeOptions = includeOptions
            };

            // http://epplus.codeplex.com/wikipage?title=WebapplicationExample
            // https://stackoverflow.com/questions/30570336/export-to-excel-as-a-response-in-web-api
            byte[] bytes = null;
            serv.getXlsxFile(o, ref bytes);

//            HttpResponseMessage response = new HttpResponseMessage();
//            response.Content = new ByteArrayContent(bytes);
//            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
//            response.Content.Headers.ContentDisposition.FileName = tablename + ".xlsx";
//            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
//            response.Content.Headers.ContentLength = bytes.Length;
//            response.StatusCode = HttpStatusCode.OK;            
            return new FileContentResult(bytes, new MediaTypeHeaderValue("application/ms-excel"));
        }
    }
}
