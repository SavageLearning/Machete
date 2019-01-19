using AutoMapper;
using Machete.Api.ViewModel;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService serv;
        private readonly IMapper map;

        public LookupsController(ILookupService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/Lookups
        public ActionResult Get()
        {
            var result = serv.GetMany(l => l.active == true)
                .Select(e => map.Map<Domain.Lookup, Lookup>(e))
                .AsEnumerable();
            return new JsonResult(new { data = result });
        }

        public ActionResult Get(string category)
        {
            var result = serv.GetMany(w => w.category == category && w.active == true)
                .Select(e => map.Map<Domain.Lookup, Lookup>(e))
                .AsEnumerable();
            return new JsonResult(new { data = result });
        }

        // GET: api/Lookups/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Lookups
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Lookups/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Lookups/5
        public void Delete(int id)
        {
        }
    }
}
