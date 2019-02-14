using System.Linq;
using AutoMapper;
using Machete.Service;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/lookups")]
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
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            var result = serv.GetMany(l => l.active)
                .Select(e => map.Map<Domain.Lookup, Machete.Web.ViewModel.Api.Lookup>(e))
                .AsEnumerable();
            return new JsonResult(new { data = result });
        }

        [HttpGet]
        [Route("{category}")]
        public ActionResult Get(string category)
        {
            var result = serv.GetMany(w => w.category == category && w.active)
                .Select(e => map.Map<Domain.Lookup, Machete.Web.ViewModel.Api.Lookup>(e))
                .AsEnumerable();
            return new JsonResult(new { data = result });
        }

        // GET: api/Lookups/5
        [HttpGet]
        [Route("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Lookups
        [HttpPost("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Lookups/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        // DELETE: api/Lookups/5
        public void Delete(int id)
        {
        }
    }
}
