using System;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Helpers.Api;
using Microsoft.AspNetCore.Mvc;
using ScheduleRule = Machete.Web.ViewModel.Api.ScheduleRule;

namespace Machete.Web.Controllers.Api
{
    [Route("api/[controller]")]
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
        [ClaimsAuthorization(claimType: CAType.Role, claimValues: new[] { CV.Admin, CV.Employer })]
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
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ScheduleRules
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ScheduleRules/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ScheduleRules/5
        public void Delete(int id)
        {
        }
    }
}
