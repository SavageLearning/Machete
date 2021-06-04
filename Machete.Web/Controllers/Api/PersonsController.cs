using System.Collections.Generic;
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
    public class PersonsController 
        : MacheteApi2Controller<Person,PersonVM> 
    {
        public PersonsController(IPersonService serv, IMapper map) : base(serv, map) {}

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<PersonVM>> Get(
            [FromQuery]ApiRequestParams apiRequestParams) 
        { 
            return base.Get(apiRequestParams);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<PersonVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Post([FromBody]PersonVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Put(int id, [FromBody]PersonVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Delete(int id) { return base.Delete(id); }
    }
}
