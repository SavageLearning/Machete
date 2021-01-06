using System.Collections.Generic;
using System.Threading.Tasks;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Service.shared;
using Machete.Web.Controllers.Api.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Machete.Web.Controllers.Api
{
    [Route("api/workers")]
    [ApiController]
    public class WorkersController : MacheteApiController
    {
        private readonly IPersonService _personService;
        private readonly IWorkerService _workerService;
        private readonly ILookupService _lookupService;

        public WorkersController(
            IPersonService personService,
            IWorkerService workerService,
            ILookupService lookupService)
        {
            this._workerService = workerService;
            this._lookupService = lookupService;
            this._personService = personService;
        }

        // GET: api/workers/in-skill
        // [Authorize(Roles = "Administrator, Hirer")]
        [HttpGet]
        [Route("in-skill/{skillId}")]
        public ActionResult<PagedResults<WorkforceList>> GetWorkersInSkill(
            [FromRoute] int skillId,
            [FromQuery] ApiRequestParams apiRequestParams)
        {
            var workforceList = _workerService.GetWorkersInSkill(skillId, apiRequestParams);

            return Ok(workforceList);
            // return Ok(workforceList); 
        }

        // GET: api/workers/skill
        // [Authorize(Roles = "Administrator, Hirer")]
        [HttpGet]
        [Route("skills")]
        public ActionResult<IEnumerable<Lookup>> GetSkills()
        {
            var skills = _lookupService.GetSkills();
            return Ok(skills);
        }

    }
}