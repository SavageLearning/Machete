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
    public class ScheduleRulesController : MacheteApi2Controller<ScheduleRule, ScheduleRuleVM>
    {

        public ScheduleRulesController(IScheduleRuleService serv, IMapper map) : base(serv, map) {}

        // GET: api/ScheduleRules
        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<ScheduleRuleVM>> Get(
            [FromQuery]int displayLength = 10,
            [FromQuery]int displayStart = 0) 
        { 
            return base.Get(displayLength, displayStart); 
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<ScheduleRuleVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<ScheduleRuleVM> Post([FromBody]ScheduleRuleVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<ScheduleRuleVM> Put(int id, [FromBody]ScheduleRuleVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<ScheduleRuleVM> Delete(int id) { return base.Delete(id); }

    }
}
