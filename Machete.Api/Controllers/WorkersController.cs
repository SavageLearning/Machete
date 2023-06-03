using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO = Machete.Service.DTO;

namespace Machete.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController
        : MacheteApiController<Worker, WorkerVM, WorkerListVM>
    {
        public WorkersController(IWorkerService serv, IMapper map) : base(serv, map) { }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<WorkerListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams)
        {
            return base.Get(apiRequestParams);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkerVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Post([FromBody] WorkerVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Put(int id, [FromBody] WorkerVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Delete(int id) { return base.Delete(id); }
    }
}
