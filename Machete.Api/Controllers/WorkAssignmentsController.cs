using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkAssignmentsController
        : MacheteApiController<WorkAssignment, WorkAssignmentVM, WorkAssignmentListVM>
    {
        public WorkAssignmentsController(IWorkAssignmentService serv, IMapper map) : base(serv, map) { }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<WorkAssignmentListVM>> Get(ApiRequestParams apiRequestParams)
        {
            return base.Get(apiRequestParams);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkAssignmentVM> Get([FromRoute] int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkAssignmentVM> Post([FromBody] WorkAssignmentVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkAssignmentVM> Put([FromRoute] int id, [FromBody] WorkAssignmentVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult Delete([FromRoute] int id) { return base.Delete(id); }
    }
}
