﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleRulesController : MacheteApiController<ScheduleRule, ScheduleRuleVM, ScheduleRuleListVM>
    {

        public ScheduleRulesController(IScheduleRuleService serv, IMapper map) : base(serv, map) { }

        // GET: api/ScheduleRules
        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public ActionResult<IEnumerable<ScheduleRuleListVM>> Get()
        {
            return base.Get(new ApiRequestParams() { AllRecords = true });
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<ScheduleRuleVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<ScheduleRuleVM> Post([FromBody] ScheduleRuleVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<ScheduleRuleVM> Put(int id, [FromBody] ScheduleRuleVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete(int id) { return base.Delete(id); }

    }
}
