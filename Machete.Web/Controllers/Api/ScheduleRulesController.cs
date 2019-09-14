using System;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers.Api.Abstracts;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleRule = Machete.Web.ViewModel.Api.ScheduleRule;

namespace Machete.Web.Controllers.Api
{
    [Route("api/schedulerules")]
    [ApiController]
    public class ScheduleRulesController : MacheteApiController
    {
        private readonly IScheduleRuleService serv;
        private readonly IMapper map;

        public ScheduleRulesController(IScheduleRuleService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/ScheduleRules
        [Authorize(Roles = "Administrator, Hirer")]
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var result = serv.GetAll()
                    .Select(e => map.Map<Domain.ScheduleRule, ScheduleRule>(e))
                    .AsEnumerable();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        // GET: api/ScheduleRules/5
        [HttpGet]
        [Route("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ScheduleRules
        [HttpPost("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ScheduleRules/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ScheduleRules/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
