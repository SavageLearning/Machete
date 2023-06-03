using System.Collections.Generic;
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
    public class PersonsController
        : MacheteApiController<Person, PersonVM, PersonListVM>
    {
        public PersonsController(IPersonService serv, IMapper map) : base(serv, map) { }

        [HttpGet, Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<IEnumerable<PersonListVM>> Get(
            [FromQuery] ApiRequestParams apiRequestParams)
        {
            return base.Get(apiRequestParams);
        }

        [HttpGet("{id}"), Authorize(Roles = "Administrator, Manager, Phonedesk, Hirer")]
        public new ActionResult<PersonVM> Get(int id) { return base.Get(id); }

        [HttpPost, Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Post([FromBody] PersonVM value) { return base.Post(value); }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Put(int id, [FromBody] PersonVM value) { return base.Put(id, value); }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public new ActionResult<PersonVM> Delete(int id) { return base.Delete(id); }
    }
}
