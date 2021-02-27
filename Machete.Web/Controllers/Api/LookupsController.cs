using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
        [Route("api/[controller]")]
        [ApiController]
    public class LookupsController : MacheteApi2Controller<Lookup, LookupVM> 
    {
        public LookupsController(ILookupService serv, IMapper map) : base(serv, map) {}

        // GET: api/Lookups
        [HttpGet]
        public virtual ActionResult<IEnumerable<LookupVM>> Get()
        {
            IEnumerable<LookupVM> result;
            try {
                result = service.GetMany(l => l.active)
                    .Select(e => map.Map<Lookup, LookupVM>(e))
                    .AsEnumerable();
            }
            catch(Exception ex) {
                return StatusCode(500, ex);
            }
            return Ok(result);
        }

        [HttpGet("{category}")]
        public ActionResult<IEnumerable<LookupVM>> GetByCategory(string category)
        {
            IEnumerable<LookupVM> result;
            try {
                result = service.GetMany(w => w.category == category && w.active)
                    .Select(e => map.Map<Lookup, LookupVM>(e))
                    .AsEnumerable();
            }
            catch (Exception ex) {
                return StatusCode(500, ex);
            }
            return Ok(result);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<LookupVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<LookupVM> Post([FromBody]LookupVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<LookupVM> Put(int id, [FromBody]LookupVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<LookupVM> Delete(int id) { return base.Delete(id); }
    }
}
