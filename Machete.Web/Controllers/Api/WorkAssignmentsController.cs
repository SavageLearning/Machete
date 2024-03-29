﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.ViewModel.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkAssignmentsController 
        : MacheteApi2Controller<WorkAssignment,WorkAssignmentVM> 
    {
        public WorkAssignmentsController(IWorkAssignmentService serv, IMapper map) : base(serv, map) {}

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<WorkAssignmentVM>> Get(ApiRequestParams apiRequestParams) 
        {
            return base.Get(apiRequestParams); 
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkAssignmentVM> Get([FromRoute]int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkAssignmentVM> Post([FromBody]WorkAssignmentVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkAssignmentVM> Put([FromRoute]int id, [FromBody]WorkAssignmentVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete([FromRoute]int id) { return base.Delete(id); }
    }
}
