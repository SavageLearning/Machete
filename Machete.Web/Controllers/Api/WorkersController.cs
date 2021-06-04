using System.Collections.Generic;
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
    public class WorkersController 
        : MacheteApi2Controller<Worker,WorkerVM> 
    {
        public WorkersController(IWorkerService serv, IMapper map) : base(serv, map) {}

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<WorkerVM>> Get(
            [FromQuery]ApiRequestParams apiRequestParams) 
        { 
            return base.Get(apiRequestParams); 
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<WorkerVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Post([FromBody]WorkerVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Put(int id, [FromBody]WorkerVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<WorkerVM> Delete(int id) { return base.Delete(id); }
    }
}
